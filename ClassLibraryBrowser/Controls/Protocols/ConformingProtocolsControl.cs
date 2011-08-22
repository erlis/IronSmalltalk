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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Description;
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols
{
    public partial class ConformingProtocolsControl : UserControl
    {
        private ValueHolder<Protocol> _protocol;

        [Browsable(false)]
        public ValueHolder<Protocol> ProtocolHolder
        {
            get
            {
                return this._protocol;
            }
            set
            {
                if ((this._protocol == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._protocol == value)
                    return;
                if (this._protocol != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._protocol = value;
                value.Changed += new ValueChangedEventHandler<Protocol>(this.ProtocolChanged);
            }
        }

        private CollectionHolder<Protocol> _allProtocols;

        public CollectionHolder<Protocol> AllProtocolsHolder
        {
            get
            {
                return this._allProtocols;
            }
            set
            {
                if ((this._allProtocols == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._allProtocols == value)
                    return;
                if (this._allProtocols != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._allProtocols = value;
                value.Changed += new ValueChangedEventHandler<NotifyingCollection<Protocol>>(this.AllProtocolsChanged);
                value.CollectionChanged += new CollectionChangedEventHandler(this.AllProtocolsChanged);
            }
        }

        void ProtocolChanged(object sender, ValueChangedEventArgs<Protocol> e)
        {
            this.UpdateList();
        }

        void AllProtocolsChanged(object sender, EventArgs e)
        {
            this.UpdateList();
        }

        public ConformingProtocolsControl()
        {
            InitializeComponent();
        }

        public void UpdateList()
        {
            if (this.ProtocolHolder == null)
                return;
            this.listProtocols.BeginUpdate();
            try
            {
                this.listProtocols.Items.Clear();
                if (this.ProtocolHolder.Value == null)
                    return;
                List<Protocol> recursionSet = new List<Protocol>();
                this.AddProtocol(this.ProtocolHolder.Value, recursionSet, 0);
            }
            finally
            {
                this.listProtocols.EndUpdate();
            }
        }

        private void AddProtocol(Protocol prot, List<Protocol> recursionSet, int level)
        {
            if (this.AllProtocolsHolder == null)
                return;
            if (this.AllProtocolsHolder.Collection == null)
                return;
            IEnumerable<Protocol> conforming = this.AllProtocolsHolder.Collection
                .Where(p => p.ConformsTo.Contains(prot.Name)).OrderBy(p => p.Name);

            foreach (Protocol p in conforming)
            {
                if (!recursionSet.Contains(p))
                {
                    recursionSet.Add(p);
                    string tmp = "";
                    for (int i = 0; i < level; i++)
                        tmp = tmp + "   ";
                    ListViewItem lvi = this.listProtocols.Items.Add(tmp + p.Name);
                    lvi.SubItems.Add(p.DocumentationId);
                    lvi.SubItems.Add(p.IsAbstract ? "Yes" : "No");
                    lvi.SubItems.Add(p.Messages.Count.ToString());
                    lvi.SubItems.Add(p.StandardGlobals.Count(g => g.Definition is GlobalClass).ToString());
                    lvi.SubItems.Add(p.StandardGlobals.Count(g => g.Definition is GlobalVariable).ToString());
                    lvi.SubItems.Add(p.StandardGlobals.Count(g => g.Definition is GlobalConstant).ToString());
                    lvi.SubItems.Add(p.StandardGlobals.Count(g => g.Definition is GlobalPool).ToString());
                    lvi.SubItems.Add(p.StandardGlobals.Count(g => g.Definition == null).ToString());
                    this.AddProtocol(p, recursionSet, level + 1);
                }
            }
        }
    }
}
