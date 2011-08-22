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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Globals
{
    public partial class GlobalsControl : UserControl, IMainFormMenuConsumer
    {
        private bool Updating = false;

        #region Value Holders and Accessors

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
                this.globalDefinitionControl.SystemImplementationHolder = value;
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
        public ValueHolder<Global> GlobalHolder
        {
            get
            {
                return this._global;
            }
            set
            {
                if (value == null)
                    return;
                if (this._global != null)
                    this._global.Changed -= new ValueChangedEventHandler<Global>(this.GlobalChanged);
                this._global = value;
                if (this._global != null)
                    this._global.Changed += new ValueChangedEventHandler<Global>(this.GlobalChanged);
                this.globalDefinitionControl.GlobalHolder = value;
            }
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

        #endregion

        public GlobalsControl()
        {
            this.AllGlobalsHolder = new SortedSetHolder<GlobalItem>();
            this.AllGlobalsHolder.CollectionChanged += new CollectionChangedEventHandler(this.AllGlobalsChanged);
            InitializeComponent();
            this.GlobalHolder = new ValueHolder<Definitions.Implementation.Global>();
            this.Enabled = false;
        }

        #region View Updating

        private void SystemChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            if (this.SystemImplementation == null)
                this.AllGlobalsHolder.Collection = null;
            else
                this.AllGlobalsHolder.Collection = this.SystemImplementation.GlobalItems;
            this.AllGlobalsChanged(sender, e);

            this.GlobalHolder.Value = null;
            if (this.GlobalsMenu != null)
                this.GlobalsMenu.Enabled = (this.SystemImplementation != null) & this.Visible;
            if (this.CreateNewGlobalMenuItem != null)
                this.CreateNewGlobalMenuItem.Enabled = (this.SystemImplementation != null);
            if (this.CreateMissingGlobalMenuItem != null)
                this.CreateMissingGlobalMenuItem.Enabled = (this.SystemImplementation != null);
        }

        private void AllGlobalsChanged(object sender, EventArgs e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.Enabled = (this.GlobalItems != null);
                this.FillGlobalList();
                this.SetMenuItemState(this.Visible);
            }
            finally
            {
                this.Updating = remember;
            }
            // This must be outside the Updating block
            this.SetSelection();
        }

        private void GlobalChanged(object sender, ValueChangedEventArgs<Global> e)
        {
            this.SetSelection();
            this.SetMenuItemState(this.Visible);
        }

        private void FillGlobalList()
        {
            this.listGlobals.BeginUpdate();
            try
            {

                HashSet<string> expanded = new HashSet<string>();
                this.listGlobals.Items.Clear();
                if (this.GlobalItems == null)
                    return;
                var globals = this.GlobalItems.Where(e => e is Global).Cast<Global>();
                foreach (var global in globals)
                {
                    ListViewItem lvi = this.listGlobals.Items.Add(global.Name, "structure");
                    lvi.Tag = global;
                    lvi.SubItems.Add((global.GlobalType == GlobalTypeEnum.Constant) ? "Constrant" : "Variable");
                }
            }
            finally
            {
                this.listGlobals.EndUpdate();
            }
            this.SetSelection();
        }

        private void SetSelection()
        {
            foreach(ListViewItem lvi in this.listGlobals.Items)
                lvi.Selected = (lvi.Tag == this.Global);
        }

        #endregion

        #region Menu

        private ToolStripMenuItem GlobalsMenu;
        private ToolStripMenuItem CreateNewGlobalMenuItem;
        private ToolStripMenuItem SaveGlobalMenuItem;
        private ToolStripMenuItem DeleteGlobalMenuItem;
        private ToolStripMenuItem CreateMissingGlobalMenuItem;

        private void SetMenuItemState(bool visible)
        {
            if (this.SaveGlobalMenuItem != null)
                this.SaveGlobalMenuItem.Enabled = (this.Global != null);
            if (this.DeleteGlobalMenuItem != null)
                this.DeleteGlobalMenuItem.Enabled = (this.Global != null);
        }

        void IMainFormMenuConsumer.AddMainMenuItems(MenuStrip mainMenu)
        {
            this.GlobalsMenu = new ToolStripMenuItem();
            this.GlobalsMenu.MergeIndex = 10;
            this.GlobalsMenu.MergeAction = MergeAction.Insert;
            this.GlobalsMenu.Name = "GlobalsMenu";
            this.GlobalsMenu.Text = "Global";
            this.GlobalsMenu.Enabled = false;
            mainMenu.Items.Add(this.GlobalsMenu);

            this.CreateNewGlobalMenuItem = new ToolStripMenuItem();
            this.CreateNewGlobalMenuItem.Name = "CreateNewGlobalMenuItem";
            this.CreateNewGlobalMenuItem.Text = "Create New Global";
            this.CreateNewGlobalMenuItem.Enabled = false;
            this.CreateNewGlobalMenuItem.MergeIndex = 10;
            this.CreateNewGlobalMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewGlobalMenuItem.Click += new EventHandler(this.CreateNewGlobalMenuItem_Click);
            this.GlobalsMenu.DropDownItems.Add(this.CreateNewGlobalMenuItem);

            this.SaveGlobalMenuItem = new ToolStripMenuItem();
            this.SaveGlobalMenuItem.Name = "SaveGlobalMenuItem";
            this.SaveGlobalMenuItem.Text = "Save Global";
            this.SaveGlobalMenuItem.Enabled = false;
            this.SaveGlobalMenuItem.MergeIndex = 20;
            this.SaveGlobalMenuItem.MergeAction = MergeAction.Insert;
            this.SaveGlobalMenuItem.Click += new EventHandler(this.SaveGlobalMenuItem_Click);
            this.GlobalsMenu.DropDownItems.Add(this.SaveGlobalMenuItem);

            this.DeleteGlobalMenuItem = new ToolStripMenuItem();
            this.DeleteGlobalMenuItem.Name = "DeleteGlobalMenuItem";
            this.DeleteGlobalMenuItem.Text = "Delete Global";
            this.DeleteGlobalMenuItem.Enabled = false;
            this.DeleteGlobalMenuItem.MergeIndex = 30;
            this.DeleteGlobalMenuItem.MergeAction = MergeAction.Insert;
            this.DeleteGlobalMenuItem.Click += new EventHandler(this.DeleteGlobalMenuItem_Click);
            this.GlobalsMenu.DropDownItems.Add(this.DeleteGlobalMenuItem);

            ToolStripSeparator tsp = new ToolStripSeparator();
            tsp.Name = "ToolStripSeparatorClasses";
            tsp.MergeIndex = 100;
            tsp.MergeAction = MergeAction.Insert;
            this.GlobalsMenu.DropDownItems.Add(tsp);

            this.CreateMissingGlobalMenuItem = new ToolStripMenuItem();
            this.CreateMissingGlobalMenuItem.Name = "CreateMissingGlobalMenuItem";
            this.CreateMissingGlobalMenuItem.Text = "Create Protocol Defined Globals ...";
            this.CreateMissingGlobalMenuItem.Enabled = false;
            this.CreateMissingGlobalMenuItem.MergeIndex = 30;
            this.CreateMissingGlobalMenuItem.MergeAction = MergeAction.Insert;
            this.CreateMissingGlobalMenuItem.Click += new EventHandler(this.CreateMissingGlobalMenuItem_Click);
            this.GlobalsMenu.DropDownItems.Add(this.CreateMissingGlobalMenuItem);
        }

        private void CreateNewGlobalMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SystemImplementation == null)
                return;
            Global global = new Global(this.SystemImplementation);
            global.Name = "";
            if (!this.GlobalHolder.SetValue(global))
                return;
        }

        private void SaveGlobalMenuItem_Click(object sender, EventArgs e)
        {
            this.globalDefinitionControl.Save();
        }

        private void DeleteGlobalMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Global == null)
                return;
            if (this.SystemImplementation == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete global '{0}'?", this.Global.Name),
                "Delete Global", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.SystemImplementation.GlobalItems.Remove(this.Global);
            //this.SystemImplementationHolder.TriggerChanged(this.SystemImplementation, this.SystemImplementation);
            this.GlobalHolder.Value = null;
        }

        private void CreateMissingGlobalMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SystemImplementation == null)
                return;

            List<Definitions.Description.Global> globals = new List<Definitions.Description.Global>();
            foreach (var prot in this.SystemImplementation.SmalltalkSystem.SystemDescription.Protocols)
                globals.AddRange(prot.StandardGlobals.Where(g => (g.Definition == null) || (g.Definition is Definitions.Description.GlobalVariable) || (g.Definition is Definitions.Description.GlobalConstant)));

            SelectDialog<Definitions.Description.Global> dlg = new SelectDialog<Definitions.Description.Global>();
            dlg.CheckBoxes = true;
            dlg.Items = globals.Where(g => !this.SystemImplementation.GlobalItems.Any(x => x.Name == g.Name)).OrderBy(g => g.Name);
            dlg.Columns = new SelectDialogColumn<Definitions.Description.Global>[] {
                new SelectDialogColumn<Definitions.Description.Global>("Global", 200, (g => g.Name)),
                new SelectDialogColumn<Definitions.Description.Global>("Protocol", 200, (g => g.Protocol.Name)) };

            if (dlg.ShowDialog(this.TopLevelControl) != DialogResult.OK)
                return;

            foreach (var g in dlg.SelectedItems)
            {
                Global global = new Global(this.SystemImplementation);
                global.Name = g.Name;
                global.GlobalType = (g.Definition is Definitions.Description.GlobalConstant) ? GlobalTypeEnum.Constant : GlobalTypeEnum.Variable;
                global.Description = g.Description;
                global.ImplementedProtocols.Add(g.Protocol.Name);
                global.Initializer.Source = g.Initializer;
                global.DefiningProtocol = g.Protocol.Name;
                global.Description = g.Description;

                if (!this.SystemImplementation.GlobalItems.Contains(global))
                    this.SystemImplementation.GlobalItems.Add(global);
            }
            this.SystemImplementation.GlobalItems.TriggerChanged();
        }

        public void VisibilityChanged(bool visible)
        {
            if (this.GlobalsMenu != null)
                this.GlobalsMenu.Enabled = visible && (this.SystemImplementation != null);
        }

        #endregion

        private void listGlobals_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (e.Item == null)
                return;
            Global gl = e.Item.Tag as Global;
            this._global.Value = gl;
            e.Item = this.listGlobals.Items.Cast<ListViewItem>()
                .FirstOrDefault(i => i.Tag == this._global.Value);
        }
    }
}
