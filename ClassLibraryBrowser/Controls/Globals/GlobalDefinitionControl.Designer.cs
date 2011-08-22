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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Globals
{
    partial class GlobalDefinitionControl
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.Label label4;
            this.txtName = new System.Windows.Forms.TextBox();
            this.comboImplementedProtocols = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.AppendingComboBox();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.sourceEditControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.SourceEditControl();
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(112, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(187, 20);
            this.txtName.TabIndex = 9;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // comboImplementedProtocols
            // 
            this.comboImplementedProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboImplementedProtocols.FormattingEnabled = true;
            this.comboImplementedProtocols.Location = new System.Drawing.Point(112, 29);
            this.comboImplementedProtocols.Name = "comboImplementedProtocols";
            this.comboImplementedProtocols.Separator = " ";
            this.comboImplementedProtocols.Size = new System.Drawing.Size(187, 21);
            this.comboImplementedProtocols.TabIndex = 12;
            this.comboImplementedProtocols.TextChanged += new System.EventHandler(this.comboImplementedProtocols_TextChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(103, 13);
            label2.TabIndex = 10;
            label2.Text = "Implemented Prots. :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(71, 13);
            label1.TabIndex = 11;
            label1.Text = "Global Name:";
            // 
            // comboType
            // 
            this.comboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "Variable",
            "Constant"});
            this.comboType.Location = new System.Drawing.Point(112, 57);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(187, 21);
            this.comboType.TabIndex = 13;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 60);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(67, 13);
            label3.TabIndex = 11;
            label3.Text = "Global Type:";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            splitContainer1.Location = new System.Drawing.Point(0, 84);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.descriptionControl);
            splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(this.sourceEditControl);
            splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(6);
            splitContainer1.Size = new System.Drawing.Size(302, 296);
            splitContainer1.SplitterDistance = 155;
            splitContainer1.TabIndex = 14;
            // 
            // sourceEditControl
            // 
            this.sourceEditControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceEditControl.Location = new System.Drawing.Point(6, 22);
            this.sourceEditControl.Name = "sourceEditControl";
            this.sourceEditControl.Size = new System.Drawing.Size(290, 112);
            this.sourceEditControl.Source = "";
            this.sourceEditControl.TabIndex = 0;
            this.sourceEditControl.Changed += new System.EventHandler(this.sourceEditControl_Changed);
            // 
            // descriptionControl
            // 
            this.descriptionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionControl.Location = new System.Drawing.Point(6, 6);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = true;
            this.descriptionControl.Size = new System.Drawing.Size(290, 143);
            this.descriptionControl.TabIndex = 0;
            this.descriptionControl.Changed += new System.EventHandler(this.descriptionControl_Changed);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 6);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(50, 13);
            label4.TabIndex = 12;
            label4.Text = "Initializer:";
            // 
            // GlobalDefinitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.comboImplementedProtocols);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(label1);
            this.Name = "GlobalDefinitionControl";
            this.Size = new System.Drawing.Size(302, 380);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtName;
        public AppendingComboBox comboImplementedProtocols;
        private System.Windows.Forms.ComboBox comboType;
        private DescriptionControl descriptionControl;
        private SourceEditControl sourceEditControl;
    }
}
