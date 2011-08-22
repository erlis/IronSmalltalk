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
using IronSmalltalk.Compiler;
using System.IO;
using System.Reflection;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.LexicalTokens;

namespace TestPlayground
{
    public partial class LexTester : Form
    {
        public LexTester()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("{0}, {1}, {2}",
                this.txtSource.SelectionStart,
                this.txtSource.SelectionLength,
                this.txtSource.SelectedText));
        }

        private void btnTokanize_Click(object sender, EventArgs e)
        {
            this.listTokens.Items.Clear();

            string txt = this.txtSource.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.txtSource.Text;
            StringReader reader = new StringReader(txt);
            Scanner lexer = new Scanner(reader);

            Preference preference = Preference.Default;
            if (this.comboPreference.SelectedIndex == 1)
                preference = Preference.NegativeSign;
            if (this.comboPreference.SelectedIndex == 2)
                preference = Preference.VerticalBar;
            if (this.comboPreference.SelectedIndex == 3)
                preference = Preference.NegativeSign | Preference.VerticalBar;
            if (this.comboPreference.SelectedIndex == 4)
                preference = Preference.UnquotedSelector;

            Token token;
            while ((token = lexer.GetToken(preference)) != null)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = token.GetType().Name;
                lvi.SubItems.Add(token.StartPosition.ToString());
                lvi.SubItems.Add(token.StopPosition.ToString());
                lvi.SubItems.Add(this.GetTokenValue(token));
                lvi.SubItems.Add(token.SourceString);
                lvi.SubItems.Add(token.IsValid ? "Y" : "N");
                lvi.SubItems.Add(token.ScanError);
                lvi.Tag = token;

                this.listTokens.Items.Add(lvi);
            }
        }

        private void listTokens_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = this.listTokens.FocusedItem;
            if (lvi == null)
                return;
            Token token = lvi.Tag as Token;
            if (token == null)
                return;
            this.txtSource.SelectionStart = token.StartPosition.Position;
            this.txtSource.SelectionLength = token.StopPosition.Position - token.StartPosition.Position + 1;
        }

        private string GetTokenValue(Token token)
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

        private void LexTester_Load(object sender, EventArgs e)
        {
            this.comboPreference.Items.Clear();
            this.comboPreference.Items.Add("Preference: Default");
            this.comboPreference.Items.Add("Preference: NegativeSign");
            this.comboPreference.Items.Add("Preference: VerticalBar");
            this.comboPreference.Items.Add("Preference: NegativeSign | VerticalBar");
            this.comboPreference.Items.Add("Preference: UnquotedSelector");
            this.comboPreference.SelectedIndex = 0;
        }
    }
}
