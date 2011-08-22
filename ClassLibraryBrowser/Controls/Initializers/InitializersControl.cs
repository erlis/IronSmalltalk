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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Initializers
{
    public partial class InitializersControl : UserControl, IDragDropClient
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


        public InitializersControl()
        {
            InitializeComponent();
        }

        private void SystemImplementationChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            this.FillView();
        }

        public void VisibilityChanged(bool visible)
        {
            if (visible)
                this.FillView();
        }

        private void FillView()
        {
            this.treeInitializers.Nodes.Clear();
            if (this.SystemImplementation == null)
                return;

            this.treeInitializers.BeginUpdate();
            var items = InitializerInfoBase.GetItems(this.SystemImplementation);
            foreach (var item in items)
                item.CreateTreeNode(this.treeInitializers.Nodes);
            this.treeInitializers.EndUpdate();
        }

        private abstract class InitializerInfoBase : IComparable<InitializerInfoBase>
        {
            public string Heading;
            public string ImageKey;
            public abstract decimal SortOrder { get; set; }

            protected InitializerInfoBase(string heading, string imageKey)
            {
                if (String.IsNullOrWhiteSpace(heading) || String.IsNullOrWhiteSpace(imageKey))
                    throw new ArgumentNullException();
                this.Heading = heading;
                this.ImageKey = imageKey;
            }

            protected static string GetImageKey(IInitializableObject owner)
            {
                if (owner is Class)
                    return "class";
                if (owner is Global)
                    return "structure";
                if (owner is PoolValue)
                    return "field";
                throw new ArgumentException(String.Format("Unknown object: {0}", owner));
            }

            protected static string GetHeading(IInitializableObject owner)
            {
                if (owner is Class)
                    return ((Class)owner).Name;
                if (owner is Global)
                    return ((Global)owner).Name;
                if (owner is PoolValue)
                    return ((PoolValue)owner).Name;
                throw new ArgumentException(String.Format("Unknown object: {0}", owner));
            }

            public static InitializerInfoBase GetInfo(GlobalItem item)
            {
                if (item is IInitializableObject)
                {
                    IInitializableObject initializable = (IInitializableObject)item;
                    if ((initializable.Initializer == null) || String.IsNullOrWhiteSpace(initializable.Initializer.Source))
                        return null;
                    return new InitializerInfo(initializable);
                }
                if (item is Pool)
                {
                    Pool pool = (Pool)item;
                    List<InitializerInfoBase> values = new List<InitializerInfoBase>();
                    foreach (PoolValue value in pool.Values)
                    {
                        if ((value.Initializer != null) && !String.IsNullOrWhiteSpace(value.Initializer.Source))
                            values.Add(new InitializerInfo(value));
                    }
                    if (values.Count == 0)
                        return null;
                    values.Sort();
                    InitializerInfoContainer info = new InitializerInfoContainer(pool.Name, "enum");
                    info.SortOrder = Math.Truncate(values[0].SortOrder);
                    info.Items.AddRange(values);
                    info.ResortChildItems();
                    return info;
                }
                throw new ArgumentException(String.Format("Unknown object: {0}", item));
            }

            public static List<InitializerInfoBase> GetItems(SystemImplementation system)
            {
                if (system == null)
                    throw new ArgumentNullException();
                List<InitializerInfoBase> result = new List<InitializerInfoBase>();
                foreach (GlobalItem item in system.GlobalItems)
                {
                    InitializerInfoBase info = GetInfo(item);
                    if (info != null)
                        result.Add(info);
                }

                result.Sort();
                for (int i = 0; i < result.Count; i++)
                    result[i].SortOrder = i;
                return result;
            }

            public virtual TreeNode CreateTreeNode(TreeNodeCollection collection)
            {
                TreeNode node = collection.Add(this.Heading, this.Heading, this.ImageKey, this.ImageKey);
                node.Tag = this;
                return node;
            }

            int IComparable<InitializerInfoBase>.CompareTo(InitializerInfoBase other)
            {
                int res = this.SortOrder.CompareTo(other.SortOrder);
                if (res != 0)
                    return res;
                return this.Heading.CompareTo(other.Heading);
            }

            public override string ToString()
            {
                return String.Format("{0} : {1}", this.Heading, this.SortOrder);
            }
        }

        private class InitializerInfoContainer : InitializerInfoBase
        {
            public readonly List<InitializerInfoBase> Items = new List<InitializerInfoBase>();

            public InitializerInfoContainer(string heading, string imageKey)
                : base(heading, imageKey)
            {
            }

            public void ResortChildItems()
            {
                decimal step;
                if (this.Items.Count < 10)
                    step = 0.1m;
                else if (this.Items.Count < 100)
                    step = 0.01m;
                else if (this.Items.Count < 1000)
                    step = 0.001m;
                else if (this.Items.Count < 10000)
                    step = 0.0001m;
                else if (this.Items.Count < 100000)
                    step = 0.00001m;
                else
                    throw new NotImplementedException("Too lazy to do power and log, so just hardcoded intervals up to 100K.");

                decimal sortBase = Math.Truncate(this.SortOrder);
                for (int i = 0; i < this.Items.Count; i++)
                    this.Items[i].SortOrder = sortBase + i * step;
            }

            public override TreeNode CreateTreeNode(TreeNodeCollection collection)
            {
                TreeNode node = base.CreateTreeNode(collection);
                foreach (InitializerInfoBase item in this.Items)
                    item.CreateTreeNode(node.Nodes);
                return node;
            }

            private decimal _sortOrder;
            public override decimal SortOrder
            {
                get { return this._sortOrder; }
                set { this._sortOrder = value; }
            }
        }

        private class InitializerInfo : InitializerInfoBase
        {
            public IInitializer Initializer;
            public IInitializableObject Owner;

            public InitializerInfo(IInitializableObject owner)
                : base(InitializerInfo.GetHeading(owner), InitializerInfo.GetImageKey(owner))
            {
                if (owner == null) 
                    throw new ArgumentNullException();
                if ((owner.Initializer == null) || String.IsNullOrWhiteSpace(owner.Initializer.Source))
                    throw new ArgumentException();

                this.Owner = owner;
                this.Initializer = owner.Initializer;
                this.SortOrder = this.Initializer.SortKey;
            }

            public override TreeNode CreateTreeNode(TreeNodeCollection collection)
            {
                TreeNode node = base.CreateTreeNode(collection);
                node.ToolTipText = this.Initializer.Source;
                return node;
            }

            public override decimal SortOrder
            {
                get { return this.Initializer.SortKey; }
                set {
                    //Console.Clear();
                    Console.WriteLine(String.Format("{0} => {1}", this, value));
                    
                    this.Initializer.SortKey = value; }
            }
        }

        #region Drag-Drop

        private readonly TreeViewDragDropHelper DragHelper = new TreeViewDragDropHelper();

        private void treeInitializers_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node == null)
                return;
            InitializerInfoBase info = node.Tag as InitializerInfoBase;
            if (info == null)
                return;

            this.DragHelper.DoDragDrop(this, node, DragDropEffects.Move);
        }

        bool IDragDropClient.IsDropAllowed(TreeNode sourceNode, TreeNode targetNode)
        {
            if (sourceNode == null)
                return false;
            if (targetNode == null)
                return false;
            if (sourceNode == targetNode)
                return false;
            if (sourceNode.PrevNode == targetNode)
                return false;
            if ((sourceNode.Parent == null) && (targetNode.Parent == null))
                return true;
            return (sourceNode.Parent == targetNode.Parent);
        }

        void IDragDropClient.CompleteDrop(TreeNode sourceNode, TreeNode targetNode)
        {
            if (!((IDragDropClient)this).IsDropAllowed(sourceNode, targetNode))
                return;

            TreeNodeCollection col = (sourceNode.Parent == null) ? this.treeInitializers.Nodes : sourceNode.Parent.Nodes;
            this.ReorderNodes(sourceNode, targetNode, col);
            this.ResortItems(col);
        }

        private void ReorderNodes(TreeNode sourceNode, TreeNode targetNode, TreeNodeCollection collection)
        {
            int iS = collection.IndexOf(sourceNode);
            int iT = collection.IndexOf(targetNode);

            if (iS < iT)
            {
                // Moving down
                decimal sk = ((InitializerInfoBase)collection[iT].Tag).SortOrder;
                for (int i = iT; i >= iS + 1; i--)
                    ((InitializerInfoBase)collection[i].Tag).SortOrder = ((InitializerInfoBase)collection[i - 1].Tag).SortOrder;
                ((InitializerInfoBase)collection[iS].Tag).SortOrder = sk;
            }
            else
            {
                // Moving up
                decimal sk = ((InitializerInfoBase)collection[iT + 1].Tag).SortOrder;
                for (int i = iT + 1; i < iS; i++)
                    ((InitializerInfoBase)collection[i].Tag).SortOrder = ((InitializerInfoBase)collection[i + 1].Tag).SortOrder;
                ((InitializerInfoBase)collection[iS].Tag).SortOrder = sk;
            }

            if (sourceNode.Tag is InitializerInfoContainer)
                ((InitializerInfoContainer)sourceNode.Tag).ResortChildItems();
            if (targetNode.Tag is InitializerInfoContainer)
                ((InitializerInfoContainer)targetNode.Tag).ResortChildItems();
        }

        private void ResortItems(TreeNodeCollection col)
        {
            if (col.Count == 0)
                return;
            TreeView tree = col[0].TreeView;
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode node in col)
                nodes.Add(node);
            tree.BeginUpdate();
            foreach (TreeNode node in nodes)
                col.Remove(node);
            foreach (TreeNode node in nodes.OrderBy(n => (InitializerInfoBase)n.Tag))
                col.Add(node);
            //foreach (TreeNode node in nodes)
            //    node.ToolTipText = ((InitializerInfoBase)node.Tag).SortOrder.ToString();
            tree.EndUpdate();
        }

        private void Swap<TValue>(TreeNode sourceNode, TreeNode targetNode, Func<TreeNode, TValue> get, Action<TreeNode, TValue> set)
        {
            TValue tmp = get(sourceNode);
            set(sourceNode, get(targetNode));
            set(targetNode, tmp);
        }

        #endregion
    }
}
