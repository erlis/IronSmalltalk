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
    partial class ProtocolsControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProtocolsControl));
            this.tabControlProtocol = new System.Windows.Forms.TabControl();
            this.tabPageProtocol = new System.Windows.Forms.TabPage();
            this.protocolDefinitionControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ProtocolDefinitionControl();
            this.tabPageGlobals = new System.Windows.Forms.TabPage();
            this.protocolGlobalControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ProtocolGlobalControl();
            this.tabPageMessages = new System.Windows.Forms.TabPage();
            this.protocolMessagesControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ProtocolMessagesControl();
            this.tabPageConformingProtocols = new System.Windows.Forms.TabPage();
            this.conformingProtocolsControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ConformingProtocolsControl();
            this.tabPageConformsToProtocols = new System.Windows.Forms.TabPage();
            this.conformsToProtocolsControl = new IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols.ConformsToProtocolsControl();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.comboProtocols = new System.Windows.Forms.ComboBox();
            this.tabControlProtocol.SuspendLayout();
            this.tabPageProtocol.SuspendLayout();
            this.tabPageGlobals.SuspendLayout();
            this.tabPageMessages.SuspendLayout();
            this.tabPageConformingProtocols.SuspendLayout();
            this.tabPageConformsToProtocols.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlProtocol
            // 
            this.tabControlProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlProtocol.Controls.Add(this.tabPageProtocol);
            this.tabControlProtocol.Controls.Add(this.tabPageGlobals);
            this.tabControlProtocol.Controls.Add(this.tabPageMessages);
            this.tabControlProtocol.Controls.Add(this.tabPageConformingProtocols);
            this.tabControlProtocol.Controls.Add(this.tabPageConformsToProtocols);
            this.tabControlProtocol.Enabled = false;
            this.tabControlProtocol.ImageList = this.imageList;
            this.tabControlProtocol.Location = new System.Drawing.Point(3, 30);
            this.tabControlProtocol.Name = "tabControlProtocol";
            this.tabControlProtocol.SelectedIndex = 0;
            this.tabControlProtocol.Size = new System.Drawing.Size(586, 343);
            this.tabControlProtocol.TabIndex = 6;
            this.tabControlProtocol.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlProtocol_Selected);
            this.tabControlProtocol.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlProtocol_Deselecting);
            // 
            // tabPageProtocol
            // 
            this.tabPageProtocol.Controls.Add(this.protocolDefinitionControl);
            this.tabPageProtocol.ImageKey = "handshake";
            this.tabPageProtocol.Location = new System.Drawing.Point(4, 23);
            this.tabPageProtocol.Name = "tabPageProtocol";
            this.tabPageProtocol.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProtocol.Size = new System.Drawing.Size(578, 316);
            this.tabPageProtocol.TabIndex = 0;
            this.tabPageProtocol.Text = "Protocol";
            this.tabPageProtocol.UseVisualStyleBackColor = true;
            // 
            // protocolDefinitionControl
            // 
            this.protocolDefinitionControl.AllProtocolsHolder = null;
            this.protocolDefinitionControl.Dirty = false;
            this.protocolDefinitionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.protocolDefinitionControl.Location = new System.Drawing.Point(3, 3);
            this.protocolDefinitionControl.Name = "protocolDefinitionControl";
            this.protocolDefinitionControl.ProtocolHolder = null;
            this.protocolDefinitionControl.Size = new System.Drawing.Size(572, 310);
            this.protocolDefinitionControl.SystemDescriptionHolder = null;
            this.protocolDefinitionControl.TabIndex = 0;
            // 
            // tabPageGlobals
            // 
            this.tabPageGlobals.Controls.Add(this.protocolGlobalControl);
            this.tabPageGlobals.ImageKey = "structure";
            this.tabPageGlobals.Location = new System.Drawing.Point(4, 23);
            this.tabPageGlobals.Name = "tabPageGlobals";
            this.tabPageGlobals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGlobals.Size = new System.Drawing.Size(578, 316);
            this.tabPageGlobals.TabIndex = 1;
            this.tabPageGlobals.Text = "Globals";
            this.tabPageGlobals.UseVisualStyleBackColor = true;
            // 
            // protocolGlobalControl
            // 
            this.protocolGlobalControl.Dirty = false;
            this.protocolGlobalControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.protocolGlobalControl.Location = new System.Drawing.Point(3, 3);
            this.protocolGlobalControl.Name = "protocolGlobalControl";
            this.protocolGlobalControl.ProtocolHolder = null;
            this.protocolGlobalControl.Size = new System.Drawing.Size(572, 310);
            this.protocolGlobalControl.TabIndex = 0;
            // 
            // tabPageMessages
            // 
            this.tabPageMessages.Controls.Add(this.protocolMessagesControl);
            this.tabPageMessages.ImageKey = "method";
            this.tabPageMessages.Location = new System.Drawing.Point(4, 23);
            this.tabPageMessages.Name = "tabPageMessages";
            this.tabPageMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMessages.Size = new System.Drawing.Size(578, 316);
            this.tabPageMessages.TabIndex = 2;
            this.tabPageMessages.Text = "Messages";
            this.tabPageMessages.UseVisualStyleBackColor = true;
            // 
            // protocolMessagesControl
            // 
            this.protocolMessagesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.protocolMessagesControl.Location = new System.Drawing.Point(3, 3);
            this.protocolMessagesControl.Name = "protocolMessagesControl";
            this.protocolMessagesControl.ProtocolHolder = null;
            this.protocolMessagesControl.Size = new System.Drawing.Size(572, 310);
            this.protocolMessagesControl.TabIndex = 0;
            // 
            // tabPageConformingProtocols
            // 
            this.tabPageConformingProtocols.Controls.Add(this.conformingProtocolsControl);
            this.tabPageConformingProtocols.ImageKey = "hierarchy";
            this.tabPageConformingProtocols.Location = new System.Drawing.Point(4, 23);
            this.tabPageConformingProtocols.Name = "tabPageConformingProtocols";
            this.tabPageConformingProtocols.Size = new System.Drawing.Size(578, 316);
            this.tabPageConformingProtocols.TabIndex = 3;
            this.tabPageConformingProtocols.Text = "Conforming Protocols";
            this.tabPageConformingProtocols.UseVisualStyleBackColor = true;
            // 
            // conformingProtocolsControl
            // 
            this.conformingProtocolsControl.AllProtocolsHolder = null;
            this.conformingProtocolsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conformingProtocolsControl.Location = new System.Drawing.Point(0, 0);
            this.conformingProtocolsControl.Name = "conformingProtocolsControl";
            this.conformingProtocolsControl.ProtocolHolder = null;
            this.conformingProtocolsControl.Size = new System.Drawing.Size(578, 316);
            this.conformingProtocolsControl.TabIndex = 0;
            // 
            // tabPageConformsToProtocols
            // 
            this.tabPageConformsToProtocols.Controls.Add(this.conformsToProtocolsControl);
            this.tabPageConformsToProtocols.ImageKey = "hierarchy";
            this.tabPageConformsToProtocols.Location = new System.Drawing.Point(4, 23);
            this.tabPageConformsToProtocols.Name = "tabPageConformsToProtocols";
            this.tabPageConformsToProtocols.Size = new System.Drawing.Size(578, 316);
            this.tabPageConformsToProtocols.TabIndex = 4;
            this.tabPageConformsToProtocols.Text = "Conforms to Protocols";
            this.tabPageConformsToProtocols.UseVisualStyleBackColor = true;
            // 
            // conformsToProtocolsControl
            // 
            this.conformsToProtocolsControl.AllProtocolsHolder = null;
            this.conformsToProtocolsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conformsToProtocolsControl.Location = new System.Drawing.Point(0, 0);
            this.conformsToProtocolsControl.Name = "conformsToProtocolsControl";
            this.conformsToProtocolsControl.ProtocolHolder = null;
            this.conformsToProtocolsControl.Size = new System.Drawing.Size(578, 316);
            this.conformsToProtocolsControl.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "method");
            this.imageList.Images.SetKeyName(1, "field");
            this.imageList.Images.SetKeyName(2, "structure");
            this.imageList.Images.SetKeyName(3, "handshake");
            this.imageList.Images.SetKeyName(4, "enum");
            this.imageList.Images.SetKeyName(5, "class");
            this.imageList.Images.SetKeyName(6, "source");
            this.imageList.Images.SetKeyName(7, "document");
            this.imageList.Images.SetKeyName(8, "hierarchy");
            // 
            // comboProtocols
            // 
            this.comboProtocols.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboProtocols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProtocols.Enabled = false;
            this.comboProtocols.FormattingEnabled = true;
            this.comboProtocols.Location = new System.Drawing.Point(3, 3);
            this.comboProtocols.Name = "comboProtocols";
            this.comboProtocols.Size = new System.Drawing.Size(586, 21);
            this.comboProtocols.TabIndex = 5;
            this.comboProtocols.SelectedIndexChanged += new System.EventHandler(this.comboProtocols_SelectedIndexChanged);
            this.comboProtocols.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.comboProtocols_Format);
            // 
            // ProtocolsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlProtocol);
            this.Controls.Add(this.comboProtocols);
            this.DoubleBuffered = true;
            this.Name = "ProtocolsControl";
            this.Size = new System.Drawing.Size(592, 376);
            this.tabControlProtocol.ResumeLayout(false);
            this.tabPageProtocol.ResumeLayout(false);
            this.tabPageGlobals.ResumeLayout(false);
            this.tabPageMessages.ResumeLayout(false);
            this.tabPageConformingProtocols.ResumeLayout(false);
            this.tabPageConformsToProtocols.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabPage tabPageProtocol;
        public System.Windows.Forms.TabPage tabPageGlobals;
        public System.Windows.Forms.TabPage tabPageMessages;
        private System.Windows.Forms.TabPage tabPageConformingProtocols;
        public System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TabControl tabControlProtocol;
        private System.Windows.Forms.ComboBox comboProtocols;
        private ProtocolDefinitionControl protocolDefinitionControl;
        private ProtocolGlobalControl protocolGlobalControl;
        private ProtocolMessagesControl protocolMessagesControl;
        private ConformingProtocolsControl conformingProtocolsControl;
        private System.Windows.Forms.TabPage tabPageConformsToProtocols;
        private ConformsToProtocolsControl conformsToProtocolsControl;
    }
}
