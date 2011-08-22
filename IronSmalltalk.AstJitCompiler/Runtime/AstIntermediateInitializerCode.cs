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
using IronSmalltalk.Runtime;
using IronSmalltalk.Compiler.SemanticNodes;
using System.Linq.Expressions;
using System.Dynamic;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime.CodeGeneration.Visiting;
using IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Runtime.CodeGeneration.BindingScopes;

namespace IronSmalltalk.AstJitCompiler.Runtime
{
    public class AstIntermediateInitializerCode : IntermediateInitializerCode
    {
        public InitializerNode ParseTree { get; private set; }

        public AstIntermediateInitializerCode(InitializerNode parseTree)
        {
            if (parseTree == null)
                throw new ArgumentNullException();
            this.ParseTree = parseTree;
        }

        public override InitializerCompilationResult CompileClassInitializer(SmalltalkRuntime runtime, SmalltalkClass cls)
        {
            return this.CompileClassInitializer(runtime, cls, runtime.GlobalScope);
        }

        private InitializerCompilationResult CompileClassInitializer(SmalltalkRuntime runtime, SmalltalkClass cls, SmalltalkNameScope globalNameScope)
        {
            return this.Compile(runtime,
                BindingScope.ForClassInitializer(cls, globalNameScope),
                ReservedScope.ForClassInitializer());
        }

        public override InitializerCompilationResult CompileGlobalInitializer(SmalltalkRuntime runtime)
        {
            return this.CompileGlobalInitializer(runtime, runtime.GlobalScope);
        }

        private InitializerCompilationResult CompileGlobalInitializer(SmalltalkRuntime runtime, SmalltalkNameScope globalNameScope)
        {
            return this.Compile(runtime,
                BindingScope.ForGlobalInitializer(globalNameScope),
                ReservedScope.ForGlobalInitializer());
        }

        public override InitializerCompilationResult CompilePoolItemInitializer(SmalltalkRuntime runtime, Pool pool)
        {
            return this.CompilePoolItemInitializer(runtime, pool, runtime.GlobalScope);
        }

        private InitializerCompilationResult CompilePoolItemInitializer(SmalltalkRuntime runtime, Pool pool, SmalltalkNameScope globalNameScope)
        {
            return this.Compile(runtime,
                BindingScope.ForPoolInitializer(pool, globalNameScope),
                ReservedScope.ForPoolInitializer());
        }

        public override InitializerCompilationResult CompileProgramInitializer(SmalltalkRuntime runtime)
        {
            return this.CompileProgramInitializer(runtime, runtime.GlobalScope);
        }

        private InitializerCompilationResult CompileProgramInitializer(SmalltalkRuntime runtime, SmalltalkNameScope globalNameScope)
        {
            return this.Compile(runtime,
                BindingScope.ForProgramInitializer(globalNameScope),
                ReservedScope.ForProgramInitializer());
        }

        private InitializerCompilationResult Compile(SmalltalkRuntime runtime, BindingScope globalScope, BindingScope reservedScope)
        {
            InitializerVisitor visitor = new InitializerVisitor(runtime, globalScope, reservedScope);
            var code = this.ParseTree.Accept(visitor);
            return new InitializerCompilationResult(code, visitor.BindingRestrictions);
        }

        public override bool ValidateClassInitializer(SmalltalkNameScope globalNameScope, SmalltalkClass cls, IIntermediateCodeValidationErrorSink errorSink)
        {
            return this.Validate(globalNameScope, errorSink, rt => this.CompileClassInitializer(rt, cls, globalNameScope));
        }

        public override bool ValidateGlobalInitializer(SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink)
        {
            return this.Validate(globalNameScope, errorSink, rt => this.CompileGlobalInitializer(rt, globalNameScope));
        }

        public override bool ValidatePoolItemInitializer(SmalltalkNameScope globalNameScope, Pool pool, IIntermediateCodeValidationErrorSink errorSink)
        {
            return this.Validate(globalNameScope, errorSink, rt => this.CompilePoolItemInitializer(rt, pool, globalNameScope));
        }

        public override bool ValidateProgramInitializer(SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink)
        {
            return this.Validate(globalNameScope, errorSink, rt => this.CompileProgramInitializer(rt, globalNameScope));
        }

        private bool Validate(SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink, Func<SmalltalkRuntime, InitializerCompilationResult> func)
        {
            return AstIntermediateMethodCode.Validate(this.ParseTree, errorSink, delegate()
            {
                Expression rt = Expression.Parameter(typeof(SmalltalkRuntime), "rt");
                Expression self = Expression.Parameter(typeof(object), "self");
                return func(globalNameScope.Runtime);
            });
        }
    }
}
