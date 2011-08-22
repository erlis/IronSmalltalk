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
    public class NotifyingCollection<TItem> : IList<TItem>
    {
        private List<TItem> _items = new List<TItem>();
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

        public int IndexOf(TItem item)
        {
            return this._items.IndexOf(item);
        }

        public void Insert(int index, TItem item)
        {
            this._items.Insert(index, item);
            this.TriggerChanged();
        }

        public void RemoveAt(int index)
        {
            this.RemoveAt(index);
            this.TriggerChanged();
        }

        public TItem this[int index]
        {
            get
            {
                return this._items[index];
            }
            set
            {
                this._items[index] = value;
                this.TriggerChanged();
            }
        }

        public void Add(TItem item)
        {
            this._items.Add(item);
            this.TriggerChanged();
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
            get { return false; }
        }

        public bool Remove(TItem item)
        {
            bool removed = this._items.Remove(item);
            if (removed)
                this.TriggerChanged();
            return removed;
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
