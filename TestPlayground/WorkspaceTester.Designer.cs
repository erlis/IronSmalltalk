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

namespace TestPlayground
{
    partial class WorkspaceTester
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TabControl tabControl1;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkspaceTester));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonInstall = new System.Windows.Forms.Button();
            this.buttonCreateEnvironment = new System.Windows.Forms.Button();
            this.textInstall = new System.Windows.Forms.TextBox();
            this.textResultInstall = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboSelf = new System.Windows.Forms.ComboBox();
            this.buttonEvalExpression = new System.Windows.Forms.Button();
            this.buttonJitExpression = new System.Windows.Forms.Button();
            this.textEvaluate = new System.Windows.Forms.TextBox();
            this.textResultEvaluate = new System.Windows.Forms.TextBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(this.tabPage1);
            tabControl1.Controls.Add(this.tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(852, 389);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(844, 363);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Install";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.textResultInstall);
            splitContainer1.Size = new System.Drawing.Size(838, 357);
            splitContainer1.SplitterDistance = 230;
            splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(this.buttonInstall, 1, 1);
            tableLayoutPanel3.Controls.Add(this.buttonCreateEnvironment, 0, 1);
            tableLayoutPanel3.Controls.Add(this.textInstall, 0, 0);
            tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new System.Drawing.Size(838, 230);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // buttonInstall
            // 
            this.buttonInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInstall.Location = new System.Drawing.Point(422, 203);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(413, 24);
            this.buttonInstall.TabIndex = 8;
            this.buttonInstall.Text = "Install";
            this.buttonInstall.UseVisualStyleBackColor = true;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // buttonCreateEnvironment
            // 
            this.buttonCreateEnvironment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateEnvironment.Location = new System.Drawing.Point(3, 203);
            this.buttonCreateEnvironment.Name = "buttonCreateEnvironment";
            this.buttonCreateEnvironment.Size = new System.Drawing.Size(413, 24);
            this.buttonCreateEnvironment.TabIndex = 7;
            this.buttonCreateEnvironment.Text = "New Env.";
            this.buttonCreateEnvironment.UseVisualStyleBackColor = true;
            this.buttonCreateEnvironment.Click += new System.EventHandler(this.buttonCreateEnvironment_Click);
            // 
            // textInstall
            // 
            this.textInstall.AcceptsTab = true;
            this.textInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            tableLayoutPanel3.SetColumnSpan(this.textInstall, 2);
            this.textInstall.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textInstall.HideSelection = false;
            this.textInstall.Location = new System.Drawing.Point(3, 3);
            this.textInstall.Multiline = true;
            this.textInstall.Name = "textInstall";
            this.textInstall.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textInstall.Size = new System.Drawing.Size(832, 194);
            this.textInstall.TabIndex = 6;
            // 
            // textResultInstall
            // 
            this.textResultInstall.AcceptsTab = true;
            this.textResultInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textResultInstall.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textResultInstall.HideSelection = false;
            this.textResultInstall.Location = new System.Drawing.Point(0, 0);
            this.textResultInstall.Multiline = true;
            this.textResultInstall.Name = "textResultInstall";
            this.textResultInstall.ReadOnly = true;
            this.textResultInstall.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textResultInstall.Size = new System.Drawing.Size(838, 123);
            this.textResultInstall.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(844, 363);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Evaluate";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tableLayoutPanel4);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.textResultEvaluate);
            splitContainer2.Size = new System.Drawing.Size(838, 357);
            splitContainer2.SplitterDistance = 214;
            splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            tableLayoutPanel4.Controls.Add(panel1, 2, 1);
            tableLayoutPanel4.Controls.Add(this.buttonEvalExpression, 1, 1);
            tableLayoutPanel4.Controls.Add(this.buttonJitExpression, 0, 1);
            tableLayoutPanel4.Controls.Add(this.textEvaluate, 0, 0);
            tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel4.Size = new System.Drawing.Size(838, 214);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(this.comboSelf);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(561, 187);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(274, 24);
            panel1.TabIndex = 10;
            // 
            // comboSelf
            // 
            this.comboSelf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSelf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSelf.FormattingEnabled = true;
            this.comboSelf.Items.AddRange(new object[] {
            "nil",
            "result"});
            this.comboSelf.Location = new System.Drawing.Point(35, 3);
            this.comboSelf.Name = "comboSelf";
            this.comboSelf.Size = new System.Drawing.Size(236, 21);
            this.comboSelf.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 13);
            label1.TabIndex = 0;
            label1.Text = "self:";
            // 
            // buttonEvalExpression
            // 
            this.buttonEvalExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEvalExpression.Location = new System.Drawing.Point(282, 187);
            this.buttonEvalExpression.Name = "buttonEvalExpression";
            this.buttonEvalExpression.Size = new System.Drawing.Size(273, 24);
            this.buttonEvalExpression.TabIndex = 9;
            this.buttonEvalExpression.Text = "Eval Expression";
            this.buttonEvalExpression.UseVisualStyleBackColor = true;
            this.buttonEvalExpression.Click += new System.EventHandler(this.buttonEvalExpression_Click);
            // 
            // buttonJitExpression
            // 
            this.buttonJitExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonJitExpression.Location = new System.Drawing.Point(3, 187);
            this.buttonJitExpression.Name = "buttonJitExpression";
            this.buttonJitExpression.Size = new System.Drawing.Size(273, 24);
            this.buttonJitExpression.TabIndex = 8;
            this.buttonJitExpression.Text = "JIT Expression";
            this.buttonJitExpression.UseVisualStyleBackColor = true;
            this.buttonJitExpression.Click += new System.EventHandler(this.buttonJitExpression_Click);
            // 
            // textEvaluate
            // 
            this.textEvaluate.AcceptsTab = true;
            this.textEvaluate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            tableLayoutPanel4.SetColumnSpan(this.textEvaluate, 3);
            this.textEvaluate.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEvaluate.HideSelection = false;
            this.textEvaluate.Location = new System.Drawing.Point(3, 3);
            this.textEvaluate.Multiline = true;
            this.textEvaluate.Name = "textEvaluate";
            this.textEvaluate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textEvaluate.Size = new System.Drawing.Size(832, 178);
            this.textEvaluate.TabIndex = 7;
            // 
            // textResultEvaluate
            // 
            this.textResultEvaluate.AcceptsTab = true;
            this.textResultEvaluate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textResultEvaluate.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textResultEvaluate.HideSelection = false;
            this.textResultEvaluate.Location = new System.Drawing.Point(0, 0);
            this.textResultEvaluate.Multiline = true;
            this.textResultEvaluate.Name = "textResultEvaluate";
            this.textResultEvaluate.ReadOnly = true;
            this.textResultEvaluate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textResultEvaluate.Size = new System.Drawing.Size(838, 139);
            this.textResultEvaluate.TabIndex = 8;
            // 
            // WorkspaceTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 389);
            this.Controls.Add(tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorkspaceTester";
            this.Text = "Compiler / Installer / Code Generator - Tester";
            this.Load += new System.EventHandler(this.WorkspaceTester_Load);
            tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer2)).EndInit();
            splitContainer2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonInstall;
        private System.Windows.Forms.Button buttonCreateEnvironment;
        public System.Windows.Forms.TextBox textInstall;
        public System.Windows.Forms.TextBox textResultInstall;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox comboSelf;
        private System.Windows.Forms.Button buttonEvalExpression;
        private System.Windows.Forms.Button buttonJitExpression;
        public System.Windows.Forms.TextBox textEvaluate;
        public System.Windows.Forms.TextBox textResultEvaluate;


    }
}
