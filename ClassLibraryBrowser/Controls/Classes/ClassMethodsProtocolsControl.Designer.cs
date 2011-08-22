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
    partial class ClassMethodsProtocolsControl
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
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.ColumnHeader columnHeader14;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassMethodsProtocolsControl));
            this.comboUpToClass = new System.Windows.Forms.ComboBox();
            this.comboInstanceOrClass = new System.Windows.Forms.ComboBox();
            this.listMethodProtocols = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(this.comboUpToClass, 0, 2);
            tableLayoutPanel1.Controls.Add(this.comboInstanceOrClass, 0, 0);
            tableLayoutPanel1.Controls.Add(this.listMethodProtocols, 0, 1);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel1.Size = new System.Drawing.Size(150, 150);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // comboUpToClass
            // 
            this.comboUpToClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboUpToClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboUpToClass.FormattingEnabled = true;
            this.comboUpToClass.Location = new System.Drawing.Point(0, 127);
            this.comboUpToClass.Margin = new System.Windows.Forms.Padding(0);
            this.comboUpToClass.Name = "comboUpToClass";
            this.comboUpToClass.Size = new System.Drawing.Size(150, 21);
            this.comboUpToClass.TabIndex = 3;
            this.comboUpToClass.SelectedIndexChanged += new System.EventHandler(this.comboUpToClass_SelectedIndexChanged);
            this.comboUpToClass.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.comboUpToClass_Format);
            // 
            // comboInstanceOrClass
            // 
            this.comboInstanceOrClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboInstanceOrClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInstanceOrClass.FormattingEnabled = true;
            this.comboInstanceOrClass.Items.AddRange(new object[] {
            "Instance Methods",
            "Class Methods"});
            this.comboInstanceOrClass.Location = new System.Drawing.Point(0, 0);
            this.comboInstanceOrClass.Margin = new System.Windows.Forms.Padding(0);
            this.comboInstanceOrClass.Name = "comboInstanceOrClass";
            this.comboInstanceOrClass.Size = new System.Drawing.Size(150, 21);
            this.comboInstanceOrClass.TabIndex = 2;
            this.comboInstanceOrClass.SelectedIndexChanged += new System.EventHandler(this.comboInstanceOrClass_SelectedIndexChanged);
            // 
            // listMethodProtocols
            // 
            this.listMethodProtocols.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader14});
            this.listMethodProtocols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMethodProtocols.FullRowSelect = true;
            this.listMethodProtocols.HideSelection = false;
            this.listMethodProtocols.Location = new System.Drawing.Point(0, 23);
            this.listMethodProtocols.Margin = new System.Windows.Forms.Padding(0);
            this.listMethodProtocols.MultiSelect = false;
            this.listMethodProtocols.Name = "listMethodProtocols";
            this.listMethodProtocols.ShowGroups = false;
            this.listMethodProtocols.ShowItemToolTips = true;
            this.listMethodProtocols.Size = new System.Drawing.Size(150, 104);
            this.listMethodProtocols.SmallImageList = this.imageList;
            this.listMethodProtocols.TabIndex = 1;
            this.listMethodProtocols.UseCompatibleStateImageBehavior = false;
            this.listMethodProtocols.View = System.Windows.Forms.View.Details;
            this.listMethodProtocols.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listMethodProtocols_ItemChanging);
            // 
            // columnHeader14
            // 
            columnHeader14.Text = "Protocol";
            columnHeader14.Width = 185;
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
            // ClassMethodsProtocolsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableLayoutPanel1);
            this.Name = "ClassMethodsProtocolsControl";
            tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboInstanceOrClass;
        public VetoingListView listMethodProtocols;
        public System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ComboBox comboUpToClass;
    }
}
