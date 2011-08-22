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
    public abstract class IntermediateInitializerCode : IntermediateCodeBase
    {
        public abstract InitializerCompilationResult CompileGlobalInitializer(SmalltalkRuntime runtime);
        public abstract InitializerCompilationResult CompileProgramInitializer(SmalltalkRuntime runtime);
        public abstract InitializerCompilationResult CompileClassInitializer(SmalltalkRuntime runtime, SmalltalkClass cls);
        public abstract InitializerCompilationResult CompilePoolItemInitializer(SmalltalkRuntime runtime, Pool pool);

        public abstract bool ValidateGlobalInitializer(SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink);
        public abstract bool ValidateProgramInitializer(SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink);
        public abstract bool ValidateClassInitializer(SmalltalkNameScope globalNameScope, SmalltalkClass cls, IIntermediateCodeValidationErrorSink errorSink);
        public abstract bool ValidatePoolItemInitializer(SmalltalkNameScope globalNameScope, Pool pool, IIntermediateCodeValidationErrorSink errorSink);
    }

    public class InitializerCompilationResult : CompilationResult<Expression<Func<SmalltalkRuntime, object, object>>>
    {
        /// <summary>
        /// Create a new InitializerCompilationResult.
        /// </summary>
        /// <param name="executableCode">The Expression for the executable code.</param>
        public InitializerCompilationResult(Expression<Func<SmalltalkRuntime, object, object>> executableCode)
            : base(executableCode, null)
        {
        }

        /// <summary>
        /// Create a new InitializerCompilationResult.
        /// </summary>
        /// <param name="executableCode">The Expression for the executable code.</param>
        /// <param name="restrictions">Optional restrictions attached to the executable code expression.</param>
        public InitializerCompilationResult(Expression<Func<SmalltalkRuntime, object, object>> executableCode, BindingRestrictions restrictions)
            : base(executableCode, restrictions)
        {
        }
    }
}
