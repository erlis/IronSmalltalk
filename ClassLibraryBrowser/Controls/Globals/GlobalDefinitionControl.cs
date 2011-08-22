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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Globals
{
    public partial class GlobalDefinitionControl : UserControl
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
                    this._system.Changed -= new ValueChangedEventHandler<SystemImplementation>(this.SystemChanged);
                this._system = value;
                if (this._system != null)
                    this._system.Changed += new ValueChangedEventHandler<SystemImplementation>(this.SystemChanged);
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


        private ValueHolder<Global> _global;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<Global> GlobalHolder
        {
            get
            {
                return this._global;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._global != null)
                {
                    this._global.Changed -= new ValueChangedEventHandler<Global>(this.GlobalChanged);
                    this._global.Changing -= new ValueChangingEventHandler<Global>(this.GlobalChanging);
                }
                this._global = value;
                this._global.Changing += new ValueChangingEventHandler<Global>(this.GlobalChanging);
                this._global.Changed += new ValueChangedEventHandler<Global>(this.GlobalChanged);
            }
        }

        void _global_Changing(object sender, ValueChangingEventArgs<Global> e)
        {
            throw new NotImplementedException();
        }

        public Global Global
        {
            get
            {
                if (this.GlobalHolder == null)
                    return null;
                return this.GlobalHolder.Value;
            }
        }

        private readonly SortedSetHolder<GlobalItem> AllGlobalsHolder;

        public NotifyingSortedSet<GlobalItem> GlobalItems
        {
            get
            {
                if (this.AllGlobalsHolder == null)
                    return null;
                return this.AllGlobalsHolder.Collection;
            }
        }

        public GlobalDefinitionControl()
        {
            this.AllGlobalsHolder = new SortedSetHolder<GlobalItem>();
            this.AllGlobalsHolder.CollectionChanged += new CollectionChangedEventHandler(this.AllGlobalsChanged);
            InitializeComponent();
            this.Enabled = false;
            this.comboImplementedProtocols.Separator = ", ";
        }

        private void SystemChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            if (this.SystemImplementation == null)
                this.AllGlobalsHolder.Collection = null;
            else
                this.AllGlobalsHolder.Collection = this.SystemImplementation.GlobalItems;
            this.AllGlobalsChanged(sender, e);
        }

        #region Fill

        private void AllGlobalsChanged(object sender, EventArgs e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillProtocolsList();
                this.MarkClean();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void GlobalChanged(object sender, ValueChangedEventArgs<Global> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillProtocolsList();
                this.FillView();
                this.MarkClean();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void SetNameState()
        {
            bool ok = (this.Global == null) || (this.ValidateName() == null);
            this.txtName.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private void FillProtocolsList()
        {
            IEnumerable<string> protocols = new string[0];
            if (this.SystemImplementation != null)
                protocols = this.SystemImplementation.SmalltalkSystem.SystemDescription.Protocols.Select(p => p.Name);

            this.comboImplementedProtocols.SetChoices(protocols);
        }

        private void FillView()
        {
            if (this.Global == null)
            {
                this.Enabled = false;
                this.txtName.Text = null;
                this.comboImplementedProtocols.Text = null;
                this.comboType.SelectedIndex = -1;
                this.descriptionControl.Text = null;
                this.sourceEditControl.Source = null;
            }
            else
            {
                this.txtName.Text = this.Global.Name;
                this.comboImplementedProtocols.SetValues(this.Global.ImplementedProtocols);
                if (this.Global.GlobalType == GlobalTypeEnum.Constant)
                    this.comboType.SelectedIndex = 1;
                else
                    this.comboType.SelectedIndex = 0;
                this.descriptionControl.Html = this.Global.Description;
                this.sourceEditControl.Source = this.Global.Initializer.Source;
                this.Enabled = true;
            }
        }

        #endregion

        #region Save

        void GlobalChanging(object sender, ValueChangingEventArgs<Global> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.txtName.Text),
                "Global", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
            //this.AllProtocolsChanged(this, e);
            { }
            else
                e.Cancel = true;
        }

        private void MarkDirty()
        {
            this.Dirty = true;
        }

        private void MarkClean()
        {
            this.Dirty = false;
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
            

            this.Global.Name = this.txtName.Text.Trim();
            this.Global.Description = this.descriptionControl.Html;
            if (this.comboType.SelectedIndex == 1)
                this.Global.GlobalType = GlobalTypeEnum.Constant;
            else 
                this.Global.GlobalType = GlobalTypeEnum.Variable;
            this.Global.Initializer.Source = this.sourceEditControl.Source;

            this.Global.ImplementedProtocols.Clear();
            if (!String.IsNullOrWhiteSpace(this.comboImplementedProtocols.Text))
                this.Global.ImplementedProtocols.UnionWith(this.comboImplementedProtocols.Text.Split(',').Where(n => !String.IsNullOrWhiteSpace(n)).Select(n => n.Trim()));

            var prot = this.Global.Parent.SmalltalkSystem.SystemDescription.Protocols.FirstOrDefault(p => p.StandardGlobals.Any(g => g.Name == this.Global.Name));
            if (prot != null)
                this.Global.DefiningProtocol = prot.Name;

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.SystemImplementation.GlobalItems.Contains(this.Global))
                    this.SystemImplementation.GlobalItems.Add(this.Global);
                this.MarkClean();
                this.SystemImplementation.GlobalItems.TriggerChanged();
                this.GlobalHolder.TriggerChanged(this.Global, this.Global);
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetNameState();
            this.MarkDirty();
        }

        private void comboImplementedProtocols_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void descriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void sourceEditControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        #region Validation

        private string ValidateName()
        {
            if (String.IsNullOrWhiteSpace(this.txtName.Text))
                return "Global must have a name";
            if (this.SystemImplementation != null)
            {
                foreach (GlobalItem g in this.SystemImplementation.GlobalItems)
                {
                    if (g != this.Global)
                    {
                        if (g.Name == this.txtName.Text)
                            return "Global name must be unuque";
                    }
                }
            }
            return null;
        }

        #endregion

        #endregion
    }
}
