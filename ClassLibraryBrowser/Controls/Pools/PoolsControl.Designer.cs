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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools
{
    partial class PoolsControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.poolDefinitionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools.PoolDefinitionControl();
            this.poolValuesControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Pools.PoolValuesControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.poolDefinitionControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.poolValuesControl);
            this.splitContainer1.Size = new System.Drawing.Size(550, 400);
            this.splitContainer1.SplitterDistance = 267;
            this.splitContainer1.TabIndex = 0;
            // 
            // poolDefinitionControl
            // 
            this.poolDefinitionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poolDefinitionControl.Location = new System.Drawing.Point(0, 0);
            this.poolDefinitionControl.Name = "poolDefinitionControl";
            this.poolDefinitionControl.Size = new System.Drawing.Size(267, 400);
            this.poolDefinitionControl.TabIndex = 0;
            // 
            // poolValuesControl
            // 
            this.poolValuesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poolValuesControl.Location = new System.Drawing.Point(0, 0);
            this.poolValuesControl.Name = "poolValuesControl";
            this.poolValuesControl.Size = new System.Drawing.Size(279, 400);
            this.poolValuesControl.TabIndex = 0;
            // 
            // PoolsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PoolsControl";
            this.Size = new System.Drawing.Size(550, 400);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private PoolDefinitionControl poolDefinitionControl;
        private PoolValuesControl poolValuesControl;
    }
}
