/*
 * **************************************************************************
 *
 * Copyright (c) The IronSmalltalk Project. 
 *
 * This source code is subject to terms and conditions of the 
 * license agreement found in the solution directory. 
 * See: $(SolutionDir)\License.htm ... in the root of this distribution.
 * By using this source code in any fashion, you are agreeing 
 * to be bound by the terms of the license agreement.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **************************************************************************
*/

using System;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime.Bindings;

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This class handles initialization for both classes and global variables / constants
    /// </remarks>
    public class GlobalInitializer : InitializerDefinition
    {
        public SourceReference<string> GlobalName { get; private set; }

        public GlobalInitializer(SourceReference<string> globalName, ISourceCodeReferenceService sourceCodeService, ISourceCodeReferenceService methodSourceCodeService, IntermediateInitializerCode code)
            : base(sourceCodeService, methodSourceCodeService, code)
        {
            if (globalName == null)
                throw new ArgumentNullException("globalName");
            this.GlobalName = globalName;
        }

        public override string ToString()
        {
            return String.Format("{0} initializer", this.GlobalName.Value);
        }

        protected internal override bool ValidateInitializer(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 2. Get the global.
            GlobalVariableOrConstantBinding globalBinding = installer.GetGlobalVariableOrConstantBinding(this.GlobalName.Value);
            ClassBinding classBinding = installer.GetClassBinding(this.GlobalName.Value);

            // 3. Check that such a binding exists ... but not both
            if (!((globalBinding == null) ^ (classBinding == null)))
                return installer.ReportError(this.GlobalName, InstallerErrors.GlobalInvalidName);
            if ((classBinding != null) && (classBinding.Value == null))
                throw new InvalidOperationException("Should have been set in ClassDefinition.CreataGlobalObject().");

            if (classBinding != null)
                return this.IntermediateCode.ValidateClassInitializer(installer.NameScope, classBinding.Value,
                    new IntermediateCodeValidationErrorSink(this.MethodSourceCodeService, installer));

            if (globalBinding.IsConstantBinding && globalBinding.HasBeenSet)
                return installer.ReportError(this.GlobalName, InstallerErrors.GlobalIsConstant);

            return this.IntermediateCode.ValidateGlobalInitializer(installer.NameScope,
                    new IntermediateCodeValidationErrorSink(this.MethodSourceCodeService, installer));
        }

        protected internal override void Execute(SmalltalkRuntime runtime)
        {
            GlobalVariableOrConstantBinding globalBinding = runtime.GlobalScope.GetGlobalVariableOrConstantBinding(this.GlobalName.Value);
            if (globalBinding != null)
            {
                var compilationResult = this.IntermediateCode.CompileGlobalInitializer(runtime);
                var code = compilationResult.ExecutableCode.Compile();
                var value = code(runtime, null);
                globalBinding.SetValue(value);
                return;
            }
            SmalltalkClass cls = runtime.GetClass(this.GlobalName.Value);
            if (cls != null)
            {
                var compilationResult = this.IntermediateCode.CompileClassInitializer(runtime, cls);
                var code = compilationResult.ExecutableCode.Compile();
                code(runtime, cls);
                return;
            }
            throw new InvalidOperationException(); // Should not get here
        }

    }
}
