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
    public partial class PoolValuesControl : UserControl
    {
        public bool Dirty { get; set; }
        private bool Updating = false;

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
                    this._pool.Changed -= new ValueChangedEventHandler<Pool>(this.PoolChanged);
                    this._pool.Changing -= new ValueChangingEventHandler<Definitions.Implementation.Pool>(this.PoolChanging);
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

        private ValueHolder<PoolValue> _poolValue;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<PoolValue> PoolValueHolder
        {
            get
            {
                return this._poolValue;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._poolValue != null)
                {
                    this._poolValue.Changing -= new ValueChangingEventHandler<Definitions.Implementation.PoolValue>(this.PoolValueChanging);
                    this._poolValue.Changed -= new ValueChangedEventHandler<PoolValue>(this.PoolValueChanged);
                }
                this._poolValue = value;
                this._poolValue.Changing += new ValueChangingEventHandler<Definitions.Implementation.PoolValue>(this.PoolValueChanging);
                this._poolValue.Changed += new ValueChangedEventHandler<PoolValue>(this.PoolValueChanged);
            }
        }

        public PoolValue PoolValue
        {
            get
            {
                if (this.PoolValueHolder == null)
                    return null;
                return this.PoolValueHolder.Value;
            }
        }

        public PoolValuesControl()
        {
            InitializeComponent();
        }

        private void PoolChanged(object sender, ValueChangedEventArgs<Pool> e)
        {
            this.Enabled = (this.Pool != null);
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillVariablesListView();
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
                String.Format("Do you want to save changes to {0}?", this.txtPoolVarName.Text),
                "Pool Value", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
            //this.AllProtocolsChanged(this, e);
            { }
            else
                e.Cancel = true;
        }

        private void PoolValueChanging(object sender, ValueChangingEventArgs<PoolValue> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.txtPoolVarName.Text),
                "Pool Value", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
            //this.AllProtocolsChanged(this, e);
            { }
            else
                e.Cancel = true;
        }

        private void PoolValueChanged(object sender, ValueChangedEventArgs<PoolValue> e)
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

        private void FillVariablesListView()
        {
            this.listPoolVars.Items.Clear();
            if (this.Pool == null)
                return;

            foreach (PoolValue value in this.Pool.Values.OrderBy(v => v.Name))
            {
                ListViewItem lvi = this.listPoolVars.Items.Add(value.Name, "field");
                lvi.SubItems.Add((value.PoolValueType == PoolValueTypeEnum.Constant) ? "Constant" : "Variable");
                lvi.Tag = value;
                lvi.Selected = (value == this.PoolValue);
            }
        }

        private void SetSelection()
        {
            foreach (ListViewItem lvi in this.listPoolVars.Items)
                lvi.Selected = (lvi.Tag == this.PoolValue);
        }

        private void FillDefinitionView()
        {
            if (this.PoolValue == null)
            {
                this.txtPoolVarName.Text = null;
                this.txtPoolVarName.Enabled = false;
                this.txtSourceCode.Text = null;
                this.txtSourceCode.Enabled = false;
                this.comboType.SelectedIndex = -1;
                this.comboType.Enabled = false;
            }
            else
            {
                this.txtPoolVarName.Text = this.PoolValue.Name;
                this.txtPoolVarName.Enabled = true;
                this.txtSourceCode.Text = this.PoolValue.Initializer.Source;
                this.txtSourceCode.Enabled = true;
                this.comboType.SelectedIndex = (this.PoolValue.PoolValueType == PoolValueTypeEnum.Constant) ? 0 : 1;
                this.comboType.Enabled = true;
            }
        }

        public bool Save()
        {
            if (this.Pool == null)
                return false;

            string msg = this.ValidateName();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            this.PoolValue.Name = this.txtPoolVarName.Text.Trim();
            if (this.comboType.SelectedIndex == 0)
                this.PoolValue.PoolValueType = PoolValueTypeEnum.Constant;
            else
                this.PoolValue.PoolValueType = PoolValueTypeEnum.Variable;
            this.PoolValue.Initializer.Source = this.txtSourceCode.Text;

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.Pool.Values.Contains(this.PoolValue))
                    this.Pool.Values.Add(this.PoolValue);
                this.MarkClean();
                this.PoolValueHolder.TriggerChanged(this.PoolValue, this.PoolValue);
                this.FillVariablesListView();
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
            this.txtPoolVarName.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private string ValidateName()
        {
            if (String.IsNullOrWhiteSpace(this.txtPoolVarName.Text))
                return "Pool value must have a name";
            if (this.Pool != null)
            {
                foreach (PoolValue v in this.Pool.Values)
                {
                    if (v != this.PoolValue)
                    {
                        if (v.Name == this.txtPoolVarName.Text)
                            return "Pool value name must be unuque";
                    }
                }
            }
            return null;
        }

        private void txtPoolVarName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetNameState();
            this.MarkDirty();
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void listPoolValues_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (e.Item == null)
                return;
            PoolValue item = e.Item.Tag as PoolValue;
            this._poolValue.Value = item;
            e.Item = this.listPoolVars.Items.Cast<ListViewItem>()
                .FirstOrDefault(i => i.Tag == this._poolValue.Value);
        }

        private void txtSourceCode_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void txtSourceCode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Alt) && (e.KeyCode == Keys.S))
            {
                if (this.Save())
                    e.SuppressKeyPress = true;
            }
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.A))
            {
                this.txtSourceCode.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        public void SetPrimaryFocus()
        {
            this.txtPoolVarName.Focus();
        }
    }
}
