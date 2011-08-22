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
using System.Linq.Expressions;
using AST = System.Linq.Expressions;

namespace IronSmalltalk.Runtime.CodeGeneration.Bindings
{
    public abstract class ExpressionBinding<TExpression> : NameBinding
        where TExpression : Expression
    {
        public TExpression Expression { get; protected set; }

        public ExpressionBinding(string name)
            : base(name)
        {
        }

        public override Expression GenerateReadExpression(IBindingClient client)
        {
            return this.Expression;
        }
    }

    public class ConstantBinding : ExpressionBinding<ConstantExpression>
    {
        public ConstantBinding(string name, object value)
            : base(name)
        {
            if (value == null)
                throw new ArgumentNullException();

            this.Expression = AST.Expression.Constant(value);
        }

        public ConstantBinding(string name, object value, Type type)
            : base(name)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            // It's OK for value to be null, as long as type is given
            this.Expression = AST.Expression.Constant(value, type);
        }

        /// <summary>
        /// This returns true if the value of the binding will always be the same. 
        /// Some read-only bindings (e.g. self, super) are NOT constant-value-bindings.
        /// </summary>
        public override bool IsConstantValueBinding
        {
            get { return true; }
        }
    }

    public class ArgumentBinding : ExpressionBinding<Expression>
    {
        public ArgumentBinding(string name)
            : base(name)
        {
            this.Expression = AST.Expression.Parameter(typeof(object), name);
        }

        public ArgumentBinding(string name, Expression expression)
            : base(name)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            this.Expression = expression;
        }
    }

    public class TemporaryBinding : ExpressionBinding<ParameterExpression>, IAssignableBinding
    {
        public TemporaryBinding(string name)
            : base(name)
        {
            this.Expression = AST.Expression.Variable(typeof(object), name);
        }

        public Expression GenerateAssignExpression(Expression value, IBindingClient client)
        {
            return AST.Expression.Assign(this.Expression, value);
        }
    }
}
