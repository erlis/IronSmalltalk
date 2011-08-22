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
using IronSmalltalk.Common;
using System.Threading;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Testing
{
    public partial class WorkspaceForm : Form, IWorkspaceClient
    {
        private Workspace Workspace;
        public WorkspaceForm()
        {
            this.Workspace = new Testing.Workspace(this);
            this.ReportErrorDelegate = new ReportErrorDelegateDefinition(this.ReportError);
            this.ReportResultDelegate = new ReportResultDelegateDefinition(this.ReportResult);
            this.InstallSourceCodeDelegate = new InstallSourceCodeDelegateDefinition(this.GetInstallSourceCode);
            this.EvaluateSourceCodeDelegate = new EvaluateSourceCodeDelegateDefinition(this.GetEvaluateSourceCode);
            InitializeComponent();
            this.TabPageChanged();
        }

        public WorkspaceForm(string installCode, string evalCode)
            : this()
        {
            this.txtEvaluate.Text = evalCode;
            this.txtInstall.Text = installCode;
            if (String.IsNullOrWhiteSpace(this.txtInstall.Text))
            {
                this.txtInstall.Text = "Smalltalk interchangeVersion: '1.0'!\n";
            }
            else
            {
                this.tabControl.SelectedTab = this.tabPageInstall;
                this.listErrors.Items.Clear();
                if (this.Workspace.Install())
                {
                    this.listErrors.Items.Add("OK");
                    this.tabControl.SelectedTab = this.tabPageEvaluate;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.FileName = Properties.Settings.Default.LastWorkspaceSaveFileName;
            if (this.saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            Properties.Settings.Default.LastWorkspaceSaveFileName = this.saveFileDialog.FileName;
            Properties.Settings.Default.Save();

            string txt;
            if (this.tabControl.SelectedTab == this.tabPageInstall)
                txt = this.txtInstall.Text;
            else
                txt = this.txtEvaluate.Text;
            System.IO.File.WriteAllText(this.saveFileDialog.FileName, txt, System.Text.Encoding.UTF8);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog.FileName = Properties.Settings.Default.LastWorkspaceOpenFileName;
            if (this.openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;
            Properties.Settings.Default.LastWorkspaceOpenFileName = this.openFileDialog.FileName;
            Properties.Settings.Default.Save();

            string txt = System.IO.File.ReadAllText(this.openFileDialog.FileName, System.Text.Encoding.UTF8);
            if (this.tabControl.SelectedTab == this.tabPageInstall)
                this.txtInstall.Text = txt;
            else
                this.txtEvaluate.Text = txt;
        }

        private void evaluateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab != this.tabPageEvaluate)
                return;

            Properties.Settings.Default.LastWorkspaceEvaluateText = this.txtEvaluate.Text;
            Properties.Settings.Default.Save();

            this.txtResult.Text = null;
            if (!this.Evaluate())
                this.txtResult.Text = "ERROR\n" + this.txtResult.Text;
        }

        private void inspectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.evaluateToolStripMenuItem_Click(sender, e);
            object value = this.Workspace.LastResult;
            System.Diagnostics.Debugger.Break();
        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab != this.tabPageInstall)
                return;

            Properties.Settings.Default.LastWorkspaceInstallText = this.txtInstall.Text;
            Properties.Settings.Default.Save();

            this.listErrors.Items.Clear();
            if (this.Workspace.Install())
                this.listErrors.Items.Add("OK");
        }

        private void newEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Workspace = new Workspace(this);
            this.listErrors.Items.Clear();
            this.txtResult.Text = null;
        }

        private void listErrors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listErrors.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = this.listErrors.SelectedItems[0];
            if (!(lvi.Tag is SourceLocation[]))
                return;
            SourceLocation[] args = (SourceLocation[])lvi.Tag;
            if ((args == null) || (args.Length != 2))
                return;
            int start = args[0].Position;
            int end = args[1].Position;

            this.txtInstall.SelectionLength = 0;
            this.txtInstall.SelectionStart = Math.Max(start, 0);
            this.txtInstall.SelectionLength = Math.Max(end - start + 1, 0);
            this.txtInstall.ScrollToCaret();
        }

        private void listErrors_Resize(object sender, EventArgs e)
        {
            int w = this.listErrors.Width;
            w = w - this.listErrors.Columns[0].Width;
            w = w - this.listErrors.Columns[1].Width;
            w = Math.Max(w - 20, 50);
            this.listErrors.Columns[2].Width = w;
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            this.TabPageChanged();
        }

        private void TabPageChanged()
        {
            if (this.tabControl.SelectedTab == this.tabPageInstall)
            {
                this.evaluateToolStripMenuItem.Enabled = false;
                this.inspectToolStripMenuItem.Enabled = false;
                this.installToolStripMenuItem.Enabled = true;
                this.newEnvironmentToolStripMenuItem.Enabled = true;
                this.poolsToolStripMenuItem.Enabled = false;
                this.variablesToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.evaluateToolStripMenuItem.Enabled = true;
                this.inspectToolStripMenuItem.Enabled = true;
                this.installToolStripMenuItem.Enabled = false;
                this.newEnvironmentToolStripMenuItem.Enabled = true;
                this.poolsToolStripMenuItem.Enabled = false;
                this.variablesToolStripMenuItem.Enabled = false;
            }
        }

        private void WorkspaceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((this.Owner != null) && !this.Owner.IsDisposed)
                this.Owner.Activate();
        }

        private void WorkspaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LastWorkspaceEvaluateText = this.txtEvaluate.Text;
            Properties.Settings.Default.LastWorkspaceInstallText = this.txtInstall.Text;
            Properties.Settings.Default.Save();
        }

        private void txtEvaluate_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ch = (int)e.KeyChar;
            if ((ch == 19) || (ch == 4))
                this.evaluateToolStripMenuItem_Click(sender, e);

            if (ch == 27)
                this.CancelEvaluation();
        }

        private void txtInstall_KeyPress(object sender, KeyPressEventArgs e)
        {
            int ch = (int)e.KeyChar;
            if ((ch == 14) || (ch == 19))
                this.installToolStripMenuItem_Click(sender, e);

        }

        private void CancelEvaluation()
        {
            Thread thread = this.EvaluationThread;
            if (thread != null)
            {
                thread.Abort();
                if (!thread.Join(1500))
                    this.txtResult.Text = "Thread Abort Timed-Out\n" + this.txtResult.Text;
                this.txtResult.Text = "Aborted Execution\n" + this.txtResult.Text;
                this.EvaluateSuccess = false;
                if (this.EvaluationThread == thread)
                    this.EvaluationThread = null;   // Yes, this is not thread safe ... so! This is just a dev. tool!
            }
        }

        private bool EvaluateSuccess;

        private bool Evaluate()
        {
            this.CancelEvaluation();
            this.EvaluateSuccess = false;
            Thread thread = new Thread(new ThreadStart(this.InternalEvaluate));
            thread.Name = "ST Evaliation Thread";
            thread.Start();
            while (!thread.Join(10))
                Application.DoEvents(); // Not the best design ... but will do the job
            return this.EvaluateSuccess;
        }

        private void InternalEvaluate()
        {
            this.CancelEvaluation();
            this.EvaluationThread = System.Threading.Thread.CurrentThread;
            this.Workspace.Evaluate();
            this.EvaluateSuccess = true;
            if (this.EvaluationThread == System.Threading.Thread.CurrentThread)
                this.EvaluationThread = null;   // Yes, this is not thread safe ... so! This is just a dev. tool!
        }

        private Thread EvaluationThread;

        private delegate void ReportErrorDelegateDefinition(string message, SourceLocation start, SourceLocation end);
        private ReportErrorDelegateDefinition ReportErrorDelegate;

        private void ReportError(string message, SourceLocation start, SourceLocation end)
        {
            if (this.tabControl.SelectedTab == this.tabPageInstall)
            {
                ListViewItem lvi = this.listErrors.Items.Add(start.ToString());
                lvi.SubItems.Add(end.ToString());
                lvi.SubItems.Add(message);
                lvi.Tag = new SourceLocation[] { start, end };
            }
            else
            {
                this.txtResult.Text = this.txtResult.Text +
                    String.Format("ERROR [{0} - {1}]: {2}\r\n", start, end, message);
            }
        }

        void IWorkspaceClient.ReportError(string message, SourceLocation start, SourceLocation end)
        {
            if (this.IsHandleCreated)
                this.Invoke(this.ReportErrorDelegate, message, start, end);
            else
                this.ReportError(message, start, end);
        }

        private delegate void ReportResultDelegateDefinition(string message);
        private ReportResultDelegateDefinition ReportResultDelegate;

        private void ReportResult(string message)
        {
            this.txtResult.Text = message;
        }

        void IWorkspaceClient.ReportResult(string message)
        {
            if (this.IsHandleCreated)
                this.Invoke(this.ReportResultDelegate, message);
            else
                this.ReportResult(message);
        }


        private delegate string InstallSourceCodeDelegateDefinition();
        private InstallSourceCodeDelegateDefinition InstallSourceCodeDelegate;

        string IWorkspaceClient.InstallSourceCode
        {
            get
            {
                if (this.IsHandleCreated)
                    return (string)this.Invoke(this.InstallSourceCodeDelegate);
                else
                    return this.GetInstallSourceCode();
            }
        }

        private string GetInstallSourceCode()
        {
            string txt = this.txtInstall.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.txtInstall.Text;
            return txt;
        }

        private delegate string EvaluateSourceCodeDelegateDefinition();
        private EvaluateSourceCodeDelegateDefinition EvaluateSourceCodeDelegate;

        private string GetEvaluateSourceCode()
        {
            string txt = this.txtEvaluate.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.txtEvaluate.Text;
            return txt;
        }

        string IWorkspaceClient.EvaluateSourceCode
        {
            get
            {
                if (this.IsHandleCreated)
                    return (string)this.Invoke(this.EvaluateSourceCodeDelegate);
                else
                    return this.GetEvaluateSourceCode();
            }
        }
    }
}
