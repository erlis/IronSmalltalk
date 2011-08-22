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
    partial class PoolDefinitionControl
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
            System.Windows.Forms.SplitContainer splitContainer9;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PoolDefinitionControl));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.ColumnHeader columnHeader4;
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.txtName = new System.Windows.Forms.TextBox();
            this.listPools = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            splitContainer9 = new System.Windows.Forms.SplitContainer();
            label1 = new System.Windows.Forms.Label();
            columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(splitContainer9)).BeginInit();
            splitContainer9.Panel1.SuspendLayout();
            splitContainer9.Panel2.SuspendLayout();
            splitContainer9.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer9
            // 
            splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer9.Location = new System.Drawing.Point(0, 0);
            splitContainer9.Name = "splitContainer9";
            splitContainer9.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer9.Panel1
            // 
            splitContainer9.Panel1.Controls.Add(this.listPools);
            // 
            // splitContainer9.Panel2
            // 
            splitContainer9.Panel2.Controls.Add(this.descriptionControl);
            splitContainer9.Panel2.Controls.Add(this.txtName);
            splitContainer9.Panel2.Controls.Add(label1);
            splitContainer9.Size = new System.Drawing.Size(400, 400);
            splitContainer9.SplitterDistance = 200;
            splitContainer9.TabIndex = 1;
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
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(67, 6);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(326, 20);
            this.txtName.TabIndex = 7;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(62, 13);
            label1.TabIndex = 8;
            label1.Text = "Pool Name:";
            // 
            // listPools
            // 
            this.listPools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPools.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader4});
            this.listPools.FullRowSelect = true;
            this.listPools.HideSelection = false;
            this.listPools.Location = new System.Drawing.Point(6, 6);
            this.listPools.MultiSelect = false;
            this.listPools.Name = "listPools";
            this.listPools.ShowGroups = false;
            this.listPools.Size = new System.Drawing.Size(387, 186);
            this.listPools.SmallImageList = this.imageList;
            this.listPools.TabIndex = 5;
            this.listPools.UseCompatibleStateImageBehavior = false;
            this.listPools.View = System.Windows.Forms.View.Details;
            this.listPools.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listPools_ItemChanging);
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Pool";
            columnHeader4.Width = 279;
            // 
            // descriptionControl
            // 
            this.descriptionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionControl.Enabled = false;
            this.descriptionControl.Location = new System.Drawing.Point(6, 29);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = true;
            this.descriptionControl.Size = new System.Drawing.Size(387, 160);
            this.descriptionControl.TabIndex = 11;
            this.descriptionControl.Changed += new System.EventHandler(this.descriptionControl_Changed);
            // 
            // PoolDefinitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer9);
            this.Name = "PoolDefinitionControl";
            this.Size = new System.Drawing.Size(400, 400);
            splitContainer9.Panel1.ResumeLayout(false);
            splitContainer9.Panel2.ResumeLayout(false);
            splitContainer9.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer9)).EndInit();
            splitContainer9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox txtName;
        private DescriptionControl descriptionControl;
        private VetoingListView listPools;
        public System.Windows.Forms.ImageList imageList;

    }
}
