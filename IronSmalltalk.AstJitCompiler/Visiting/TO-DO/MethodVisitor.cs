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
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Runtime.CodeGeneration.BindingScopes;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Runtime.Behavior;
using System.Dynamic;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    public class MethodVisitor : RootEncoderVisitor<Expression, MethodNode>
    {
        internal readonly DynamicMetaObject[] PassedArguments;
        internal readonly DynamicMetaObject SelfArgument;

        public MethodVisitor(SmalltalkRuntime runtime, BindingScope globalScope, BindingScope reservedScope, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope)
            : base(runtime, globalScope, reservedScope)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            if (superScope == null)
                throw new ArgumentNullException("superScope");
            this.SelfArgument = self;
            this.PassedArguments = arguments;
            this._superLookupScope = superScope;
        }

        private readonly Symbol _superLookupScope;
        public override Symbol SuperLookupScope
        {
            get { return this._superLookupScope; }
        }

        protected override void DefineArguments(MethodNode node)
        {
            for (int i = 0; i < node.Arguments.Count; i++)
                this.DefineArgument(node.Arguments[i].Token.Value, this.PassedArguments[i].Expression);
        }

        protected override List<Expression> GenerateExpressions(MethodNode node, out StatementVisitor visitor)
        {
            List<Expression> expressions = new List<Expression>();
            if (node.Primitive != null)
                expressions.AddRange(node.Primitive.Accept(new PrimitiveCallVisitor(this, (node.Statements != null))));

            expressions.AddRange(base.GenerateExpressions(node, out visitor));

            if ((node.Primitive == null) && ((expressions.Count == 0) || !visitor.HasReturned))
            {
                // If no explicit return, a method must return self. 
                NameBinding binding = this.GetBinding(SemanticConstants.Self);
                expressions.Add(binding.GenerateReadExpression(this));
            }

            return expressions;
        }

        public override Expression VisitMethod(MethodNode node)
        {
            return this.InternalVisitFunction(node);
        }

        internal NameBinding GetLocalVariable(string name)
        {
            NameBinding result = this.LocalScope.GetBinding(name);
            if (result != null)
                return result;

            return new ErrorBinding(name);
        }
    }
}
