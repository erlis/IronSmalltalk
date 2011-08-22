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
using IronSmalltalk.Runtime.Internal;

namespace IronSmalltalk.Runtime.Bindings
{
    /// <summary>
    /// Binding of a global named object, i.e. class, pool, global variable or constant.
    /// </summary>
    public abstract class GlobalBinding<TValue> : DiscreteBinding<TValue>, IDiscreteGlobalBinding
        where TValue : class
    {
        /// <summary>
        /// Create a new global binding.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public GlobalBinding(Symbol name)
            : base(name)
        {
        }
    }

    /// <summary>
    /// Binding of a global named object, i.e. class, pool, global variable or constant.
    /// </summary>
    public interface IDiscreteGlobalBinding : IDiscreteBinding
    {
    }

    /// <summary>
    /// Global binding to a class object. This is a constant binding.
    /// </summary>
    public class ClassBinding : GlobalBinding<SmalltalkClass>
    {
        /// <summary>
        /// Create a new global class binding.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public ClassBinding(Symbol name)
            : base(name)
        {

        }

        /// <summary>
        /// Tells the system to set the HasBeenSet property to true only 
        /// when the value provided to SetValue method is not null.
        /// </summary>
        protected override bool RequiresValue
        {
            get { return true; }
        }
    }

    /// <summary>
    /// Global binding to a pool object. This is a constant binding.
    /// </summary>
    public class PoolBinding : GlobalBinding<Pool>
    {
        /// <summary>
        /// Create a new global pool binding.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public PoolBinding(Symbol name)
            : base(name)
        {

        }

        /// <summary>
        /// Tells the system to set the HasBeenSet property to true only 
        /// when the value provided to SetValue method is not null.
        /// </summary>
        protected override bool RequiresValue
        {
            get { return true; }
        }
    }

    /// <summary>
    /// Global binding to a global variable or global constant.
    /// </summary>
    public abstract class GlobalVariableOrConstantBinding : GlobalBinding<object>
    {
        /// <summary>
        /// Create a new global variable or constant binding.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public GlobalVariableOrConstantBinding(Symbol name)
            : base(name)
        {
        }
    }

    /// <summary>
    /// Global binding to a global variable.
    /// </summary>
    public class GlobalVariableBinding : GlobalVariableOrConstantBinding, IWritableBinding
    {
        /// <summary>
        /// Create a new global variable binding.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public GlobalVariableBinding(Symbol name)
            : base(name)
        {
        }

        /// <summary>
        /// Sets the value of the binding.
        /// </summary>
        /// <remarks>
        /// The "new" keywords is needed due to technical reasons. 
        /// The Value.get method is semantically identical to the base class' method.
        /// </remarks>
        public new object Value
        {
            get { return this._value; }
            set { this.SetValue(value); }
        }
    }

    /// <summary>
    /// Global binding to a global constant. This is a constant binding.
    /// </summary>
    public class GlobalConstantBinding : GlobalVariableOrConstantBinding
    {
        /// <summary>
        /// Create a new global constant binding.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public GlobalConstantBinding(Symbol name)
            : base(name)
        {

        }
    }
}
