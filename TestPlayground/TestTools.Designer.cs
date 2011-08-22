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
    partial class TestTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestTools));
            this.buttonLexTester = new System.Windows.Forms.Button();
            this.buttonParseTester = new System.Windows.Forms.Button();
            this.buttonBulkParseTest = new System.Windows.Forms.Button();
            this.buttonProcessInterchangeFiles = new System.Windows.Forms.Button();
            this.buttonInstallTester = new System.Windows.Forms.Button();
            this.buttonSimpleRuntimeTest = new System.Windows.Forms.Button();
            this.buttonWorkspaceTester = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLexTester
            // 
            this.buttonLexTester.Location = new System.Drawing.Point(12, 12);
            this.buttonLexTester.Name = "buttonLexTester";
            this.buttonLexTester.Size = new System.Drawing.Size(260, 32);
            this.buttonLexTester.TabIndex = 0;
            this.buttonLexTester.Text = "Lex - Tester";
            this.buttonLexTester.UseVisualStyleBackColor = true;
            this.buttonLexTester.Click += new System.EventHandler(this.buttonLexTester_Click);
            // 
            // buttonParseTester
            // 
            this.buttonParseTester.Location = new System.Drawing.Point(12, 50);
            this.buttonParseTester.Name = "buttonParseTester";
            this.buttonParseTester.Size = new System.Drawing.Size(260, 32);
            this.buttonParseTester.TabIndex = 1;
            this.buttonParseTester.Text = "Parse - Tester";
            this.buttonParseTester.UseVisualStyleBackColor = true;
            this.buttonParseTester.Click += new System.EventHandler(this.buttonParseTester_Click);
            // 
            // buttonBulkParseTest
            // 
            this.buttonBulkParseTest.Location = new System.Drawing.Point(12, 88);
            this.buttonBulkParseTest.Name = "buttonBulkParseTest";
            this.buttonBulkParseTest.Size = new System.Drawing.Size(260, 32);
            this.buttonBulkParseTest.TabIndex = 1;
            this.buttonBulkParseTest.Text = "Bulk Parse Tester";
            this.buttonBulkParseTest.UseVisualStyleBackColor = true;
            this.buttonBulkParseTest.Click += new System.EventHandler(this.buttonBulkParseTest_Click);
            // 
            // buttonProcessInterchangeFiles
            // 
            this.buttonProcessInterchangeFiles.Location = new System.Drawing.Point(12, 126);
            this.buttonProcessInterchangeFiles.Name = "buttonProcessInterchangeFiles";
            this.buttonProcessInterchangeFiles.Size = new System.Drawing.Size(260, 32);
            this.buttonProcessInterchangeFiles.TabIndex = 2;
            this.buttonProcessInterchangeFiles.Text = "Process Interchange Files";
            this.buttonProcessInterchangeFiles.UseVisualStyleBackColor = true;
            this.buttonProcessInterchangeFiles.Click += new System.EventHandler(this.buttonProcessInterchangeFiles_Click);
            // 
            // buttonInstallTester
            // 
            this.buttonInstallTester.Location = new System.Drawing.Point(12, 202);
            this.buttonInstallTester.Name = "buttonInstallTester";
            this.buttonInstallTester.Size = new System.Drawing.Size(260, 32);
            this.buttonInstallTester.TabIndex = 2;
            this.buttonInstallTester.Text = "Install Tester";
            this.buttonInstallTester.UseVisualStyleBackColor = true;
            this.buttonInstallTester.Click += new System.EventHandler(this.buttonInstallTester_Click);
            // 
            // buttonSimpleRuntimeTest
            // 
            this.buttonSimpleRuntimeTest.Location = new System.Drawing.Point(12, 240);
            this.buttonSimpleRuntimeTest.Name = "buttonSimpleRuntimeTest";
            this.buttonSimpleRuntimeTest.Size = new System.Drawing.Size(260, 32);
            this.buttonSimpleRuntimeTest.TabIndex = 2;
            this.buttonSimpleRuntimeTest.Text = "Simple Runtime Test";
            this.buttonSimpleRuntimeTest.UseVisualStyleBackColor = true;
            this.buttonSimpleRuntimeTest.Click += new System.EventHandler(this.buttonSimpleRuntimeTest_Click);
            // 
            // buttonWorkspaceTester
            // 
            this.buttonWorkspaceTester.Location = new System.Drawing.Point(12, 164);
            this.buttonWorkspaceTester.Name = "buttonWorkspaceTester";
            this.buttonWorkspaceTester.Size = new System.Drawing.Size(260, 32);
            this.buttonWorkspaceTester.TabIndex = 2;
            this.buttonWorkspaceTester.Text = "Workspace Test";
            this.buttonWorkspaceTester.UseVisualStyleBackColor = true;
            this.buttonWorkspaceTester.Click += new System.EventHandler(this.buttonWorkspaceTester_Click);
            // 
            // TestTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 299);
            this.Controls.Add(this.buttonWorkspaceTester);
            this.Controls.Add(this.buttonSimpleRuntimeTest);
            this.Controls.Add(this.buttonInstallTester);
            this.Controls.Add(this.buttonProcessInterchangeFiles);
            this.Controls.Add(this.buttonBulkParseTest);
            this.Controls.Add(this.buttonParseTester);
            this.Controls.Add(this.buttonLexTester);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestTools";
            this.Text = "Iron Smalltalk Test Tools";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLexTester;
        private System.Windows.Forms.Button buttonParseTester;
        private System.Windows.Forms.Button buttonBulkParseTest;
        private System.Windows.Forms.Button buttonProcessInterchangeFiles;
        private System.Windows.Forms.Button buttonInstallTester;
        private System.Windows.Forms.Button buttonSimpleRuntimeTest;
        private System.Windows.Forms.Button buttonWorkspaceTester;
    }
}
