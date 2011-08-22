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
using System.Linq.Expressions;

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    public abstract class InitializerDefinition : CodeBasedDefinition<IntermediateInitializerCode>
    {
        public InitializerDefinition(ISourceCodeReferenceService sourceCodeService, ISourceCodeReferenceService methodSourceCodeService, IntermediateInitializerCode code)
            : base(sourceCodeService, methodSourceCodeService, code)
        {
        }

                /// <summary>
        /// Add annotations the the object being created.
        /// </summary>
        /// <param name="installer">Context which is performing the installation.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool AnnotateObject(IInstallerContext installer)
        {
            // Those are run immedeately, so nothing to annotate.
            return true; 
        }

        protected internal abstract bool ValidateInitializer(IInstallerContext installer);

        protected internal abstract void Execute(SmalltalkRuntime runtime);
    }
}
