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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools
{

    public partial class PoolDefinitionControl : UserControl
    {
        public bool Dirty { get; set; }
        private bool Updating = false;

        private ValueHolder<SystemImplementation> _system;

        [Browsable(false)]
        public ValueHolder<SystemImplementation> SystemImplementationHolder
        {
            get
            {
                return this._system;
            }
            set
            {
                if (this._system != null)
                    this._system.Changed -= new ValueChangedEventHandler<SystemImplementation>(this.SystemImplementationChanged);
                this._system = value;
                if (this._system != null)
                    this._system.Changed += new ValueChangedEventHandler<SystemImplementation>(this.SystemImplementationChanged);
            }
        }

        public SystemImplementation SystemImplementation
        {
            get
            {
                if (this.SystemImplementationHolder == null)
                    return null;
                return this.SystemImplementationHolder.Value;
            }
        }

        private ValueHolder<Pool> _pool;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<Pool> PoolHolder
        {
            get
            {
                return this._pool;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._pool != null)
                {
                    this._pool.Changing -= new ValueChangingEventHandler<Definitions.Implementation.Pool>(this.PoolChanging);
                    this._pool.Changed -= new ValueChangedEventHandler<Pool>(this.PoolChanged);
                }
                this._pool = value;
                this._pool.Changing += new ValueChangingEventHandler<Definitions.Implementation.Pool>(this.PoolChanging);
                this._pool.Changed += new ValueChangedEventHandler<Pool>(this.PoolChanged);
            }
        }

        public Pool Pool
        {
            get
            {
                if (this.PoolHolder == null)
                    return null;
                return this.PoolHolder.Value;
            }
        }

        public PoolDefinitionControl()
        {
            InitializeComponent();
        }

        private void SystemImplementationChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillList();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void PoolChanged(object sender, ValueChangedEventArgs<Pool> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillDefinitionView();
                this.SetSelection();
                this.MarkClean();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void PoolChanging(object sender, ValueChangingEventArgs<Pool> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.txtName.Text),
                "Pool", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
            //this.AllProtocolsChanged(this, e);
            { }
            else
                e.Cancel = true;
        }

        private void FillList()
        {
            this.listPools.Items.Clear();
            if (this.SystemImplementation == null)
                return;

            foreach (Pool pool in this.SystemImplementation.Pools.OrderBy(p => p.Name))
            {
                ListViewItem lvi = this.listPools.Items.Add(pool.Name, "enum");
                lvi.Tag = pool;
                lvi.Selected = (pool == this.Pool);
            }
        }

        private void SetSelection()
        {
            foreach (ListViewItem lvi in this.listPools.Items)
                lvi.Selected = (lvi.Tag == this.Pool);
        }

        private void FillDefinitionView()
        {
            if (this.Pool == null)
            {
                this.txtName.Text = null;
                this.txtName.Enabled = false;
                this.descriptionControl.Text = null;
                this.descriptionControl.Enabled = false;
            }
            else
            {
                this.txtName.Text = this.Pool.Name;
                this.txtName.Enabled = true;
                this.descriptionControl.Html = this.Pool.Description;
                this.descriptionControl.Enabled = true;
            }
        }

        public bool Save()
        {
            if (this.SystemImplementation == null)
                return false;

            string msg = this.ValidateName();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this.Pool.Name = this.txtName.Text.Trim();
            this.Pool.Description = this.descriptionControl.Html;

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.SystemImplementation.GlobalItems.Contains(this.Pool))
                    this.SystemImplementation.GlobalItems.Add(this.Pool);
                this.MarkClean();
                this.SystemImplementation.GlobalItems.TriggerChanged();
                this.PoolHolder.TriggerChanged(this.Pool, this.Pool);
                this.FillList();
                this.SetSelection();
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

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
            bool ok = (this.Pool == null) || (this.ValidateName() == null);
            this.txtName.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private string ValidateName()
        {
            if (String.IsNullOrWhiteSpace(this.txtName.Text))
                return "Pool must have a name";
            if (this.SystemImplementation != null)
            {
                foreach (GlobalItem g in this.SystemImplementation.GlobalItems)
                {
                    if (g != this.Pool)
                    {
                        if (g.Name == this.txtName.Text)
                            return "Pool name must be unuque";
                    }
                }
            }
            return null;
        }

        #region UI Events

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetNameState();
            this.MarkDirty();
        }

        private void descriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void listPools_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (e.Item == null)
                return;
            Pool pool = e.Item.Tag as Pool;
            this._pool.Value = pool;
            e.Item = this.listPools.Items.Cast<ListViewItem>()
                .FirstOrDefault(i => i.Tag == this._pool.Value);
        }

        #endregion

        public void SetPrimaryFocus()
        {
            this.txtName.Focus();
        }
    }
}
