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
    /// The discrete binding mechanism is a binding between a variable (instance variables excluded)
    /// and the value hold by that variable.
    /// </summary>
    public abstract class DiscreteBinding<TValue> : Binding<TValue>, IDiscreteBinding
        where TValue : class 
    {
        /// <summary>
        /// Creates a new discrete binding with the given name.
        /// </summary>
        /// <param name="name">Name of the binding.</param>
        public DiscreteBinding(Symbol name)
            : base(name)
        {
        }

        /// <summary>
        /// Set the value of the binding binding.
        /// </summary>
        /// <remarks>
        /// Event this method is public, it is not intended to be used by normal clients.
        /// The purpose of this method is to allow the variable to be initialized, 
        /// i.e. it is intended for Initializers to set the initial value.
        /// Normal clients should use the Value.set property where available and set
        /// the value trough it. If not available, then it is a constant binding.
        /// </remarks>
        /// <param name="value">The new value of the binding.</param>
        public virtual void SetValue(TValue value)
        {
            if (this.IsConstantBinding && this.HasBeenSet)
                throw new InvalidOperationException("Cannot change value of a constant binding. Value is already set.");
            this._value = value;
            if (this.RequiresValue)
                this.HasBeenSet = (value != null);
            else
                this.HasBeenSet = true;
        }

        /// <summary>
        /// Determines if a value (anything other than null) is required.
        /// </summary>
        /// <remarks>
        /// If this property returns true and a client tries to set the value of the binding to null,
        /// then the HasBeenSet property will stay unchanges. Normally HasBeenSet will be set to true
        /// when a client sets the value of the binding (iregardles of null or not null).
        /// </remarks>
        protected virtual bool RequiresValue
        {
            get { return false; }
        }

        #region Annotations

        /// <summary>
        /// Annotations that may be added to the binding.
        /// </summary>
        private Dictionary<string, string> _annotations;

        /// <summary>
        /// The annotation pairs associated with the annotetable object.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Annotations
        {
            get
            {
                if (this._annotations == null)
                    return AnnotationsHelper.Empty;
                return this._annotations;
            }
        }

        /// <summary>
        /// Set (or overwrite) an annotation on the annotetable object.
        /// </summary>
        /// <param name="key">Key of the annotation.</param>
        /// <param name="value">Value or null to remove the annotation.</param>
        public void Annotate(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();
            if (value == null)
            {
                if (this._annotations == null)
                    return;
                this._annotations.Remove(key);
            }
            else
            {
                if (this._annotations == null)
                    this._annotations = new Dictionary<string, string>();
                this._annotations[key] = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// The discrete binding mechanism is a binding between a variable (instance variables excluded)
    /// and the value hold by that variable.
    /// </summary>
    public interface IDiscreteBinding : IBinding, IAnnotetable
    {
    }


    /// <summary>
    /// Binding to a class variable. A class contains a list of those.
    /// </summary>
    public class ClassVariableBinding : DiscreteBinding<object>, IWritableBinding
    {
        public ClassVariableBinding(Symbol name)
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


}
