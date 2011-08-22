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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Initializers
{
    partial class InitializersControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializersControl));
            this.treeInitializers = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeInitializers
            // 
            this.treeInitializers.AllowDrop = true;
            this.treeInitializers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeInitializers.HideSelection = false;
            this.treeInitializers.ImageIndex = 0;
            this.treeInitializers.ImageList = this.imageList;
            this.treeInitializers.ItemHeight = 18;
            this.treeInitializers.Location = new System.Drawing.Point(0, 0);
            this.treeInitializers.Name = "treeInitializers";
            this.treeInitializers.SelectedImageIndex = 0;
            this.treeInitializers.ShowNodeToolTips = true;
            this.treeInitializers.Size = new System.Drawing.Size(356, 258);
            this.treeInitializers.TabIndex = 0;
            this.treeInitializers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeInitializers_ItemDrag);
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
            // InitializersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeInitializers);
            this.DoubleBuffered = true;
            this.Name = "InitializersControl";
            this.Size = new System.Drawing.Size(356, 258);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeInitializers;
        public System.Windows.Forms.ImageList imageList;
    }
}
