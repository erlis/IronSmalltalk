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
    public partial class ClassMethodsListControl : UserControl
    {
        #region Value Holders

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
            }
        }

        private ValueHolder<Class> _includeUpToClass;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<Class> IncludeUpToClassHolder
        {
            get
            {
                return this._includeUpToClass;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._includeUpToClass != null)
                    this._includeUpToClass.Changed -= new ValueChangedEventHandler<Class>(this.IncludeUpToClassChanged);
                this._includeUpToClass = value;
                this._includeUpToClass.Changed += new ValueChangedEventHandler<Class>(this.IncludeUpToClassChanged);
            }
        }

        private ValueHolder<MethodType> _methodType;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<MethodType> MethodTypeHolder
        {
            get
            {
                return this._methodType;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._methodType != null)
                    this._methodType.Changed -= new ValueChangedEventHandler<MethodType>(this.MethodTypeChanged);
                this._methodType = value;
                this._methodType.Changed += new ValueChangedEventHandler<MethodType>(this.MethodTypeChanged);
            }
        }

        private ValueHolder<string> _protocol;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<string> ProtocolNameHolder
        {
            get
            {
                return this._protocol;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._protocol != null)
                    this._protocol.Changed -= new ValueChangedEventHandler<string>(this.ProtocolChanged);
                this._protocol = value;
                this._protocol.Changed += new ValueChangedEventHandler<string>(this.ProtocolChanged);
            }
        }


        private ValueHolder<string> _method;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<string> MethodNameHolder
        {
            get
            {
                return this._method;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._method != null)
                    this._method.Changed -= new ValueChangedEventHandler<string>(this.MethodChanged);
                this._method = value;
                this._method.Changed += new ValueChangedEventHandler<string>(this.MethodChanged);
            }
        }

        #region Value Accessors

        public Class Class
        {
            get
            {
                if (this.ClassHolder == null)
                    return null;
                return this.ClassHolder.Value;
            }
        }

        public Class IncludeUpToClass
        {
            get
            {
                if (this.IncludeUpToClassHolder == null)
                    return this.Class;
                return this.IncludeUpToClassHolder.Value;
            }
        }

        public string ProtocolName
        {
            get
            {
                if (this.ProtocolNameHolder == null)
                    return null;
                return this.ProtocolNameHolder.Value;
            }
        }

        public MethodType MethodType
        {
            get
            {
                if (this.MethodTypeHolder == null)
                    return MethodType.Instance;
                else
                    return this.MethodTypeHolder.Value;
            }
        }

        public string MethodName
        {
            get
            {
                if (this.MethodNameHolder == null)
                    return null;
                return this.MethodNameHolder.Value;
            }
        }

        #endregion

        #endregion


        public ClassMethodsListControl()
        {
            this.MethodNameHolder = new ValueHolder<string>();
            InitializeComponent();
            this.Enabled = false;
        }

        #region Fill List

        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            this.MethodNameHolder.Value = "newMethod";
            e.Transaction.AddAction(this.FillView);
        }

        private void MethodTypeChanged(object sender, ValueChangedEventArgs<MethodType> e)
        {
            this.MethodNameHolder.Value = "newMethod";
            e.Transaction.AddAction(this.FillView);
        }

        private void ProtocolChanged(object sender, ValueChangedEventArgs<string> e)
        {
            this.MethodNameHolder.Value = "newMethod";
            e.Transaction.AddAction(this.FillView);
        }

        private void IncludeUpToClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            this.MethodNameHolder.Value = "newMethod";
            e.Transaction.AddAction(this.FillView);
        }

        private void MethodChanged(object sender, ValueChangedEventArgs<string> e)
        {
            e.Transaction.AddAction(this.FillView);
        }

        private void FillView()
        {
            if (this.ListItemChanging)
                return;
            this.Enabled = false;
            this.listMethods.Items.Clear();
            if (this.Class == null)
                return;
            if (String.IsNullOrWhiteSpace(this.ProtocolName))
                return;
            if (this.MethodType == null)
                return;

            this.listMethods.BeginUpdate();
            Font normalFont = this.listMethods.Font;
            Font boldFont = new Font(normalFont, FontStyle.Bold);

            List<Class> classesList;
            List<Class> superclassesList;
            Dictionary<string, Class> classesMap;
            Dictionary<string, List<Class>> protocolNameClassesMap;
            Dictionary<string, List<Class>> protocolNameSuperslassesMap;
            Dictionary<string, List<Class>> methodNameClassesMap;
            Dictionary<string, List<Class>> methodNameSuperclassesMap;
            Dictionary<string, List<string>> methodNameProtocolNamesMap;
            Dictionary<string, List<string>> allMethodNameProtocolNamesMap;
            Dictionary<string, Definitions.Description.Protocol> protocolMap;
            HashSet<string> subclassResponsibilityMethods;
            Dictionary<string, List<Class>> methodNameLocalImplementorsMap;
            Dictionary<string, List<Class>> methodNameSuperImplementorsMap;

            MethodHelper.BuildLists(this.Class, this.IncludeUpToClass, this.MethodType, out classesList, out superclassesList,
                out classesMap, out protocolNameClassesMap, out protocolNameSuperslassesMap, out methodNameClassesMap,
                out methodNameSuperclassesMap, out methodNameProtocolNamesMap, out allMethodNameProtocolNamesMap,
                out protocolMap, out subclassResponsibilityMethods, out methodNameLocalImplementorsMap, out methodNameSuperImplementorsMap);

            /* *** CREATE THE LIST OF METHODS ***
             * The logic is as follows:
             *      1. Consider methods that:
             *          a. Are implemented by the this.Class itself
             *          b. Are implemented by any superclass up to and incl. this.IncludeUpToClass
             *          c. Are defined (actually messages) in any protocol that this.Class wants to implement
             *          d. Are defined (actually messages) in any protocol that any superclass up to and incl. this.IncludeUpToClass wants to implement
             *          * Obviously, depending on this.MethodType we concider only the class methods/protocols or the instance ones
             *      2. Filter methods so:
             *          a. If this.ProtocolName is "ANY", then include everything.
             *          b. If this.ProtocolName is "PRIVATE", then include method found in 1.a. or 1.b. but NOT also in 1.c. and 1.d.
             *          c. Include methods found in 1.c. or 1.d. according to the name of the selected protocol
             *          d. Similar to b., but also method in any protocol that any of the superclasses wants to implements
             *      
             *      3. Create list items for each method with following details:
             *          a. Method selector
             *          b. Lists if protocols defining it (similar to 1.c and 1.d but up to the root class)
             *          c. Local implementors of the methods (subclasses of this.Class, but NOT this.Class itself)
             *          d. Super implementors of the methods (superclasses of this.IncludeUpToClass, but NOT this.IncludeUpToClass itself)
             *      4. Decorate (color) each item:
             *          a. Only defined (in protocols) but not implemented: Gray
             *          b. Implementation is empty and super implementor exists that says subclassResponsibility: Red
             *          c. Found in 1.a. or 1.c. (as oposite to 1.b and 1.d): Bold Font
             */


            HashSet<string> methodNames = new HashSet<string>();
            methodNames.UnionWith(methodNameClassesMap.Keys);
            methodNames.UnionWith(methodNameProtocolNamesMap.Keys);

            foreach (string selector in methodNames.OrderBy(s => s))
            {
                bool include;
                if (this.ProtocolName == "ANY")
                    include = true;
                else if (this.ProtocolName == "PRIVATE")
                    include = !methodNameProtocolNamesMap.ContainsKey(selector) || (methodNameProtocolNamesMap[selector].Count == 0);
                else
                    include = allMethodNameProtocolNamesMap.ContainsKey(selector) && allMethodNameProtocolNamesMap[selector].Contains(this.ProtocolName);

                if (include)
                {
                    ListViewItem lvi = this.listMethods.Items.Add(selector, "method");
                    lvi.Tag = selector;
                    lvi.UseItemStyleForSubItems = false;

                    // Is the method implemented by any class (as oposite by only defined by a protocol implemented by the class)
                    if (!methodNameClassesMap.ContainsKey(selector))
                        lvi.ForeColor = Color.Gray;

                    // Is this a method that we must override?
                    if (subclassResponsibilityMethods.Contains(selector))
                        lvi.ForeColor = Color.Red;


                    IEnumerable<string> definingProtocols = new string[0];
                    if (allMethodNameProtocolNamesMap.ContainsKey(selector))
                        definingProtocols = allMethodNameProtocolNamesMap[selector];

                    // This is a method defined in locally implemented protocol
                    if (definingProtocols.Any(pn => protocolNameClassesMap.ContainsKey(pn) && protocolNameClassesMap[pn].Contains(this.Class)))
                        lvi.Font = boldFont;

                    // This is a locally implemented method
                    if (methodNameClassesMap.ContainsKey(selector) && methodNameClassesMap[selector].Contains(this.Class))
                        lvi.Font = boldFont;

                    IEnumerable<Class> implementingClasses = new Class[0];
                    if (methodNameClassesMap.ContainsKey(selector))
                        implementingClasses = methodNameClassesMap[selector];

                    // Protocols: (which protocols defines the method)
                    string protocols = String.Join(", ", definingProtocols.OrderBy(p => p));
                    lvi.SubItems.Add((String.IsNullOrWhiteSpace(protocols) ? "PRIVATE" : protocols));
                    lvi.SubItems[lvi.SubItems.Count - 1].Font = lvi.Font;
                    lvi.SubItems[lvi.SubItems.Count - 1].ForeColor = lvi.ForeColor;

                    // Local Implementors: (excluding the current class)
                    lvi.SubItems.Add(String.Join(", ", methodNameLocalImplementorsMap[selector].Select(c => c.Name))).Font = normalFont;

                    // Super Implementors: (excluding listed classes)
                    lvi.SubItems.Add(String.Join(", ", methodNameSuperImplementorsMap[selector].Select(c => c.Name))).Font = normalFont;

                    lvi.ToolTipText = String.Format("Defined: {0}\nImplemented: {1}",
                        protocols, String.Join(", ", implementingClasses.Select(c => c.Name)));

                    lvi.Selected = (this.MethodName == selector);
                }
            }

            this.listMethods.EndUpdate();

            if (this.listMethods.SelectedItems.Count > 0)
                this.listMethods.SelectedItems[0].EnsureVisible();

            this.Enabled = true;
        }



        #endregion

        private bool ListItemChanging = false;
        private void listMethods_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (e.Item == null)
                return;

            bool oldValue = this.ListItemChanging;
            this.ListItemChanging = true;
            try
            {
                string selector = e.Item.Tag as string;
                this.MethodNameHolder.Value = selector;
                e.Item = this.listMethods.Items.Cast<ListViewItem>()
                    .FirstOrDefault(i => String.Equals(i.Tag as string, this.MethodName));
            }
            finally
            {
                this.ListItemChanging = oldValue;
            }
        }

        private void listMethods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if ((this.Class != null) && (this.MethodType != null))
                {
                    if (this.listMethods.SelectedIndices.Count == 0)
                        return;
                    int sel = this.listMethods.SelectedIndices[0];

                    ISet<Method> methods;
                    if (this.MethodType == Classes.MethodType.Class)
                        methods = this.Class.ClassMethods;
                    else
                        methods = this.Class.InstanceMethods;
                    Method method = methods.FirstOrDefault(m => m.Selector == this.MethodName);
                    if (method != null)
                    {
                        methods.Remove(method);
                        this.ProtocolNameHolder.TriggerChanged(this.ProtocolName, this.ProtocolName);
                        this.MethodNameHolder.Value = null;

                        if (sel < this.listMethods.Items.Count)
                        {
                            this.listMethods.Items[sel].Focused = true;
                            this.listMethods.Items[sel].Selected = true;
                        }
                        this.listMethods.Focus();
                        return;
                    }
                }
                //MessageBox.Show("Could not delete method", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
