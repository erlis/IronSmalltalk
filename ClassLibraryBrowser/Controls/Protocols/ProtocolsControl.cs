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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols
{
    public partial class ProtocolsControl : UserControl, IMainFormMenuConsumer
    {
        private ValueHolder<SystemDescription> _classLibrary;
        private ToolStripMenuItem ProtocolMenu;

        [Browsable(false)]
        public ValueHolder<SystemDescription> SystemDescriptionHolder
        {
            get
            {
                return this._classLibrary;
            }
            set
            {
                if ((this._classLibrary == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._classLibrary == value)
                    return;
                if (this._classLibrary != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._classLibrary = value;
                value.Changed += new ValueChangedEventHandler<SystemDescription>(this.ClassLibraryChanged);
                this.protocolDefinitionControl.SystemDescriptionHolder = value;
            }
        }

        public SystemDescription SystemDescription
        {
            get
            {
                if (this.SystemDescriptionHolder == null)
                    return null;
                return this.SystemDescriptionHolder.Value;
            }
        }

        private void ClassLibraryChanged(object sender, ValueChangedEventArgs<SystemDescription> e)
        {
            this.FillView();
            this.ProtocolHolder.Value = null;
            if (this.SystemDescription == null)
                this.AllProtocolsHolder.Collection = null;
            else
                this.AllProtocolsHolder.Collection = this.SystemDescription.Protocols;
            if (this.ProtocolMenu != null)
                this.ProtocolMenu.Enabled = (this.SystemDescription != null) && this.Visible;
        }

        private ValueHolder<Protocol> _protocol;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
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

        public Protocol Protocol
        {
            get { return this.ProtocolHolder.Value; }
        }

        private void ProtocolChanged(object sender, ValueChangedEventArgs<Protocol> e)
        {
            this.tabControlProtocol.Enabled = (this.Protocol != null);
            this.protocolDefinitionControl.Enabled = (this.Protocol != null);
            this.protocolGlobalControl.Enabled = (this.Protocol != null);
            this.protocolMessagesControl.Enabled = (this.Protocol != null);
            this.conformingProtocolsControl.Enabled = (this.Protocol != null);
            this.conformsToProtocolsControl.Enabled = (this.Protocol != null);
            this.comboProtocols.SelectedItem = e.NewValue;
        }

        private CollectionHolder<Protocol> AllProtocolsHolder;

        public ProtocolsControl()
        {
            this.ProtocolHolder = new ValueHolder<Protocol>();
            this.AllProtocolsHolder = new CollectionHolder<Protocol>();
            this.AllProtocolsHolder.CollectionChanged += new CollectionChangedEventHandler(this.AllProtocolsChanged);
            InitializeComponent();
            this.protocolDefinitionControl.ProtocolHolder = this.ProtocolHolder;
            this.protocolDefinitionControl.AllProtocolsHolder = this.AllProtocolsHolder;
            this.protocolGlobalControl.ProtocolHolder = this.ProtocolHolder;
            this.protocolMessagesControl.ProtocolHolder = this.ProtocolHolder;
            this.conformingProtocolsControl.AllProtocolsHolder = this.AllProtocolsHolder;
            this.conformingProtocolsControl.ProtocolHolder = this.ProtocolHolder;
            this.conformsToProtocolsControl.AllProtocolsHolder = this.AllProtocolsHolder;
            this.conformsToProtocolsControl.ProtocolHolder = this.ProtocolHolder;
            this.FillView();
        }

        private void AllProtocolsChanged(object sender, EventArgs e)
        {
            object selected = this.comboProtocols.SelectedItem;
            this.FillView();
            this.comboProtocols.SelectedItem = selected;
        }


        private void FillView()
        {
            this.comboProtocols.Items.Clear();
            if (this.SystemDescription != null)
                this.comboProtocols.Items.AddRange(this.SystemDescription.Protocols.OrderBy(prot => prot.Name).ToArray());
            this.comboProtocols.Enabled = (this.SystemDescription != null);
            this.tabControlProtocol.Enabled = (this.Protocol != null);
        }

        private void comboProtocols_Format(object sender, ListControlConvertEventArgs e)
        {
            Protocol prot = e.ListItem as Protocol;
            if (prot == null)
                return;

            int undefined = prot.StandardGlobals.Count(g => g.Definition == null);
            string txt = "";
            if (undefined > 0)
                txt = " (" + undefined.ToString() + ")";
            e.Value = prot.Name + txt;
        }

        private void comboProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            Protocol prot = this.comboProtocols.SelectedItem as Protocol;
            this.ProtocolHolder.Value = prot;

            if (this.ProtocolHolder.Value != prot)
                this.comboProtocols.SelectedItem = this.ProtocolHolder.Value;
        }

        private void tabControlProtocol_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.Cancel)
                return;
            if (e.Action != TabControlAction.Deselecting)
                return;

            if (e.TabPage == this.tabPageProtocol)
                e.Cancel = !this.ProtocolHolder.TriggerChanging(this.Protocol, this.Protocol);
            if (e.TabPage == this.tabPageGlobals)
                e.Cancel = !this.protocolGlobalControl.GlobalHolder.TriggerChanging(
                    this.protocolGlobalControl.Global, this.protocolGlobalControl.Global);
            if (e.TabPage == this.tabPageMessages)
                e.Cancel = !this.protocolMessagesControl.MessageHolder.TriggerChanging(
                    this.protocolMessagesControl.Message, this.protocolMessagesControl.Message);
        }

        void IMainFormMenuConsumer.AddMainMenuItems(MenuStrip mainMenu)
        {
            this.ProtocolMenu = new ToolStripMenuItem();
            this.ProtocolMenu.MergeIndex = 10;
            this.ProtocolMenu.MergeAction = MergeAction.Insert;
            this.ProtocolMenu.Name = "ProtocolMenu";
            this.ProtocolMenu.Text = "Protocol";
            this.ProtocolMenu.Enabled = false;
            mainMenu.Items.Add(this.ProtocolMenu);

            this.protocolDefinitionControl.AddMenuItems(this.ProtocolMenu);
            this.protocolGlobalControl.AddMenuItems(this.ProtocolMenu);
            this.protocolMessagesControl.AddMenuItems(this.ProtocolMenu);
        }

        private void tabControlProtocol_Selected(object sender, TabControlEventArgs e)
        {
            this.protocolDefinitionControl.VisibilityChanged(e.TabPage == this.tabPageProtocol);
            this.protocolGlobalControl.VisibilityChanged(e.TabPage == this.tabPageGlobals);
            this.protocolMessagesControl.VisibilityChanged(e.TabPage == this.tabPageMessages);
        }

        public void VisibilityChanged(bool visible)
        {
            if (this.ProtocolMenu != null)
                this.ProtocolMenu.Enabled = visible && (this.SystemDescription != null);
        }
    }
}
