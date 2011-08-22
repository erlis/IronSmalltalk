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
    public partial class ProtocolMessagesControl : UserControl
    {
        private bool Updating;

        #region Value Holders and Accessors

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
                this.protocolMessageControl.ProtocolHolder = value;
            }
        }

        public Protocol Protocol
        {
            get
            {
                if (this.ProtocolHolder == null)
                    return null;
                return this.ProtocolHolder.Value;
            }
        }

        private ValueHolder<Definitions.Description.Message> _message;

        [Browsable(false)]
        public ValueHolder<Definitions.Description.Message> MessageHolder
        {
            get
            {
                return this._message;
            }
            set
            {
                if ((this._message == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._message == value)
                    return;
                if (this._message != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._message = value;
                value.Changed += new ValueChangedEventHandler<Definitions.Description.Message>(this.MessageChanged);
            }
        }

        public Definitions.Description.Message Message
        {
            get
            {
                if (this.MessageHolder == null)
                    return null;
                return this.MessageHolder.Value;
            }
        }

        #endregion


        public ProtocolMessagesControl()
        {
            this.MessageHolder = new ValueHolder<Definitions.Description.Message>();
            InitializeComponent();
            this.protocolMessageControl.MessageHolder = this.MessageHolder;
        }

        #region View Updating

        private void ProtocolChanged(object sender, ValueChangedEventArgs<Protocol> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.Enabled = (this.Protocol != null);
                this.FillMessagesList();
                this.SetMenuItemState(this.Visible);
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void MessageChanged(object sender, ValueChangedEventArgs<Definitions.Description.Message> e)
        {
            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                this.listMessages.SelectedItems.Clear();
                foreach (ListViewItem lvi in this.listMessages.Items)
                {
                    if (lvi.Tag == e.NewValue)
                        lvi.Selected = true;
                }
            }
            finally
            {
                this.Updating = false;
            }
            this.SetMenuItemState(this.Visible);
        }

        private void FillMessagesList()
        {
            this.listMessages.BeginUpdate();
            try
            {
                this.listMessages.Items.Clear();
                if (this.Protocol == null)
                    return;
                foreach (Definitions.Description.Message item in this.Protocol.Messages.OrderBy(m => m.Selector))
                {
                    ListViewItem lvi = this.listMessages.Items.Add(item.Selector);
                    lvi.Name = item.Selector;
                    lvi.Tag = item;
                }
            }
            finally
            {
                this.listMessages.EndUpdate();
            }
        }

        private void listMessages_SizeChanged(object sender, EventArgs e)
        {
            this.listMessages.Columns[0].Width = this.listMessages.Width - 5;
        }

        private void ProtocolMessagesControl_Load(object sender, EventArgs e)
        {
            this.listMessages.Columns[0].Width = this.listMessages.Width - 5;
        }

        private void listMessages_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (this.Updating)
                return;
            if (e.Item == null)
                return;
            Definitions.Description.Message m = e.Item.Tag as Definitions.Description.Message;
            this.MessageHolder.Value = m;
            e.Item = this.listMessages.Items.Cast<ListViewItem>()
                .FirstOrDefault(i => i.Tag == this.Message);
        }

        #endregion

        #region Menu

        private ToolStripMenuItem CreateNewMessageMenuItem;
        private ToolStripMenuItem SaveMessageMenuItem;
        private ToolStripMenuItem DeleteMessageMenuItem;

        public void AddMenuItems(ToolStripMenuItem parentMenu)
        {
            ToolStripSeparator tsp = new ToolStripSeparator();
            tsp.Name = "ToolStripSeparatorMessage";
            tsp.MergeIndex = 200;
            tsp.MergeAction = MergeAction.Insert;
            parentMenu.DropDownItems.Add(tsp);

            this.CreateNewMessageMenuItem = new ToolStripMenuItem();
            this.CreateNewMessageMenuItem.Name = "CreateNewMessageMenuItem";
            this.CreateNewMessageMenuItem.Text = "Create New Message";
            this.CreateNewMessageMenuItem.Enabled = false;
            this.CreateNewMessageMenuItem.MergeIndex = 210;
            this.CreateNewMessageMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewMessageMenuItem.Click += new EventHandler(this.CreateNewMessagelMenuItem_Click);
            parentMenu.DropDownItems.Add(this.CreateNewMessageMenuItem);

            this.SaveMessageMenuItem = new ToolStripMenuItem();
            this.SaveMessageMenuItem.Name = "SaveMessageMenuItem";
            this.SaveMessageMenuItem.Text = "Save Message";
            this.SaveMessageMenuItem.Enabled = false;
            this.SaveMessageMenuItem.MergeIndex = 220;
            this.SaveMessageMenuItem.MergeAction = MergeAction.Insert;
            this.SaveMessageMenuItem.Click += new EventHandler(this.SaveMessagelMenuItem_Click);
            parentMenu.DropDownItems.Add(this.SaveMessageMenuItem);

            this.DeleteMessageMenuItem = new ToolStripMenuItem();
            this.DeleteMessageMenuItem.Name = "DeleteMessageMenuItem";
            this.DeleteMessageMenuItem.Text = "Delete Message";
            this.DeleteMessageMenuItem.Enabled = false;
            this.DeleteMessageMenuItem.MergeIndex = 230;
            this.DeleteMessageMenuItem.MergeAction = MergeAction.Insert;
            this.DeleteMessageMenuItem.Click += new EventHandler(this.DeleteMessageMenuItem_Click);
            parentMenu.DropDownItems.Add(this.DeleteMessageMenuItem);
        }

        void DeleteMessageMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Protocol == null)
                return;
            if (this.Message == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete message #{0}?", this.Message.Selector),
                "Delete Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.Message.Protocol.Messages.Remove(this.Message);
            this.MessageHolder.Value = null;
            this.ProtocolHolder.TriggerChanged(this.Protocol, this.Protocol);
        }

        private void SaveMessagelMenuItem_Click(object sender, EventArgs e)
        {
            this.protocolMessageControl.Save();
        }

        private void CreateNewMessagelMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Protocol == null)
                return;
            Definitions.Description.Message m = new Definitions.Description.Message(this.Protocol);
            if (!this.MessageHolder.SetValue(m))
                return;
        }

        public void VisibilityChanged(bool visible)
        {
            this.SetMenuItemState(visible);
        }

        private void SetMenuItemState(bool visible)
        {
            if (!visible)
            {
                if (this.CreateNewMessageMenuItem != null)
                    this.CreateNewMessageMenuItem.Enabled = false;
                if (this.SaveMessageMenuItem != null)
                    this.SaveMessageMenuItem.Enabled = false;
                if (this.DeleteMessageMenuItem != null)
                    this.DeleteMessageMenuItem.Enabled = false;
                return;
            }

            if (this.CreateNewMessageMenuItem != null)
                this.CreateNewMessageMenuItem.Enabled = (this.Protocol != null);
            if (this.SaveMessageMenuItem != null)
                this.SaveMessageMenuItem.Enabled = (this.Message != null);
            if (this.DeleteMessageMenuItem != null)
                this.DeleteMessageMenuItem.Enabled = (this.Message != null);
        }

        #endregion

    }
}
