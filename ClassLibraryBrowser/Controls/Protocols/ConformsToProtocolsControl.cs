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
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Description;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols
{
    public partial class ConformsToProtocolsControl : UserControl
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

        public ConformsToProtocolsControl()
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
                if (this.AllProtocolsHolder == null)
                    return;
                if (this.AllProtocolsHolder.Collection == null)
                    return;

                Dictionary<string, int> protocolLevelMap = new Dictionary<string, int>();
                this.BuildProtocolLevelMap(this.ProtocolHolder.Value.Name, 0, protocolLevelMap);
                int maxDepth = protocolLevelMap.Values.Max();
                foreach (string key in protocolLevelMap.Keys.ToArray()) // Needs the ToArray() copy due to mutation
                    protocolLevelMap[key] = maxDepth - protocolLevelMap[key];

                IEnumerable<string> sorted = protocolLevelMap.Keys.OrderBy(k => protocolLevelMap[k]).ThenBy(k => k);
                foreach (string key in sorted)
                    this.AddProtocol(key, protocolLevelMap[key]);
            }
            finally
            {
                this.listProtocols.EndUpdate();
            }
        }

        private void BuildProtocolLevelMap(string protocolName, int level, Dictionary<string, int> protocolLevelMap)
        {
            if (this.AllProtocolsHolder == null)
                return;
            if (this.AllProtocolsHolder.Collection == null)
                return;
            int currentLevel = 0;
            bool existed = protocolLevelMap.TryGetValue(protocolName, out currentLevel);
            protocolLevelMap[protocolName] = Math.Max(currentLevel, level);

            if (existed)
                return;
            Protocol protocol = this.AllProtocolsHolder.Collection.FirstOrDefault(p => p.Name == protocolName);
            if (protocol == null)
                return;
            foreach(string conformsTo in protocol.ConformsTo)
                this.BuildProtocolLevelMap(conformsTo, level + 1, protocolLevelMap);
        }

        private void AddProtocol(string protocolName, int level)
        {
            Protocol prot = this.AllProtocolsHolder.Collection.FirstOrDefault(p => p.Name == protocolName);
            string tmp = "";
            for (int i = 0; i < level; i++)
                tmp = tmp + "   ";
            ListViewItem lvi = this.listProtocols.Items.Add(tmp + protocolName);
            if (prot != null)
            {
                lvi.SubItems.Add(prot.DocumentationId);
                lvi.SubItems.Add(prot.IsAbstract ? "Yes" : "No");
                lvi.SubItems.Add(prot.Messages.Count.ToString());
                lvi.SubItems.Add(prot.StandardGlobals.Count(g => g.Definition is GlobalClass).ToString());
                lvi.SubItems.Add(prot.StandardGlobals.Count(g => g.Definition is GlobalVariable).ToString());
                lvi.SubItems.Add(prot.StandardGlobals.Count(g => g.Definition is GlobalConstant).ToString());
                lvi.SubItems.Add(prot.StandardGlobals.Count(g => g.Definition is GlobalPool).ToString());
                lvi.SubItems.Add(prot.StandardGlobals.Count(g => g.Definition == null).ToString());
            }
        }
    }
}
