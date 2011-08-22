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
    partial class ProtocolMessagesControl
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
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.ColumnHeader columnHeader3;
            this.listMessages = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.VetoingListView();
            this.protocolMessageControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ProtocolMessageControl();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(this.listMessages);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this.protocolMessageControl);
            splitContainer1.Size = new System.Drawing.Size(492, 421);
            splitContainer1.SplitterDistance = 190;
            splitContainer1.TabIndex = 1;
            // 
            // listMessages
            // 
            this.listMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader3});
            this.listMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMessages.FullRowSelect = true;
            this.listMessages.HideSelection = false;
            this.listMessages.Location = new System.Drawing.Point(0, 0);
            this.listMessages.MultiSelect = false;
            this.listMessages.Name = "listMessages";
            this.listMessages.ShowGroups = false;
            this.listMessages.Size = new System.Drawing.Size(190, 421);
            this.listMessages.TabIndex = 0;
            this.listMessages.UseCompatibleStateImageBehavior = false;
            this.listMessages.View = System.Windows.Forms.View.Details;
            this.listMessages.ItemChanging += new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.ListItemChangingEventHandler(this.listMessages_ItemChanging);
            this.listMessages.SizeChanged += new System.EventHandler(this.listMessages_SizeChanged);
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Message";
            columnHeader3.Width = 194;
            // 
            // protocolMessageControl
            // 
            this.protocolMessageControl.Dirty = false;
            this.protocolMessageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.protocolMessageControl.Location = new System.Drawing.Point(0, 0);
            this.protocolMessageControl.MessageHolder = null;
            this.protocolMessageControl.Name = "protocolMessageControl";
            this.protocolMessageControl.ProtocolHolder = null;
            this.protocolMessageControl.Size = new System.Drawing.Size(298, 421);
            this.protocolMessageControl.TabIndex = 0;
            // 
            // ProtocolMessagesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "ProtocolMessagesControl";
            this.Size = new System.Drawing.Size(492, 421);
            this.Load += new System.EventHandler(this.ProtocolMessagesControl_Load);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ProtocolMessageControl protocolMessageControl;
        private VetoingListView listMessages;
    }
}
