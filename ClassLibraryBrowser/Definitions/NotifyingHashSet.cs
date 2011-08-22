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
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions
{
    public class NotifyingHashSet<TItem> : ISet<TItem>
    {
        private HashSet<TItem> _items = new HashSet<TItem>();
        public event CollectionChangedEventHandler Changed;

        public void TriggerChanged()
        {
            if (this.Changed == null)
                return;
            ChangTransaction.Perform(delegate(ChangTransaction t)
            {
                CollectionChangedEventArgs e = new CollectionChangedEventArgs(t);
                this.Changed(this, e);
            });
        }



        public bool Add(TItem item)
        {
            bool result = this._items.Add(item);
            if (result)
                this.TriggerChanged();
            return result;
        }

        public void ExceptWith(IEnumerable<TItem> other)
        {
            int cnt = this._items.Count;
            this._items.ExceptWith(other);
            if (cnt != this._items.Count)
                this.TriggerChanged();
        }

        public void IntersectWith(IEnumerable<TItem> other)
        {
            int cnt = this._items.Count;
            this._items.IntersectWith(other);
            if (cnt != this._items.Count)
                this.TriggerChanged();
        }

        public bool IsProperSubsetOf(IEnumerable<TItem> other)
        {
            return this._items.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<TItem> other)
        {
            return this._items.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<TItem> other)
        {
            return this._items.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<TItem> other)
        {
            return this._items.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<TItem> other)
        {
            return this._items.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<TItem> other)
        {
            return this._items.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<TItem> other)
        {
            int cnt = this._items.Count;
            this._items.SymmetricExceptWith(other);
            if (cnt != this._items.Count)
                this.TriggerChanged();
        }

        public void UnionWith(IEnumerable<TItem> other)
        {
            int cnt = this._items.Count;
            this._items.UnionWith(other);
            if (cnt != this._items.Count)
                this.TriggerChanged();
        }

        void ICollection<TItem>.Add(TItem item)
        {
            this.Add(item);
        }

        public void Clear()
        {
            this._items.Clear();
            this.TriggerChanged();
        }

        public bool Contains(TItem item)
        {
            return this._items.Contains(item);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            this._items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._items.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<TItem>)this._items).IsReadOnly; }
        }

        public bool Remove(TItem item)
        {
            bool result = this._items.Remove(item);
            if (result)
                this.TriggerChanged();
            return result;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }
    }
}
