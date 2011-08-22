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
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Compiler.SemanticNodes;
using System.Reflection;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.VseCompatibility;
using IronSmalltalk.Common;

namespace TestPlayground
{
    public partial class ParseTester : Form, IParseErrorSink 
    {
        Parser parser;
        public Dictionary<object, List<string>> Errors;

        public ParseTester()
        {
            InitializeComponent();
        }

        private void btnParseInitializer_Click(object sender, EventArgs e)
        {
            this.treeParseNodes.Nodes.Clear();
            this.listProperties.Items.Clear();

            string txt = this.txtSource.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.txtSource.Text;
            StringReader reader = new StringReader(txt);

            parser = new VseCompatibleParser();
            this.Errors = new Dictionary<object, List<string>>();
            parser.ErrorSink = this;
            InitializerNode node = parser.ParseInitializer(reader);

            this.VisitNode(node, null, null);
        }

        private void btnParseMethod_Click(object sender, EventArgs e)
        {
            this.treeParseNodes.Nodes.Clear();
            this.listProperties.Items.Clear();

            string txt = this.txtSource.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.txtSource.Text;
            StringReader reader = new StringReader(txt);

            parser = new VseCompatibleParser();
            this.Errors = new Dictionary<object, List<string>>();
            parser.ErrorSink = this;
            MethodNode node = parser.ParseMethod(reader);

            this.VisitNode(node, null, null);
        }

        private void VisitNode(IParseNode node, IParseNode parentNode, TreeNode parentTreeItem)
        {
            if (node == null)
                throw new ArgumentNullException();

            TreeNode treeNode = new TreeNode(node.ToString());
            treeNode.Tag = node;
            treeNode.ToolTipText = treeNode.Text;

            if (parentTreeItem == null)
                this.treeParseNodes.Nodes.Add(treeNode);
            else
                parentTreeItem.Nodes.Add(treeNode);

            if (this.Errors.ContainsKey(node))
            {
                treeNode.ForeColor = Color.Red;
                TreeNode n = treeNode;
                while (n != null)
                {
                    n.Expand();
                    n = n.Parent;
                }
            }

            foreach (IParseNode child in node.GetChildNodes())
                this.VisitNode(child, node, treeNode);
        }

        private void treeParseNodes_Click(object sender, EventArgs e)
        {
        }

        private string GetTokenValue(IToken token)
        {
            Type t = token.GetType();
            PropertyInfo[] infos = t.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (PropertyInfo info in infos)
            {
                if (!(new string[] { "StartPosition", "StopPosition", "IsValid", "ScanError", "SourceString" }).Contains(info.Name))
                {
                    object value = info.GetValue(token, null);
                    map[info.Name] = String.Format("{0}", value);
                }
            }

            if ((map.Count == 1) && map.ContainsKey("Value"))
                return map["Value"];

            StringBuilder sb = new StringBuilder();
            foreach (var pair in map)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.AppendFormat("{0}: {1}", pair.Key, pair.Value);
            }
            return sb.ToString();
        }

        private void treeParseNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.listProperties.Items.Clear();

            if (this.treeParseNodes.SelectedNode == null)
                return;
            IParseNode node = this.treeParseNodes.SelectedNode.Tag as IParseNode;
            if (node == null)
                return;

            this.SelectNodeSource(node);

            Type t = node.GetType();
            PropertyInfo[] infos = t.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infos)
            {
                if (!(new string[] { "Tokens", "Comments" }).Contains(info.Name))
                {
                    object value = info.GetValue(node, null);
                    if (value == null)
                        value = "<null>";
                    this.listProperties.Items.Add(info.Name).SubItems.Add(String.Format("{0}", value));
                }
            }

            foreach (IToken token in node.GetTokens())
            {
                this.listProperties.Items.Add(String.Format("Token: {0}", token.GetType().Name))
                    .SubItems.Add(String.Format("{0}{1} - {2}: {3}",
                    (token.IsValid ? "" : "ERROR: "), token.StartPosition, token.StopPosition,
                    this.GetTokenValue(token)));
            }

            if (this.Errors.ContainsKey(node))
            {
                foreach (string err in this.Errors[node])
                {
                    ListViewItem lvi = this.listProperties.Items.Add("Error:");
                    lvi.SubItems.Add(err).ForeColor = Color.Red;
                    lvi.ForeColor = Color.Red;
                }
            }
        }

        private void SelectNodeSource(IParseNode node)
        {
            if (node == null)
                this.txtSource.SelectionLength = 0;

            int start = -1;
            int end = -1;
            this.FindSourcePoints(node, ref start, ref end);
            if ((start == -1) || (end == -1) || (start > end))
            {
                this.txtSource.SelectionLength = 0;
            }
            else
            {
                this.txtSource.SelectionStart = start;
                this.txtSource.SelectionLength = end - start + 1;
            }
        }

        private void FindSourcePoints(IParseNode node, ref int start, ref int end)
        {
            foreach (IToken token in node.GetTokens())
            {
                if (start == -1)
                    start = token.StartPosition.Position;
                else
                    start = Math.Min(start, token.StartPosition.Position);
                if (end == -1)
                    end = token.StopPosition.Position;
                else
                    end = Math.Max(end, token.StopPosition.Position);
            }

            foreach (IParseNode child in node.GetChildNodes())
            {
                this.FindSourcePoints(child, ref start, ref end);
            }
        }

        void IParseErrorSink.AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
        {
            if (!this.Errors.ContainsKey(this))
                this.Errors.Add(this, new List<string>());
            this.Errors[this].Add(parseErrorMessage);
        }

        void IParseErrorSink.AddParserError(IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
        {
            if (!this.Errors.ContainsKey(node))
                this.Errors.Add(node, new List<string>());
            this.Errors[node].Add(parseErrorMessage);
        }

        void IronSmalltalk.Compiler.LexicalAnalysis.IScanErrorSink.AddScanError(IronSmalltalk.Compiler.LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
        {
            if (!this.Errors.ContainsKey(token))
                this.Errors.Add(token, new List<string>());
            this.Errors[token].Add(scanErrorMessage);
        }
    }
}
