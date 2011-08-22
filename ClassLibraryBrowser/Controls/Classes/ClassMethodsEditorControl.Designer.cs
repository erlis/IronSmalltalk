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
    partial class ClassMethodsEditorControl
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
            System.Windows.Forms.SplitContainer splitContainer5;
            System.Windows.Forms.SplitContainer splitContainer6;
            this.classHierarchy = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassHierarchyControl();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.classMethodsProtocols = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassMethodsProtocolsControl();
            this.classMethodsList = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassMethodsListControl();
            this.methodDefinition = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Methods.MethodDefinitionControl();
            splitContainer5 = new System.Windows.Forms.SplitContainer();
            splitContainer6 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(splitContainer5)).BeginInit();
            splitContainer5.Panel1.SuspendLayout();
            splitContainer5.Panel2.SuspendLayout();
            splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer6)).BeginInit();
            splitContainer6.Panel1.SuspendLayout();
            splitContainer6.Panel2.SuspendLayout();
            splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer5
            // 
            splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer5.Location = new System.Drawing.Point(0, 0);
            splitContainer5.Name = "splitContainer5";
            splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            splitContainer5.Panel1.Controls.Add(splitContainer6);
            // 
            // splitContainer5.Panel2
            // 
            splitContainer5.Panel2.Controls.Add(this.methodDefinition);
            splitContainer5.Size = new System.Drawing.Size(427, 309);
            splitContainer5.SplitterDistance = 151;
            splitContainer5.TabIndex = 1;
            // 
            // splitContainer6
            // 
            splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer6.Location = new System.Drawing.Point(0, 0);
            splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            splitContainer6.Panel1.Controls.Add(this.classHierarchy);
            // 
            // splitContainer6.Panel2
            // 
            splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            splitContainer6.Size = new System.Drawing.Size(427, 151);
            splitContainer6.SplitterDistance = 141;
            splitContainer6.TabIndex = 0;
            // 
            // classHierarchy
            // 
            this.classHierarchy.ClassHolder = null;
            this.classHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classHierarchy.Enabled = false;
            this.classHierarchy.Location = new System.Drawing.Point(0, 0);
            this.classHierarchy.Name = "classHierarchy";
            this.classHierarchy.Size = new System.Drawing.Size(141, 151);
            this.classHierarchy.SystemImplementationHolder = null;
            this.classHierarchy.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.classMethodsProtocols);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.classMethodsList);
            this.splitContainer7.Size = new System.Drawing.Size(282, 151);
            this.splitContainer7.SplitterDistance = 125;
            this.splitContainer7.TabIndex = 0;
            // 
            // classMethodsProtocols
            // 
            this.classMethodsProtocols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classMethodsProtocols.Enabled = false;
            this.classMethodsProtocols.Location = new System.Drawing.Point(0, 0);
            this.classMethodsProtocols.Name = "classMethodsProtocols";
            this.classMethodsProtocols.Size = new System.Drawing.Size(125, 151);
            this.classMethodsProtocols.TabIndex = 0;
            // 
            // classMethodsList
            // 
            this.classMethodsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classMethodsList.Location = new System.Drawing.Point(0, 0);
            this.classMethodsList.Name = "classMethodsList";
            this.classMethodsList.Size = new System.Drawing.Size(153, 151);
            this.classMethodsList.TabIndex = 0;
            // 
            // methodDefinition
            // 
            this.methodDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.methodDefinition.Location = new System.Drawing.Point(0, 0);
            this.methodDefinition.Name = "methodDefinition";
            this.methodDefinition.Size = new System.Drawing.Size(427, 154);
            this.methodDefinition.TabIndex = 0;
            // 
            // ClassMethodsEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer5);
            this.Name = "ClassMethodsEditorControl";
            this.Size = new System.Drawing.Size(427, 309);
            splitContainer5.Panel1.ResumeLayout(false);
            splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer5)).EndInit();
            splitContainer5.ResumeLayout(false);
            splitContainer6.Panel1.ResumeLayout(false);
            splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer6)).EndInit();
            splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer7;
        private ClassHierarchyControl classHierarchy;
        private ClassMethodsListControl classMethodsList;
        private ClassMethodsProtocolsControl classMethodsProtocols;
        private Methods.MethodDefinitionControl methodDefinition;
    }
}
