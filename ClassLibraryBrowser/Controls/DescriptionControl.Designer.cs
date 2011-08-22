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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls
{
    partial class DescriptionControl
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
            this.panelWebControl = new System.Windows.Forms.Panel();
            this.webControl = new System.Windows.Forms.WebBrowser();
            this.labelDescription = new System.Windows.Forms.Label();
            this.panelWebControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWebControl
            // 
            this.panelWebControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWebControl.BackColor = System.Drawing.SystemColors.Control;
            this.panelWebControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWebControl.Controls.Add(this.webControl);
            this.panelWebControl.Location = new System.Drawing.Point(0, 19);
            this.panelWebControl.Name = "panelWebControl";
            this.panelWebControl.Size = new System.Drawing.Size(150, 131);
            this.panelWebControl.TabIndex = 1;
            // 
            // webControl
            // 
            this.webControl.AllowNavigation = false;
            this.webControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webControl.IsWebBrowserContextMenuEnabled = false;
            this.webControl.Location = new System.Drawing.Point(0, 0);
            this.webControl.MinimumSize = new System.Drawing.Size(20, 20);
            this.webControl.Name = "webControl";
            this.webControl.Size = new System.Drawing.Size(148, 129);
            this.webControl.TabIndex = 9;
            this.webControl.Visible = false;
            this.webControl.Validating += new System.ComponentModel.CancelEventHandler(this.webControl_Validating);
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(-3, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelDescription.Size = new System.Drawing.Size(63, 19);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DescriptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelWebControl);
            this.Controls.Add(this.labelDescription);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DescriptionControl";
            this.panelWebControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.WebBrowser webControl;
        internal System.Windows.Forms.Label labelDescription;
        internal System.Windows.Forms.Panel panelWebControl;


    }
}
