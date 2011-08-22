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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Validation
{
    partial class ImplementationValidationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TabPage tabPageProtocols;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ColumnHeader columnHeader9;
            System.Windows.Forms.TabPage tabPageClasses;
            System.Windows.Forms.ColumnHeader columnHeader10;
            System.Windows.Forms.ColumnHeader columnHeader11;
            System.Windows.Forms.ColumnHeader columnHeader12;
            System.Windows.Forms.ColumnHeader columnHeader13;
            System.Windows.Forms.TabPage tabPageMethods;
            System.Windows.Forms.ColumnHeader columnHeader5;
            System.Windows.Forms.ColumnHeader columnHeader6;
            System.Windows.Forms.TabPage tabPageGlobals;
            System.Windows.Forms.TabPage tabPageNativeMethodNames;
            System.Windows.Forms.ColumnHeader columnHeader14;
            System.Windows.Forms.ColumnHeader columnHeader15;
            System.Windows.Forms.ColumnHeader columnHeader16;
            System.Windows.Forms.ColumnHeader columnHeader17;
            this.listProtocols = new System.Windows.Forms.ListView();
            this.listClasses = new System.Windows.Forms.ListView();
            this.listMethods = new System.Windows.Forms.ListView();
            this.listGlobals = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listNativeMethodNames = new System.Windows.Forms.ListView();
            this.tabControl = new System.Windows.Forms.TabControl();
            tabPageProtocols = new System.Windows.Forms.TabPage();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tabPageClasses = new System.Windows.Forms.TabPage();
            columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tabPageMethods = new System.Windows.Forms.TabPage();
            columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tabPageGlobals = new System.Windows.Forms.TabPage();
            tabPageNativeMethodNames = new System.Windows.Forms.TabPage();
            columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tabPageProtocols.SuspendLayout();
            tabPageClasses.SuspendLayout();
            tabPageMethods.SuspendLayout();
            tabPageGlobals.SuspendLayout();
            tabPageNativeMethodNames.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageProtocols
            // 
            tabPageProtocols.Controls.Add(this.listProtocols);
            tabPageProtocols.Location = new System.Drawing.Point(4, 22);
            tabPageProtocols.Name = "tabPageProtocols";
            tabPageProtocols.Padding = new System.Windows.Forms.Padding(3);
            tabPageProtocols.Size = new System.Drawing.Size(489, 312);
            tabPageProtocols.TabIndex = 0;
            tabPageProtocols.Text = "Protocols";
            tabPageProtocols.UseVisualStyleBackColor = true;
            // 
            // listProtocols
            // 
            this.listProtocols.AllowColumnReorder = true;
            this.listProtocols.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2,
            columnHeader3,
            columnHeader4,
            columnHeader9});
            this.listProtocols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listProtocols.FullRowSelect = true;
            this.listProtocols.GridLines = true;
            this.listProtocols.HideSelection = false;
            this.listProtocols.Location = new System.Drawing.Point(3, 3);
            this.listProtocols.MultiSelect = false;
            this.listProtocols.Name = "listProtocols";
            this.listProtocols.ShowGroups = false;
            this.listProtocols.Size = new System.Drawing.Size(483, 306);
            this.listProtocols.TabIndex = 0;
            this.listProtocols.UseCompatibleStateImageBehavior = false;
            this.listProtocols.View = System.Windows.Forms.View.Details;
            this.listProtocols.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listProtocols_ColumnClick);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Protocol";
            columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Abs.";
            columnHeader2.Width = 35;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Conforms To";
            columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Implemented By";
            columnHeader4.Width = 150;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "Conforms To incl. Inherited";
            columnHeader9.Width = 150;
            // 
            // tabPageClasses
            // 
            tabPageClasses.Controls.Add(this.listClasses);
            tabPageClasses.Location = new System.Drawing.Point(4, 22);
            tabPageClasses.Name = "tabPageClasses";
            tabPageClasses.Padding = new System.Windows.Forms.Padding(3);
            tabPageClasses.Size = new System.Drawing.Size(489, 312);
            tabPageClasses.TabIndex = 1;
            tabPageClasses.Text = "Classes";
            tabPageClasses.UseVisualStyleBackColor = true;
            // 
            // listClasses
            // 
            this.listClasses.AllowColumnReorder = true;
            this.listClasses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader10,
            columnHeader11,
            columnHeader12,
            columnHeader13});
            this.listClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listClasses.FullRowSelect = true;
            this.listClasses.GridLines = true;
            this.listClasses.HideSelection = false;
            this.listClasses.Location = new System.Drawing.Point(3, 3);
            this.listClasses.MultiSelect = false;
            this.listClasses.Name = "listClasses";
            this.listClasses.ShowGroups = false;
            this.listClasses.Size = new System.Drawing.Size(483, 306);
            this.listClasses.TabIndex = 1;
            this.listClasses.UseCompatibleStateImageBehavior = false;
            this.listClasses.View = System.Windows.Forms.View.Details;
            this.listClasses.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listClasses_ColumnClick);
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "Class";
            columnHeader10.Width = 100;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "Implements Inst.";
            columnHeader11.Width = 180;
            // 
            // columnHeader12
            // 
            columnHeader12.Text = "Implements Cls.";
            columnHeader12.Width = 180;
            // 
            // columnHeader13
            // 
            columnHeader13.Text = "Missing";
            columnHeader13.Width = 120;
            // 
            // tabPageMethods
            // 
            tabPageMethods.Controls.Add(this.listMethods);
            tabPageMethods.Location = new System.Drawing.Point(4, 22);
            tabPageMethods.Name = "tabPageMethods";
            tabPageMethods.Size = new System.Drawing.Size(489, 312);
            tabPageMethods.TabIndex = 2;
            tabPageMethods.Text = "Methods";
            tabPageMethods.UseVisualStyleBackColor = true;
            // 
            // listMethods
            // 
            this.listMethods.AllowColumnReorder = true;
            this.listMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMethods.FullRowSelect = true;
            this.listMethods.GridLines = true;
            this.listMethods.HideSelection = false;
            this.listMethods.Location = new System.Drawing.Point(0, 0);
            this.listMethods.MultiSelect = false;
            this.listMethods.Name = "listMethods";
            this.listMethods.ShowGroups = false;
            this.listMethods.Size = new System.Drawing.Size(489, 312);
            this.listMethods.TabIndex = 1;
            this.listMethods.UseCompatibleStateImageBehavior = false;
            this.listMethods.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Global";
            columnHeader5.Width = 120;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Defined In";
            columnHeader6.Width = 120;
            // 
            // tabPageGlobals
            // 
            tabPageGlobals.Controls.Add(this.listGlobals);
            tabPageGlobals.Location = new System.Drawing.Point(4, 22);
            tabPageGlobals.Name = "tabPageGlobals";
            tabPageGlobals.Size = new System.Drawing.Size(489, 312);
            tabPageGlobals.TabIndex = 3;
            tabPageGlobals.Text = "Globals";
            tabPageGlobals.UseVisualStyleBackColor = true;
            // 
            // listGlobals
            // 
            this.listGlobals.AllowColumnReorder = true;
            this.listGlobals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader5,
            columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listGlobals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGlobals.FullRowSelect = true;
            this.listGlobals.GridLines = true;
            this.listGlobals.HideSelection = false;
            this.listGlobals.Location = new System.Drawing.Point(0, 0);
            this.listGlobals.MultiSelect = false;
            this.listGlobals.Name = "listGlobals";
            this.listGlobals.ShowGroups = false;
            this.listGlobals.Size = new System.Drawing.Size(489, 312);
            this.listGlobals.TabIndex = 0;
            this.listGlobals.UseCompatibleStateImageBehavior = false;
            this.listGlobals.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Type";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Implemented By";
            this.columnHeader8.Width = 120;
            // 
            // tabPageNativeMethodNames
            // 
            tabPageNativeMethodNames.Controls.Add(this.listNativeMethodNames);
            tabPageNativeMethodNames.Location = new System.Drawing.Point(4, 22);
            tabPageNativeMethodNames.Name = "tabPageNativeMethodNames";
            tabPageNativeMethodNames.Padding = new System.Windows.Forms.Padding(3);
            tabPageNativeMethodNames.Size = new System.Drawing.Size(489, 312);
            tabPageNativeMethodNames.TabIndex = 4;
            tabPageNativeMethodNames.Text = "Native Method Names";
            tabPageNativeMethodNames.UseVisualStyleBackColor = true;
            // 
            // listNativeMethodNames
            // 
            this.listNativeMethodNames.AllowColumnReorder = true;
            this.listNativeMethodNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader14,
            columnHeader15,
            columnHeader16,
            columnHeader17});
            this.listNativeMethodNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listNativeMethodNames.FullRowSelect = true;
            this.listNativeMethodNames.GridLines = true;
            this.listNativeMethodNames.HideSelection = false;
            this.listNativeMethodNames.Location = new System.Drawing.Point(3, 3);
            this.listNativeMethodNames.MultiSelect = false;
            this.listNativeMethodNames.Name = "listNativeMethodNames";
            this.listNativeMethodNames.ShowGroups = false;
            this.listNativeMethodNames.Size = new System.Drawing.Size(483, 306);
            this.listNativeMethodNames.TabIndex = 2;
            this.listNativeMethodNames.UseCompatibleStateImageBehavior = false;
            this.listNativeMethodNames.View = System.Windows.Forms.View.Details;
            this.listNativeMethodNames.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listNativeMethodNames_ColumnClick);
            // 
            // columnHeader14
            // 
            columnHeader14.Text = "Class";
            columnHeader14.Width = 100;
            // 
            // columnHeader15
            // 
            columnHeader15.Text = "Method";
            columnHeader15.Width = 180;
            // 
            // columnHeader16
            // 
            columnHeader16.Text = "Native Name";
            columnHeader16.Width = 180;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(tabPageProtocols);
            this.tabControl.Controls.Add(tabPageGlobals);
            this.tabControl.Controls.Add(tabPageClasses);
            this.tabControl.Controls.Add(tabPageMethods);
            this.tabControl.Controls.Add(tabPageNativeMethodNames);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(497, 338);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            // 
            // columnHeader17
            // 
            columnHeader17.Text = "Conflict";
            // 
            // ImplementationValidationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Enabled = false;
            this.Name = "ImplementationValidationControl";
            this.Size = new System.Drawing.Size(497, 338);
            tabPageProtocols.ResumeLayout(false);
            tabPageClasses.ResumeLayout(false);
            tabPageMethods.ResumeLayout(false);
            tabPageGlobals.ResumeLayout(false);
            tabPageNativeMethodNames.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ListView listProtocols;
        private System.Windows.Forms.ListView listGlobals;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ListView listClasses;
        private System.Windows.Forms.ListView listMethods;
        private System.Windows.Forms.ListView listNativeMethodNames;
    }
}
