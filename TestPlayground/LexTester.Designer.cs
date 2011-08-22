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

namespace TestPlayground
{
    partial class LexTester
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
            System.Windows.Forms.SplitContainer splitContainer;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LexTester));
            this.comboPreference = new System.Windows.Forms.ComboBox();
            this.btnTokanize = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtTokenInfo = new System.Windows.Forms.TextBox();
            this.listTokens = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            splitContainer = new System.Windows.Forms.SplitContainer();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(splitContainer)).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 0);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(this.comboPreference);
            splitContainer.Panel1.Controls.Add(this.btnTokanize);
            splitContainer.Panel1.Controls.Add(this.txtSource);
            splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(this.txtTokenInfo);
            splitContainer.Panel2.Controls.Add(this.listTokens);
            splitContainer.Size = new System.Drawing.Size(712, 546);
            splitContainer.SplitterDistance = 248;
            splitContainer.TabIndex = 2;
            // 
            // comboPreference
            // 
            this.comboPreference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboPreference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPreference.FormattingEnabled = true;
            this.comboPreference.Location = new System.Drawing.Point(476, 220);
            this.comboPreference.Name = "comboPreference";
            this.comboPreference.Size = new System.Drawing.Size(227, 21);
            this.comboPreference.TabIndex = 3;
            // 
            // btnTokanize
            // 
            this.btnTokanize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTokanize.Location = new System.Drawing.Point(6, 219);
            this.btnTokanize.Name = "btnTokanize";
            this.btnTokanize.Size = new System.Drawing.Size(464, 23);
            this.btnTokanize.TabIndex = 2;
            this.btnTokanize.Text = "Tokanize";
            this.btnTokanize.UseVisualStyleBackColor = true;
            this.btnTokanize.Click += new System.EventHandler(this.btnTokanize_Click);
            // 
            // txtSource
            // 
            this.txtSource.AcceptsTab = true;
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSource.HideSelection = false;
            this.txtSource.Location = new System.Drawing.Point(9, 12);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSource.Size = new System.Drawing.Size(694, 201);
            this.txtSource.TabIndex = 1;
            // 
            // txtTokenInfo
            // 
            this.txtTokenInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTokenInfo.Location = new System.Drawing.Point(9, 203);
            this.txtTokenInfo.Multiline = true;
            this.txtTokenInfo.Name = "txtTokenInfo";
            this.txtTokenInfo.ReadOnly = true;
            this.txtTokenInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTokenInfo.Size = new System.Drawing.Size(694, 79);
            this.txtTokenInfo.TabIndex = 1;
            // 
            // listTokens
            // 
            this.listTokens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listTokens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader7,
            this.columnHeader5,
            this.columnHeader6});
            this.listTokens.FullRowSelect = true;
            this.listTokens.GridLines = true;
            this.listTokens.Location = new System.Drawing.Point(9, 3);
            this.listTokens.MultiSelect = false;
            this.listTokens.Name = "listTokens";
            this.listTokens.ShowGroups = false;
            this.listTokens.Size = new System.Drawing.Size(694, 194);
            this.listTokens.TabIndex = 0;
            this.listTokens.UseCompatibleStateImageBehavior = false;
            this.listTokens.View = System.Windows.Forms.View.Details;
            this.listTokens.SelectedIndexChanged += new System.EventHandler(this.listTokens_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Type";
            columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Start";
            this.columnHeader2.Width = 40;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "End";
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 200;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Gen. Source";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "OK";
            this.columnHeader5.Width = 30;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Error Msg.";
            this.columnHeader6.Width = 70;
            // 
            // LexTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 546);
            this.Controls.Add(splitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LexTester";
            this.Text = "Lex-Tester";
            this.Load += new System.EventHandler(this.LexTester_Load);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer)).EndInit();
            splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTokanize;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTokenInfo;
        private System.Windows.Forms.ListView listTokens;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ComboBox comboPreference;


    }
}

