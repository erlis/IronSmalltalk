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
    public abstract class XBindingScope
    {
        protected Dictionary<string, XBinding> Bindings = new Dictionary<string, XBinding>();

        public object this[string key]
        {
            get
            {
                if (String.IsNullOrWhiteSpace(key))
                    throw new ArgumentNullException();
                XBinding result = this.GetBinding(key);
                if (result == null)
                    throw new ArgumentOutOfRangeException(String.Format("No item exists with key: {0}", key));
                return result.Value;
            }
        }

        public IEnumerable<string> Keys
        {
            get { return this.Bindings.Keys; }
        }

        public virtual XBinding GetBinding(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();
            XBinding result;
            if (this.Bindings.TryGetValue(key, out result))
                return result;
            return null;
        }

        protected virtual XBinding GetOrCreateBinding(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();
            XBinding result;
            if (this.Bindings.TryGetValue(key, out result))
                return result;
            result = new XBinding(key);
            this.Bindings.Add(key, result);
            return result;
        }

        protected virtual XBinding CreateBinding(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();
            XBinding result = new XBinding(key);
            this.Bindings.Add(key, result);
            return result;
        }
    }
}
