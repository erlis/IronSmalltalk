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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools
{
    partial class PoolValuesControl
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
            System.Windows.Forms.SplitContainer splitContainer10;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PoolValuesControl));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label17;
            System.Windows.Forms.Label label16;
            this.listPoolVars = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.txtSourceCode = new System.Windows.Forms.TextBox();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.txtPoolVarName = new System.Windows.Forms.TextBox();
            splitContainer10 = new System.Windows.Forms.SplitContainer();
            columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            label1 = new System.Windows.Forms.Label();
            label17 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(splitContainer10)).BeginInit();
            splitContainer10.Panel1.SuspendLayout();
            splitContainer10.Panel2.SuspendLayout();
            splitContainer10.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer10
            // 
            splitContainer10.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer10.Location = new System.Drawing.Point(0, 0);
            splitContainer10.Name = "splitContainer10";
            splitContainer10.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer10.Panel1
            // 
            splitContainer10.Panel1.Controls.Add(this.listPoolVars);
            // 
            // splitContainer10.Panel2
            // 
            splitContainer10.Panel2.Controls.Add(this.txtSourceCode);
            splitContainer10.Panel2.Controls.Add(label1);
            splitContainer10.Panel2.Controls.Add(this.comboType);
            splitContainer10.Panel2.Controls.Add(this.txtPoolVarName);
            splitContainer10.Panel2.Controls.Add(label17);
            splitContainer10.Panel2.Controls.Add(label16);
            splitContainer10.Size = new System.Drawing.Size(400, 400);
            splitContainer10.SplitterDistance = 200;
            splitContainer10.TabIndex = 1;
            // 
            // listPoolVars
            // 
            this.listPoolVars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPoolVars.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader4,
            columnHeader1});
            this.listPoolVars.FullRowSelect = true;
            this.listPoolVars.HideSelection = false;
            this.listPoolVars.Location = new System.Drawing.Point(4, 6);
            this.listPoolVars.MultiSelect = false;
            this.listPoolVars.Name = "listPoolVars";
            this.listPoolVars.ShowGroups = false;
            this.listPoolVars.Size = new System.Drawing.Size(387, 186);
            this.listPoolVars.SmallImageList = this.imageList;
            this.listPoolVars.TabIndex = 4;
            this.listPoolVars.UseCompatibleStateImageBehavior = false;
            this.listPoolVars.View = System.Windows.Forms.View.Details;
            this.listPoolVars.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listPoolValues_ItemChanging);
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Pool Value";
            columnHeader4.Width = 214;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Type";
            columnHeader1.Width = 69;
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
            // txtSourceCode
            // 
            this.txtSourceCode.AcceptsTab = true;
            this.txtSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceCode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceCode.Location = new System.Drawing.Point(7, 81);
            this.txtSourceCode.Multiline = true;
            this.txtSourceCode.Name = "txtSourceCode";
            this.txtSourceCode.Size = new System.Drawing.Size(384, 107);
            this.txtSourceCode.TabIndex = 10;
            this.txtSourceCode.TextChanged += new System.EventHandler(this.txtSourceCode_TextChanged);
            this.txtSourceCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSourceCode_KeyDown);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 35);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(34, 13);
            label1.TabIndex = 9;
            label1.Text = "Type:";
            // 
            // comboType
            // 
            this.comboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "Constant",
            "Variable"});
            this.comboType.Location = new System.Drawing.Point(67, 32);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(324, 21);
            this.comboType.TabIndex = 8;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // txtPoolVarName
            // 
            this.txtPoolVarName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPoolVarName.Location = new System.Drawing.Point(67, 6);
            this.txtPoolVarName.Name = "txtPoolVarName";
            this.txtPoolVarName.Size = new System.Drawing.Size(325, 20);
            this.txtPoolVarName.TabIndex = 6;
            this.txtPoolVarName.TextChanged += new System.EventHandler(this.txtPoolVarName_TextChanged);
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(3, 65);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(50, 13);
            label17.TabIndex = 4;
            label17.Text = "Initializer:";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(3, 9);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(38, 13);
            label16.TabIndex = 5;
            label16.Text = "Name:";
            // 
            // PoolValuesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer10);
            this.Name = "PoolValuesControl";
            this.Size = new System.Drawing.Size(400, 400);
            splitContainer10.Panel1.ResumeLayout(false);
            splitContainer10.Panel2.ResumeLayout(false);
            splitContainer10.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer10)).EndInit();
            splitContainer10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VetoingListView listPoolVars;
        private System.Windows.Forms.TextBox txtPoolVarName;
        private System.Windows.Forms.ComboBox comboType;
        public System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox txtSourceCode;

    }
}
