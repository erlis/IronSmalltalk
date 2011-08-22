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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators
{
    public delegate void CollectionChangedEventHandler(object sender, CollectionChangedEventArgs e);
    public delegate void ValueChangedEventHandler<TValue>(object sender, ValueChangedEventArgs<TValue> e);
    public delegate void ValueChangingEventHandler<TValue>(object sender, ValueChangingEventArgs<TValue> e);

    public class CollectionChangedEventArgs : EventArgs
    {
        public ChangTransaction Transaction { get; private set; }

        public CollectionChangedEventArgs(ChangTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException();

            this.Transaction = transaction;
        }
    }

    public class ValueChangedEventArgs<TValue> : EventArgs
    {
        public TValue OldValue { get; private set; }
        public TValue NewValue { get; private set; }
        public ChangTransaction Transaction { get; private set; }

        public ValueChangedEventArgs(TValue oldValue, TValue newValue, ChangTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException();

            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.Transaction = transaction;
        }
    }

    public class ValueChangingEventArgs<TValue> : ValueChangedEventArgs<TValue>
    {
        public bool Cancel { get; set; }

        public ValueChangingEventArgs(TValue oldValue, TValue newValue, ChangTransaction transaction)
            : base(oldValue, newValue, transaction)
        {
            this.Cancel = false;
        }

        public ValueChangingEventArgs(TValue oldValue, TValue newValue, ChangTransaction transaction, bool cancel)
            : base(oldValue, newValue, transaction)
        {
            this.Cancel = cancel;
        }        
    }

    public class ValueHolder<TValue>
        where TValue : class
    {
        private TValue _value;

        public TValue Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this.SetValue(value);
            }
        }

        public bool SetValue(TValue value)
        {
            if (this._value == value)
                return true;
            if (!this.TriggerChanging(this._value, value))
                return false; // Veto
            TValue oldValue = this._value;
            this._value = value;
            this.TriggerChanged(oldValue, value);
            return true;
        }

        public event ValueChangedEventHandler<TValue> Changed;
        public event ValueChangingEventHandler<TValue> Changing;

        public bool TriggerChanging(TValue oldValue, TValue newValue)
        {
            if (this.Changing == null)
                return true;
            return ChangTransaction.Perform(delegate(ChangTransaction t)
            {
                ValueChangingEventArgs<TValue> e = new ValueChangingEventArgs<TValue>(oldValue, newValue, t);
                this.Changing(this, e);
                return !e.Cancel;
            });
        }

        public void TriggerChanged(TValue oldValue, TValue newValue)
        {
            if (this.Changed == null)
                return;
            ChangTransaction.Perform(delegate(ChangTransaction t)
            {
                ValueChangedEventArgs<TValue> e = new ValueChangedEventArgs<TValue>(oldValue, newValue, t);
                this.Changed(this, e);
            });
        }
    }

}
