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
    partial class ConformingProtocolsControl
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
            System.Windows.Forms.ColumnHeader columnHeader5;
            System.Windows.Forms.ColumnHeader columnHeader13;
            System.Windows.Forms.ColumnHeader columnHeader12;
            System.Windows.Forms.ColumnHeader columnHeader6;
            System.Windows.Forms.ColumnHeader columnHeader7;
            System.Windows.Forms.ColumnHeader columnHeader8;
            System.Windows.Forms.ColumnHeader columnHeader9;
            System.Windows.Forms.ColumnHeader columnHeader10;
            System.Windows.Forms.ColumnHeader columnHeader11;
            this.listProtocols = new System.Windows.Forms.ListView();
            columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Protocol";
            columnHeader5.Width = 230;
            // 
            // columnHeader13
            // 
            columnHeader13.Text = "X3J20";
            columnHeader13.Width = 70;
            // 
            // columnHeader12
            // 
            columnHeader12.Text = "Abstr.";
            columnHeader12.Width = 50;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Msgs.";
            columnHeader6.Width = 50;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Classes";
            columnHeader7.Width = 50;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Globals";
            columnHeader8.Width = 50;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "Consts.";
            columnHeader9.Width = 50;
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "Pools";
            columnHeader10.Width = 50;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "Undef.";
            columnHeader11.Width = 50;
            // 
            // listProtocols
            // 
            this.listProtocols.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader5,
            columnHeader13,
            columnHeader12,
            columnHeader6,
            columnHeader7,
            columnHeader8,
            columnHeader9,
            columnHeader10,
            columnHeader11});
            this.listProtocols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listProtocols.FullRowSelect = true;
            this.listProtocols.Location = new System.Drawing.Point(0, 0);
            this.listProtocols.MultiSelect = false;
            this.listProtocols.Name = "listProtocols";
            this.listProtocols.ShowGroups = false;
            this.listProtocols.Size = new System.Drawing.Size(428, 259);
            this.listProtocols.TabIndex = 1;
            this.listProtocols.UseCompatibleStateImageBehavior = false;
            this.listProtocols.View = System.Windows.Forms.View.Details;
            // 
            // ConformingProtocolsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listProtocols);
            this.DoubleBuffered = true;
            this.Name = "ConformingProtocolsControl";
            this.Size = new System.Drawing.Size(428, 259);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listProtocols;
    }
}
