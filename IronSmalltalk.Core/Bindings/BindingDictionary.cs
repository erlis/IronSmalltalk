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
using System.Collections;

namespace IronSmalltalk.Runtime.Bindings
{
    /// <summary>
    /// This is a dictionary of binding items. It resembles a normal dictionary,
    /// but item can be accessed using both Strings or Symbols as the key. 
    /// It is also read-only to most clients, so they can't modify it and break consistency.
    /// </summary>
    /// <typeparam name="TItem">Type of the binding.</typeparam>
    /// <remarks>
    /// Instances of BindingDictionary are not synchronized. 
    /// This class is intended for read-only purposes. When the environment 
    /// is to be modified, the installer creates a copy of the dictionary 
    /// and replaces the reference to the whole dictionary (actually, to
    /// the namescope) in one atomic operation.
    /// </remarks>
    public class BindingDictionary<TItem> :
        IDictionary<Symbol, TItem>, IDictionary<string, TItem>, // IDictionary,
        IEnumerable<TItem>, IEnumerable, ICollection<TItem> //, ICollection
        where TItem : IBinding
    {
        private Dictionary<Symbol, TItem> _contents;
        public SmalltalkRuntime Runtime { get; private set; }
        private bool _readOnly = false;

        public BindingDictionary(SmalltalkRuntime runtime)
            : this(runtime, 100)
        {
        }

        public BindingDictionary(SmalltalkRuntime runtime, int initialCapacity)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            this.Runtime = runtime;
            this._contents = new Dictionary<Symbol, TItem>(initialCapacity);
        }

        #region Accessing 

        public TItem this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException();
                return this[this.ToSymbol(key)];
            }
        }

        public TItem this[Symbol key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException();
                TItem value;
                if (this.TryGetValue(key, out value))
                    return value;
                else
                    return this.ElementMissing(key.Value);
            }
        }

        public bool TryGetValue(string key, out TItem binding)
        {
            if (key == null)
                throw new ArgumentNullException();
            return this.TryGetValue(this.ToSymbol(key), out binding);
        }

        public bool TryGetValue(Symbol key, out TItem binding)
        {
            if (key == null)
                throw new ArgumentNullException();
            return this._contents.TryGetValue(key, out binding);
        }

        public int Count
        {
            get { return this._contents.Count; }
        }

        public ICollection<Symbol> Keys
        {
            get { return this._contents.Keys; }
        }

        public ICollection<TItem> Values
        {
            get { return this._contents.Values; }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return this._contents.Select(pair => pair.Value).GetEnumerator();
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        #endregion

        #region Modifying

        public void Add(TItem binding)
        {
            if (binding == null)
                throw new ArgumentNullException();
            if (this._readOnly)
                throw new InvalidOperationException("Dictionary is in read-only state.");
            this._contents.Add(binding.Name, binding);
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            if (items == null)
                throw new ArgumentNullException();
            if (this._readOnly)
                throw new InvalidOperationException("Dictionary is in read-only state.");
            foreach (TItem item in items)
                this._contents.Add(item.Name, item);
        }

        public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException();
            return this.Remove(this.ToSymbol(key));
        }

        public bool Remove(Symbol key)
        {
            if (key == null)
                throw new ArgumentNullException();
            if (this._readOnly)
                throw new InvalidOperationException("Dictionary is in read-only state.");
            return this._contents.Remove(key);
        }

        public bool Remove(TItem binding)
        {
            if (binding == null)
                throw new ArgumentNullException();
            return this.Remove(binding.Name);
        }

        public void InternalClear()
        {
            if (this._readOnly)
                throw new InvalidOperationException("Dictionary is in read-only state.");
            this._contents.Clear();
        }

        #endregion

        #region Testing

        public bool ContainsKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException();
            return this.ContainsKey(this.ToSymbol(key));
        }

        public bool ContainsKey(Symbol key)
        {
            if (key == null)
                throw new ArgumentNullException();
            TItem item;
            return this.TryGetValue(key, out item) && (item != null);
        }

        public bool Contains(TItem binding)
        {
            if (binding == null)
                throw new ArgumentNullException();
            TItem item;
            return this.TryGetValue(binding.Name, out item) && Object.ReferenceEquals(item, binding);
        }

        #endregion

        #region Helpers

        internal void WriteProtect()
        {
            // Once write-protected, it cannot be unprotected to read-write state!
            this._readOnly = true;
        }

        private Symbol ToSymbol(string key)
        {
            return this.Runtime.GetSymbol(key);
        }

        private TItem ElementMissing(string key)
        {
            throw new KeyNotFoundException(String.Format("The given key '{0}' was not present in the dictionary.", key));
        }

        #endregion

        #region interface IDictionary<Symbol, TItem>

        void IDictionary<Symbol, TItem>.Add(Symbol key, TItem value)
        {
            throw new NotSupportedException();
            //if (key == null)
            //    throw new ArgumentNullException("key");
            //if (value == null)
            //    throw new ArgumentNullException("value");
            //if (key != value.Name)
            //    throw new ArgumentException("Key must be equal to Value.Name.");
            //this.Add(value);
        }

        bool IDictionary<Symbol, TItem>.ContainsKey(Symbol key)
        {
            return this.ContainsKey(key);
        }

        ICollection<Symbol> IDictionary<Symbol, TItem>.Keys
        {
            get { return this.Keys; }
        }

        bool IDictionary<Symbol, TItem>.Remove(Symbol key)
        {
            throw new NotSupportedException();
            //return this.Remove(key);
        }

        bool IDictionary<Symbol, TItem>.TryGetValue(Symbol key, out TItem value)
        {
            return this.TryGetValue(key, out value);
        }

        ICollection<TItem> IDictionary<Symbol, TItem>.Values
        {
            get { return this.Values; }
        }

        TItem IDictionary<Symbol, TItem>.this[Symbol key]
        {
            get
            {
                return this[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        void ICollection<KeyValuePair<Symbol, TItem>>.Add(KeyValuePair<Symbol, TItem> item)
        {
            throw new NotSupportedException();
            //if (item.Value == null)
            //    throw new ArgumentException("Item.Value must not be null.");
            //if (item.Key != item.Value.Name)
            //    throw new ArgumentException("Item.Key must be equal to Item.Value.Name.");
            //this.Add(item.Value);
        }

        void ICollection<KeyValuePair<Symbol, TItem>>.Clear()
        {
            throw new NotSupportedException();
            //this.Clear();
        }

        bool ICollection<KeyValuePair<Symbol, TItem>>.Contains(KeyValuePair<Symbol, TItem> item)
        {
            TItem candidate;
            return this.TryGetValue(item.Key, out candidate) && Object.ReferenceEquals(candidate, item.Value);
        }

        void ICollection<KeyValuePair<Symbol, TItem>>.CopyTo(KeyValuePair<Symbol, TItem>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            if (array.Length < (this.Count + arrayIndex))
                throw new ArgumentException("The number of elements in the source ICollection <T> is greater than the available space from arrayIndex to the end of the destination array.");

            foreach (TItem item in this)
            {
                array[arrayIndex] = new KeyValuePair<Symbol, TItem>(item.Name, item);
                arrayIndex++;
            }
        }

        int ICollection<KeyValuePair<Symbol, TItem>>.Count
        {
            get { return this.Count; }
        }

        bool ICollection<KeyValuePair<Symbol, TItem>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<Symbol, TItem>>.Remove(KeyValuePair<Symbol, TItem> item)
        {
            throw new NotSupportedException();
            //if (item.Value == null)
            //    return false; // Item.Value must not be null.
            //if (item.Key != item.Value.Name)
            //    return false; // Item.Key must be equal to Item.Value.Name.
            //return this.Remove(item.Value);
        }

        IEnumerator<KeyValuePair<Symbol, TItem>> IEnumerable<KeyValuePair<Symbol, TItem>>.GetEnumerator()
        {
            return this._contents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region interface IDictionary<string, TItem>

        void IDictionary<string, TItem>.Add(string key, TItem value)
        {
            throw new NotSupportedException();
            //if (key == null)
            //    throw new ArgumentNullException("key");
            //if (value == null)
            //    throw new ArgumentNullException("value");
            //if (key != value.Name.Value)
            //    throw new ArgumentException("Key must be equal to Value.Name.Value.");
            //this.Add(value);
        }

        bool IDictionary<string, TItem>.ContainsKey(string key)
        {
            return this.ContainsKey(key);
        }

        ICollection<string> IDictionary<string, TItem>.Keys
        {
            get { return this.Keys.Select(symbol => symbol.Value).ToArray(); }
        }

        bool IDictionary<string, TItem>.Remove(string key)
        {
            throw new NotSupportedException();
            //return this.Remove(key);
        }

        bool IDictionary<string, TItem>.TryGetValue(string key, out TItem value)
        {
            return this.TryGetValue(key, out value);
        }

        ICollection<TItem> IDictionary<string, TItem>.Values
        {
            get { return this.Values; }
        }

        TItem IDictionary<string, TItem>.this[string key]
        {
            get
            {
                return this[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        void ICollection<KeyValuePair<string, TItem>>.Add(KeyValuePair<string, TItem> item)
        {

            //if (item.Value == null)
            //    throw new ArgumentException("Item.Value must not be null.");
            //if (item.Key != item.Value.Name.Value)
            //    throw new ArgumentException("Item.Key must be equal to Item.Value.Name.Value.");
            //this.Add(item.Value);
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<string, TItem>>.Clear()
        {
            throw new NotSupportedException();
            //this.Clear();
        }

        bool ICollection<KeyValuePair<string, TItem>>.Contains(KeyValuePair<string, TItem> item)
        {
            TItem candidate;
            return this.TryGetValue(item.Key, out candidate) && Object.ReferenceEquals(candidate, item.Value);
        }

        void ICollection<KeyValuePair<string, TItem>>.CopyTo(KeyValuePair<string, TItem>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            if (array.Length < (this.Count + arrayIndex))
                throw new ArgumentException("The number of elements in the source ICollection <T> is greater than the available space from arrayIndex to the end of the destination array.");

            foreach (TItem item in this)
            {
                array[arrayIndex] = new KeyValuePair<string, TItem>(item.Name.Value, item);
                arrayIndex++;
            }
        }

        int ICollection<KeyValuePair<string, TItem>>.Count
        {
            get { return this.Count; }
        }

        bool ICollection<KeyValuePair<string, TItem>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<string, TItem>>.Remove(KeyValuePair<string, TItem> item)
        {
            throw new NotSupportedException();
            //if (item.Value == null)
            //    return false; // Item.Value must not be null.
            //if (item.Key != item.Value.Name.Value)
            //    return false; // Item.Key must be equal to Item.Value.Name.Value.
            //return this.Remove(item.Value);
        }

        IEnumerator<KeyValuePair<string, TItem>> IEnumerable<KeyValuePair<string, TItem>>.GetEnumerator()
        {
            return this._contents.Select(pair => new KeyValuePair<string, TItem>(pair.Key.Value, pair.Value)).GetEnumerator();
        }

        #endregion

        #region interface IDictionary

        //void IDictionary.Add(object key, object value)
        //{
        //    Symbol ckey;
        //    try
        //    {
        //        ckey = (Symbol)key;
        //    }
        //    catch (InvalidCastException)
        //    {
        //        throw new ArgumentException(String.Format(
        //            "The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", key, typeof(Symbol)), "key");
        //    }
        //    TItem cvalue;
        //    try
        //    {
        //        cvalue = (TItem)value;
        //    }
        //    catch (InvalidCastException)
        //    {
        //        throw new ArgumentException(String.Format(
        //            "The value \"{0}\" is not of type \"{1}\" and cannot be used in this generic collection.", key, typeof(Symbol)), "value");
        //    }

        //    if (ckey == null)
        //        throw new ArgumentNullException("key");
        //    if (cvalue == null)
        //        throw new ArgumentNullException("value");

        //    ((IDictionary<Symbol, TItem>)this).Add(ckey, cvalue);
        //}

        //bool IDictionary.Contains(object key)
        //{
        //    if (key == null)
        //        throw new ArgumentNullException("key");
        //    if (key is Symbol)
        //        return this.ContainsKey((Symbol)key);
        //    if (key is string)
        //        return this.ContainsKey((string)key);
        //    return false;
        //}

        //IDictionaryEnumerator IDictionary.GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}

        //bool IDictionary.IsFixedSize
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //bool IDictionary.IsReadOnly
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //ICollection IDictionary.Keys
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //void IDictionary.Remove(object key)
        //{
        //    throw new NotImplementedException();
        //}

        //ICollection IDictionary.Values
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //object IDictionary.this[object key]
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        #endregion

        #region Interface ICollection<TItem>

        void ICollection<TItem>.Add(TItem item)
        {
            throw new NotSupportedException();
            //this.Add(item);
        }

        void ICollection<TItem>.Clear()
        {
            throw new NotSupportedException();
            //this.Clear();
        }

        bool ICollection<TItem>.Contains(TItem item)
        {
            return this.Contains(item);
        }

        void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex)
        {
            this.CopyTo(array, arrayIndex);
        }

        int ICollection<TItem>.Count
        {
            get { return this.Count; }
        }

        bool ICollection<TItem>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<TItem>.Remove(TItem item)
        {
            throw new NotSupportedException();
            //return this.Remove(item);
        }

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region interface ICollection

        //void ICollection.CopyTo(Array array, int index)
        //{
        //    throw new NotImplementedException();
        //}

        //int ICollection.Count
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //bool ICollection.IsSynchronized
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //object ICollection.SyncRoot
        //{
        //    get { throw new NotImplementedException(); }
        //}

        #endregion
    }
}
