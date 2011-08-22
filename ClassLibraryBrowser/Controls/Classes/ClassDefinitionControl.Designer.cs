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
    partial class ClassDefinitionControl
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
            System.Windows.Forms.Label label15;
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.Label label5;
            this.descriptionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.DescriptionControl();
            this.sourceEditControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.SourceEditControl();
            this.txtClassClassInstanceVars = new System.Windows.Forms.TextBox();
            this.txtClassClassVars = new System.Windows.Forms.TextBox();
            this.txtClassInstanceVars = new System.Windows.Forms.TextBox();
            this.comboClassInstanceState = new System.Windows.Forms.ComboBox();
            this.comboClassPools = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.AppendingComboBox();
            this.comboClassSuperclass = new System.Windows.Forms.ComboBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.comboImplementedInstanceProtocols = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.AppendingComboBox();
            this.comboImplementedClassProtocols = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.AppendingComboBox();
            this.textNativeType = new System.Windows.Forms.TextBox();
            label15 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(10, 164);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(80, 13);
            label15.TabIndex = 5;
            label15.Text = "Imported Pools:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(10, 138);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(82, 13);
            label14.TabIndex = 4;
            label14.Text = "Class Inst. Vars:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(10, 86);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(75, 13);
            label13.TabIndex = 3;
            label13.Text = "Instance Vars:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(10, 112);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(59, 13);
            label12.TabIndex = 8;
            label12.Text = "Class Vars:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(10, 59);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(79, 13);
            label11.TabIndex = 7;
            label11.Text = "Instance State:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(10, 32);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(62, 13);
            label10.TabIndex = 6;
            label10.Text = "Superclass:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(10, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 13);
            label1.TabIndex = 6;
            label1.Text = "Class Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(10, 191);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(88, 13);
            label2.TabIndex = 5;
            label2.Text = "Impl. Inst. Prots. :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(10, 218);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(93, 13);
            label3.TabIndex = 5;
            label3.Text = "Impl. Class Prots. :";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(10, 245);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(68, 13);
            label4.TabIndex = 4;
            label4.Text = "Native Type:";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            splitContainer1.Location = new System.Drawing.Point(13, 265);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.descriptionControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(this.sourceEditControl);
            splitContainer1.Size = new System.Drawing.Size(269, 151);
            splitContainer1.SplitterDistance = 80;
            splitContainer1.TabIndex = 12;
            // 
            // descriptionControl
            // 
            this.descriptionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionControl.Location = new System.Drawing.Point(0, 0);
            this.descriptionControl.Margin = new System.Windows.Forms.Padding(0);
            this.descriptionControl.Name = "descriptionControl";
            this.descriptionControl.ShowLabel = true;
            this.descriptionControl.Size = new System.Drawing.Size(269, 80);
            this.descriptionControl.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(-3, 4);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(50, 13);
            label5.TabIndex = 14;
            label5.Text = "Initializer:";
            // 
            // sourceEditControl
            // 
            this.sourceEditControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceEditControl.Location = new System.Drawing.Point(0, 20);
            this.sourceEditControl.Name = "sourceEditControl";
            this.sourceEditControl.Size = new System.Drawing.Size(269, 47);
            this.sourceEditControl.Source = "";
            this.sourceEditControl.TabIndex = 13;
            this.sourceEditControl.Load += new System.EventHandler(this.sourceEditControl_Load);
            // 
            // txtClassClassInstanceVars
            // 
            this.txtClassClassInstanceVars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassClassInstanceVars.Location = new System.Drawing.Point(109, 135);
            this.txtClassClassInstanceVars.Name = "txtClassClassInstanceVars";
            this.txtClassClassInstanceVars.Size = new System.Drawing.Size(173, 20);
            this.txtClassClassInstanceVars.TabIndex = 6;
            this.txtClassClassInstanceVars.TextChanged += new System.EventHandler(this.txtClassClassInstanceVars_TextChanged);
            // 
            // txtClassClassVars
            // 
            this.txtClassClassVars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassClassVars.Location = new System.Drawing.Point(109, 109);
            this.txtClassClassVars.Name = "txtClassClassVars";
            this.txtClassClassVars.Size = new System.Drawing.Size(173, 20);
            this.txtClassClassVars.TabIndex = 5;
            this.txtClassClassVars.TextChanged += new System.EventHandler(this.txtClassClassVars_TextChanged);
            // 
            // txtClassInstanceVars
            // 
            this.txtClassInstanceVars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassInstanceVars.Location = new System.Drawing.Point(109, 83);
            this.txtClassInstanceVars.Name = "txtClassInstanceVars";
            this.txtClassInstanceVars.Size = new System.Drawing.Size(173, 20);
            this.txtClassInstanceVars.TabIndex = 4;
            this.txtClassInstanceVars.TextChanged += new System.EventHandler(this.txtClassInstanceVars_TextChanged);
            // 
            // comboClassInstanceState
            // 
            this.comboClassInstanceState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboClassInstanceState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClassInstanceState.FormattingEnabled = true;
            this.comboClassInstanceState.Items.AddRange(new object[] {
            "None (Named Variables Only)",
            "Byte Indexable",
            "Object Indexable",
            "Native"});
            this.comboClassInstanceState.Location = new System.Drawing.Point(109, 56);
            this.comboClassInstanceState.Name = "comboClassInstanceState";
            this.comboClassInstanceState.Size = new System.Drawing.Size(173, 21);
            this.comboClassInstanceState.TabIndex = 3;
            this.comboClassInstanceState.SelectedIndexChanged += new System.EventHandler(this.comboClassInstanceState_SelectedIndexChanged);
            // 
            // comboClassPools
            // 
            this.comboClassPools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboClassPools.FormattingEnabled = true;
            this.comboClassPools.Location = new System.Drawing.Point(109, 161);
            this.comboClassPools.Name = "comboClassPools";
            this.comboClassPools.Separator = " ";
            this.comboClassPools.Size = new System.Drawing.Size(173, 21);
            this.comboClassPools.TabIndex = 7;
            this.comboClassPools.TextChanged += new System.EventHandler(this.comboClassPools_TextChanged);
            // 
            // comboClassSuperclass
            // 
            this.comboClassSuperclass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboClassSuperclass.FormattingEnabled = true;
            this.comboClassSuperclass.Location = new System.Drawing.Point(109, 29);
            this.comboClassSuperclass.Name = "comboClassSuperclass";
            this.comboClassSuperclass.Size = new System.Drawing.Size(173, 21);
            this.comboClassSuperclass.TabIndex = 2;
            this.comboClassSuperclass.TextChanged += new System.EventHandler(this.comboClassSuperclass_TextChanged);
            // 
            // txtClassName
            // 
            this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassName.Location = new System.Drawing.Point(109, 3);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(173, 20);
            this.txtClassName.TabIndex = 1;
            this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
            // 
            // comboImplementedInstanceProtocols
            // 
            this.comboImplementedInstanceProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboImplementedInstanceProtocols.FormattingEnabled = true;
            this.comboImplementedInstanceProtocols.Location = new System.Drawing.Point(109, 188);
            this.comboImplementedInstanceProtocols.Name = "comboImplementedInstanceProtocols";
            this.comboImplementedInstanceProtocols.Separator = " ";
            this.comboImplementedInstanceProtocols.Size = new System.Drawing.Size(173, 21);
            this.comboImplementedInstanceProtocols.TabIndex = 8;
            this.comboImplementedInstanceProtocols.TextChanged += new System.EventHandler(this.comboImplementedInstanceProtocols_TextChanged);
            // 
            // comboImplementedClassProtocols
            // 
            this.comboImplementedClassProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboImplementedClassProtocols.FormattingEnabled = true;
            this.comboImplementedClassProtocols.Location = new System.Drawing.Point(109, 215);
            this.comboImplementedClassProtocols.Name = "comboImplementedClassProtocols";
            this.comboImplementedClassProtocols.Separator = " ";
            this.comboImplementedClassProtocols.Size = new System.Drawing.Size(173, 21);
            this.comboImplementedClassProtocols.TabIndex = 9;
            this.comboImplementedClassProtocols.TextChanged += new System.EventHandler(this.comboImplementedClassProtocols_TextChanged);
            // 
            // textNativeType
            // 
            this.textNativeType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textNativeType.Location = new System.Drawing.Point(109, 242);
            this.textNativeType.Name = "textNativeType";
            this.textNativeType.Size = new System.Drawing.Size(173, 20);
            this.textNativeType.TabIndex = 6;
            this.textNativeType.TextChanged += new System.EventHandler(this.textNativeType_TextChanged);
            // 
            // ClassDefinitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.Controls.Add(this.textNativeType);
            this.Controls.Add(this.txtClassClassInstanceVars);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.txtClassClassVars);
            this.Controls.Add(this.txtClassInstanceVars);
            this.Controls.Add(this.comboClassInstanceState);
            this.Controls.Add(this.comboImplementedClassProtocols);
            this.Controls.Add(this.comboImplementedInstanceProtocols);
            this.Controls.Add(this.comboClassPools);
            this.Controls.Add(label3);
            this.Controls.Add(this.comboClassSuperclass);
            this.Controls.Add(label2);
            this.Controls.Add(label4);
            this.Controls.Add(label15);
            this.Controls.Add(label14);
            this.Controls.Add(label13);
            this.Controls.Add(label12);
            this.Controls.Add(label11);
            this.Controls.Add(label1);
            this.Controls.Add(label10);
            this.Name = "ClassDefinitionControl";
            this.Size = new System.Drawing.Size(285, 422);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtClassClassInstanceVars;
        public System.Windows.Forms.TextBox txtClassClassVars;
        public System.Windows.Forms.TextBox txtClassInstanceVars;
        public System.Windows.Forms.ComboBox comboClassInstanceState;
        public AppendingComboBox comboClassPools;
        public System.Windows.Forms.ComboBox comboClassSuperclass;
        public System.Windows.Forms.TextBox txtClassName;
        public AppendingComboBox comboImplementedInstanceProtocols;
        public AppendingComboBox comboImplementedClassProtocols;
        public System.Windows.Forms.TextBox textNativeType;
        private DescriptionControl descriptionControl;
        private SourceEditControl sourceEditControl;
    }
}
