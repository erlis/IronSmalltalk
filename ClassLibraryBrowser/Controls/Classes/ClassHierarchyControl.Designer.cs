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
    partial class ClassHierarchyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassHierarchyControl));
            this.treeClasses = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeClasses
            // 
            this.treeClasses.AllowDrop = true;
            this.treeClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeClasses.HideSelection = false;
            this.treeClasses.ImageIndex = 0;
            this.treeClasses.ImageList = this.imageList;
            this.treeClasses.LabelEdit = true;
            this.treeClasses.Location = new System.Drawing.Point(0, 0);
            this.treeClasses.Name = "treeClasses";
            this.treeClasses.SelectedImageIndex = 0;
            this.treeClasses.ShowNodeToolTips = true;
            this.treeClasses.Size = new System.Drawing.Size(150, 150);
            this.treeClasses.TabIndex = 1;
            this.treeClasses.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeClasses_AfterLabelEdit);
            this.treeClasses.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeClasses_ItemDrag);
            this.treeClasses.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeClasses_BeforeSelect);
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
            // ClassHierarchyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeClasses);
            this.DoubleBuffered = true;
            this.Name = "ClassHierarchyControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeClasses;
        public System.Windows.Forms.ImageList imageList;
    }
}
