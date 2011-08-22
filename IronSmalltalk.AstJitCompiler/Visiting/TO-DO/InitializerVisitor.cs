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
using IronSmalltalk.Compiler.SemanticNodes;
using System.Linq.Expressions;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Runtime.CodeGeneration.BindingScopes;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Behavior;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    public class InitializerVisitor : RootEncoderVisitor<Expression<Func<SmalltalkRuntime, object, object>>, InitializerNode>
    {
        public InitializerVisitor(SmalltalkRuntime runtime, BindingScope globalScope, BindingScope reservedScope)
            : base(runtime, globalScope, reservedScope)
        {
        }

        public override Symbol SuperLookupScope
        {
            get { return null; }
        }

        protected override void DefineArguments(InitializerNode node)
        {
        }

        protected override List<Expression> GenerateExpressions(InitializerNode node, out StatementVisitor visitor)
        {
            List<Expression> expressions = base.GenerateExpressions(node, out visitor);

            if (expressions.Count == 0)
            {
                NameBinding binding = this.GetBinding(SemanticConstants.Self);
                if (binding.IsErrorBinding)
                    binding = this.GetBinding(SemanticConstants.Nil);
                expressions.Add(binding.GenerateReadExpression(this));
            }

            return expressions;
        }


        public override Expression<Func<SmalltalkRuntime, object, object>> VisitInitializer(InitializerNode node)
        {
            ParameterExpression envParam = Expression.Parameter(typeof(SmalltalkRuntime), "Runtime");
            ParameterExpression selfParam = null;
            NameBinding binding = this.GetBinding(SemanticConstants.Self);
            if ((binding != null) && !binding.IsErrorBinding)
                selfParam = binding.GenerateReadExpression(this) as ParameterExpression;
            if (selfParam == null)
                selfParam = Expression.Parameter(typeof(object), "self");
            Expression body = this.InternalVisitFunction(node);

            return Expression.Lambda<Func<SmalltalkRuntime, object, object>>(body, envParam, selfParam);
        }
    }
}
