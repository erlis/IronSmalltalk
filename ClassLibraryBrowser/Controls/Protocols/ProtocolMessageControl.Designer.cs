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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols
{
    partial class ProtocolMessageControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TabControl tabMethod;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.Panel panel2;
            System.Windows.Forms.Panel panel3;
            System.Windows.Forms.Label label3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProtocolMessageControl));
            this.tabPageMethodDefinition = new System.Windows.Forms.TabPage();
            this.errorsControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.synopsisControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.txtMethodDocId = new System.Windows.Forms.TextBox();
            this.txtMethodSelector = new System.Windows.Forms.TextBox();
            this.tabPageMethodDescription = new System.Windows.Forms.TabPage();
            this.panelRefinementDescription = new System.Windows.Forms.Panel();
            this.buttonRefinementCancel = new System.Windows.Forms.Button();
            this.buttonRefinementOK = new System.Windows.Forms.Button();
            this.refinementDescription = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.panelRefinement = new System.Windows.Forms.Panel();
            this.buttonRemoveRefinement = new System.Windows.Forms.Button();
            this.buttonAddRefinement = new System.Windows.Forms.Button();
            this.comboRefinementProtocols = new System.Windows.Forms.ComboBox();
            this.listRefinement = new System.Windows.Forms.ListView();
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.tabPageMethodParameters = new System.Windows.Forms.TabPage();
            this.listParameters = new System.Windows.Forms.ListView();
            this.listParameterProtocol = new System.Windows.Forms.ListView();
            this.buttonAddParameterProtocol = new System.Windows.Forms.Button();
            this.comboParameterProtocols = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listReturnValueProtocols = new System.Windows.Forms.ListView();
            this.buttonAddReturnValueProtocol = new System.Windows.Forms.Button();
            this.comboReturnValueProtocols = new System.Windows.Forms.ComboBox();
            this.returnValueDescriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.comboReturnValueAliasing = new System.Windows.Forms.ComboBox();
            this.tabPageMethodSource = new System.Windows.Forms.TabPage();
            this.sourceEditControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.SourceEditControl();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuParameterAliasing = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unspecifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.capturedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncapturedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabMethod = new System.Windows.Forms.TabControl();
            label6 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            label2 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            groupBox1 = new System.Windows.Forms.GroupBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            panel1 = new System.Windows.Forms.Panel();
            groupBox2 = new System.Windows.Forms.GroupBox();
            panel2 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            tabMethod.SuspendLayout();
            this.tabPageMethodDefinition.SuspendLayout();
            this.tabPageMethodDescription.SuspendLayout();
            this.panelRefinementDescription.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            this.panelRefinement.SuspendLayout();
            this.tabPageMethodParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            this.tabPageMethodSource.SuspendLayout();
            this.contextMenuParameterAliasing.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMethod
            // 
            tabMethod.Controls.Add(this.tabPageMethodDefinition);
            tabMethod.Controls.Add(this.tabPageMethodDescription);
            tabMethod.Controls.Add(this.tabPageMethodParameters);
            tabMethod.Controls.Add(this.tabPageMethodSource);
            tabMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            tabMethod.ImageList = this.imageList;
            tabMethod.Location = new System.Drawing.Point(0, 0);
            tabMethod.Name = "tabMethod";
            tabMethod.SelectedIndex = 0;
            tabMethod.Size = new System.Drawing.Size(363, 354);
            tabMethod.TabIndex = 1;
            // 
            // tabPageMethodDefinition
            // 
            this.tabPageMethodDefinition.Controls.Add(this.errorsControl);
            this.tabPageMethodDefinition.Controls.Add(this.synopsisControl);
            this.tabPageMethodDefinition.Controls.Add(label6);
            this.tabPageMethodDefinition.Controls.Add(label4);
            this.tabPageMethodDefinition.Controls.Add(label1);
            this.tabPageMethodDefinition.Controls.Add(label7);
            this.tabPageMethodDefinition.Controls.Add(this.txtMethodDocId);
            this.tabPageMethodDefinition.Controls.Add(this.txtMethodSelector);
            this.tabPageMethodDefinition.ImageKey = "method";
            this.tabPageMethodDefinition.Location = new System.Drawing.Point(4, 23);
            this.tabPageMethodDefinition.Name = "tabPageMethodDefinition";
            this.tabPageMethodDefinition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMethodDefinition.Size = new System.Drawing.Size(355, 327);
            this.tabPageMethodDefinition.TabIndex = 0;
            this.tabPageMethodDefinition.Text = "Definition";
            this.tabPageMethodDefinition.UseVisualStyleBackColor = true;
            // 
            // errorsControl
            // 
            this.errorsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorsControl.Location = new System.Drawing.Point(72, 157);
            this.errorsControl.Margin = new System.Windows.Forms.Padding(0);
            this.errorsControl.Name = "errorsControl";
            this.errorsControl.ShowLabel = false;
            this.errorsControl.Size = new System.Drawing.Size(277, 163);
            this.errorsControl.TabIndex = 9;
            this.errorsControl.Changed += new System.EventHandler(this.errorsControl_Changed);
            // 
            // synopsisControl
            // 
            this.synopsisControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.synopsisControl.Location = new System.Drawing.Point(72, 55);
            this.synopsisControl.Margin = new System.Windows.Forms.Padding(0);
            this.synopsisControl.Name = "synopsisControl";
            this.synopsisControl.ShowLabel = false;
            this.synopsisControl.Size = new System.Drawing.Size(277, 95);
            this.synopsisControl.TabIndex = 9;
            this.synopsisControl.Changed += new System.EventHandler(this.synopsisControl_Changed);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(7, 9);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(38, 13);
            label6.TabIndex = 8;
            label6.Text = "Name:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 161);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(37, 13);
            label4.TabIndex = 7;
            label4.Text = "Errors:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 61);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 13);
            label1.TabIndex = 7;
            label1.Text = "Synopsis:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(7, 35);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(40, 13);
            label7.TabIndex = 7;
            label7.Text = "X3J20:";
            // 
            // txtMethodDocId
            // 
            this.txtMethodDocId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethodDocId.Location = new System.Drawing.Point(72, 32);
            this.txtMethodDocId.Name = "txtMethodDocId";
            this.txtMethodDocId.Size = new System.Drawing.Size(277, 20);
            this.txtMethodDocId.TabIndex = 6;
            this.txtMethodDocId.TextChanged += new System.EventHandler(this.txtMethodDocId_TextChanged);
            // 
            // txtMethodSelector
            // 
            this.txtMethodSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethodSelector.Location = new System.Drawing.Point(72, 6);
            this.txtMethodSelector.Name = "txtMethodSelector";
            this.txtMethodSelector.Size = new System.Drawing.Size(277, 20);
            this.txtMethodSelector.TabIndex = 5;
            this.txtMethodSelector.TextChanged += new System.EventHandler(this.txtMethodSelector_TextChanged);
            // 
            // tabPageMethodDescription
            // 
            this.tabPageMethodDescription.Controls.Add(this.panelRefinementDescription);
            this.tabPageMethodDescription.Controls.Add(this.panelRefinement);
            this.tabPageMethodDescription.Controls.Add(label2);
            this.tabPageMethodDescription.Controls.Add(this.descriptionControl);
            this.tabPageMethodDescription.ImageKey = "document";
            this.tabPageMethodDescription.Location = new System.Drawing.Point(4, 23);
            this.tabPageMethodDescription.Name = "tabPageMethodDescription";
            this.tabPageMethodDescription.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMethodDescription.Size = new System.Drawing.Size(355, 327);
            this.tabPageMethodDescription.TabIndex = 2;
            this.tabPageMethodDescription.Text = "Description";
            this.tabPageMethodDescription.UseVisualStyleBackColor = true;
            // 
            // panelRefinementDescription
            // 
            this.panelRefinementDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRefinementDescription.Controls.Add(tableLayoutPanel1);
            this.panelRefinementDescription.Controls.Add(this.refinementDescription);
            this.panelRefinementDescription.Location = new System.Drawing.Point(9, 164);
            this.panelRefinementDescription.Name = "panelRefinementDescription";
            this.panelRefinementDescription.Size = new System.Drawing.Size(338, 157);
            this.panelRefinementDescription.TabIndex = 19;
            this.panelRefinementDescription.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(this.buttonRefinementCancel, 1, 0);
            tableLayoutPanel1.Controls.Add(this.buttonRefinementOK, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(20, 123);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(298, 33);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // buttonRefinementCancel
            // 
            this.buttonRefinementCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRefinementCancel.Location = new System.Drawing.Point(152, 3);
            this.buttonRefinementCancel.Name = "buttonRefinementCancel";
            this.buttonRefinementCancel.Size = new System.Drawing.Size(143, 27);
            this.buttonRefinementCancel.TabIndex = 1;
            this.buttonRefinementCancel.Text = "Cancel";
            this.buttonRefinementCancel.UseVisualStyleBackColor = true;
            this.buttonRefinementCancel.Click += new System.EventHandler(this.buttonRefinementCancel_Click);
            // 
            // buttonRefinementOK
            // 
            this.buttonRefinementOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRefinementOK.Location = new System.Drawing.Point(3, 3);
            this.buttonRefinementOK.Name = "buttonRefinementOK";
            this.buttonRefinementOK.Size = new System.Drawing.Size(143, 27);
            this.buttonRefinementOK.TabIndex = 0;
            this.buttonRefinementOK.Text = "OK";
            this.buttonRefinementOK.UseVisualStyleBackColor = true;
            this.buttonRefinementOK.Click += new System.EventHandler(this.buttonRefinementOK_Click);
            // 
            // refinementDescription
            // 
            this.refinementDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.refinementDescription.Location = new System.Drawing.Point(20, 19);
            this.refinementDescription.Margin = new System.Windows.Forms.Padding(0);
            this.refinementDescription.Name = "refinementDescription";
            this.refinementDescription.ShowLabel = false;
            this.refinementDescription.Size = new System.Drawing.Size(298, 101);
            this.refinementDescription.TabIndex = 0;
            // 
            // panelRefinement
            // 
            this.panelRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRefinement.Controls.Add(this.buttonRemoveRefinement);
            this.panelRefinement.Controls.Add(this.buttonAddRefinement);
            this.panelRefinement.Controls.Add(this.comboRefinementProtocols);
            this.panelRefinement.Controls.Add(this.listRefinement);
            this.panelRefinement.Location = new System.Drawing.Point(9, 164);
            this.panelRefinement.Name = "panelRefinement";
            this.panelRefinement.Size = new System.Drawing.Size(338, 157);
            this.panelRefinement.TabIndex = 16;
            // 
            // buttonRemoveRefinement
            // 
            this.buttonRemoveRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveRefinement.Enabled = false;
            this.buttonRemoveRefinement.Location = new System.Drawing.Point(274, 0);
            this.buttonRemoveRefinement.Name = "buttonRemoveRefinement";
            this.buttonRemoveRefinement.Size = new System.Drawing.Size(64, 23);
            this.buttonRemoveRefinement.TabIndex = 18;
            this.buttonRemoveRefinement.Text = "Remove";
            this.buttonRemoveRefinement.UseVisualStyleBackColor = true;
            this.buttonRemoveRefinement.Click += new System.EventHandler(this.buttonRemoveRefinement_Click);
            // 
            // buttonAddRefinement
            // 
            this.buttonAddRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddRefinement.Enabled = false;
            this.buttonAddRefinement.Location = new System.Drawing.Point(223, 0);
            this.buttonAddRefinement.Name = "buttonAddRefinement";
            this.buttonAddRefinement.Size = new System.Drawing.Size(45, 23);
            this.buttonAddRefinement.TabIndex = 18;
            this.buttonAddRefinement.Text = "Add";
            this.buttonAddRefinement.UseVisualStyleBackColor = true;
            this.buttonAddRefinement.Click += new System.EventHandler(this.buttonAddRefinementProtocol_Click);
            // 
            // comboRefinementProtocols
            // 
            this.comboRefinementProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboRefinementProtocols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRefinementProtocols.FormattingEnabled = true;
            this.comboRefinementProtocols.Location = new System.Drawing.Point(0, 1);
            this.comboRefinementProtocols.Name = "comboRefinementProtocols";
            this.comboRefinementProtocols.Size = new System.Drawing.Size(217, 21);
            this.comboRefinementProtocols.TabIndex = 17;
            this.comboRefinementProtocols.SelectedIndexChanged += new System.EventHandler(this.comboRefinementProtocols_SelectedIndexChanged);
            this.comboRefinementProtocols.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.comboRefinementProtocols_Format);
            // 
            // listRefinement
            // 
            this.listRefinement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listRefinement.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
            this.listRefinement.FullRowSelect = true;
            this.listRefinement.HideSelection = false;
            this.listRefinement.Location = new System.Drawing.Point(0, 24);
            this.listRefinement.MultiSelect = false;
            this.listRefinement.Name = "listRefinement";
            this.listRefinement.ShowGroups = false;
            this.listRefinement.ShowItemToolTips = true;
            this.listRefinement.Size = new System.Drawing.Size(338, 133);
            this.listRefinement.TabIndex = 16;
            this.listRefinement.UseCompatibleStateImageBehavior = false;
            this.listRefinement.View = System.Windows.Forms.View.Details;
            this.listRefinement.SelectedIndexChanged += new System.EventHandler(this.listRefinement_SelectedIndexChanged);
            this.listRefinement.DoubleClick += new System.EventHandler(this.listRefinement_DoubleClick);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Protocol";
            columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Description";
            columnHeader2.Width = 300;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 148);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 13);
            label2.TabIndex = 14;
            label2.Text = "Refinement:";
            // 
            // descriptionControl
            // 
            this.descriptionControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionControl.Location = new System.Drawing.Point(8, 3);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = false;
            this.descriptionControl.Size = new System.Drawing.Size(339, 138);
            this.descriptionControl.TabIndex = 12;
            // 
            // tabPageMethodParameters
            // 
            this.tabPageMethodParameters.Controls.Add(splitContainer1);
            this.tabPageMethodParameters.ImageKey = "parameter";
            this.tabPageMethodParameters.Location = new System.Drawing.Point(4, 23);
            this.tabPageMethodParameters.Name = "tabPageMethodParameters";
            this.tabPageMethodParameters.Size = new System.Drawing.Size(355, 327);
            this.tabPageMethodParameters.TabIndex = 3;
            this.tabPageMethodParameters.Text = "Params";
            this.tabPageMethodParameters.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox2);
            splitContainer1.Size = new System.Drawing.Size(355, 327);
            splitContainer1.SplitterDistance = 163;
            splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(355, 163);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Parameters:";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(this.listParameters, 0, 0);
            tableLayoutPanel2.Controls.Add(panel1, 1, 0);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new System.Drawing.Size(349, 144);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // listParameters
            // 
            this.listParameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader3,
            columnHeader4});
            this.listParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listParameters.FullRowSelect = true;
            this.listParameters.HideSelection = false;
            this.listParameters.LabelEdit = true;
            this.listParameters.Location = new System.Drawing.Point(3, 3);
            this.listParameters.MultiSelect = false;
            this.listParameters.Name = "listParameters";
            this.listParameters.ShowGroups = false;
            this.listParameters.Size = new System.Drawing.Size(168, 138);
            this.listParameters.TabIndex = 2;
            this.listParameters.UseCompatibleStateImageBehavior = false;
            this.listParameters.View = System.Windows.Forms.View.Details;
            this.listParameters.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listParameters_AfterLabelEdit);
            this.listParameters.SelectedIndexChanged += new System.EventHandler(this.listParameters_SelectedIndexChanged);
            this.listParameters.DoubleClick += new System.EventHandler(this.listParameters_DoubleClick);
            this.listParameters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listParameters_KeyDown);
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Parameter";
            columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Aliasing";
            columnHeader4.Width = 80;
            // 
            // panel1
            // 
            panel1.Controls.Add(this.listParameterProtocol);
            panel1.Controls.Add(this.buttonAddParameterProtocol);
            panel1.Controls.Add(this.comboParameterProtocols);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(177, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(169, 138);
            panel1.TabIndex = 3;
            // 
            // listParameterProtocol
            // 
            this.listParameterProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listParameterProtocol.LabelWrap = false;
            this.listParameterProtocol.Location = new System.Drawing.Point(3, 30);
            this.listParameterProtocol.Name = "listParameterProtocol";
            this.listParameterProtocol.Size = new System.Drawing.Size(166, 108);
            this.listParameterProtocol.TabIndex = 7;
            this.listParameterProtocol.UseCompatibleStateImageBehavior = false;
            this.listParameterProtocol.View = System.Windows.Forms.View.List;
            this.listParameterProtocol.DoubleClick += new System.EventHandler(this.listParameterProtocol_DoubleClick);
            this.listParameterProtocol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listParameterProtocol_KeyDown);
            // 
            // buttonAddParameterProtocol
            // 
            this.buttonAddParameterProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddParameterProtocol.Location = new System.Drawing.Point(146, 2);
            this.buttonAddParameterProtocol.Name = "buttonAddParameterProtocol";
            this.buttonAddParameterProtocol.Size = new System.Drawing.Size(23, 23);
            this.buttonAddParameterProtocol.TabIndex = 6;
            this.buttonAddParameterProtocol.Text = "+";
            this.buttonAddParameterProtocol.UseVisualStyleBackColor = true;
            this.buttonAddParameterProtocol.Click += new System.EventHandler(this.buttonAddParameterProtocol_Click);
            // 
            // comboParameterProtocols
            // 
            this.comboParameterProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboParameterProtocols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboParameterProtocols.FormattingEnabled = true;
            this.comboParameterProtocols.Location = new System.Drawing.Point(3, 3);
            this.comboParameterProtocols.Name = "comboParameterProtocols";
            this.comboParameterProtocols.Size = new System.Drawing.Size(137, 21);
            this.comboParameterProtocols.TabIndex = 5;
            this.comboParameterProtocols.SelectedIndexChanged += new System.EventHandler(this.comboParameterProtocols_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.tableLayoutPanel3);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(355, 160);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Return Value:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(panel2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(panel3, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(349, 141);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(this.listReturnValueProtocols);
            panel2.Controls.Add(this.buttonAddReturnValueProtocol);
            panel2.Controls.Add(this.comboReturnValueProtocols);
            panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(177, 3);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(169, 135);
            panel2.TabIndex = 4;
            // 
            // listReturnValueProtocols
            // 
            this.listReturnValueProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listReturnValueProtocols.LabelWrap = false;
            this.listReturnValueProtocols.Location = new System.Drawing.Point(3, 30);
            this.listReturnValueProtocols.Name = "listReturnValueProtocols";
            this.listReturnValueProtocols.Size = new System.Drawing.Size(166, 105);
            this.listReturnValueProtocols.TabIndex = 7;
            this.listReturnValueProtocols.UseCompatibleStateImageBehavior = false;
            this.listReturnValueProtocols.View = System.Windows.Forms.View.List;
            this.listReturnValueProtocols.DoubleClick += new System.EventHandler(this.listReturnValueProtocols_DoubleClick);
            this.listReturnValueProtocols.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listReturnValueProtocols_KeyDown);
            // 
            // buttonAddReturnValueProtocol
            // 
            this.buttonAddReturnValueProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddReturnValueProtocol.Location = new System.Drawing.Point(146, 2);
            this.buttonAddReturnValueProtocol.Name = "buttonAddReturnValueProtocol";
            this.buttonAddReturnValueProtocol.Size = new System.Drawing.Size(23, 23);
            this.buttonAddReturnValueProtocol.TabIndex = 6;
            this.buttonAddReturnValueProtocol.Text = "+";
            this.buttonAddReturnValueProtocol.UseVisualStyleBackColor = true;
            this.buttonAddReturnValueProtocol.Click += new System.EventHandler(this.buttonAddReturnValueProtocol_Click);
            // 
            // comboReturnValueProtocols
            // 
            this.comboReturnValueProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboReturnValueProtocols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboReturnValueProtocols.FormattingEnabled = true;
            this.comboReturnValueProtocols.Location = new System.Drawing.Point(3, 3);
            this.comboReturnValueProtocols.Name = "comboReturnValueProtocols";
            this.comboReturnValueProtocols.Size = new System.Drawing.Size(137, 21);
            this.comboReturnValueProtocols.TabIndex = 5;
            this.comboReturnValueProtocols.SelectedIndexChanged += new System.EventHandler(this.comboReturnValueProtocols_SelectedIndexChanged);
            // 
            // panel3
            // 
            panel3.Controls.Add(this.returnValueDescriptionControl);
            panel3.Controls.Add(this.comboReturnValueAliasing);
            panel3.Controls.Add(label3);
            panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            panel3.Location = new System.Drawing.Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(168, 135);
            panel3.TabIndex = 5;
            // 
            // returnValueDescriptionControl
            // 
            this.returnValueDescriptionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.returnValueDescriptionControl.Location = new System.Drawing.Point(3, 27);
            this.returnValueDescriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.returnValueDescriptionControl.Name = "returnValueDescriptionControl";
            this.returnValueDescriptionControl.ShowLabel = false;
            this.returnValueDescriptionControl.Size = new System.Drawing.Size(165, 108);
            this.returnValueDescriptionControl.TabIndex = 2;
            this.returnValueDescriptionControl.Changed += new System.EventHandler(this.returnValueDescriptionControl_Changed);
            // 
            // comboReturnValueAliasing
            // 
            this.comboReturnValueAliasing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboReturnValueAliasing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboReturnValueAliasing.FormattingEnabled = true;
            this.comboReturnValueAliasing.Items.AddRange(new object[] {
            "Unspecified",
            "State",
            "New"});
            this.comboReturnValueAliasing.Location = new System.Drawing.Point(52, 3);
            this.comboReturnValueAliasing.Name = "comboReturnValueAliasing";
            this.comboReturnValueAliasing.Size = new System.Drawing.Size(116, 21);
            this.comboReturnValueAliasing.TabIndex = 1;
            this.comboReturnValueAliasing.SelectedIndexChanged += new System.EventHandler(this.comboReturnValueAliasing_SelectedIndexChanged);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(0, 7);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(46, 13);
            label3.TabIndex = 0;
            label3.Text = "Aliasing:";
            // 
            // tabPageMethodSource
            // 
            this.tabPageMethodSource.Controls.Add(this.sourceEditControl);
            this.tabPageMethodSource.ImageKey = "source";
            this.tabPageMethodSource.Location = new System.Drawing.Point(4, 23);
            this.tabPageMethodSource.Name = "tabPageMethodSource";
            this.tabPageMethodSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMethodSource.Size = new System.Drawing.Size(355, 327);
            this.tabPageMethodSource.TabIndex = 1;
            this.tabPageMethodSource.Text = "Source Code";
            this.tabPageMethodSource.UseVisualStyleBackColor = true;
            // 
            // sourceEditControl
            // 
            this.sourceEditControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceEditControl.Location = new System.Drawing.Point(6, 6);
            this.sourceEditControl.Name = "sourceEditControl";
            this.sourceEditControl.Size = new System.Drawing.Size(343, 315);
            this.sourceEditControl.Source = "";
            this.sourceEditControl.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "method");
            this.imageList.Images.SetKeyName(1, "field");
            this.imageList.Images.SetKeyName(2, "structure");
            this.imageList.Images.SetKeyName(3, "handshake");
            this.imageList.Images.SetKeyName(4, "enum");
            this.imageList.Images.SetKeyName(5, "class");
            this.imageList.Images.SetKeyName(6, "source");
            this.imageList.Images.SetKeyName(7, "document");
            this.imageList.Images.SetKeyName(8, "hierarchy");
            this.imageList.Images.SetKeyName(9, "parameter");
            // 
            // contextMenuParameterAliasing
            // 
            this.contextMenuParameterAliasing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unspecifiedToolStripMenuItem,
            this.capturedToolStripMenuItem,
            this.uncapturedToolStripMenuItem});
            this.contextMenuParameterAliasing.Name = "contextMenuParameterAliasing";
            this.contextMenuParameterAliasing.Size = new System.Drawing.Size(137, 70);
            // 
            // unspecifiedToolStripMenuItem
            // 
            this.unspecifiedToolStripMenuItem.Name = "unspecifiedToolStripMenuItem";
            this.unspecifiedToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.unspecifiedToolStripMenuItem.Text = "Unspecified";
            this.unspecifiedToolStripMenuItem.Click += new System.EventHandler(this.unspecifiedToolStripMenuItem_Click);
            // 
            // capturedToolStripMenuItem
            // 
            this.capturedToolStripMenuItem.Name = "capturedToolStripMenuItem";
            this.capturedToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.capturedToolStripMenuItem.Text = "Captured";
            this.capturedToolStripMenuItem.Click += new System.EventHandler(this.capturedToolStripMenuItem_Click);
            // 
            // uncapturedToolStripMenuItem
            // 
            this.uncapturedToolStripMenuItem.Name = "uncapturedToolStripMenuItem";
            this.uncapturedToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.uncapturedToolStripMenuItem.Text = "Uncaptured";
            this.uncapturedToolStripMenuItem.Click += new System.EventHandler(this.uncapturedToolStripMenuItem_Click);
            // 
            // ProtocolMessageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tabMethod);
            this.DoubleBuffered = true;
            this.Name = "ProtocolMessageControl";
            this.Size = new System.Drawing.Size(363, 354);
            tabMethod.ResumeLayout(false);
            this.tabPageMethodDefinition.ResumeLayout(false);
            this.tabPageMethodDefinition.PerformLayout();
            this.tabPageMethodDescription.ResumeLayout(false);
            this.tabPageMethodDescription.PerformLayout();
            this.panelRefinementDescription.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            this.panelRefinement.ResumeLayout(false);
            this.tabPageMethodParameters.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            this.tabPageMethodSource.ResumeLayout(false);
            this.contextMenuParameterAliasing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabPage tabPageMethodDefinition;
        public System.Windows.Forms.TabPage tabPageMethodDescription;
        public System.Windows.Forms.TabPage tabPageMethodSource;
        public System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox txtMethodDocId;
        private System.Windows.Forms.TextBox txtMethodSelector;
        private DescriptionControl descriptionControl;
        private System.Windows.Forms.Panel panelRefinementDescription;
        private System.Windows.Forms.Button buttonRefinementCancel;
        private System.Windows.Forms.Button buttonRefinementOK;
        private System.Windows.Forms.Panel panelRefinement;
        private System.Windows.Forms.Button buttonRemoveRefinement;
        private System.Windows.Forms.Button buttonAddRefinement;
        private System.Windows.Forms.ComboBox comboRefinementProtocols;
        private System.Windows.Forms.ListView listRefinement;
        private DescriptionControl refinementDescription;
        private System.Windows.Forms.TabPage tabPageMethodParameters;
        private System.Windows.Forms.ContextMenuStrip contextMenuParameterAliasing;
        private System.Windows.Forms.ToolStripMenuItem unspecifiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem capturedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncapturedToolStripMenuItem;
        private System.Windows.Forms.ListView listParameters;
        private System.Windows.Forms.ListView listParameterProtocol;
        private System.Windows.Forms.Button buttonAddParameterProtocol;
        private System.Windows.Forms.ComboBox comboParameterProtocols;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListView listReturnValueProtocols;
        private System.Windows.Forms.Button buttonAddReturnValueProtocol;
        private System.Windows.Forms.ComboBox comboReturnValueProtocols;
        private DescriptionControl returnValueDescriptionControl;
        private System.Windows.Forms.ComboBox comboReturnValueAliasing;
        private DescriptionControl synopsisControl;
        private DescriptionControl errorsControl;
        private SourceEditControl sourceEditControl;
    }
}
