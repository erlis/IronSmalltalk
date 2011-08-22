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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes
{
    public partial class ClassDefinitionControl : UserControl
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


        private ValueHolder<Class> _class;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<Class> ClassHolder
        {
            get
            {
                return this._class;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._class != null)
                {
                    this._class.Changed -= new ValueChangedEventHandler<Class>(this.ClassChanged);
                    this._class.Changing -= new ValueChangingEventHandler<Class>(this.ClassChanging);
                }
                this._class = value;
                this._class.Changing += new ValueChangingEventHandler<Class>(this.ClassChanging);
                this._class.Changed += new ValueChangedEventHandler<Class>(this.ClassChanged);
            }
        }

        public Class Class
        {
            get
            {
                if (this.ClassHolder == null)
                    return null;
                return this.ClassHolder.Value;
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

        public ClassDefinitionControl()
        {
            this.AllGlobalsHolder = new SortedSetHolder<GlobalItem>();
            this.AllGlobalsHolder.CollectionChanged += new CollectionChangedEventHandler(this.AllGlobalsChanged);
            InitializeComponent();
            this.Enabled = false;
            this.comboImplementedInstanceProtocols.Separator = ", ";
            this.comboImplementedClassProtocols.Separator = ", ";
        }

        private void SystemChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            if (this.SystemImplementation == null)
                this.AllGlobalsHolder.Collection = null;
            else
                this.AllGlobalsHolder.Collection = this.SystemImplementation.GlobalItems;
            this.AllGlobalsChanged(sender, e);
        }

        private void AllGlobalsChanged(object sender, EventArgs e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillSuperclassList();
                this.FillPoolsList();
                this.FillProtocolsList();
                this.MarkClean();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillSuperclassList();
                this.FillPoolsList();
                this.FillProtocolsList();
                this.FillView();
                this.MarkClean();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void FillView()
        {
            if (this.Class == null)
            {
                this.Enabled = false;
                this.txtClassName.Text = null;
                this.comboClassSuperclass.Text = null;
                this.comboClassInstanceState.SelectedIndex = -1;
                this.txtClassInstanceVars.Text = null;
                this.txtClassClassVars.Text = null;
                this.txtClassClassInstanceVars.Text = null;
                this.comboClassPools.Text = null;
                this.comboImplementedInstanceProtocols.Text = null;
                this.descriptionControl.Text = null;
                this.textNativeType.Text = null;
                this.sourceEditControl.Source = null;
            }
            else
            {
                this.txtClassName.Text = this.Class.Name;
                this.comboClassSuperclass.Text = this.Class.SuperclassName;
                if (this.Class.InstanceState == InstanceStateEnum.ByteIndexable)
                    this.comboClassInstanceState.SelectedIndex = 1;
                else if (this.Class.InstanceState == InstanceStateEnum.ObjectIndexable)
                    this.comboClassInstanceState.SelectedIndex = 2;
                else if (this.Class.InstanceState == InstanceStateEnum.Native)
                    this.comboClassInstanceState.SelectedIndex = 3;
                else
                    this.comboClassInstanceState.SelectedIndex = 0;
                this.txtClassInstanceVars.Text = String.Join(" ", this.Class.InstanceVariables.OrderBy(n => n));
                this.txtClassClassVars.Text = String.Join(" ", this.Class.ClassVariables.OrderBy(n => n));
                this.txtClassClassInstanceVars.Text = String.Join(" ", this.Class.ClassInstanceVariables.OrderBy(n => n));
                this.comboClassPools.SetValues(this.Class.SharedPools);
                this.comboImplementedInstanceProtocols.SetValues(this.Class.ImplementedInstanceProtocols);
                this.comboImplementedClassProtocols.SetValues(this.Class.ImplementedClassProtocols);
                this.descriptionControl.Html = this.Class.Description;
                this.textNativeType.Text = this.Class.Annotations.GetFirst("ist.runtime.native-class");
                this.sourceEditControl.Source = this.Class.Initializer.Source;
                this.Enabled = true;
            }
        }

        private void FillSuperclassList()
        {
            string old = this.comboClassSuperclass.Text;
            this.comboClassSuperclass.Items.Clear();
            try
            {
                if (this.SystemImplementation == null)
                    return;
                var classes = this.SystemImplementation.Classes;
                if (this.Class != null)
                {
                    classes = classes.Except(this.Class.AllSubclasses());
                    classes = classes.Where(cls => cls != this.Class);
                }
                classes = classes.OrderBy(cls => cls.Name);

                this.comboClassSuperclass.Items.AddRange(classes.Select(cls => cls.Name).ToArray());
            }
            finally
            {
                this.comboClassSuperclass.Text = old;
            }
        }

        private void FillPoolsList()
        {
            if (this.SystemImplementation == null)
                this.comboClassPools.SetChoices(new string[0]);
            else
                this.comboClassPools.SetChoices(this.SystemImplementation.Pools.Select(p => p.Name));
        }

        private void FillProtocolsList()
        {
            IEnumerable<string> protocols = new string[0];
            if (this.SystemImplementation != null)
                protocols = this.SystemImplementation.SmalltalkSystem.SystemDescription.Protocols.Select(p => p.Name);

            this.comboImplementedClassProtocols.SetChoices(protocols);
            this.comboImplementedInstanceProtocols.SetChoices(protocols);
        }

        #region Saving

        void ClassChanging(object sender, ValueChangingEventArgs<Class> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.txtClassName.Text),
                "Class", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
            //this.AllProtocolsChanged(this, e);
            { }
            else
                e.Cancel = true;
        }

        public bool Save()
        {
            if (this.SystemImplementation == null)
                return false;

            string[] msgs = new string[] {
                this.ValidateName(),
                this.ValidateSuperclass(),
                this.ValidateInstanceVariables(),
                this.ValidateClassVariables(),
                this.ValidateClassInstanceVariables(),
                this.ValidatePools() };
            foreach (string msg in msgs)
            {
                if (msg != null)
                {
                    MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            this.Class.Name = this.txtClassName.Text.Trim();
            this.Class.SuperclassName = this.comboClassSuperclass.Text.Trim();
            this.Class.Description = this.descriptionControl.Html;
            this.Class.Initializer.Source = this.sourceEditControl.Source;
            if (this.comboClassInstanceState.SelectedIndex == 1)
                this.Class.InstanceState = InstanceStateEnum.ByteIndexable;
            else if (this.comboClassInstanceState.SelectedIndex == 2)
                this.Class.InstanceState = InstanceStateEnum.ObjectIndexable;
            else if (this.comboClassInstanceState.SelectedIndex == 3)
                this.Class.InstanceState = InstanceStateEnum.Native;
            else
                this.Class.InstanceState = InstanceStateEnum.NamedObjectVariables;
            this.SetNames(this.Class.InstanceVariables, this.txtClassInstanceVars.Text);
            this.SetNames(this.Class.ClassVariables, this.txtClassClassVars.Text);
            this.SetNames(this.Class.ClassInstanceVariables, this.txtClassClassInstanceVars.Text);
            this.SetNames(this.Class.SharedPools, this.comboClassPools.Text);
            this.Class.Annotations.Replace("ist.runtime.native-class", this.textNativeType.Text);

            this.Class.ImplementedInstanceProtocols.Clear();
            if (!String.IsNullOrWhiteSpace(this.comboImplementedInstanceProtocols.Text))
                this.Class.ImplementedInstanceProtocols.UnionWith(this.comboImplementedInstanceProtocols.Text.Split(',').Where(n => !String.IsNullOrWhiteSpace(n)).Select(n => n.Trim()));
            this.Class.ImplementedClassProtocols.Clear();
            if (!String.IsNullOrWhiteSpace(this.comboImplementedClassProtocols.Text))
                this.Class.ImplementedClassProtocols.UnionWith(this.comboImplementedClassProtocols.Text.Split(',').Where(n => !String.IsNullOrWhiteSpace(n)).Select(n => n.Trim()));

            var prot = this.Class.Parent.SmalltalkSystem.SystemDescription.Protocols.FirstOrDefault(p => p.StandardGlobals.Any(g => g.Name == this.Class.Name));
            if (prot != null)
                this.Class.DefiningProtocol = prot.Name;

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.SystemImplementation.GlobalItems.Contains(this.Class))
                    this.SystemImplementation.GlobalItems.Add(this.Class);
                //if (this.IsNew)
                //    this.SystemDescription.Protocols.Add(this.Current);
                this.MarkClean();
                this.SystemImplementation.GlobalItems.TriggerChanged();
                this.ClassHolder.TriggerChanged(this.Class, this.Class);
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

        private void SetNames(ISet<string> set, string names)
        {
            if (set == null)
                return;
            set.Clear();
            if (String.IsNullOrWhiteSpace(names))
                return;
            set.UnionWith(names.Split(' ').Where(n => !String.IsNullOrWhiteSpace(n)).Select(n => n.Trim()));
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
            bool ok = (this.Class == null) || (this.ValidateName() == null);
            this.txtClassName.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private void SetSuperclassState()
        {
            bool ok = (this.Class == null) || (this.ValidateSuperclass() == null);
            this.comboClassSuperclass.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        #region UI Events

        private void txtClassName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetNameState();
            this.MarkDirty();
        }

        private void comboClassSuperclass_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.SetSuperclassState();
            this.MarkDirty();
        }

        private void descriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void comboClassInstanceState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void txtClassInstanceVars_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void txtClassClassVars_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void textNativeType_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }


        private void txtClassClassInstanceVars_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void comboClassPools_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void comboImplementedInstanceProtocols_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void comboImplementedClassProtocols_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void sourceEditControl_Load(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        #endregion

        #region Validation

        private string ValidateName()
        {
            if (String.IsNullOrWhiteSpace(this.txtClassName.Text))
                return "Class must have a name";
            if (this.SystemImplementation != null)
            {
                foreach (GlobalItem g in this.SystemImplementation.GlobalItems)
                {
                    if (g != this.Class)
                    {
                        if (g.Name == this.txtClassName.Text)
                            return "Class name must be unuque";
                    }
                }
            }
            return null;
        }

        private string ValidateSuperclass()
        {
            if (this.SystemImplementation == null)
                return null;
            string name = this.comboClassSuperclass.Text;
            if (String.IsNullOrWhiteSpace(name))
                return null;
            if (name == this.txtClassName.Text)
                return "Superclass name cannot be same as class name";
            Class superclass = this.SystemImplementation.Classes.FirstOrDefault(cls => cls.Name == name);
            if (superclass == null)
                return "Superclass with the given name does not exists";
            var subclasses = this.Class.AllSubclasses();
            if (subclasses.Contains(superclass))
                return "Superclass will create a circular reference";
            return null;
        }

        private string ValidateInstanceVariables()
        {
            return null; // TO-DO
        }

        public string ValidateClassVariables()
        {
            return null; // TO-DO
        }

        public string ValidateClassInstanceVariables()
        {
            return null; // TO-DO
        }

        public string ValidatePools()
        {
            return null; // TO-DO
        }

        #endregion




        #endregion
    }
}
