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
using System.Reflection;

namespace IronSmalltalk.Runtime.CodeGeneration.Bindings
{
    /// <summary>
    /// Binding information to a discrete runtime binding.
    /// </summary>
    /// <typeparam name="TBinding">Type of discrete runtime binding.</typeparam>
    public abstract class DiscreteBinding<TBinding> : NameBinding
        where TBinding : IronSmalltalk.Runtime.Bindings.IDiscreteBinding
    {
        public TBinding Binding { get; private set; }

        public DiscreteBinding(string name, TBinding binding)
            : base(name)
        {
            if (binding == null)
                throw new ArgumentNullException("binding");
            this.Binding = binding;
        }

        public override System.Linq.Expressions.Expression GenerateReadExpression(IBindingClient client)
        {
            if (this.IsConstantValueBinding)
                return Expression.Constant(this.Binding.Value, typeof(object));
            return Expression.Property(
                Expression.Constant(this.Binding, typeof(TBinding)),
                DiscreteBinding<TBinding>.GetPropertyInfo);
        }

        private static PropertyInfo _getPropertyInfo;

        /// <summary>
        /// Proptery info for the get_Value property of the discrete variable binding.
        /// </summary>
        protected static PropertyInfo GetPropertyInfo
        {
            get
            {
                if (DiscreteBinding<TBinding>._getPropertyInfo == null)
                {
                    // NB: We can't use GetProperty("Value") due to AmbiguousMatchException, therefore do stuff by hand
                    IEnumerable<PropertyInfo> properties = typeof(TBinding).GetProperties(
                        BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                        BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                    // Filter only properties named "Value"
                    properties = properties.Where(pi => (pi.Name == "Value"));
                    // First try for a property declared directly in the defining type.
                    DiscreteBinding<TBinding>._getPropertyInfo = properties.Where(pi => (pi.DeclaringType == typeof(TBinding))).FirstOrDefault();
                    // If not, it may be defined in the superclass, just take any property found (there should be just one)
                    if (DiscreteBinding<TBinding>._getPropertyInfo == null)
                        DiscreteBinding<TBinding>._getPropertyInfo = properties.FirstOrDefault();

                    if (DiscreteBinding<TBinding>._getPropertyInfo == null)
                        throw new InvalidOperationException("Expected type to have getter property named Value");
                }
                return DiscreteBinding<TBinding>._getPropertyInfo;
            }
        }

        private static PropertyInfo _setPropertyInfo;

        /// <summary>
        /// Proptery info for the set_Value property of the discrete variable binding.
        /// </summary>
        /// <remarks>
        /// This method should only be used by subclasses the are assignable binding 
        /// and where TBinding is a class that implements the set_Value property.
        /// </remarks>
        protected static PropertyInfo SetPropertyInfo
        {
            get
            {
                if (DiscreteBinding<TBinding>._setPropertyInfo == null)
                {
                    // NB: We can't use GetProperty("Value") due to AmbiguousMatchException, therefore do stuff by hand
                    IEnumerable<PropertyInfo> properties = typeof(TBinding).GetProperties(
                        BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy |
                        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
                    // Filter only properties named "Value"
                    properties = properties.Where(pi => (pi.Name == "Value"));
                    // First try for a property declared directly in the defining type.
                    DiscreteBinding<TBinding>._setPropertyInfo = properties.Where(pi => (pi.DeclaringType == typeof(TBinding))).FirstOrDefault();
                    // If not, it may be defined in the superclass, just take any property found (there should be just one)
                    if (DiscreteBinding<TBinding>._setPropertyInfo == null)
                        DiscreteBinding<TBinding>._setPropertyInfo = properties.FirstOrDefault();

                    if (DiscreteBinding<TBinding>._setPropertyInfo == null)
                        throw new InvalidOperationException("Expected type to have setter property named Value");
                }
                return DiscreteBinding<TBinding>._setPropertyInfo;
            }
        }
    }
}
