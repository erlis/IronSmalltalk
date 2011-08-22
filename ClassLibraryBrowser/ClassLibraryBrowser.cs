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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;
using System.Xml;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Description;

namespace IronSmalltalk.Tools.ClassLibraryBrowser
{
    public partial class ClassLibraryBrowser : Form
    {
        #region Browser

        [Browsable(false)]
        public readonly ValueHolder<SmalltalkSystem> SmalltalkSystemHolder;
        [Browsable(false)]
        public readonly ValueHolder<SystemDescription> SystemDescriptionHolder;
        [Browsable(false)]
        public readonly ValueHolder<SystemImplementation> SystemImplementationHolder;

        public SmalltalkSystem SmalltalkSystem
        {
            get
            {
                if (this.SmalltalkSystemHolder == null)
                    return null;
                return this.SmalltalkSystemHolder.Value;
            }
        }

        public ClassLibraryBrowser()
        {
            InitializeComponent();
            this.SmalltalkSystemHolder = new ValueHolder<SmalltalkSystem>();
            this.SmalltalkSystemHolder.Changed += new ValueChangedEventHandler<SmalltalkSystem>(this.SmalltalkSystemChanged);
            this.SystemDescriptionHolder = new ValueHolder<SystemDescription>();
            this.SystemImplementationHolder = new ValueHolder<SystemImplementation>();
            this.protocolsControl.SystemDescriptionHolder = this.SystemDescriptionHolder;
            this.classesControl.SystemImplementationHolder = this.SystemImplementationHolder;
            this.globalsControl.SystemImplementationHolder = this.SystemImplementationHolder;
            this.classMethodsEditor.SystemImplementationHolder = this.SystemImplementationHolder;
            this.poolsControl.SystemImplementationHolder = this.SystemImplementationHolder;
            this.initializersControl.SystemImplementationHolder = this.SystemImplementationHolder;
            this.implementationValidationControl.SmalltalkSystemHolder = this.SmalltalkSystemHolder;

            this.AddMainMenuItems(this);
            List<ToolStripItem> items = new List<ToolStripItem>();
            foreach (ToolStripItem tsi in this.menuStrip.Items)
                items.Add(tsi);
            this.menuStrip.Items.Clear();
            this.menuStrip.Items.AddRange(items.OrderBy(tsi => tsi.MergeIndex).ToArray());
        }

        private void AddMainMenuItems(Control control)
        {
            if (control is Controls.IMainFormMenuConsumer)
                ((Controls.IMainFormMenuConsumer)control).AddMainMenuItems(this.menuStrip);
            foreach (Control child in control.Controls)
                this.AddMainMenuItems(child);
        }

        private void SmalltalkSystemChanged(object sender, ValueChangedEventArgs<SmalltalkSystem> e)
        {
            this.saveAsToolStripMenuItem.Enabled = (this.SmalltalkSystem != null);
            this.saveToolStripMenuItem.Enabled = (this.SmalltalkSystem != null);
            this.fileOutToolStripMenuItem.Enabled = (this.SmalltalkSystem != null);
            if (this.SmalltalkSystem == null)
            {
                this.SystemDescriptionHolder.Value = null;
                this.SystemImplementationHolder.Value = null;
            }
            else
            {
                this.SystemDescriptionHolder.Value = this.SmalltalkSystem.SystemDescription;
                this.SystemImplementationHolder.Value = this.SmalltalkSystem.SystemImplementation;
            }
        }

        private void ClassLibraryBrowser_Load(object sender, EventArgs e)
        {
            string filename = Properties.Settings.Default.LastFileName;
            if (!String.IsNullOrWhiteSpace(filename) && File.Exists(filename))
            {
                this.openLastToolStripMenuItem.Text = Properties.Settings.Default.LastFileName;
                this.openLastToolStripMenuItem.Visible = true;
            }
            this.protocolsControl.VisibilityChanged(false);
            this.classesControl.VisibilityChanged(false);
            this.globalsControl.VisibilityChanged(false);
            this.classMethodsEditor.VisibilityChanged(false);
        }


        #endregion

        #region File Menu

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog.InitialDirectory = Path.GetDirectoryName(Application.StartupPath);
            if (this.openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            SmalltalkSystem ss = SmalltalkSystem.LoadFrom(this.openFileDialog.FileName);
            Properties.Settings.Default.LastFileName = this.openFileDialog.FileName;
            Properties.Settings.Default.Save();
            this.openLastToolStripMenuItem.Text = Properties.Settings.Default.LastFileName;
            this.openLastToolStripMenuItem.Visible = true;
            this.SmalltalkSystemHolder.Value = ss;
        }

        private void openLastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = Properties.Settings.Default.LastFileName;
            if (String.IsNullOrWhiteSpace(filename))
                return;
            if (!File.Exists(filename))
                return;
            SmalltalkSystem ss = SmalltalkSystem.LoadFrom(filename);
            this.SmalltalkSystemHolder.Value = ss;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SmalltalkSystem == null)
                return;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;
            this.CreateBackup(this.SmalltalkSystem.FilePath);
            using (XmlWriter writer = XmlWriter.Create(this.SmalltalkSystem.FilePath, settings))
                this.SmalltalkSystem.Save(writer);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SmalltalkSystem == null)
                return;
            this.saveFileDialog.InitialDirectory = Path.GetDirectoryName(Application.StartupPath);
            if (this.saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;
            this.CreateBackup(this.saveFileDialog.FileName);
            using(XmlWriter writer = XmlWriter.Create(this.saveFileDialog.FileName, settings))
                this.SmalltalkSystem.Save(writer);
        }

        private void CreateBackup(string filepath)
        {
            if (String.IsNullOrWhiteSpace(filepath))
                return;
            if (!File.Exists(filepath))
                return;
            string backupName = Path.Combine(
                Path.GetDirectoryName(filepath),
                Path.GetFileNameWithoutExtension(filepath) +
                String.Format(".Backup {0:yyyyMMdd HHmmss}", DateTime.Now) + 
                Path.GetExtension(filepath));
            File.Copy(filepath, backupName);
        }

        private void fileOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.SmalltalkSystem == null)
                return;
            this.fileOutSaveDialog.InitialDirectory = Path.GetDirectoryName(Application.StartupPath);
            if (this.fileOutSaveDialog.ShowDialog(this) != DialogResult.OK)
                return;
            this.CreateBackup(this.fileOutSaveDialog.FileName);

            using (StreamWriter writer = new StreamWriter(this.fileOutSaveDialog.FileName, false, Encoding.UTF8))
                Definitions.Saving.FileOutWriterIST10.FileOut(this.SmalltalkSystem.SystemImplementation, writer);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Workspace Menu

        private void emptyWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Testing.WorkspaceForm ws = new Testing.WorkspaceForm(null, null);
            ws.Show();
        }

        private void newWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string installText = null;
            if (this.SmalltalkSystem != null)
            {
                StringBuilder str = new StringBuilder();
                StringWriter writer = new StringWriter(str);
                Definitions.Saving.FileOutWriterIST10.FileOut(this.SmalltalkSystem.SystemImplementation, writer);
                installText = str.ToString();
            }

            Testing.WorkspaceForm ws = new Testing.WorkspaceForm(
                installText,
                Properties.Settings.Default.LastWorkspaceEvaluateText);
            ws.Show();
        }

        private void lastWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Testing.WorkspaceForm ws = new Testing.WorkspaceForm(
                Properties.Settings.Default.LastWorkspaceInstallText,
                Properties.Settings.Default.LastWorkspaceEvaluateText);
            ws.Show();
        }

        #endregion

        private void tabControlMain_Selected(object sender, TabControlEventArgs e)
        {
            this.protocolsControl.VisibilityChanged(e.TabPage == this.tabProtocols);
            this.classesControl.VisibilityChanged(e.TabPage == this.tabClassHierarchy);
            this.globalsControl.VisibilityChanged(e.TabPage == this.tabGlobals);
            this.classMethodsEditor.VisibilityChanged(e.TabPage == this.tabMethods);
            this.poolsControl.VisibilityChanged(e.TabPage == this.tabPools);
            this.initializersControl.VisibilityChanged(e.TabPage == this.tabInitializers);
        }

    }
}
