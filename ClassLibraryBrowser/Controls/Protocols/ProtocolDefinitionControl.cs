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
    public partial class ProtocolDefinitionControl : UserControl
    {
        public bool Dirty { get; set; }
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
                value.Changing += new ValueChangingEventHandler<Definitions.Description.Protocol>(this.ProtocolChanging);
            }
        }

        private ValueHolder<SystemDescription> _systemDescription;

        [Browsable(false)]
        public ValueHolder<SystemDescription> SystemDescriptionHolder
        {
            get
            {
                return this._systemDescription;
            }
            set
            {
                if ((this._systemDescription == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._systemDescription == value)
                    return;
                if (this._systemDescription != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._systemDescription = value;
                value.Changed += new ValueChangedEventHandler<SystemDescription>(this.ClassLibraryChanged);
            }

        }

        private void ClassLibraryChanged(object sender, ValueChangedEventArgs<SystemDescription> e)
        {
            this.SetMenuItemState(this.Visible);
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

        private Protocol Protocol
        {
            get
            {
                if (this.ProtocolHolder == null)
                    return null;
                return this.ProtocolHolder.Value;
            }
        }

        private IEnumerable<Protocol> Protocols
        {
            get
            {
                if (this.AllProtocolsHolder == null)
                    return null;
                return this.AllProtocolsHolder.Collection;
            }
        }

        #endregion


        public ProtocolDefinitionControl()
        {
            InitializeComponent();
        }

        #region View Updating

        private void ProtocolChanged(object sender, ValueChangedEventArgs<Protocol> e)
        {
            this.AllProtocolsChanged(sender, e);
            this.SetMenuItemState(this.Visible);
        }

        private void AllProtocolsChanged(object sender, EventArgs e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillView();
                this.SetNameState();
                this.SetConformsToState();
                this.MarkClean();
                this.SetMenuItemState(this.Visible);
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void FillView()
        {
            this.listProtocolConformsTo.BeginUpdate();
            this.listProtocolConformsTo.Items.Clear();
            if (this.Protocols != null)
            {
                foreach (Protocol p in this.Protocols.OrderBy(p => p.Name))
                    this.listProtocolConformsTo.Items.Add(p.Name, p.Name, 0).Tag = p;
            }
            this.listProtocolConformsTo.EndUpdate();

            if (this.Protocol == null)
            {
                this.Enabled = false;
                this.txtProtocolDocId.Text = null;
                this.txtProtocolName.Text = null;
                this.descriptionControl.Text = null;
                return;
            }

            this.Enabled = true;
            this.txtProtocolName.Text = this.Protocol.Name;
            this.txtProtocolDocId.Text = this.Protocol.DocumentationId;
            this.checkBoxProtocolAbstract.Checked = this.Protocol.IsAbstract;
            this.descriptionControl.Html = this.Protocol.Description;
            foreach (string pn in this.Protocol.ConformsTo)
                this.listProtocolConformsTo.Items.Cast<ListViewItem>().Where(lvi => lvi.Name == pn).Select(lvi => lvi.Checked = true).Count();
        }

        private void SetNameState()
        {
            bool ok = (this.Protocol == null) || (this.ValidateName() == null);
            this.txtProtocolName.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private void SetConformsToState()
        {
            bool ok = (this.Protocol == null) || (this.ValidateConformsTo() == null);
            this.listProtocolConformsTo.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        #endregion

        #region Editing and Saving

        private void MarkDirty()
        {
            this.Dirty = true;
        }

        private void MarkClean()
        {
            this.Dirty = false;
        }

        private string ValidateName()
        {
            if (String.IsNullOrWhiteSpace(this.txtProtocolName.Text))
                return "Protocol must have a name";
            if (this.Protocols != null)
            {
                foreach (Protocol p in this.Protocols)
                {
                    if (p != this.Protocol)
                    {
                        if (p.Name == this.txtProtocolName.Text)
                            return "Protocol name must be unuque";
                    }
                }
            }
            if (this.checkBoxProtocolAbstract.Checked)
            {
                if (Char.IsUpper(this.txtProtocolName.Text[0]))
                    this.txtProtocolName.Text = Char.ToLower(this.txtProtocolName.Text[0]).ToString() 
                        + this.txtProtocolName.Text.Substring(1);
            }
            else
            {
                if (Char.IsLower(this.txtProtocolName.Text[0]))
                    this.txtProtocolName.Text = Char.ToUpper(this.txtProtocolName.Text[0]).ToString() 
                        + this.txtProtocolName.Text.Substring(1);
            }
            return null;
        }

        private string ValidateConformsTo()
        {
            if (this.Protocols == null)
                return null;
            Dictionary<string, Protocol> map = new Dictionary<string, Protocol>();
            foreach (Protocol p in this.Protocols)
                map.Add(p.Name, p);

            foreach (ListViewItem lvi in this.listProtocolConformsTo.CheckedItems)
            {
                List<Protocol> recursionSet = new List<Protocol>();
                recursionSet.Add(this.Protocol);
                if (this.HasConformsToCircularReference(map[lvi.Name], recursionSet, map))
                    return "Conforms-to contains a circular reference.";
            }
            return null;
        }

        private bool HasConformsToCircularReference(Protocol p, List<Protocol> recursionSet, Dictionary<string, Protocol> map)
        {
            if (p == this.Protocol)
                return true;
            if (recursionSet.Contains(p))
                return false;
            recursionSet.Add(p);
            foreach (string name in p.ConformsTo)
            {
                if (name != "ANY")
                {
                    if (this.HasConformsToCircularReference(map[name], recursionSet, map))
                        return true;
                }
            }
            return false;
        }

        private void txtProtocolName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetNameState();
            this.MarkDirty();
        }

        private void txtProtocolDocId_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void checkBoxProtocolAbstract_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void listProtocolConformsTo_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (this.Updating)
                return;
            this.SetConformsToState();
            this.MarkDirty();
        }

        private void descriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void ProtocolChanging(object sender, ValueChangingEventArgs<Protocol> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.txtProtocolName.Text),
                "Protocol", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
                this.AllProtocolsChanged(this, e);
            else
                e.Cancel = true;
        }

        public bool Save()
        {
            if (this.Protocols == null)
                return false;

            string msg = this.ValidateName();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            msg = this.ValidateConformsTo();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this.Protocol.Name = this.txtProtocolName.Text;
            this.Protocol.Description = this.descriptionControl.Html;
            this.Protocol.DocumentationId = this.txtProtocolDocId.Text;
            this.Protocol.IsAbstract = this.checkBoxProtocolAbstract.Checked;
            this.Protocol.ConformsTo.Clear();
            foreach (ListViewItem lvi in this.listProtocolConformsTo.CheckedItems)
                this.Protocol.ConformsTo.Add(lvi.Name);

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.AllProtocolsHolder.Collection.Contains(this.Protocol))
                    this.AllProtocolsHolder.Collection.Add(this.Protocol);
                //if (this.IsNew)
                //    this.SystemDescription.Protocols.Add(this.Current);
                this.MarkClean();
                this.ProtocolHolder.TriggerChanged(this.Protocol, this.Protocol);
                this.AllProtocolsHolder.Collection.TriggerChanged(); 
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

        #endregion

        #region Menu

        private ToolStripMenuItem CreateNewProtocolMenuItem;
        private ToolStripMenuItem SaveProtocolMenuItem;
        private ToolStripMenuItem DeleteProtocolMenuItem;
        public void AddMenuItems(ToolStripMenuItem parentMenu)
        {
            this.CreateNewProtocolMenuItem = new ToolStripMenuItem();
            this.CreateNewProtocolMenuItem.Name = "CreateNewProtocolMenuItem";
            this.CreateNewProtocolMenuItem.Text = "Create New Protocol";
            this.CreateNewProtocolMenuItem.Enabled = false;
            this.CreateNewProtocolMenuItem.MergeIndex = 10;
            this.CreateNewProtocolMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewProtocolMenuItem.Click += new EventHandler(this.CreateNewProtocolMenuItem_Click);
            parentMenu.DropDownItems.Add(this.CreateNewProtocolMenuItem);

            this.SaveProtocolMenuItem = new ToolStripMenuItem();
            this.SaveProtocolMenuItem.Name = "SaveProtocolMenuItem";
            this.SaveProtocolMenuItem.Text = "Save Protocol";
            this.SaveProtocolMenuItem.Enabled = false;
            this.SaveProtocolMenuItem.MergeIndex = 20;
            this.SaveProtocolMenuItem.MergeAction = MergeAction.Insert;
            this.SaveProtocolMenuItem.Click += new EventHandler(this.SaveProtocolMenuItem_Click);
            parentMenu.DropDownItems.Add(this.SaveProtocolMenuItem);

            this.DeleteProtocolMenuItem = new ToolStripMenuItem();
            this.DeleteProtocolMenuItem.Name = "DeleteProtocolMenuItem";
            this.DeleteProtocolMenuItem.Text = "Delete Protocol";
            this.DeleteProtocolMenuItem.Enabled = false;
            this.DeleteProtocolMenuItem.MergeIndex = 20;
            this.DeleteProtocolMenuItem.MergeAction = MergeAction.Insert;
            this.DeleteProtocolMenuItem.Click += new EventHandler(this.DeleteProtocolMenuItem_Click);
            parentMenu.DropDownItems.Add(this.DeleteProtocolMenuItem);
        }

        private void DeleteProtocolMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Protocol == null)
                return;
            if (this.AllProtocolsHolder == null)
                return;
            if (this.AllProtocolsHolder.Collection == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete protocol '{0}'?", this.Protocol.Name),
                "Delete Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.AllProtocolsHolder.Collection.Remove(this.Protocol);
            this.ProtocolHolder.Value = null;
        }

        private void SaveProtocolMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void CreateNewProtocolMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SystemDescriptionHolder == null)
                return;
            SystemDescription sd = this.SystemDescriptionHolder.Value;
            if (sd == null)
                return;
            Protocol p = new Protocol(sd);
            p.Name = "";
            if (!this.ProtocolHolder.SetValue(p))
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
                if (this.CreateNewProtocolMenuItem != null)
                    this.CreateNewProtocolMenuItem.Enabled = false;
                if (this.SaveProtocolMenuItem != null)
                    this.SaveProtocolMenuItem.Enabled = false;
                if (this.DeleteProtocolMenuItem != null)
                    this.DeleteProtocolMenuItem.Enabled = false;
                return;
            }

            if (this.CreateNewProtocolMenuItem != null)
                this.CreateNewProtocolMenuItem.Enabled = (this.SystemDescriptionHolder != null) && (this.SystemDescriptionHolder.Value != null);
            if (this.SaveProtocolMenuItem != null)
                this.SaveProtocolMenuItem.Enabled = (this.Protocol != null);
            if (this.DeleteProtocolMenuItem != null)
                this.DeleteProtocolMenuItem.Enabled = (this.Protocol != null);
        }

        #endregion

    }
}
