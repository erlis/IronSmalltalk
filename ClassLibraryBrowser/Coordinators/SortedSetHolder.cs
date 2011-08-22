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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators
{
    public class SortedSetHolder<TItem>
    {
        private NotifyingSortedSet<TItem> _collection;

        public NotifyingSortedSet<TItem> Collection
        {
            get
            {
                return this._collection;
            }
            set
            {
                this.SetValue(value);
            }
        }

        public bool SetValue(NotifyingSortedSet<TItem> collection)
        {
            if (this._collection == collection)
                return true;
            if (!this.TriggerChanging(this._collection, collection))
                return false; // Veto
            NotifyingSortedSet<TItem> oldValue = this._collection;
            if (this._collection != null)
                this._collection.Changed -= new CollectionChangedEventHandler(this.TriggerCollectionChanged);
            this._collection = collection;
            if (this._collection != null)
                this._collection.Changed += new CollectionChangedEventHandler(this.TriggerCollectionChanged);
            this.TriggerChanged(oldValue, collection);
            return true;
        }

        void TriggerCollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            if (this.CollectionChanged == null)
                return;
            this.CollectionChanged(sender, e);
        }

        public event ValueChangedEventHandler<NotifyingSortedSet<TItem>> Changed;
        public event ValueChangingEventHandler<NotifyingSortedSet<TItem>> Changing;
        public event CollectionChangedEventHandler CollectionChanged;

        public bool TriggerChanging(NotifyingSortedSet<TItem> oldValue, NotifyingSortedSet<TItem> newValue)
        {
            if (this.Changing == null)
                return true;
            return ChangTransaction.Perform(delegate(ChangTransaction t)
            {
                ValueChangingEventArgs<NotifyingSortedSet<TItem>> e =
                    new ValueChangingEventArgs<NotifyingSortedSet<TItem>>(oldValue, newValue, t);
                this.Changing(this, e);
                return !e.Cancel;
            });
        }

        public void TriggerChanged(NotifyingSortedSet<TItem> oldValue, NotifyingSortedSet<TItem> newValue)
        {
            if (this.Changed == null)
                return;
            ChangTransaction.Perform(delegate(ChangTransaction t)
            {
                ValueChangingEventArgs<NotifyingSortedSet<TItem>> e =
                    new ValueChangingEventArgs<NotifyingSortedSet<TItem>>(oldValue, newValue, t);
                this.Changed(this, e);
            });
        }
    }
}
