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
    partial class ClassesControl
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
            System.Windows.Forms.SplitContainer splitContainer;
            this.classHierarchyControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassHierarchyControl();
            this.classDefinitionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes.ClassDefinitionControl();
            splitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(splitContainer)).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(this.classHierarchyControl);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(this.classDefinitionControl);
            splitContainer.Size = new System.Drawing.Size(504, 325);
            splitContainer.SplitterDistance = 224;
            splitContainer.TabIndex = 0;
            // 
            // classHierarchyControl
            // 
            this.classHierarchyControl.ClassHolder = null;
            this.classHierarchyControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classHierarchyControl.Enabled = false;
            this.classHierarchyControl.Location = new System.Drawing.Point(0, 0);
            this.classHierarchyControl.Name = "classHierarchyControl";
            this.classHierarchyControl.Size = new System.Drawing.Size(224, 325);
            this.classHierarchyControl.SystemImplementationHolder = null;
            this.classHierarchyControl.TabIndex = 0;
            // 
            // classDefinitionControl
            // 
            this.classDefinitionControl.Dirty = false;
            this.classDefinitionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.classDefinitionControl.Enabled = false;
            this.classDefinitionControl.Location = new System.Drawing.Point(0, 0);
            this.classDefinitionControl.Name = "classDefinitionControl";
            this.classDefinitionControl.Size = new System.Drawing.Size(276, 325);
            this.classDefinitionControl.SystemImplementationHolder = null;
            this.classDefinitionControl.TabIndex = 0;
            // 
            // ClassesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer);
            this.Name = "ClassesControl";
            this.Size = new System.Drawing.Size(504, 325);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer)).EndInit();
            splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ClassHierarchyControl classHierarchyControl;
        private ClassDefinitionControl classDefinitionControl;

    }
}
