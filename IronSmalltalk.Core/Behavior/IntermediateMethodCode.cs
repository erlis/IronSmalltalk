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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using IronSmalltalk.Runtime.Bindings;
using System.Dynamic;

namespace IronSmalltalk.Runtime.Behavior
{
    public abstract class IntermediateMethodCode : IntermediateCodeBase
    {
        public abstract MethodCompilationResult CompileInstanceMethod(SmalltalkRuntime runtime, SmalltalkClass cls, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope);
        public abstract MethodCompilationResult CompileClassMethod(SmalltalkRuntime runtime, SmalltalkClass cls, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope);

        public abstract bool ValidateInstanceMethod(SmalltalkClass cls, SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink);
        public abstract bool ValidateClassMethod(SmalltalkClass cls, SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink);
    }

    public class MethodCompilationResult : CompilationResult<Expression>
    {
        /// <summary>
        /// Create a new MethodCompilationResult.
        /// </summary>
        /// <param name="executableCode">The Expression for the executable code.</param>
        public MethodCompilationResult(Expression executableCode)
            : base(executableCode, null)
        {
        }

        /// <summary>
        /// Create a new MethodCompilationResult.
        /// </summary>
        /// <param name="executableCode">The Expression for the executable code.</param>
        /// <param name="restrictions">Optional restrictions attached to the executable code expression.</param>
        public MethodCompilationResult(Expression executableCode, BindingRestrictions restrictions)
            : base(executableCode, restrictions)
        {
        }
    }
}
