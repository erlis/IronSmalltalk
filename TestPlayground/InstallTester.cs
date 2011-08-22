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
using IronSmalltalk.Runtime;
using IronSmalltalk.Compiler.Interchange;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Interchange;
using IronSmalltalk;
using IronSmalltalk.Common;

namespace TestPlayground
{
    public partial class InstallTester : Form, IInterchangeErrorSink, IInstallErrorSink 
    {
        SmalltalkEnvironment Environment;
        public InstallTester()
        {
            InitializeComponent();
        }

        private void buttonCreateEnvironment_Click(object sender, EventArgs e)
        {
            this.Environment = new SmalltalkEnvironment();
            this.listErrors.Items.Clear();
        }

        private void buttonDebug_Click(object sender, EventArgs e)
        {
            SmalltalkEnvironment env = this.Environment;
            System.Diagnostics.Debugger.Break();
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            if (this.Environment == null)
            {
                MessageBox.Show("First, create the environment.");
                return;
            }
            Properties.Settings.Default.LastInstallerSource = this.txtSource.Text;
            Properties.Settings.Default.Save();
            this.listErrors.Items.Clear();

            this.Environment.CompilerService.InstallSource(this.txtSource.Text, this, this);
        }

        private void AddError(string type, SourceLocation startPosition, SourceLocation stopPosition, string errorMessage)
        {
            ListViewItem lvi = this.listErrors.Items.Add(type);
            lvi.SubItems.Add(startPosition.ToString());
            lvi.SubItems.Add(stopPosition.ToString());
            lvi.SubItems.Add(errorMessage);
            lvi.Tag = new SourceLocation[] {startPosition, stopPosition};
        }

        void IInterchangeErrorSink.AddInterchangeError(SourceLocation startPosition, SourceLocation stopPosition, string errorMessage)
        {
            this.AddError("Interchange", startPosition, stopPosition, errorMessage);
        }

        void IronSmalltalk.Compiler.SemanticAnalysis.IParseErrorSink.AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
        {
            this.AddError("Parse", startPosition, stopPosition, parseErrorMessage);
        }

        void IronSmalltalk.Compiler.SemanticAnalysis.IParseErrorSink.AddParserError(IronSmalltalk.Compiler.SemanticNodes.IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
        {
            this.AddError("Parse", startPosition, stopPosition, parseErrorMessage);
        }

        void IronSmalltalk.Compiler.LexicalAnalysis.IScanErrorSink.AddScanError(IronSmalltalk.Compiler.LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
        {
            this.AddError("Scan", startPosition, stopPosition, scanErrorMessage);
        }

        void IInstallErrorSink.AddInstallError(string installErrorMessage, ISourceReference sourceReference)
        {
            this.AddError("Install", sourceReference.StartPosition, sourceReference.StopPosition, installErrorMessage);
        }

        private void listErrors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listErrors.FocusedItem == null)
            {
                this.txtSource.SelectionLength = 0;
                return;
            }
            if (!(this.listErrors.FocusedItem.Tag is SourceLocation[]))
            {
                this.txtSource.SelectionLength = 0;
                return;
            }
            SourceLocation[] sel = (SourceLocation[])this.listErrors.FocusedItem.Tag;
            this.txtSource.SelectionStart = sel[0].Position;
            this.txtSource.SelectionLength = sel[1].Position - sel[0].Position + 1;
        }

        private void InstallTester_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.LastInstallerSource != null)
                this.txtSource.Text = Properties.Settings.Default.LastInstallerSource;
            this.Environment = new SmalltalkEnvironment();
        }
    }
}
