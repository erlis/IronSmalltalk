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
    partial class GlobalsControl
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalsControl));
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.listGlobals = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            this.globalDefinitionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Globals.GlobalDefinitionControl();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.listGlobals);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.globalDefinitionControl);
            splitContainer1.Size = new System.Drawing.Size(616, 366);
            splitContainer1.SplitterDistance = 310;
            splitContainer1.TabIndex = 0;
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
            // listGlobals
            // 
            this.listGlobals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2});
            this.listGlobals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGlobals.FullRowSelect = true;
            this.listGlobals.GridLines = true;
            this.listGlobals.HideSelection = false;
            this.listGlobals.LargeImageList = this.imageList;
            this.listGlobals.Location = new System.Drawing.Point(0, 0);
            this.listGlobals.MultiSelect = false;
            this.listGlobals.Name = "listGlobals";
            this.listGlobals.ShowGroups = false;
            this.listGlobals.Size = new System.Drawing.Size(310, 366);
            this.listGlobals.SmallImageList = this.imageList;
            this.listGlobals.TabIndex = 0;
            this.listGlobals.UseCompatibleStateImageBehavior = false;
            this.listGlobals.View = System.Windows.Forms.View.Details;
            this.listGlobals.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listGlobals_ItemChanging);
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Global";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Type";
            columnHeader2.Width = 80;
            // 
            // globalDefinitionControl
            // 
            this.globalDefinitionControl.Dirty = false;
            this.globalDefinitionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalDefinitionControl.Enabled = false;
            this.globalDefinitionControl.Location = new System.Drawing.Point(0, 0);
            this.globalDefinitionControl.Name = "globalDefinitionControl";
            this.globalDefinitionControl.Size = new System.Drawing.Size(302, 366);
            this.globalDefinitionControl.SystemImplementationHolder = null;
            this.globalDefinitionControl.TabIndex = 0;
            // 
            // GlobalsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.Name = "GlobalsControl";
            this.Size = new System.Drawing.Size(616, 366);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VetoingListView listGlobals;
        private GlobalDefinitionControl globalDefinitionControl;
        public System.Windows.Forms.ImageList imageList;

    }
}
