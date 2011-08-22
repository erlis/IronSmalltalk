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
    public partial class ClassMethodsProtocolsControl : UserControl
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
                this._protocol = value;
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

        #endregion

        #endregion

        public ClassMethodsProtocolsControl()
        {
            InitializeComponent();
            this.IncludeUpToClassHolder = new ValueHolder<Class>();
            this.ProtocolNameHolder = new ValueHolder<string>();
            this.MethodTypeHolder = new ValueHolder<MethodType>();
            this.MethodTypeHolder.Value = MethodType.Instance;
            this.Enabled = false;
            this.comboInstanceOrClass.SelectedIndex = 0;
        }

        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            this.ProtocolNameHolder.Value = "ANY";
            this.IncludeUpToClassHolder.Value = this.Class;
            e.Transaction.AddAction(this.FillIncludeUpToClassView);
            e.Transaction.AddAction(this.FillView);
        }

        private void MethodTypeChanged(object sender, ValueChangedEventArgs<MethodType> e)
        {
            this.ProtocolNameHolder.Value = "ANY";
            e.Transaction.AddAction(this.FillView);
        }

        private void IncludeUpToClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            this.ProtocolNameHolder.Value = "ANY";
            e.Transaction.AddAction(this.FillView);
        }

        private void FillView()
        {
            this.Enabled = false;
            this.listMethodProtocols.Items.Clear();
            if (this.Class == null)
                return;

            ListViewItem lvi = this.listMethodProtocols.Items.Add("ANY", "handshake");
            lvi.Tag = "ANY";
            lvi.Selected = true;

            Dictionary<string, List<string>> protMap = new Dictionary<string, List<string>>();
            HashSet<string> methods = new HashSet<string>();

            foreach (Class cls in this.Class.WithAllSuperclasses())
            {
                var protocols = (this.MethodType == MethodType.Instance) ? cls.ImplementedInstanceProtocols : cls.ImplementedClassProtocols;
                foreach (string pn in protocols)
                {
                    if (!protMap.ContainsKey(pn))
                        protMap.Add(pn, new List<string>());
                    protMap[pn].Add(cls.Name);
                }

                var mths = (this.MethodType == MethodType.Instance) ? cls.InstanceMethods : cls.ClassMethods;
                foreach (Method mth in mths)
                    methods.Add(mth.Selector);

                if (cls == this.IncludeUpToClass)
                    break;
            }

            if (this.IncludeUpToClass != null)
            {
                foreach (Class cls in this.IncludeUpToClass.AllSuperclasses())
                {
                    var protocols = (this.MethodType == MethodType.Instance) ? cls.ImplementedInstanceProtocols : cls.ImplementedClassProtocols;
                    foreach (string pn in protocols)
                    {
                        var prot = cls.Parent.SmalltalkSystem.SystemDescription.Protocols.First(p => p.Name == pn);
                        if (prot != null)
                        {
                            if (prot.Messages.Any(msg => methods.Contains(msg.Selector)))
                            {
                                if (!protMap.ContainsKey(pn))
                                    protMap.Add(pn, new List<string>());
                                protMap[pn].Add(cls.Name);
                            }
                        }
                    }
                }
            }

            foreach (string pn in protMap.Keys.OrderBy(pn => pn))
            {
                lvi = this.listMethodProtocols.Items.Add(pn, "handshake");
                lvi.Tag = pn;

                var implementors = protMap[pn];
                if (!implementors.Contains(this.Class.Name))
                    lvi.ForeColor = Color.Gray;
                lvi.ToolTipText = String.Format("Implemented by: {0}", String.Join(", ", implementors));
            }

            lvi = this.listMethodProtocols.Items.Add("PRIVATE", "handshake");
            lvi.Tag = "PRIVATE";

            this.Enabled = true;
        }

        private void FillIncludeUpToClassView()
        {
            this.comboUpToClass.Items.Clear();
            if (this.Class == null)
                return;
            this.comboUpToClass.Items.AddRange(this.Class.WithAllSuperclasses().Reverse().ToArray());
            this.comboUpToClass.SelectedIndex = this.comboUpToClass.Items.Count - 1;
        }

        private void comboUpToClass_Format(object sender, ListControlConvertEventArgs e)
        {
            e.Value = ((Class)e.ListItem).Name;
        }

        private void listMethodProtocols_ItemChanging(object sender, ListItemChangingEventArgs e)
        {
            if (e.Item == null)
                return;
            string prot = e.Item.Tag as string;
            this.ProtocolNameHolder.Value = prot;
            e.Item = this.listMethodProtocols.Items.Cast<ListViewItem>()
                .FirstOrDefault(i => String.Equals(i.Tag as string, this.ProtocolName));
        }

        private void comboInstanceOrClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            MethodType mt = (this.comboInstanceOrClass.SelectedIndex == 1) ? MethodType.Class : MethodType.Instance;
            this.MethodTypeHolder.Value = mt;

            if (this.MethodTypeHolder.Value != mt)
                this.comboInstanceOrClass.SelectedItem = (this.MethodType == MethodType.Class) ? 1 : 0;
        }

        private void comboUpToClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Class cls = this.comboUpToClass.SelectedItem as Class;
            this.IncludeUpToClassHolder.Value = cls;

            if (this.IncludeUpToClassHolder.Value != cls)
                this.comboUpToClass.SelectedItem = cls;

        }
    }
}
