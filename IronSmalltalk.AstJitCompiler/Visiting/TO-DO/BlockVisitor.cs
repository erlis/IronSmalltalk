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
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.CodeGeneration.BindingScopes;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Execution.Internals;
using IronSmalltalk.Runtime.Behavior;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{

    public abstract class BlockVisitorBase : NestedEncoderVisitor<Expression>
    {
        protected BindingScope LocalScope { get; private set; }
        protected readonly List<ArgumentBinding> Arguments = new List<ArgumentBinding>();
        protected readonly List<TemporaryBinding> Temporaries = new List<TemporaryBinding>();

        public BlockVisitorBase(EncoderVisitor enclosingVisitor)
            : base(enclosingVisitor)
        {
            this.LocalScope = new BindingScope();
        }

        protected void DefineTemporary(string name)
        {
            TemporaryBinding temporary = new TemporaryBinding(name);
            this.Temporaries.Add(temporary);
            this.LocalScope.DefineBinding(temporary);
        }

        protected virtual void DefineArgument(string name)
        {
            this.DefineArgument(new ArgumentBinding(name));
        }

        protected virtual void DefineArgument(ArgumentBinding argument)
        {
            if (argument == null)
                throw new ArgumentNullException();
            this.Arguments.Add(argument);
            this.LocalScope.DefineBinding(argument);
        }

        protected internal override NameBinding GetBinding(string name)
        {
            NameBinding result;
            result = this.RootVisitor.ReservedScope.GetBinding(name);
            if (result != null)
                return result;

            result = this.LocalScope.GetBinding(name);
            if (result != null)
                return result;

            return base.GetBinding(name);
        }

        public override Expression VisitBlock(Compiler.SemanticNodes.BlockNode node)
        {
            for (int i = 0; i < node.Arguments.Count; i++)
                this.DefineArgument(node.Arguments[i].Token.Value);

            foreach (TemporaryVariableNode tmp in node.Temporaries)
                this.DefineTemporary(tmp.Token.Value);

            List<Expression> expressions = new List<Expression>();

            NameBinding nilBinding = this.GetBinding(SemanticConstants.Nil);

            // On each execution init all temp-vars with nil
            foreach (TemporaryBinding tmp in this.Temporaries)
                expressions.Add(tmp.GenerateAssignExpression(nilBinding.GenerateReadExpression(this), this));

            StatementVisitor visitor = new StatementVisitor(this);
            if (node.Statements != null)
                expressions.AddRange(node.Statements.Accept(visitor));

            if (expressions.Count == 0) 
                expressions.Add(nilBinding.GenerateReadExpression(this));

#if DEBUG
            if (expressions.Count == 0)
                throw new InternalCodeGenerationException("We did expect at least ONE expression");

            foreach (var expr in expressions)
            {
                if (expr.Type != typeof(object))
                    throw new InternalCodeGenerationException(String.Format("Expression does not return object! \n{0}", expr));
            }
#endif

            Expression result;
            if ((this.Temporaries.Count == 0) && (expressions.Count == 1))
                result = expressions[0];
            else
                result = Expression.Block(this.Temporaries.Select(binding => binding.Expression), expressions);

            return result;
        }
    }

    public class BlockVisitor : BlockVisitorBase
    {
        public BlockVisitor(EncoderVisitor enclosingVisitor)
            : base(enclosingVisitor)
        {
        }

        public override Expression VisitBlock(BlockNode node)
        {
            Expression result = base.VisitBlock(node);
            LambdaExpression lambda = Expression.Lambda(result, this.Arguments.Select(binding => (ParameterExpression)binding.Expression));
            return Expression.Convert(lambda, typeof(object));
        }

        protected internal override Expression Return(Expression value)
        {
            return Expression.Throw(Expression.New(BlockResult.ConstructorInfo, this.RootVisitor.HomeContext, value), typeof(object));
        }
    }

    public class InlineBlockVisitor : BlockVisitorBase
    {
        public InlineBlockVisitor(EncoderVisitor enclosingVisitor)
            : base(enclosingVisitor)
        {
        }

        protected override void DefineArgument(ArgumentBinding argument)
        {
            if (argument == null)
                throw new ArgumentNullException();
            // If already defined, do not define it twice ... because it's given externally.
            if (this.Arguments.Any(b => b.Name == argument.Name))
                return;
            base.DefineArgument(argument);
        }

        /// <summary>
        /// This allows us to define argument for inline blocks, which in reality 
        /// are not arguments but outer-scope variables.
        /// </summary>
        /// <param name="name">Name of the block argument.</param>
        /// <param name="expression">Expression that binds to the argument.</param>
        public void DefineExternalArgument(string name, Expression expression)
        {
            this.DefineArgument(new ArgumentBinding(name, expression));
        }
    }
}
