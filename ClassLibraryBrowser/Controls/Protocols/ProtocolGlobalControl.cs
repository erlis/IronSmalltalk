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
using System.Runtime.InteropServices;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols
{
    public partial class ProtocolGlobalControl : UserControl
    {
        public bool Dirty { get; set; }
        private bool Updating;

        #region Value Holders and Accessors

        private ValueHolder<Protocol> _protocol;
        private ValueHolder<Global> _global;

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

        public Protocol Protocol
        {
            get
            {
                if (this.ProtocolHolder == null)
                    return null;
                return this.ProtocolHolder.Value;
            }
        }

        public ValueHolder<Global> GlobalHolder
        {
            get { return this._global; }
        }

        public Global Global
        {
            get
            {
                if (this._global == null)
                    return null;
                return this._global.Value;
            }
        }

        #endregion

        public ProtocolGlobalControl()
        {
            this._global = new ValueHolder<Global>();
            this._global.Changed += new ValueChangedEventHandler<Definitions.Description.Global>(this.GlobalChanged);
            this._global.Changing += new ValueChangingEventHandler<Definitions.Description.Global>(this.GlobalChanging);

            InitializeComponent();
        }

        private void listGlobals_SizeChanged(object sender, EventArgs e)
        {
            this.listGlobals.Columns[0].Width = this.listGlobals.Width - 5;
        }

        private void ProtocolGlobalControl_Load(object sender, EventArgs e)
        {
            this.listGlobals.Columns[0].Width = this.listGlobals.Width - 5;
            this.FillView();
        }

        #region View Updating

        private void ProtocolChanged(object sender, ValueChangedEventArgs<Protocol> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.Enabled = (this.Protocol != null);
                this._global.Value = null;
                this.FillGlobalsList();
                this.SetMenuItemState(this.Visible);
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void FillGlobalsList()
        {
            this.listGlobals.BeginUpdate();
            try
            {
                this.listGlobals.Items.Clear();
                if (this.Protocol == null)
                    return;
                foreach (Global global in this.Protocol.StandardGlobals.OrderBy(g => g.Name))
                {
                    ListViewItem lvi = this.listGlobals.Items.Add(global.Name);
                    lvi.Name = global.Name;
                    lvi.Selected = (global == this.Global);
                    lvi.Tag = global;
                }
            }
            finally
            {
                this.listGlobals.EndUpdate();
            }
        }

        private void GlobalChanged(object sender, ValueChangedEventArgs<Global> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillView();
                this.SetNameState();
                this.MarkClean();
                this.SetMenuItemState(this.Visible);
            }
            finally
            {
                this.Updating = remember;
            }
        }

        public void FillView()
        {
            if (this.Global == null)
            {
                this.txtGlobalName.Text = "";
                this.txtGlobalName.Enabled = false;
                this.descriptionControl.Text = null;
                this.descriptionControl.Enabled = false;
            }
            else
            {
                this.txtGlobalName.Text = this.Global.Name;
                this.txtGlobalName.Enabled = true;
                this.descriptionControl.Html = this.Global.Description;
                this.descriptionControl.Enabled = true;
            }
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

        private void SetNameState()
        {
            bool ok = (this.Global == null) || (this.ValidateName() == null);
            this.txtGlobalName.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private string ValidateName()
        {
            if (String.IsNullOrWhiteSpace(this.txtGlobalName.Text))
                return "Global must have a name";
            if (this.Protocol != null)
            {
                foreach (Global g in this.Protocol.StandardGlobals)
                {
                    if (g != this.Global)
                    {
                        if (g.Name == this.txtGlobalName.Text)
                            return "Global name must be unuque";
                    }
                }
            }
            return null;
        }

        private void txtGlobalName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetNameState();
            this.MarkDirty();
        }

        private void listGlobals_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (e.Item == null)
                return;
            Global gl = e.Item.Tag as Global;
            this._global.Value = gl;
            e.Item = this.listGlobals.Items.Cast<ListViewItem>()
                .FirstOrDefault(i => i.Tag == this._global.Value);
        }

        private void descriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }


        private void GlobalChanging(object sender, ValueChangingEventArgs<Global> e)
        {
            if (e.Cancel)
                return;
            e.Cancel = !this.CheckSave();
        }

        private void ProtocolChanging(object sender, ValueChangingEventArgs<Protocol> e)
        {
            if (e.Cancel)
                return;
            e.Cancel = !this.CheckSave();
        }

        private bool CheckSave()
        {
            if (!this.Dirty)
                return true;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.txtGlobalName.Text),
                "Global", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                return this.Save();
            else if (dlgResult == DialogResult.No)
                return true;
            else
                return false;
        }

        public bool Save()
        {
            if (this.Global == null)
                return false;

            string msg = this.ValidateName();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this.Global.Name = this.txtGlobalName.Text;
            this.Global.Description = this.descriptionControl.Html;
            //this.Global.Initializer = this.richGlobalInitializer.Text;

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.Protocol.StandardGlobals.Contains(this.Global))
                    this.Protocol.StandardGlobals.Add(this.Global);
                this.MarkClean();
                this.GlobalHolder.TriggerChanged(this.Global, this.Global);
                this.ProtocolHolder.TriggerChanged(this.Protocol, this.Protocol);
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

        #endregion

        #region Menu

        private ToolStripMenuItem CreateNewGloballMenuItem;
        private ToolStripMenuItem SaveGloballMenuItem;
        private ToolStripMenuItem DeleteGlobalMenuItem;

        public void AddMenuItems(ToolStripMenuItem parentMenu)
        {
            ToolStripSeparator tsp = new ToolStripSeparator();
            tsp.Name = "ToolStripSeparatorGlobal";
            tsp.MergeIndex = 100;
            tsp.MergeAction = MergeAction.Insert;
            parentMenu.DropDownItems.Add(tsp);

            this.CreateNewGloballMenuItem = new ToolStripMenuItem();
            this.CreateNewGloballMenuItem.Name = "CreateNewGloballMenuItem";
            this.CreateNewGloballMenuItem.Text = "Create New Global";
            this.CreateNewGloballMenuItem.Enabled = false;
            this.CreateNewGloballMenuItem.MergeIndex = 110;
            this.CreateNewGloballMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewGloballMenuItem.Click += new EventHandler(this.CreateNewGloballMenuItem_Click);
            parentMenu.DropDownItems.Add(this.CreateNewGloballMenuItem);

            this.SaveGloballMenuItem = new ToolStripMenuItem();
            this.SaveGloballMenuItem.Name = "SaveGloballMenuItem";
            this.SaveGloballMenuItem.Text = "Save Global";
            this.SaveGloballMenuItem.Enabled = false;
            this.SaveGloballMenuItem.MergeIndex = 120;
            this.SaveGloballMenuItem.MergeAction = MergeAction.Insert;
            this.SaveGloballMenuItem.Click += new EventHandler(this.SaveGloballMenuItem_Click);
            parentMenu.DropDownItems.Add(this.SaveGloballMenuItem);

            this.DeleteGlobalMenuItem = new ToolStripMenuItem();
            this.DeleteGlobalMenuItem.Name = "DeleteGlobalMenuItem";
            this.DeleteGlobalMenuItem.Text = "Delete Global";
            this.DeleteGlobalMenuItem.Enabled = false;
            this.DeleteGlobalMenuItem.MergeIndex = 130;
            this.DeleteGlobalMenuItem.MergeAction = MergeAction.Insert;
            this.DeleteGlobalMenuItem.Click += new EventHandler(this.DeleteGlobalMenuItem_Click);
            parentMenu.DropDownItems.Add(this.DeleteGlobalMenuItem);
        }

        void DeleteGlobalMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Protocol == null)
                return;
            if (this.Global == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete global '{0}'?", this.Global.Name),
                "Delete Global", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.Global.Protocol.StandardGlobals.Remove(this.Global);
            this.GlobalHolder.Value = null;
            this.ProtocolHolder.TriggerChanged(this.Protocol, this.Protocol);
        }

        private void SaveGloballMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        private void CreateNewGloballMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Protocol == null)
                return;
            Global g = new Definitions.Description.Global(this.Protocol);
            g.Name = "";
            if (!this.GlobalHolder.SetValue(g))
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
                if (this.CreateNewGloballMenuItem != null)
                    this.CreateNewGloballMenuItem.Enabled = false;
                if (this.SaveGloballMenuItem != null)
                    this.SaveGloballMenuItem.Enabled = false;
                if (this.DeleteGlobalMenuItem != null)
                    this.DeleteGlobalMenuItem.Enabled = false;
                return;
            }

            if (this.CreateNewGloballMenuItem != null)
                this.CreateNewGloballMenuItem.Enabled = (this.Protocol != null);
            if (this.SaveGloballMenuItem != null)
                this.SaveGloballMenuItem.Enabled = (this.Global != null);
            if (this.DeleteGlobalMenuItem != null)
                this.DeleteGlobalMenuItem.Enabled = (this.Global != null);
        }

        #endregion
    }
}
