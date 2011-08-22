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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes
{
    public partial class ClassMethodsEditorControl : UserControl, IMainFormMenuConsumer
    {
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
                this.classHierarchy.SystemImplementationHolder = value;
                if (this._system != null)
                    this._system.Changed += new ValueChangedEventHandler<SystemImplementation>(this.SystemChanged);
            }
        }

        private void SystemChanged(object sender, ValueChangedEventArgs<SystemImplementation> e)
        {
            this.SetMenuState();
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
                this.classHierarchy.ClassHolder = value;
                this.classMethodsProtocols.ClassHolder = value;
                this.classMethodsList.ClassHolder = value;
                this.methodDefinition.ClassHolder = value;
                if (this._class != null)
                    this._class.Changed += new ValueChangedEventHandler<Class>(this.ClassChanged);
            }
        }

        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            this.SetMenuState();
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


        #endregion

        public ClassMethodsEditorControl()
        {
            InitializeComponent();
            this.ClassHolder = new ValueHolder<Class>();
            this.classMethodsList.ProtocolNameHolder = this.classMethodsProtocols.ProtocolNameHolder;
            this.classMethodsList.MethodTypeHolder = this.classMethodsProtocols.MethodTypeHolder;
            this.classMethodsList.IncludeUpToClassHolder = this.classMethodsProtocols.IncludeUpToClassHolder;
            this.methodDefinition.ProtocolNameHolder = this.classMethodsProtocols.ProtocolNameHolder;
            this.methodDefinition.MethodTypeHolder = this.classMethodsProtocols.MethodTypeHolder;
            this.methodDefinition.MethodNameHolder = this.classMethodsList.MethodNameHolder;

            this.classMethodsProtocols.ProtocolNameHolder.Changed += new ValueChangedEventHandler<string>(this.ProtocolNameChanged);
            this.classMethodsProtocols.MethodTypeHolder.Changed += new ValueChangedEventHandler<MethodType>(this.MethodTypeChanged);
            this.classMethodsList.MethodNameHolder.Changed += new ValueChangedEventHandler<string>(this.MethodNameChanged);
        }

        private void MethodNameChanged(object sender, ValueChangedEventArgs<string> e)
        {
            e.Transaction.AddAction(this.SetMenuState);
        }

        private void MethodTypeChanged(object sender, ValueChangedEventArgs<MethodType> e)
        {
            e.Transaction.AddAction(this.SetMenuState);
        }

        private void ProtocolNameChanged(object sender, ValueChangedEventArgs<string> e)
        {
            e.Transaction.AddAction(this.SetMenuState);
        }

        private ToolStripMenuItem MethodsMenu;
        private ToolStripMenuItem CreateNewMethodMenuItem;
        private ToolStripMenuItem SaveMethodMenuItem;
        private ToolStripMenuItem DeleteMethodMenuItem;

        void IMainFormMenuConsumer.AddMainMenuItems(MenuStrip mainMenu)
        {
            this.MethodsMenu = new ToolStripMenuItem();
            this.MethodsMenu.MergeIndex = 15;
            this.MethodsMenu.MergeAction = MergeAction.Insert;
            this.MethodsMenu.Name = "MethodsMenu";
            this.MethodsMenu.Text = "Method";
            this.MethodsMenu.Enabled = false;
            mainMenu.Items.Add(this.MethodsMenu);

            this.CreateNewMethodMenuItem = new ToolStripMenuItem();
            this.CreateNewMethodMenuItem.Name = "CreateNewMethodMenuItem";
            this.CreateNewMethodMenuItem.Text = "Create New Method";
            this.CreateNewMethodMenuItem.Enabled = false;
            this.CreateNewMethodMenuItem.MergeIndex = 10;
            this.CreateNewMethodMenuItem.MergeAction = MergeAction.Insert;
            this.CreateNewMethodMenuItem.Click += new EventHandler(this.CreateNewMethodMenuItem_Click);
            this.MethodsMenu.DropDownItems.Add(this.CreateNewMethodMenuItem);

            this.SaveMethodMenuItem = new ToolStripMenuItem();
            this.SaveMethodMenuItem.Name = "SaveMethodMenuItem";
            this.SaveMethodMenuItem.Text = "Save Method";
            this.SaveMethodMenuItem.Enabled = false;
            this.SaveMethodMenuItem.MergeIndex = 20;
            this.SaveMethodMenuItem.MergeAction = MergeAction.Insert;
            this.SaveMethodMenuItem.Click += new EventHandler(this.SaveClassMethodItem_Click);
            this.MethodsMenu.DropDownItems.Add(this.SaveMethodMenuItem);

            this.DeleteMethodMenuItem = new ToolStripMenuItem();
            this.DeleteMethodMenuItem.Name = "DeleteMethodMenuItem";
            this.DeleteMethodMenuItem.Text = "Delete Method";
            this.DeleteMethodMenuItem.Enabled = false;
            this.DeleteMethodMenuItem.MergeIndex = 30;
            this.DeleteMethodMenuItem.MergeAction = MergeAction.Insert;
            this.DeleteMethodMenuItem.Click += new EventHandler(this.DeletMethodMenuItem_Click);
            this.MethodsMenu.DropDownItems.Add(this.DeleteMethodMenuItem);
        }

        private void CreateNewMethodMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Class == null)
                return;
            //string selector = Microsoft.VisualBasic.Interaction.InputBox("Enter Method Selector:", "Create New Method");
            //if (String.IsNullOrWhiteSpace(selector))
            //    return;
            this.classMethodsList.MethodNameHolder.Value = "newMethod";
        }

        private void SaveClassMethodItem_Click(object sender, EventArgs e)
        {
            this.methodDefinition.Save();
        }

        private void DeletMethodMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.Class != null) && (this.classMethodsProtocols.MethodType != null))
            {
                ISet<Method> methods;
                if (this.classMethodsProtocols.MethodType == Classes.MethodType.Class)
                    methods = this.Class.ClassMethods;
                else
                    methods = this.Class.InstanceMethods;
                Method method = methods.FirstOrDefault(m => m.Selector == this.classMethodsList.MethodName);
                if (method != null)
                {
                    methods.Remove(method);
                    this.classMethodsProtocols.ProtocolNameHolder.TriggerChanged(this.classMethodsProtocols.ProtocolName, this.classMethodsProtocols.ProtocolName);
                    this.classMethodsList.MethodNameHolder.Value = null;
                    return;
                }
            }
            MessageBox.Show("Could not delete method", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void VisibilityChanged(bool visible)
        {
            if (this.MethodsMenu != null)
                this.MethodsMenu.Enabled = visible && (this.SystemImplementation != null) && (this.Class != null) && (this.classMethodsProtocols.MethodType != null);
        }

        private void SetMenuState()
        {
            if (this.MethodsMenu != null)
                this.MethodsMenu.Enabled = (this.Class != null) && (this.SystemImplementation != null) && (this.classMethodsProtocols.MethodType != null) && this.Visible;
            if (this.CreateNewMethodMenuItem != null)
                this.CreateNewMethodMenuItem.Enabled = (this.Class != null) && (this.classMethodsProtocols.MethodType != null);

            if (this.DeleteMethodMenuItem != null)
                this.DeleteMethodMenuItem.Enabled = (this.Class != null) && (this.classMethodsProtocols.MethodType != null) && !String.IsNullOrWhiteSpace(this.classMethodsList.MethodName);
            if (this.SaveMethodMenuItem != null)
                this.SaveMethodMenuItem.Enabled = (this.Class != null) && (this.classMethodsProtocols.MethodType != null) && !String.IsNullOrWhiteSpace(this.classMethodsList.MethodName);

        }

    }
}
