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

#if SILVERLIGHT

namespace System
{
    /// <summary>
    /// The serializable attribute.
    /// </summary>
    public class SerializableAttribute : Attribute
    {
    }

    /// <summary>
    /// Non serializable attribute.
    /// </summary>
    public class NonSerializedAttribute : Attribute
    {
    }
}

namespace System.Collections.Concurrent
{
    public class ConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public ConcurrentDictionary()
        {
        }

        public ConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (!this.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                this.Add(key, value);
            }
            return value;
        }

        public bool TryRemove(TKey key, out TValue value)
        {
            if (this.TryGetValue(key, out value))
            {
                this.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
        {
            if (!this.ContainsKey(key))
                return false;
            IEqualityComparer<TValue> comparer = (IEqualityComparer<TValue>)EqualityComparer<TValue>.Default;
            if (!comparer.Equals(this[key], comparisonValue))
                return false;
            this[key] = newValue;
            return true;
        }
 

    }
}

#endif
