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
    partial class ParseTester
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParseTester));
            this.btnParseMethod = new System.Windows.Forms.Button();
            this.btnParseInitializer = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.treeParseNodes = new System.Windows.Forms.TreeView();
            this.listProperties = new System.Windows.Forms.ListView();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            this.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            splitContainer1.Panel1.Controls.Add(this.txtSource);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(749, 526);
            splitContainer1.SplitterDistance = 210;
            splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(this.btnParseMethod, 0, 0);
            tableLayoutPanel1.Controls.Add(this.btnParseInitializer, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 179);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(749, 31);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // btnParseMethod
            // 
            this.btnParseMethod.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnParseMethod.Location = new System.Drawing.Point(377, 5);
            this.btnParseMethod.Name = "btnParseMethod";
            this.btnParseMethod.Size = new System.Drawing.Size(369, 23);
            this.btnParseMethod.TabIndex = 4;
            this.btnParseMethod.Text = "Parse Method";
            this.btnParseMethod.UseVisualStyleBackColor = true;
            this.btnParseMethod.Click += new System.EventHandler(this.btnParseMethod_Click);
            // 
            // btnParseInitializer
            // 
            this.btnParseInitializer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnParseInitializer.Location = new System.Drawing.Point(3, 5);
            this.btnParseInitializer.Name = "btnParseInitializer";
            this.btnParseInitializer.Size = new System.Drawing.Size(368, 23);
            this.btnParseInitializer.TabIndex = 3;
            this.btnParseInitializer.Text = "Parse Initializer";
            this.btnParseInitializer.UseVisualStyleBackColor = true;
            this.btnParseInitializer.Click += new System.EventHandler(this.btnParseInitializer_Click);
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
            this.txtSource.Size = new System.Drawing.Size(728, 161);
            this.txtSource.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this.treeParseNodes);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.listProperties);
            splitContainer2.Size = new System.Drawing.Size(749, 312);
            splitContainer2.SplitterDistance = 373;
            splitContainer2.TabIndex = 0;
            // 
            // treeParseNodes
            // 
            this.treeParseNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeParseNodes.HideSelection = false;
            this.treeParseNodes.Location = new System.Drawing.Point(0, 0);
            this.treeParseNodes.Name = "treeParseNodes";
            this.treeParseNodes.ShowNodeToolTips = true;
            this.treeParseNodes.Size = new System.Drawing.Size(373, 312);
            this.treeParseNodes.TabIndex = 0;
            this.treeParseNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeParseNodes_AfterSelect);
            this.treeParseNodes.Click += new System.EventHandler(this.treeParseNodes_Click);
            // 
            // listProperties
            // 
            this.listProperties.AllowColumnReorder = true;
            this.listProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
            this.listProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listProperties.FullRowSelect = true;
            this.listProperties.HideSelection = false;
            this.listProperties.Location = new System.Drawing.Point(0, 0);
            this.listProperties.MultiSelect = false;
            this.listProperties.Name = "listProperties";
            this.listProperties.ShowGroups = false;
            this.listProperties.ShowItemToolTips = true;
            this.listProperties.Size = new System.Drawing.Size(372, 312);
            this.listProperties.TabIndex = 0;
            this.listProperties.UseCompatibleStateImageBehavior = false;
            this.listProperties.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Property";
            columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Value";
            columnHeader2.Width = 200;
            // 
            // ParseTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 526);
            this.Controls.Add(splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ParseTester";
            this.Text = "Parse - Tester";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).EndInit();
            splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnParseMethod;
        private System.Windows.Forms.Button btnParseInitializer;
        private System.Windows.Forms.TreeView treeParseNodes;
        private System.Windows.Forms.ListView listProperties;
        public System.Windows.Forms.TextBox txtSource;
    }
}
