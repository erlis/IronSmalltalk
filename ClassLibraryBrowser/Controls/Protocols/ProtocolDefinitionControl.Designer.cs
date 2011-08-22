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
    partial class ProtocolDefinitionControl
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
            System.Windows.Forms.SplitContainer splitContainer4;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.Label label4;
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.txtProtocolDocId = new System.Windows.Forms.TextBox();
            this.txtProtocolName = new System.Windows.Forms.TextBox();
            this.checkBoxProtocolAbstract = new System.Windows.Forms.CheckBox();
            this.listProtocolConformsTo = new System.Windows.Forms.ListView();
            splitContainer4 = new System.Windows.Forms.SplitContainer();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(splitContainer4)).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer4.Location = new System.Drawing.Point(0, 0);
            splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(this.descriptionControl);
            splitContainer4.Panel1.Controls.Add(label2);
            splitContainer4.Panel1.Controls.Add(label1);
            splitContainer4.Panel1.Controls.Add(this.txtProtocolDocId);
            splitContainer4.Panel1.Controls.Add(this.txtProtocolName);
            splitContainer4.Panel1.Controls.Add(this.checkBoxProtocolAbstract);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(this.listProtocolConformsTo);
            splitContainer4.Panel2.Controls.Add(label4);
            splitContainer4.Size = new System.Drawing.Size(689, 343);
            splitContainer4.SplitterDistance = 275;
            splitContainer4.TabIndex = 9;
            // 
            // descriptionControl
            // 
            this.descriptionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionControl.Location = new System.Drawing.Point(11, 82);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = true;
            this.descriptionControl.Size = new System.Drawing.Size(260, 257);
            this.descriptionControl.TabIndex = 13;
            this.descriptionControl.Changed += new System.EventHandler(this.descriptionControl_Changed);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(8, 10);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 13);
            label2.TabIndex = 12;
            label2.Text = "Name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(8, 36);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(40, 13);
            label1.TabIndex = 10;
            label1.Text = "X3J20:";
            // 
            // txtProtocolDocId
            // 
            this.txtProtocolDocId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProtocolDocId.Location = new System.Drawing.Point(73, 33);
            this.txtProtocolDocId.Name = "txtProtocolDocId";
            this.txtProtocolDocId.Size = new System.Drawing.Size(198, 20);
            this.txtProtocolDocId.TabIndex = 8;
            this.txtProtocolDocId.TextChanged += new System.EventHandler(this.txtProtocolDocId_TextChanged);
            // 
            // txtProtocolName
            // 
            this.txtProtocolName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProtocolName.BackColor = System.Drawing.SystemColors.Window;
            this.txtProtocolName.Location = new System.Drawing.Point(73, 7);
            this.txtProtocolName.Name = "txtProtocolName";
            this.txtProtocolName.Size = new System.Drawing.Size(198, 20);
            this.txtProtocolName.TabIndex = 9;
            this.txtProtocolName.TextChanged += new System.EventHandler(this.txtProtocolName_TextChanged);
            // 
            // checkBoxProtocolAbstract
            // 
            this.checkBoxProtocolAbstract.AutoSize = true;
            this.checkBoxProtocolAbstract.Location = new System.Drawing.Point(73, 59);
            this.checkBoxProtocolAbstract.Name = "checkBoxProtocolAbstract";
            this.checkBoxProtocolAbstract.Size = new System.Drawing.Size(65, 17);
            this.checkBoxProtocolAbstract.TabIndex = 7;
            this.checkBoxProtocolAbstract.Text = "Abstract";
            this.checkBoxProtocolAbstract.UseVisualStyleBackColor = true;
            this.checkBoxProtocolAbstract.CheckedChanged += new System.EventHandler(this.checkBoxProtocolAbstract_CheckedChanged);
            // 
            // listProtocolConformsTo
            // 
            this.listProtocolConformsTo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listProtocolConformsTo.CheckBoxes = true;
            this.listProtocolConformsTo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1});
            this.listProtocolConformsTo.FullRowSelect = true;
            this.listProtocolConformsTo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listProtocolConformsTo.Location = new System.Drawing.Point(6, 26);
            this.listProtocolConformsTo.Name = "listProtocolConformsTo";
            this.listProtocolConformsTo.ShowGroups = false;
            this.listProtocolConformsTo.Size = new System.Drawing.Size(401, 313);
            this.listProtocolConformsTo.TabIndex = 9;
            this.listProtocolConformsTo.UseCompatibleStateImageBehavior = false;
            this.listProtocolConformsTo.View = System.Windows.Forms.View.Details;
            this.listProtocolConformsTo.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listProtocolConformsTo_ItemChecked);
            // 
            // columnHeader1
            // 
            columnHeader1.Width = 270;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 10);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(70, 13);
            label4.TabIndex = 8;
            label4.Text = "Conforms To:";
            // 
            // ProtocolDefinitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer4);
            this.DoubleBuffered = true;
            this.Name = "ProtocolDefinitionControl";
            this.Size = new System.Drawing.Size(689, 343);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel1.PerformLayout();
            splitContainer4.Panel2.ResumeLayout(false);
            splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer4)).EndInit();
            splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtProtocolDocId;
        private System.Windows.Forms.TextBox txtProtocolName;
        private System.Windows.Forms.CheckBox checkBoxProtocolAbstract;
        private System.Windows.Forms.ListView listProtocolConformsTo;
        private DescriptionControl descriptionControl;


    }
}
