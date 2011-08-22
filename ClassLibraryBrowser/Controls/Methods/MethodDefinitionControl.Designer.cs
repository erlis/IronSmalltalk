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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Methods
{
    partial class MethodDefinitionControl
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
            System.Windows.Forms.TabControl tabControl1;
            System.Windows.Forms.TabPage tabPage1;
            System.Windows.Forms.TabPage tabPage2;
            System.Windows.Forms.Label label1;
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.txtSourceCode = new System.Windows.Forms.TextBox();
            this.descriptionControlCopy = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.txtMethodHeader = new System.Windows.Forms.TextBox();
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.txtNativeName = new System.Windows.Forms.TextBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            label1 = new System.Windows.Forms.Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(384, 223);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(this.splitContainer);
            tabPage1.Controls.Add(this.txtMethodHeader);
            tabPage1.ImageKey = "source";
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(376, 197);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Source";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(6, 29);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.txtSourceCode);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.descriptionControlCopy);
            this.splitContainer.Size = new System.Drawing.Size(364, 162);
            this.splitContainer.SplitterDistance = 253;
            this.splitContainer.TabIndex = 7;
            // 
            // txtSourceCode
            // 
            this.txtSourceCode.AcceptsTab = true;
            this.txtSourceCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSourceCode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceCode.Location = new System.Drawing.Point(0, 0);
            this.txtSourceCode.Multiline = true;
            this.txtSourceCode.Name = "txtSourceCode";
            this.txtSourceCode.Size = new System.Drawing.Size(253, 162);
            this.txtSourceCode.TabIndex = 7;
            this.txtSourceCode.TextChanged += new System.EventHandler(this.txtSourceCode_TextChanged);
            this.txtSourceCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSourceCode_KeyDown);
            // 
            // descriptionControlCopy
            // 
            this.descriptionControlCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionControlCopy.Enabled = false;
            this.descriptionControlCopy.Location = new System.Drawing.Point(0, 0);
            this.descriptionControlCopy.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControlCopy.Name = "descriptionControlCopy";
            this.descriptionControlCopy.ShowLabel = false;
            this.descriptionControlCopy.Size = new System.Drawing.Size(107, 162);
            this.descriptionControlCopy.TabIndex = 8;
            // 
            // txtMethodHeader
            // 
            this.txtMethodHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethodHeader.Location = new System.Drawing.Point(6, 3);
            this.txtMethodHeader.Name = "txtMethodHeader";
            this.txtMethodHeader.ReadOnly = true;
            this.txtMethodHeader.Size = new System.Drawing.Size(364, 20);
            this.txtMethodHeader.TabIndex = 5;
            this.txtMethodHeader.DoubleClick += new System.EventHandler(this.txtMethodHeader_DoubleClick);
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(this.txtNativeName);
            tabPage2.Controls.Add(this.descriptionControl);
            tabPage2.ImageKey = "document";
            tabPage2.Location = new System.Drawing.Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(376, 197);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Definition";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // descriptionControl
            // 
            this.descriptionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionControl.Location = new System.Drawing.Point(3, 33);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = false;
            this.descriptionControl.Size = new System.Drawing.Size(370, 161);
            this.descriptionControl.TabIndex = 7;
            // 
            // txtNativeName
            // 
            this.txtNativeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNativeName.Location = new System.Drawing.Point(81, 7);
            this.txtNativeName.Name = "txtNativeName";
            this.txtNativeName.Size = new System.Drawing.Size(289, 20);
            this.txtNativeName.TabIndex = 8;
            this.txtNativeName.TextChanged += new System.EventHandler(this.txtNativeName_TextChanged);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 10);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 13);
            label1.TabIndex = 9;
            label1.Text = "Native Name:";
            // 
            // MethodDefinitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tabControl1);
            this.Name = "MethodDefinitionControl";
            this.Size = new System.Drawing.Size(384, 223);
            this.Load += new System.EventHandler(this.MethodDefinitionControl_Load);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox txtMethodHeader;
        private DescriptionControl descriptionControl;
        private System.Windows.Forms.TextBox txtSourceCode;
        private System.Windows.Forms.SplitContainer splitContainer;
        private DescriptionControl descriptionControlCopy;
        private System.Windows.Forms.TextBox txtNativeName;
    }
}
