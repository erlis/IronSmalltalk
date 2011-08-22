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

namespace IronSmalltalk.Runtime.Experimental
{
    public class XBinding
    {
        public string Key { get; private set; }
        public bool IsBound { get; private set; }
        private object _value;

        public XBinding(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();
            this.Key = key;
        }

        public object Value
        {
            get
            {
                if (!this.IsBound)
                    throw new InvalidOperationException(String.Format(
                        "Binding {0} is not bound yet!", this.Key));
                return this._value;
            }
            set
            {
                //if (!this._constant.HasValue)
                //    throw new InvalidOperationException("Constant/Variable state not set yet!");
                if (this._constant.HasValue && this._constant.Value)
                    throw new InvalidOperationException(String.Format(
                        "Binding {0} is a constant and cannot be changed!", this.Key));

                this._value = value;
                this.IsBound = true;
            }
        }

        private bool? _constant;

        public bool IsConstant
        {
            get
            {
                if (!this._constant.HasValue)
                    throw new InvalidOperationException("Constant/Variable state not set yet!");
                return this._constant.Value;
            }
        }

        public void MakeVariable()
        {
            if (this._constant.HasValue)
                throw new InvalidOperationException("Constant/Variable state already set!");
            this._constant = false;
        }

        public void MakeConstant()
        {
            if (this._constant.HasValue)
                throw new InvalidOperationException("Constant/Variable state already set!");
            this._constant = true;
        }

    }
}
