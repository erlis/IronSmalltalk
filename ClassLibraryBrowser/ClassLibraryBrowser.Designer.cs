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

namespace IronSmalltalk.Tools.ClassLibraryBrowser
{
    partial class ClassLibraryBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassLibraryBrowser));
            IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators.ValueHolder<IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation.Global> valueHolder_11 = new IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators.ValueHolder<IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation.Global>();
            IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators.ValueHolder<IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation.Class> valueHolder_12 = new IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators.ValueHolder<IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation.Class>();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lastWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabProtocols = new System.Windows.Forms.TabPage();
            this.protocolsControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ProtocolsControl();
            this.tabGlobals = new System.Windows.Forms.TabPage();
            this.globalsControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Globals.GlobalsControl();
            this.tabClassHierarchy = new System.Windows.Forms.TabPage();
            this.classesControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassesControl();
            this.tabMethods = new System.Windows.Forms.TabPage();
            this.classMethodsEditor = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassMethodsEditorControl();
            this.tabPools = new System.Windows.Forms.TabPage();
            this.poolsControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools.PoolsControl();
            this.tabInitializers = new System.Windows.Forms.TabPage();
            this.initializersControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Initializers.InitializersControl();
            this.tabValidate = new System.Windows.Forms.TabPage();
            this.implementationValidationControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Validation.ImplementationValidationControl();
            this.fileOutSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabProtocols.SuspendLayout();
            this.tabGlobals.SuspendLayout();
            this.tabClassHierarchy.SuspendLayout();
            this.tabMethods.SuspendLayout();
            this.tabPools.SuspendLayout();
            this.tabInitializers.SuspendLayout();
            this.tabValidate.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "method");
            this.imageList.Images.SetKeyName(1, "field");
            this.imageList.Images.SetKeyName(2, "handshake");
            this.imageList.Images.SetKeyName(3, "enum");
            this.imageList.Images.SetKeyName(4, "class");
            this.imageList.Images.SetKeyName(5, "source");
            this.imageList.Images.SetKeyName(6, "document");
            this.imageList.Images.SetKeyName(7, "hierarchy");
            this.imageList.Images.SetKeyName(8, "structure");
            this.imageList.Images.SetKeyName(9, "parameter");
            this.imageList.Images.SetKeyName(10, "validate");
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.workspaceToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(649, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.fileOutToolStripMenuItem,
            this.openLastToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.fileToolStripMenuItem.MergeIndex = 0;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // fileOutToolStripMenuItem
            // 
            this.fileOutToolStripMenuItem.Enabled = false;
            this.fileOutToolStripMenuItem.Name = "fileOutToolStripMenuItem";
            this.fileOutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.fileOutToolStripMenuItem.Text = "File Out ...";
            this.fileOutToolStripMenuItem.Click += new System.EventHandler(this.fileOutToolStripMenuItem_Click);
            // 
            // openLastToolStripMenuItem
            // 
            this.openLastToolStripMenuItem.Name = "openLastToolStripMenuItem";
            this.openLastToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openLastToolStripMenuItem.Text = "Open Last ..";
            this.openLastToolStripMenuItem.Visible = false;
            this.openLastToolStripMenuItem.Click += new System.EventHandler(this.openLastToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // workspaceToolStripMenuItem
            // 
            this.workspaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyWorkspaceToolStripMenuItem,
            this.newWorkspaceToolStripMenuItem,
            this.lastWorkspaceToolStripMenuItem});
            this.workspaceToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.workspaceToolStripMenuItem.MergeIndex = 6000;
            this.workspaceToolStripMenuItem.Name = "workspaceToolStripMenuItem";
            this.workspaceToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.workspaceToolStripMenuItem.Text = "Workspace";
            // 
            // emptyWorkspaceToolStripMenuItem
            // 
            this.emptyWorkspaceToolStripMenuItem.Name = "emptyWorkspaceToolStripMenuItem";
            this.emptyWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.emptyWorkspaceToolStripMenuItem.Text = "Empty Workspace...";
            this.emptyWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.emptyWorkspaceToolStripMenuItem_Click);
            // 
            // newWorkspaceToolStripMenuItem
            // 
            this.newWorkspaceToolStripMenuItem.Name = "newWorkspaceToolStripMenuItem";
            this.newWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.newWorkspaceToolStripMenuItem.Text = "New Workspace...";
            this.newWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.newWorkspaceToolStripMenuItem_Click);
            // 
            // lastWorkspaceToolStripMenuItem
            // 
            this.lastWorkspaceToolStripMenuItem.Name = "lastWorkspaceToolStripMenuItem";
            this.lastWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.lastWorkspaceToolStripMenuItem.Text = "Last Workspace...";
            this.lastWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.lastWorkspaceToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xml";
            this.openFileDialog.FileName = "SystemDescription.xml";
            this.openFileDialog.Filter = "XML files|*.xml|All files|*.*";
            this.openFileDialog.Title = "Open Smalltalk Class Library Definition File";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xml";
            this.saveFileDialog.FileName = "SystemDescription.xml";
            this.saveFileDialog.Filter = "XML files|*.xml|All files|*.*";
            this.saveFileDialog.Title = "Save Smalltalk Class Library Definition File";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabProtocols);
            this.tabControlMain.Controls.Add(this.tabGlobals);
            this.tabControlMain.Controls.Add(this.tabClassHierarchy);
            this.tabControlMain.Controls.Add(this.tabMethods);
            this.tabControlMain.Controls.Add(this.tabPools);
            this.tabControlMain.Controls.Add(this.tabInitializers);
            this.tabControlMain.Controls.Add(this.tabValidate);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageList;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(649, 460);
            this.tabControlMain.TabIndex = 5;
            this.tabControlMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlMain_Selected);
            // 
            // tabProtocols
            // 
            this.tabProtocols.Controls.Add(this.protocolsControl);
            this.tabProtocols.ImageKey = "handshake";
            this.tabProtocols.Location = new System.Drawing.Point(4, 23);
            this.tabProtocols.Name = "tabProtocols";
            this.tabProtocols.Padding = new System.Windows.Forms.Padding(3);
            this.tabProtocols.Size = new System.Drawing.Size(641, 433);
            this.tabProtocols.TabIndex = 0;
            this.tabProtocols.Text = "Protocols";
            this.tabProtocols.UseVisualStyleBackColor = true;
            // 
            // protocolsControl
            // 
            this.protocolsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.protocolsControl.Location = new System.Drawing.Point(3, 3);
            this.protocolsControl.Name = "protocolsControl";
            this.protocolsControl.Size = new System.Drawing.Size(635, 427);
            this.protocolsControl.SystemDescriptionHolder = null;
            this.protocolsControl.TabIndex = 0;
            // 
            // tabGlobals
            // 
            this.tabGlobals.Controls.Add(this.globalsControl);
            this.tabGlobals.ImageKey = "structure";
            this.tabGlobals.Location = new System.Drawing.Point(4, 23);
            this.tabGlobals.Name = "tabGlobals";
            this.tabGlobals.Padding = new System.Windows.Forms.Padding(3);
            this.tabGlobals.Size = new System.Drawing.Size(641, 433);
            this.tabGlobals.TabIndex = 1;
            this.tabGlobals.Text = "Globals";
            this.tabGlobals.UseVisualStyleBackColor = true;
            // 
            // globalsControl
            // 
            this.globalsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalsControl.Enabled = false;
            valueHolder_11.Value = null;
            this.globalsControl.GlobalHolder = valueHolder_11;
            this.globalsControl.Location = new System.Drawing.Point(3, 3);
            this.globalsControl.Name = "globalsControl";
            this.globalsControl.Size = new System.Drawing.Size(635, 427);
            this.globalsControl.SystemImplementationHolder = null;
            this.globalsControl.TabIndex = 0;
            // 
            // tabClassHierarchy
            // 
            this.tabClassHierarchy.Controls.Add(this.classesControl);
            this.tabClassHierarchy.ImageKey = "class";
            this.tabClassHierarchy.Location = new System.Drawing.Point(4, 23);
            this.tabClassHierarchy.Name = "tabClassHierarchy";
            this.tabClassHierarchy.Size = new System.Drawing.Size(641, 433);
            this.tabClassHierarchy.TabIndex = 2;
            this.tabClassHierarchy.Text = "Class Hierarchy";
            this.tabClassHierarchy.UseVisualStyleBackColor = true;
            // 
            // classesControl
            // 
            this.classesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classesControl.Location = new System.Drawing.Point(0, 0);
            this.classesControl.Name = "classesControl";
            this.classesControl.Size = new System.Drawing.Size(641, 433);
            this.classesControl.SystemImplementationHolder = null;
            this.classesControl.TabIndex = 0;
            // 
            // tabMethods
            // 
            this.tabMethods.Controls.Add(this.classMethodsEditor);
            this.tabMethods.ImageKey = "method";
            this.tabMethods.Location = new System.Drawing.Point(4, 23);
            this.tabMethods.Name = "tabMethods";
            this.tabMethods.Size = new System.Drawing.Size(641, 433);
            this.tabMethods.TabIndex = 6;
            this.tabMethods.Text = "Methods";
            this.tabMethods.UseVisualStyleBackColor = true;
            // 
            // classMethodsEditor
            // 
            valueHolder_12.Value = null;
            this.classMethodsEditor.ClassHolder = valueHolder_12;
            this.classMethodsEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classMethodsEditor.Location = new System.Drawing.Point(0, 0);
            this.classMethodsEditor.Name = "classMethodsEditor";
            this.classMethodsEditor.Size = new System.Drawing.Size(641, 433);
            this.classMethodsEditor.SystemImplementationHolder = null;
            this.classMethodsEditor.TabIndex = 0;
            // 
            // tabPools
            // 
            this.tabPools.Controls.Add(this.poolsControl);
            this.tabPools.ImageKey = "enum";
            this.tabPools.Location = new System.Drawing.Point(4, 23);
            this.tabPools.Name = "tabPools";
            this.tabPools.Size = new System.Drawing.Size(641, 433);
            this.tabPools.TabIndex = 3;
            this.tabPools.Text = "Pools";
            this.tabPools.UseVisualStyleBackColor = true;
            // 
            // poolsControl
            // 
            this.poolsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poolsControl.Location = new System.Drawing.Point(0, 0);
            this.poolsControl.Name = "poolsControl";
            this.poolsControl.Size = new System.Drawing.Size(641, 433);
            this.poolsControl.SystemImplementationHolder = null;
            this.poolsControl.TabIndex = 0;
            // 
            // tabInitializers
            // 
            this.tabInitializers.Controls.Add(this.initializersControl);
            this.tabInitializers.ImageKey = "source";
            this.tabInitializers.Location = new System.Drawing.Point(4, 23);
            this.tabInitializers.Name = "tabInitializers";
            this.tabInitializers.Size = new System.Drawing.Size(641, 433);
            this.tabInitializers.TabIndex = 4;
            this.tabInitializers.Text = "Initializers";
            this.tabInitializers.UseVisualStyleBackColor = true;
            // 
            // initializersControl
            // 
            this.initializersControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.initializersControl.Location = new System.Drawing.Point(0, 0);
            this.initializersControl.Name = "initializersControl";
            this.initializersControl.Size = new System.Drawing.Size(641, 433);
            this.initializersControl.SystemImplementationHolder = null;
            this.initializersControl.TabIndex = 0;
            // 
            // tabValidate
            // 
            this.tabValidate.Controls.Add(this.implementationValidationControl);
            this.tabValidate.ImageKey = "validate";
            this.tabValidate.Location = new System.Drawing.Point(4, 23);
            this.tabValidate.Name = "tabValidate";
            this.tabValidate.Padding = new System.Windows.Forms.Padding(3);
            this.tabValidate.Size = new System.Drawing.Size(641, 433);
            this.tabValidate.TabIndex = 5;
            this.tabValidate.Text = "Validate";
            this.tabValidate.UseVisualStyleBackColor = true;
            // 
            // implementationValidationControl
            // 
            this.implementationValidationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.implementationValidationControl.Enabled = false;
            this.implementationValidationControl.Location = new System.Drawing.Point(3, 3);
            this.implementationValidationControl.Name = "implementationValidationControl";
            this.implementationValidationControl.Size = new System.Drawing.Size(635, 427);
            this.implementationValidationControl.SmalltalkSystemHolder = null;
            this.implementationValidationControl.TabIndex = 0;
            // 
            // fileOutSaveDialog
            // 
            this.fileOutSaveDialog.DefaultExt = "ist";
            this.fileOutSaveDialog.FileName = "IronSmalltalk.ist";
            this.fileOutSaveDialog.Filter = "IronSmalltalk files|*.ist|Smalltalk files|*.st|All files|*.*";
            this.fileOutSaveDialog.Title = "File-out Smalltalk Interchange Format File";
            // 
            // ClassLibraryBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 484);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ClassLibraryBrowser";
            this.Text = "Class Library Browser";
            this.Load += new System.EventHandler(this.ClassLibraryBrowser_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabProtocols.ResumeLayout(false);
            this.tabGlobals.ResumeLayout(false);
            this.tabClassHierarchy.ResumeLayout(false);
            this.tabMethods.ResumeLayout(false);
            this.tabPools.ResumeLayout(false);
            this.tabInitializers.ResumeLayout(false);
            this.tabValidate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ImageList imageList;
        public System.Windows.Forms.MenuStrip menuStrip;
        public System.Windows.Forms.OpenFileDialog openFileDialog;
        public System.Windows.Forms.SaveFileDialog saveFileDialog;
        public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openLastToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem fileOutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabProtocols;
        private System.Windows.Forms.TabPage tabGlobals;
        private System.Windows.Forms.TabPage tabClassHierarchy;
        private System.Windows.Forms.TabPage tabPools;
        private System.Windows.Forms.TabPage tabInitializers;
        private Controls.Protocols.ProtocolsControl protocolsControl;
        private Controls.Classes.ClassesControl classesControl;
        private System.Windows.Forms.TabPage tabValidate;
        private Controls.Validation.ImplementationValidationControl implementationValidationControl;
        private Controls.Globals.GlobalsControl globalsControl;
        private System.Windows.Forms.TabPage tabMethods;
        private Controls.Classes.ClassMethodsEditorControl classMethodsEditor;
        public System.Windows.Forms.SaveFileDialog fileOutSaveDialog;
        private System.Windows.Forms.ToolStripMenuItem workspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emptyWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lastWorkspaceToolStripMenuItem;
        private Controls.Pools.PoolsControl poolsControl;
        private Controls.Initializers.InitializersControl initializersControl;

    }
}

