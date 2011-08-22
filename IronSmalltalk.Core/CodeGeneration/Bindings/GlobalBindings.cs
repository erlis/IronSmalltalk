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
using System.Reflection;

namespace IronSmalltalk.Runtime.CodeGeneration.Bindings
{
    public abstract class GlobalBinding<TBinding> : DiscreteBinding<TBinding>
        where TBinding : IronSmalltalk.Runtime.Bindings.IDiscreteGlobalBinding
    {
        public GlobalBinding(string name, TBinding binding)
            : base(name, binding)
        {
        }
    }

    public class GlobalVariableBinding : GlobalBinding<RTB.GlobalVariableBinding>, IAssignableBinding
    {        
        public GlobalVariableBinding(string name, RTB.GlobalVariableBinding binding)
            : base(name, binding)
        {
        }

        public System.Linq.Expressions.Expression GenerateAssignExpression(System.Linq.Expressions.Expression value, IBindingClient client)
        {

            return Expression.Assign(
                Expression.Property(
                    Expression.Constant(this.Binding, typeof(RTB.GlobalVariableBinding)),
                    GlobalVariableBinding.SetPropertyInfo), 
                value);
        }
    }

    public class GlobalConstantBinding : GlobalBinding<RTB.GlobalConstantBinding>
    {
        public GlobalConstantBinding(string name, RTB.GlobalConstantBinding binding)
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

    public class ClassBinding : GlobalBinding<RTB.ClassBinding>
    {
        public ClassBinding(string name, RTB.ClassBinding binding)
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

    public class PoolBinding : GlobalBinding<RTB.PoolBinding>
    {
        public PoolBinding(string name, RTB.PoolBinding binding)
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
