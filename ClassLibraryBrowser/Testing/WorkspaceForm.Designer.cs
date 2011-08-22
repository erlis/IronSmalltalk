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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Testing
{
    partial class WorkspaceForm
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
            System.Windows.Forms.MenuStrip menuStrip;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.ColumnHeader columnHeader;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkspaceForm));
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evaluateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inspectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.newEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtEvaluate = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtInstall = new System.Windows.Forms.TextBox();
            this.listErrors = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageEvaluate = new System.Windows.Forms.TabPage();
            this.tabPageInstall = new System.Windows.Forms.TabPage();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            menuStrip = new System.Windows.Forms.MenuStrip();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageEvaluate.SuspendLayout();
            this.tabPageInstall.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.workspaceToolStripMenuItem});
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(670, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // workspaceToolStripMenuItem
            // 
            this.workspaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.evaluateToolStripMenuItem,
            this.inspectToolStripMenuItem,
            this.installToolStripMenuItem,
            this.toolStripSeparator2,
            this.newEnvironmentToolStripMenuItem,
            this.poolsToolStripMenuItem,
            this.variablesToolStripMenuItem});
            this.workspaceToolStripMenuItem.Name = "workspaceToolStripMenuItem";
            this.workspaceToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.workspaceToolStripMenuItem.Text = "&Workspace";
            // 
            // evaluateToolStripMenuItem
            // 
            this.evaluateToolStripMenuItem.Enabled = false;
            this.evaluateToolStripMenuItem.Name = "evaluateToolStripMenuItem";
            this.evaluateToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.evaluateToolStripMenuItem.Text = "&Evaluate";
            this.evaluateToolStripMenuItem.Click += new System.EventHandler(this.evaluateToolStripMenuItem_Click);
            // 
            // inspectToolStripMenuItem
            // 
            this.inspectToolStripMenuItem.Enabled = false;
            this.inspectToolStripMenuItem.Name = "inspectToolStripMenuItem";
            this.inspectToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.inspectToolStripMenuItem.Text = "&Inspect";
            this.inspectToolStripMenuItem.Click += new System.EventHandler(this.inspectToolStripMenuItem_Click);
            // 
            // installToolStripMenuItem
            // 
            this.installToolStripMenuItem.Enabled = false;
            this.installToolStripMenuItem.Name = "installToolStripMenuItem";
            this.installToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.installToolStripMenuItem.Text = "I&nstall";
            this.installToolStripMenuItem.Click += new System.EventHandler(this.installToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            // 
            // newEnvironmentToolStripMenuItem
            // 
            this.newEnvironmentToolStripMenuItem.Name = "newEnvironmentToolStripMenuItem";
            this.newEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.newEnvironmentToolStripMenuItem.Text = "&New Environment";
            this.newEnvironmentToolStripMenuItem.Click += new System.EventHandler(this.newEnvironmentToolStripMenuItem_Click);
            // 
            // poolsToolStripMenuItem
            // 
            this.poolsToolStripMenuItem.Enabled = false;
            this.poolsToolStripMenuItem.Name = "poolsToolStripMenuItem";
            this.poolsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.poolsToolStripMenuItem.Text = "Pools...";
            // 
            // variablesToolStripMenuItem
            // 
            this.variablesToolStripMenuItem.Enabled = false;
            this.variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
            this.variablesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.variablesToolStripMenuItem.Text = "Variables...";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.Location = new System.Drawing.Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.txtEvaluate);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.txtResult);
            splitContainer1.Size = new System.Drawing.Size(656, 314);
            splitContainer1.SplitterDistance = 200;
            splitContainer1.TabIndex = 0;
            // 
            // txtEvaluate
            // 
            this.txtEvaluate.AcceptsTab = true;
            this.txtEvaluate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEvaluate.HideSelection = false;
            this.txtEvaluate.Location = new System.Drawing.Point(0, 0);
            this.txtEvaluate.MaxLength = 0;
            this.txtEvaluate.Multiline = true;
            this.txtEvaluate.Name = "txtEvaluate";
            this.txtEvaluate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEvaluate.Size = new System.Drawing.Size(656, 200);
            this.txtEvaluate.TabIndex = 0;
            this.txtEvaluate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEvaluate_KeyPress);
            // 
            // txtResult
            // 
            this.txtResult.AcceptsTab = true;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(0, 0);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(656, 110);
            this.txtResult.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer2.Location = new System.Drawing.Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this.txtInstall);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.listErrors);
            splitContainer2.Size = new System.Drawing.Size(656, 314);
            splitContainer2.SplitterDistance = 200;
            splitContainer2.TabIndex = 1;
            // 
            // txtInstall
            // 
            this.txtInstall.AcceptsTab = true;
            this.txtInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInstall.HideSelection = false;
            this.txtInstall.Location = new System.Drawing.Point(0, 0);
            this.txtInstall.MaxLength = 0;
            this.txtInstall.Multiline = true;
            this.txtInstall.Name = "txtInstall";
            this.txtInstall.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInstall.Size = new System.Drawing.Size(656, 200);
            this.txtInstall.TabIndex = 1;
            this.txtInstall.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstall_KeyPress);
            // 
            // listErrors
            // 
            this.listErrors.AutoArrange = false;
            this.listErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            columnHeader});
            this.listErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listErrors.FullRowSelect = true;
            this.listErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listErrors.HideSelection = false;
            this.listErrors.Location = new System.Drawing.Point(0, 0);
            this.listErrors.MultiSelect = false;
            this.listErrors.Name = "listErrors";
            this.listErrors.ShowGroups = false;
            this.listErrors.ShowItemToolTips = true;
            this.listErrors.Size = new System.Drawing.Size(656, 110);
            this.listErrors.TabIndex = 0;
            this.listErrors.UseCompatibleStateImageBehavior = false;
            this.listErrors.View = System.Windows.Forms.View.Details;
            this.listErrors.SelectedIndexChanged += new System.EventHandler(this.listErrors_SelectedIndexChanged);
            this.listErrors.Resize += new System.EventHandler(this.listErrors_Resize);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 150;
            // 
            // columnHeader
            // 
            columnHeader.Width = 500;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageEvaluate);
            this.tabControl.Controls.Add(this.tabPageInstall);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(670, 346);
            this.tabControl.TabIndex = 2;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // tabPageEvaluate
            // 
            this.tabPageEvaluate.Controls.Add(splitContainer1);
            this.tabPageEvaluate.Location = new System.Drawing.Point(4, 22);
            this.tabPageEvaluate.Name = "tabPageEvaluate";
            this.tabPageEvaluate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvaluate.Size = new System.Drawing.Size(662, 320);
            this.tabPageEvaluate.TabIndex = 0;
            this.tabPageEvaluate.Text = "Evaluate";
            this.tabPageEvaluate.UseVisualStyleBackColor = true;
            // 
            // tabPageInstall
            // 
            this.tabPageInstall.Controls.Add(splitContainer2);
            this.tabPageInstall.Location = new System.Drawing.Point(4, 22);
            this.tabPageInstall.Name = "tabPageInstall";
            this.tabPageInstall.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInstall.Size = new System.Drawing.Size(662, 320);
            this.tabPageInstall.TabIndex = 1;
            this.tabPageInstall.Text = "Install";
            this.tabPageInstall.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "ist";
            this.openFileDialog.Filter = "IronSmalltalk files|*.ist|Smalltalk files|*.st|All files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "ist";
            this.saveFileDialog.Filter = "IronSmalltalk files|*.ist|Smalltalk files|*.st|All files|*.*";
            // 
            // WorkspaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 370);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorkspaceForm";
            this.Text = "Workspace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkspaceForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WorkspaceForm_FormClosed);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).EndInit();
            splitContainer2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageEvaluate.ResumeLayout(false);
            this.tabPageInstall.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evaluateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem poolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newEnvironmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inspectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageEvaluate;
        private System.Windows.Forms.TextBox txtEvaluate;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TabPage tabPageInstall;
        private System.Windows.Forms.TextBox txtInstall;
        private System.Windows.Forms.ListView listErrors;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
