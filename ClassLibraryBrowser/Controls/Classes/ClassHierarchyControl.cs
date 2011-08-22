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
using System.Runtime.InteropServices;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes
{
    public partial class ClassHierarchyControl : UserControl, IDragDropClient
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
        public ValueHolder<Class> ClassHolder
        {
            get
            {
                return this._class;
            }
            set
            {
                if (this._class != null)
                    this._class.Changed -= new ValueChangedEventHandler<Class>(this.ClassChanged);
                this._class = value;
                if (this._class != null)
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

        #endregion

        public ClassHierarchyControl()
        {
            this.AllGlobalsHolder = new SortedSetHolder<GlobalItem>();
            this.AllGlobalsHolder.CollectionChanged += new CollectionChangedEventHandler(this.AllGlobalsChanged);
            InitializeComponent();
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
        }

        private void AllGlobalsChanged(object sender, EventArgs e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.Enabled = (this.GlobalItems != null);
                this.FillClassesList();
                this.SetMenuItemState(this.Visible);
            }
            finally
            {
                this.Updating = remember;
            }
            // This must be outside the Updating block
            if (this.Class != null)
                this.treeClasses.SelectedNode = this.treeClasses.Nodes.Find(this.Class.Name, true).FirstOrDefault();
        }

        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            if (e.NewValue == null)
                this.treeClasses.SelectedNode = null;
            else
                this.treeClasses.SelectedNode = this.treeClasses.Nodes.Find(e.NewValue.Name, true).FirstOrDefault();
            this.SetMenuItemState(this.Visible);
        }

        private void FillClassesList()
        {
            this.treeClasses.BeginUpdate();
            try
            {
                HashSet<string> expanded = new HashSet<string>();
                this.FindExpandedNodes(this.treeClasses.Nodes, expanded);
                this.treeClasses.Nodes.Clear();
                if (this.GlobalItems == null)
                    return;
                var classes = this.GlobalItems.Where(e => e is Class).Cast<Class>();
                HashSet<string> recursionSet = new HashSet<string>();
                this.AddClassesForSuperclass(classes, this.treeClasses.Nodes, null, recursionSet, expanded);
                // If there is an issue with the data, add classes that have illegal data
                foreach (Class cls in classes)
                {
                    if (!recursionSet.Contains(cls.Name))
                        this.AddClass(cls, this.treeClasses.Nodes);
                }
            }
            finally
            {
                this.treeClasses.EndUpdate();
            }
            if (this.Class != null)
            {
                var node = this.treeClasses.Nodes.Find(this.Class.Name, true).FirstOrDefault();
                if (node != null)
                    node.EnsureVisible();
            }
        }

        private void FindExpandedNodes(TreeNodeCollection nodes, HashSet<string> expanded)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsExpanded)
                    expanded.Add(node.Name);
                this.FindExpandedNodes(node.Nodes, expanded);
            }
        }

        private void AddClassesForSuperclass(IEnumerable<Class> classes, TreeNodeCollection nodes, string superclassName, HashSet<string> recursionSet, HashSet<string> expanded)
        {
            foreach (Class cls in this.ClassesForSuperclass(classes, superclassName, recursionSet))
            {
                TreeNode node = this.AddClass(cls, nodes);
                this.AddClassesForSuperclass(classes, node.Nodes, cls.Name, recursionSet, expanded);
                if (expanded.Contains(cls.Name))
                    node.Expand();
            }
        }

        private IEnumerable<Class> ClassesForSuperclass(IEnumerable<Class> classes, string superclassName, HashSet<string> recursionSet)
        {
            if (String.IsNullOrWhiteSpace(superclassName))
                superclassName = String.Empty;

            recursionSet.Add(superclassName);

            List<Class> result = new List<Class>();
            foreach (Class cls in classes)
            {
                string scn = cls.SuperclassName;
                if (String.IsNullOrWhiteSpace(scn))
                    scn = String.Empty;
                if ((scn == superclassName) && !recursionSet.Contains(cls.Name))
                    result.Add(cls);
            }
            return result.OrderBy(cls => cls.Name);
        }

        private TreeNode AddClass(Class cls, TreeNodeCollection nodes)
        {
            TreeNode node = nodes.Add(cls.Name, cls.Name);
            node.ImageKey = "class";
            node.SelectedImageKey = "class";
            node.Tag = cls;
            node.ToolTipText = String.Format("Defined by: {3} \nImplements (inst): {4} \nImplements (cls): {5} \nState: {6} \n\n" +
                    "Instance vars: {7} \nClass vars: {2} \nClass-instance vars: {0} \nPools: {9}",
                String.Join(" ", cls.ClassInstanceVariables),   // 0
                cls.ClassMethods.Count,                         // 1
                String.Join(" ", cls.ClassVariables),           // 2
                cls.DefiningProtocol,                           // 3
                String.Join(" ", cls.ImplementedInstanceProtocols),     // 4
                String.Join(" ", cls.ImplementedClassProtocols),        // 5
                cls.InstanceState,                              // 6
                String.Join(" ", cls.InstanceVariables),        // 7
                cls.Name,                                       // 8
                String.Join(" ", cls.SharedPools),              // 9
                cls.SuperclassName);                            // 10
            return node;
        }

        #endregion

        #region Menu

        private void SetMenuItemState(bool visible)
        {
        }

        #endregion

        private void treeClasses_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.Updating)
            {
                e.Cancel = true;
                return;
            }
            if (e.Node == null)
                return;
            if (this.ClassHolder == null)
                return;
            Class cls = e.Node.Tag as Class;
            bool old = this.Updating;
            try
            {
                this.Updating = true;
                e.Cancel = !this.ClassHolder.SetValue(cls);
            }
            finally
            {
                this.Updating = old;
            }
        }

        private void treeClasses_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if ((e.Node == null) || (e.Label == null))
                return;
            if (this.Updating || (this.Class != e.Node.Tag))
            {
                e.CancelEdit = true;
                return;
            }
            string msg = this.ValidateName(e.Label);
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.CancelEdit = true;
                return;
            }

            this.Class.Name = e.Label;

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                this.ClassHolder.TriggerChanged(this.Class, this.Class);
                this.SystemImplementation.GlobalItems.TriggerChanged();
            }
            finally
            {
                this.Updating = remember;
            }
            e.CancelEdit = true;
            e.Node.EndEdit(false);
        }

        private string ValidateName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return "Class must have a name";
            if (this.SystemImplementation != null)
            {
                foreach (GlobalItem g in this.SystemImplementation.GlobalItems)
                {
                    if (g != this.Class)
                    {
                        if (g.Name == name)
                            return "Class name must be unuque";
                    }
                }
            }
            return null;
        }

        #region Drag-Drop

        private readonly TreeViewDragDropHelper DragHelper = new TreeViewDragDropHelper();

        private void treeClasses_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node == null)
                return;
            Class cls = node.Tag as Class;
            if (cls == null)
                return;

            this.DragHelper.DoDragDrop(this, node, DragDropEffects.Move);            
        }


        bool IDragDropClient.IsDropAllowed(TreeNode sourceNode, TreeNode targetNode)
        {
            if (sourceNode == null)
                return false;
            if (targetNode == null)
                return false;
            Class sourceClass = sourceNode.Tag as Class;
            Class targetClass = targetNode.Tag as Class;
            if (sourceClass == null)
                return false;
            if (targetClass == null)
                return false;

            if (targetClass.Name == sourceClass.Name)
                return false;
            if (sourceClass.AllSubclasses().Contains(targetClass))
                return false;
            return true;
        }

        void IDragDropClient.CompleteDrop(TreeNode sourceNode, TreeNode targetNode)
        {
            if (!((IDragDropClient)this).IsDropAllowed(sourceNode, targetNode))
                return;
            Class sourceClass = (Class)sourceNode.Tag;
            Class targetClass = (Class)targetNode.Tag;

            sourceClass.SuperclassName = targetClass.Name;
            this.SystemImplementation.GlobalItems.TriggerChanged();
            this.ClassHolder.TriggerChanged(sourceClass, sourceClass);
        }

        #endregion
    }
}
