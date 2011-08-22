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
    partial class ProtocolGlobalControl
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
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.Label label8;
            this.listGlobals = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.txtGlobalName = new System.Windows.Forms.TextBox();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this.listGlobals);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.descriptionControl);
            splitContainer2.Panel2.Controls.Add(label8);
            splitContainer2.Panel2.Controls.Add(this.txtGlobalName);
            splitContainer2.Size = new System.Drawing.Size(700, 500);
            splitContainer2.SplitterDistance = 229;
            splitContainer2.TabIndex = 1;
            // 
            // listGlobals
            // 
            this.listGlobals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listGlobals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader2});
            this.listGlobals.FullRowSelect = true;
            this.listGlobals.HideSelection = false;
            this.listGlobals.Location = new System.Drawing.Point(3, 3);
            this.listGlobals.MultiSelect = false;
            this.listGlobals.Name = "listGlobals";
            this.listGlobals.ShowGroups = false;
            this.listGlobals.Size = new System.Drawing.Size(223, 494);
            this.listGlobals.TabIndex = 0;
            this.listGlobals.UseCompatibleStateImageBehavior = false;
            this.listGlobals.View = System.Windows.Forms.View.Details;
            this.listGlobals.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listGlobals_ItemChanging);
            this.listGlobals.SizeChanged += new System.EventHandler(this.listGlobals_SizeChanged);
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Global";
            columnHeader2.Width = 194;
            // 
            // descriptionControl
            // 
            this.descriptionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionControl.Location = new System.Drawing.Point(6, 29);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = true;
            this.descriptionControl.Size = new System.Drawing.Size(458, 468);
            this.descriptionControl.TabIndex = 15;
            this.descriptionControl.Changed += new System.EventHandler(this.descriptionControl_Changed);
            // 
            // label8
            // 
            label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(3, 6);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(38, 13);
            label8.TabIndex = 13;
            label8.Text = "Name:";
            // 
            // txtGlobalName
            // 
            this.txtGlobalName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGlobalName.Location = new System.Drawing.Point(87, 3);
            this.txtGlobalName.Name = "txtGlobalName";
            this.txtGlobalName.Size = new System.Drawing.Size(377, 20);
            this.txtGlobalName.TabIndex = 11;
            this.txtGlobalName.TextChanged += new System.EventHandler(this.txtGlobalName_TextChanged);
            // 
            // ProtocolGlobalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer2);
            this.DoubleBuffered = true;
            this.Name = "ProtocolGlobalControl";
            this.Size = new System.Drawing.Size(700, 500);
            this.Load += new System.EventHandler(this.ProtocolGlobalControl_Load);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).EndInit();
            splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VetoingListView listGlobals;
        private System.Windows.Forms.TextBox txtGlobalName;
        private DescriptionControl descriptionControl;

    }
}
