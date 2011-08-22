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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Validation
{
    public partial class ImplementationValidationControl : UserControl
    {
        public ImplementationValidationControl()
        {
            InitializeComponent();
        }

        #region Accessors

        private ValueHolder<Definitions.SmalltalkSystem> _smalltalkSystemHolder;

        [Browsable(false)]
        public ValueHolder<Definitions.SmalltalkSystem> SmalltalkSystemHolder
        {
            get
            {
                return this._smalltalkSystemHolder;
            }
            set
            {
                if ((this._smalltalkSystemHolder == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._smalltalkSystemHolder == value)
                    return;
                this._smalltalkSystemHolder = value;
                value.Changed += new ValueChangedEventHandler<Definitions.SmalltalkSystem>(this.SmalltalkSystemChanged);
            }
        }

        public Definitions.SmalltalkSystem SmalltalkSystem
        {
            get
            {
                if (this.SmalltalkSystemHolder == null)
                    return null;
                return this.SmalltalkSystemHolder.Value;
            }
        }

        private void SmalltalkSystemChanged(object sender, ValueChangedEventArgs<Definitions.SmalltalkSystem> e)
        {
            this.Enabled = (this.SmalltalkSystem != null);
            if (this.Enabled)
                this.FillTabPage(this.tabControl.SelectedTab);
        }

        #endregion

        #region View Fill

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            this.FillTabPage(e.TabPage);
        }

        private void FillTabPage(TabPage page)
        {
            if (page == null)
                return;
            if (page.Name == "tabPageProtocols")
                this.FillProtocolsList();
            else if (page.Name == "tabPageGlobals")
                this.FillGlobalsList();
            else if (page.Name == "tabPageClasses")
                this.FillClassesList();
            else if (page.Name == "tabPageNativeMethodNames")
                this.FillNativeMethodNames();
        }

        #region Protocols

        private void FillProtocolsList()
        {
            this.listProtocols.ListViewItemSorter = null;
            this.listProtocols.Items.Clear();
            if (this.SmalltalkSystem == null)
                return;

            var prots = this.SmalltalkSystem.SystemDescription.Protocols.OrderBy(p => p.Name);
            foreach (var prot in prots)
            {
                List<string> implemetors = new List<string>();
                foreach (var cls in this.SmalltalkSystem.SystemImplementation.Classes)
                {
                    if (cls.ImplementedInstanceProtocols.Contains(prot.Name))
                        implemetors.Add(cls.Name);
                    if (cls.ImplementedClassProtocols.Contains(prot.Name))
                        implemetors.Add(cls.Name + " class");
                }
                foreach (var global in this.SmalltalkSystem.SystemImplementation.Globals)
                {
                    if (global.ImplementedProtocols.Contains(prot.Name))
                    {
                        if (global.GlobalType == Definitions.Implementation.GlobalTypeEnum.Constant)
                            implemetors.Add(global.Name + " global constant");
                        else
                            implemetors.Add(global.Name + " global variable");
                    }
                }
                List<string> conformsToAll = new List<string>();
                this.GetAllConformsTo(prot.ConformsTo, conformsToAll);

                ListViewItem lvi = this.listProtocols.Items.Add(prot.Name);
                lvi.SubItems.Add(prot.IsAbstract ? "Yes" : "No");
                lvi.SubItems.Add(String.Join(", ", prot.ConformsTo.OrderBy(s =>s)));
                lvi.SubItems.Add(String.Join(", ", implemetors.OrderBy(s => s)));
                lvi.SubItems.Add(String.Join(", ", conformsToAll.OrderBy(s => s)));
                lvi.Tag = prot;
                if (implemetors.Count == 0)
                    lvi.ForeColor = Color.Red;
            }
        }

        private void GetAllConformsTo(IEnumerable<string> conformsTo, List<string> result)
        {
            foreach (string pn in conformsTo)
            {
                if ((pn != "ANY") && !result.Contains(pn))
                {
                    result.Add(pn);
                    Definitions.Description.Protocol prot = this.SmalltalkSystem.SystemDescription.
                        Protocols.FirstOrDefault(p => p.Name == pn);
                    if (prot != null)
                        this.GetAllConformsTo(prot.ConformsTo, result);
                }
            }
        }

        private void listProtocols_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listProtocols.ListViewItemSorter = new ListViewItemComparer(e.Column, this.listProtocols.ListViewItemSorter);
        }

        // Implements the manual sorting of items by columns.
        private class ListViewItemComparer : System.Collections.IComparer
        {
            private int ColumnIndex { get; set; }
            public ListViewItemComparer()
            {
                this.ColumnIndex = 1;
            }
            public ListViewItemComparer(int column)
                : this(column, null)
            {
            }
            public ListViewItemComparer(int column, object currentComparer)
            {
                column = column + 1;
                if ((currentComparer is ListViewItemComparer) && (((ListViewItemComparer)currentComparer).ColumnIndex == column))
                    column = -column;
                this.ColumnIndex = column;
            }
            public int Compare(object x, object y)
            {
                int col = Math.Abs(this.ColumnIndex)-1;
                string a = ((ListViewItem)x).SubItems[col].Text;
                string b = ((ListViewItem)y).SubItems[col].Text;
                int res = String.Compare(a, b);
                if (this.ColumnIndex < 0)
                    return -res;
                else
                    return res;
            }
        }

        #endregion

        #region Globals

        private void FillGlobalsList()
        {
            this.listGlobals.Items.Clear();
            if (this.SmalltalkSystem == null)
                return;

            List<Definitions.Description.Global> globals = new List<Definitions.Description.Global>();
            foreach (var prot in this.SmalltalkSystem.SystemDescription.Protocols)
                globals.AddRange(prot.StandardGlobals);
            globals.Sort(delegate(Definitions.Description.Global a, Definitions.Description.Global b) { return a.Name.CompareTo(b.Name); });

            foreach (var global in globals)
            {
                string type = String.Empty;
                // Not the best way to get the type, but ...
                if (global.Definition != null)
                    type = global.Definition.GetType().Name.Replace("Global", "");

                List<string> implementors = new List<string>();
                foreach(var item in this.SmalltalkSystem.SystemImplementation.Classes)
                {
                    if (item.Name == global.Name)
                        implementors.Add(item.Name + " (class)");
                }
                foreach(var item in this.SmalltalkSystem.SystemImplementation.GlobalConstants)
                {
                    if (item.Name == global.Name)
                        implementors.Add(item.Name + " (constant)");
                }
                foreach(var item in this.SmalltalkSystem.SystemImplementation.GlobalVariables)
                {
                    if (item.Name == global.Name)
                        implementors.Add(item.Name + " (variable)");
                }
                foreach(var item in this.SmalltalkSystem.SystemImplementation.Pools)
                {
                    if (item.Name == global.Name)
                        implementors.Add(item.Name + " (pool)");
                }

                ListViewItem lvi = this.listGlobals.Items.Add(global.Name);
                lvi.SubItems.Add(global.Protocol.Name);
                lvi.SubItems.Add(type);
                lvi.SubItems.Add(String.Join(", ", implementors.OrderBy(s=>s)));
                lvi.Tag = global;
                if (implementors.Count == 0)
                    lvi.ForeColor = Color.Red;
            }
        }

        private void listClasses_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listClasses.ListViewItemSorter = new ListViewItemComparer(e.Column, this.listClasses.ListViewItemSorter);
        }

        #endregion

        #region Classes

        private void FillClassesList()
        {
            this.listClasses.ListViewItemSorter = null;
            this.listClasses.Items.Clear();
            if (this.SmalltalkSystem == null)
                return;

            var clss = this.SmalltalkSystem.SystemImplementation.Classes.OrderBy(c => c.Name);
            foreach (var cls in clss)
            {
                HashSet<string> missing = new HashSet<string>();
                List<string> implInst = this.BuildImplementsResult(cls, missing, false);
                List<string> implCls = this.BuildImplementsResult(cls, missing, true);

                ListViewItem lvi = this.listClasses.Items.Add(cls.Name);
                lvi.SubItems.Add(String.Join(", ", implInst));
                lvi.SubItems.Add(String.Join(", ", implCls));
                lvi.SubItems.Add(String.Join(", ", missing.OrderBy(s => s)));
                lvi.Tag = cls;
                if (missing.Count != 0)
                    lvi.ForeColor = Color.Red;
            }
        }

        private List<string> BuildImplementsResult(Definitions.Implementation.Class cls, HashSet<string> missing, bool classBehavior)
        {
            List<string> result = new List<string>();

            IEnumerable<string> directProtocols;
            HashSet<string> inheritedProtocols;
            HashSet<string> implementedProtocols;

            this.GetClassProtocols(cls, classBehavior, out directProtocols, out inheritedProtocols, out implementedProtocols);

            // Class behavior also implements the instance behavior of the Class (and Object) class.
            HashSet<string> indirectImplementedProtocols = new HashSet<string>();
            if (classBehavior)
            {
                Definitions.Implementation.Class c = cls.Parent.Classes.FirstOrDefault(e => e.Name == "Class");
                if (c == null)
                    c = cls.Parent.Classes.FirstOrDefault(e => e.Name == "Object");
                if (c != null)
                {
                    IEnumerable<string> na1;
                    HashSet<string> na2;
                    this.GetClassProtocols(c, false, out na1, out na2, out indirectImplementedProtocols);
                }
            }

            // Protocols that the class wishes to implement (direct protocols)
            foreach (string protocolName in directProtocols.OrderBy(pn => pn))
            {
                if (implementedProtocols.Contains(protocolName) || indirectImplementedProtocols.Contains(protocolName))
                    result.Add(protocolName);
                else
                    result.Add("[" + protocolName + "]");

                if (!implementedProtocols.Contains(protocolName) && !indirectImplementedProtocols.Contains(protocolName))
                    missing.Add(protocolName);
            }

            // Protocols that the class have to implement, because the direct protocols inherit from those
            foreach (string protocolName in inheritedProtocols.Where(pn => pn != "ANY").OrderBy(pn => pn))
            {
                if (implementedProtocols.Contains(protocolName) || indirectImplementedProtocols.Contains(protocolName))
                    result.Add("(" + protocolName + ")");
                else
                    result.Add("[" + protocolName + "]");

                if (!implementedProtocols.Contains(protocolName) && !indirectImplementedProtocols.Contains(protocolName))
                    missing.Add(protocolName);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cls">The class</param>
        /// <param name="classBehavior">True for class behavior, false for instance.</param>
        /// <param name="directProtocols">Directly defined by the class.</param>
        /// <param name="inheritedProtocols">Indirectly defined by the class, because the direct protocols inherit from those.</param>
        /// <param name="implementedProtocols">Protocols implemented by the class or the class' superclasses.</param>
        /// <returns></returns>
        private void GetClassProtocols(Definitions.Implementation.Class cls, bool classBehavior,
            out IEnumerable<string> directProtocols, out HashSet<string> inheritedProtocols, out HashSet<string> implementedProtocols)
        {
            directProtocols = (classBehavior ? cls.ImplementedClassProtocols : cls.ImplementedInstanceProtocols);
            inheritedProtocols = new HashSet<string>();
            implementedProtocols = new HashSet<string>();

            foreach (string protocolName in directProtocols)
            {
                var protocol = this.SmalltalkSystem.SystemDescription.Protocols.FirstOrDefault(p => p.Name == protocolName);
                if (protocol != null)
                {
                    foreach (var p in protocol.AllConformsTo())
                        inheritedProtocols.Add(p);
                }
            }

            foreach (var c in cls.WithAllSuperclasses())
            {
                foreach (var p in (classBehavior ? c.ImplementedClassProtocols : c.ImplementedInstanceProtocols))
                    implementedProtocols.Add(p);
            }
        }

        #endregion

        #region Native Method Names

        private void FillNativeMethodNames()
        {
            this.listNativeMethodNames.ListViewItemSorter = null;
            this.listNativeMethodNames.Items.Clear();
            if (this.SmalltalkSystem == null)
                return;

            this.listNativeMethodNames.BeginUpdate();
            try
            {
                foreach (var cls in this.SmalltalkSystem.SystemImplementation.Classes.OrderBy(c => c.Name))
                {
                    if ((cls.InstanceState != Definitions.Implementation.InstanceStateEnum.Native) || this.IsDynamicMetaObject(cls))
                    {
                        foreach (var mth in cls.InstanceMethods.OrderBy(m => m.Selector))
                        {
                            string native = mth.Annotations.GetFirst("ist.runtime.native-name");
                            if (!String.IsNullOrWhiteSpace(native))
                            {
                                bool conflict = this.HasDuplicateInstanceNativeName(cls, mth, native);
                                ListViewItem lvi = this.listNativeMethodNames.Items.Add(cls.Name);
                                lvi.SubItems.Add(mth.Selector);
                                lvi.SubItems.Add(native);
                                lvi.SubItems.Add(conflict ? "Yes" : "No");
                                if (conflict)
                                    lvi.ForeColor = Color.DarkRed;
                            }
                        }

                        foreach (var mth in cls.ClassMethods.OrderBy(m => m.Selector))
                        {
                            string native = mth.Annotations.GetFirst("ist.runtime.native-name");
                            if (!String.IsNullOrWhiteSpace(native))
                            {
                                bool conflict = this.HasDuplicateClassNativeName(cls, mth, native);
                                ListViewItem lvi = this.listNativeMethodNames.Items.Add(cls.Name + " class");
                                lvi.SubItems.Add(mth.Selector);
                                lvi.SubItems.Add(native);
                                lvi.SubItems.Add(conflict ? "Yes" : "No");
                                if (conflict)
                                    lvi.ForeColor = Color.DarkRed;
                            }
                        }
                    }
                }
            }
            finally
            {
                this.listNativeMethodNames.EndUpdate();
            }
        }

        private bool IsDynamicMetaObject(Definitions.Implementation.Class cls)
        {
            string name = cls.Annotations.GetFirst("ist.runtime.native-class");
            if (String.IsNullOrWhiteSpace(name))
                return false;
            Type type = IronSmalltalk.Runtime.Internal.NativeTypeClassMap.GetType(name);
            if (type == null)
                return false;
            return type.GetInterfaces().Contains(typeof(System.Dynamic.IDynamicMetaObjectProvider));
        }

        private bool HasDuplicateInstanceNativeName(Definitions.Implementation.Class cls, Definitions.Implementation.Method mth, string nativeName)
        {
            return this.HasDuplicates(cls, mth, nativeName, c => c.InstanceMethods);
        }

        private bool HasDuplicateClassNativeName(Definitions.Implementation.Class cls, Definitions.Implementation.Method mth, string nativeName)
        {
            if (this.HasDuplicates(cls, mth, nativeName, c => c.ClassMethods))
                return true;
            return this.HasDuplicateInstanceNativeName(mth.Parent.Parent.Classes.FirstOrDefault(c => c.Name == "Class"), mth, nativeName);
        }

        private bool HasDuplicates(Definitions.Implementation.Class cls, Definitions.Implementation.Method mth, string nativeName,
            Func<Definitions.Implementation.Class, ISet<Definitions.Implementation.Method>> getMethods)
        {
            while (cls != null)
            {
                foreach (var m in getMethods(cls))
                {
                    if (m.Selector != mth.Selector)
                    {
                        string native = m.Annotations.GetFirst("ist.runtime.native-name");
                        if (native == nativeName)
                        {
                            if (this.NumberOfParameters(m.Selector) == this.NumberOfParameters(mth.Selector))
                                return true;
                        }
                    }
                }
                cls = cls.Parent.Classes.FirstOrDefault(c => c.Name == cls.SuperclassName);
            }
            return false;
        }

        private int NumberOfParameters(string selector)
        {
            if (selector.All(c => @"!%&*+,/<=>?@\~|-".Contains(c)))
                return 1;
            return selector.Count(c => (c == ':'));
        }

        private void listNativeMethodNames_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listNativeMethodNames.ListViewItemSorter = new ListViewItemComparer(e.Column, this.listNativeMethodNames.ListViewItemSorter);
        }

        #endregion

        #endregion
    }
}
