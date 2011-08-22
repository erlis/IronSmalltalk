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
using RTB = IronSmalltalk.Runtime.Bindings;
using System.Linq.Expressions;

namespace IronSmalltalk.Runtime.CodeGeneration.Bindings
{
    public class PoolVariableBinding : DiscreteBinding<RTB.PoolVariableBinding>, IAssignableBinding
    {
        public PoolVariableBinding(string name, RTB.PoolVariableBinding binding)
            : base(name, binding)
        {
        }

        public System.Linq.Expressions.Expression GenerateAssignExpression(System.Linq.Expressions.Expression value, IBindingClient client)
        {
            return Expression.Assign(
                Expression.Property(
                    Expression.Constant(this.Binding, typeof(RTB.PoolVariableBinding)),
                    PoolVariableBinding.SetPropertyInfo),
                value);
        }
    }

    public class PoolConstantBinding : DiscreteBinding<RTB.PoolConstantBinding>
    {
        public PoolConstantBinding(string name, RTB.PoolConstantBinding binding)
            : base(name, binding)
        {
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

}
