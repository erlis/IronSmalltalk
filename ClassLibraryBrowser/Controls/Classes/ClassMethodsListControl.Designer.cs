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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes
{
    partial class ClassMethodsListControl
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
            System.Windows.Forms.ColumnHeader columnHeader15;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassMethodsListControl));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.listMethods = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // columnHeader15
            // 
            columnHeader15.Text = "Method";
            columnHeader15.Width = 200;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Protocols";
            columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Local Implementors";
            columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Super Implementors";
            columnHeader3.Width = 200;
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
            // listMethods
            // 
            this.listMethods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader15,
            columnHeader1,
            columnHeader2,
            columnHeader3});
            this.listMethods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMethods.FullRowSelect = true;
            this.listMethods.HideSelection = false;
            this.listMethods.Location = new System.Drawing.Point(0, 0);
            this.listMethods.MultiSelect = false;
            this.listMethods.Name = "listMethods";
            this.listMethods.ShowGroups = false;
            this.listMethods.ShowItemToolTips = true;
            this.listMethods.Size = new System.Drawing.Size(150, 150);
            this.listMethods.SmallImageList = this.imageList;
            this.listMethods.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listMethods.TabIndex = 2;
            this.listMethods.UseCompatibleStateImageBehavior = false;
            this.listMethods.View = System.Windows.Forms.View.Details;
            this.listMethods.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listMethods_ItemChanging);
            this.listMethods.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listMethods_KeyDown);
            // 
            // ClassMethodsListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listMethods);
            this.Name = "ClassMethodsListControl";
            this.ResumeLayout(false);

        }

        #endregion

        public VetoingListView listMethods;
        public System.Windows.Forms.ImageList imageList;
    }
}
