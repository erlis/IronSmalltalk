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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes
{
    public partial class ClassesControl : UserControl, IMainFormMenuConsumer
    {
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
                this.classHierarchyControl.SystemImplementationHolder = value;
                this.classDefinitionControl.SystemImplementationHolder = value;
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
                    this._class.Changed -= new ValueChangedEventHandler<Class>(this.ClassChanged);
                this._class = value;
                this._class.Changed += new ValueChangedEventHandler<Class>(this.ClassChanged);
                this.classHierarchyControl.ClassHolder = value;
                this.classDefinitionControl.ClassHolder = value;
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

        public ClassesControl()
        {
            InitializeComponent();
            this.ClassHolder = new ValueHolder<Class>();
        }

        private void SystemImplementationChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            this.ClassHolder.Value = null;
            if (this.ClassesMenu != null)
                this.ClassesMenu.Enabled = (this.SystemImplementation != null) & this.Visible;
            if (this.CreateNewClassMenuItem != null)
                this.CreateNewClassMenuItem.Enabled = (this.SystemImplementation != null);
            if (this.CreateMissingClassMenuItem != null)
                this.CreateMissingClassMenuItem.Enabled = (this.SystemImplementation != null);
        }


        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            if (this.SaveClassMenuItem != null)
                this.SaveClassMenuItem.Enabled = (this.Class != null);
            if (this.DeleteClassMenuItem != null)
                this.DeleteClassMenuItem.Enabled = (this.Class != null);
            //this.Enabled = (this.Class != null);
        }

        private ToolStripMenuItem ClassesMenu;
        private ToolStripMenuItem CreateNewClassMenuItem;
        private ToolStripMenuItem SaveClassMenuItem;
        private ToolStripMenuItem DeleteClassMenuItem;
        private ToolStripMenuItem CreateMissingClassMenuItem;

        void IMainFormMenuConsumer.AddMainMenuItems(MenuStrip mainMenu)
        {
            this.ClassesMenu = new ToolStripMenuItem();
            this.ClassesMenu.MergeIndex = 10;
            this.ClassesMenu.MergeAction = MergeAction.Insert;
            this.ClassesMenu.Name = "ClassesMenu";
            this.ClassesMenu.Text = "Class";
            this.ClassesMenu.Enabled = false;
            mainMenu.Items.Add(this.ClassesMenu);

            this.CreateNewClassMenuItem = new ToolStripMenuItem();
            this.CreateNewClassMenuItem.Name = "CreateNewClassMenuItem";
            this.CreateNewClassMenuItem.Text = "Create New Class";
            this.CreateNewClassMenuItem.Enabled = false;
            this.CreateNewClassMenuItem.MergeIndex = 10;
            this.CreateNewClassMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewClassMenuItem.Click += new EventHandler(this.CreateNewClassMenuItem_Click);
            this.ClassesMenu.DropDownItems.Add(this.CreateNewClassMenuItem);

            this.SaveClassMenuItem = new ToolStripMenuItem();
            this.SaveClassMenuItem.Name = "SaveClassMenuItem";
            this.SaveClassMenuItem.Text = "Save Class";
            this.SaveClassMenuItem.Enabled = false;
            this.SaveClassMenuItem.MergeIndex = 20;
            this.SaveClassMenuItem.MergeAction = MergeAction.Insert;
            this.SaveClassMenuItem.Click += new EventHandler(this.SaveClassMenuItem_Click);
            this.ClassesMenu.DropDownItems.Add(this.SaveClassMenuItem);

            this.DeleteClassMenuItem = new ToolStripMenuItem();
            this.DeleteClassMenuItem.Name = "DeleteClassMenuItem";
            this.DeleteClassMenuItem.Text = "Delete Class";
            this.DeleteClassMenuItem.Enabled = false;
            this.DeleteClassMenuItem.MergeIndex = 30;
            this.DeleteClassMenuItem.MergeAction = MergeAction.Insert;
            this.DeleteClassMenuItem.Click += new EventHandler(this.DeleteClassMenuItem_Click);
            this.ClassesMenu.DropDownItems.Add(this.DeleteClassMenuItem);

            ToolStripSeparator tsp = new ToolStripSeparator();
            tsp.Name = "ToolStripSeparatorClasses";
            tsp.MergeIndex = 100;
            tsp.MergeAction = MergeAction.Insert;
            this.ClassesMenu.DropDownItems.Add(tsp);

            this.CreateMissingClassMenuItem = new ToolStripMenuItem();
            this.CreateMissingClassMenuItem.Name = "CreateMissingClassMenuItem";
            this.CreateMissingClassMenuItem.Text = "Create Protocol Defined Classes ...";
            this.CreateMissingClassMenuItem.Enabled = false;
            this.CreateMissingClassMenuItem.MergeIndex = 30;
            this.CreateMissingClassMenuItem.MergeAction = MergeAction.Insert;
            this.CreateMissingClassMenuItem.Click += new EventHandler(this.CreateMissingClassMenuItem_Click);
            this.ClassesMenu.DropDownItems.Add(this.CreateMissingClassMenuItem);
        }

        private void CreateNewClassMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SystemImplementation == null)
                return;
            Class cls = new Class(this.SystemImplementation);
            cls.Name = "";
            if (this.Class != null)
                cls.SuperclassName = this.Class.Name;
            if (!this.ClassHolder.SetValue(cls))
                return;
        }

        private void SaveClassMenuItem_Click(object sender, EventArgs e)
        {
            this.classDefinitionControl.Save();
        }

        private void DeleteClassMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Class == null)
                return;
            if (this.SystemImplementation == null)
                return;
            var dlgres = MessageBox.Show(String.Format("Do you want to delete class '{0}'?", this.Class.Name),
                "Delete Class", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dlgres != DialogResult.Yes)
                return;
            this.SystemImplementation.GlobalItems.Remove(this.Class);
            //this.SystemImplementationHolder.TriggerChanged(this.SystemImplementation, this.SystemImplementation);
            this.ClassHolder.Value = null;
        }

        private void CreateMissingClassMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SystemImplementation == null)
                return;

            List<Definitions.Description.Global> globals = new List<Definitions.Description.Global>();
            foreach (var prot in this.SystemImplementation.SmalltalkSystem.SystemDescription.Protocols)
                globals.AddRange(prot.StandardGlobals.Where(g => (g.Definition == null) || (g.Definition is Definitions.Description.GlobalClass)));

            SelectDialog<Definitions.Description.Global> dlg = new SelectDialog<Definitions.Description.Global>();
            dlg.CheckBoxes = true;
            dlg.Items = globals.Where(g => !this.SystemImplementation.GlobalItems.Any(x => x.Name == g.Name)).OrderBy(g => g.Name);
            dlg.Columns = new SelectDialogColumn<Definitions.Description.Global>[] {
                new SelectDialogColumn<Definitions.Description.Global>("Global", 200, (g => g.Name)),
                new SelectDialogColumn<Definitions.Description.Global>("Protocol", 200, (g => g.Protocol.Name)) };

            if (dlg.ShowDialog(this.TopLevelControl) != DialogResult.OK)
                return;

            foreach(var g in dlg.SelectedItems)
            {
                Class cls = new Class(this.SystemImplementation);
                cls.Name = g.Name;
                cls.SuperclassName = "Object";
                cls.Description = g.Description;
                cls.ImplementedClassProtocols.Add(g.Protocol.Name);
                cls.Initializer.Source = g.Initializer;
                cls.InstanceState = InstanceStateEnum.NamedObjectVariables;
                cls.DefiningProtocol = g.Protocol.Name;



                if (!this.SystemImplementation.GlobalItems.Contains(cls))
                    this.SystemImplementation.GlobalItems.Add(cls);
            }
            this.SystemImplementation.GlobalItems.TriggerChanged();
        }

        public void VisibilityChanged(bool visible)
        {
            if (this.ClassesMenu != null)
                this.ClassesMenu.Enabled = visible && (this.SystemImplementation != null);
        }
    }
}
