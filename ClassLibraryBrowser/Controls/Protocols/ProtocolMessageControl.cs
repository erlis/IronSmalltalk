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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Description;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Protocols
{
    public partial class ProtocolMessageControl : UserControl
    {
        public bool Dirty { get; set; }
        private bool Updating;

        #region Value Holders and Accessors

        private ValueHolder<Protocol> _protocol;

        [Browsable(false)]
        public ValueHolder<Protocol> ProtocolHolder
        {
            get
            {
                return this._protocol;
            }
            set
            {
                if ((this._protocol == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._protocol == value)
                    return;
                if (this._protocol != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._protocol = value;
                value.Changed += new ValueChangedEventHandler<Protocol>(this.ProtocolChanged);
            }
        }

        public Protocol Protocol
        {
            get
            {
                if (this.ProtocolHolder == null)
                    return null;
                return this.ProtocolHolder.Value;
            }
        }

        private ValueHolder<Definitions.Description.Message> _message;

        [Browsable(false)]
        public ValueHolder<Definitions.Description.Message> MessageHolder
        {
            get
            {
                return this._message;
            }
            set
            {
                if ((this._message == null) && (value == null))
                    return;
                if (value == null)
                    throw new ArgumentNullException();
                if (this._message == value)
                    return;
                if (this._message != null)
                    throw new InvalidOperationException("Value holder may be set only once.");
                this._message = value;
                value.Changing += new ValueChangingEventHandler<Definitions.Description.Message>(this.MessageChanging);
                value.Changed += new ValueChangedEventHandler<Definitions.Description.Message>(this.MessageChanged);
            }
        }

        public Definitions.Description.Message Message
        {
            get
            {
                if (this.MessageHolder == null)
                    return null;
                return this.MessageHolder.Value;
            }
        }

        #endregion

        public ProtocolMessageControl()
        {
            InitializeComponent();
        }

        #region Editing and Saving

        private void ProtocolChanged(object sender, EventArgs e)
        {
            this.MessageChanged(sender, e);
            this.comboRefinementProtocols.Items.Clear();
            if (this.Protocol != null)
                this.comboRefinementProtocols.Items.AddRange(this.Protocol.SystemDescription.Protocols.OrderBy(p => p.Name).ToArray());
            this.FillProtocolCombos();
        }

        private void MessageChanged(object sender, EventArgs e)
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.FillView();
                this.SetSelectorState();
                this.MarkClean();
            }
            finally
            {
                this.Updating = remember;
            }
        }

        private void FillView()
        {
            this.FillParameterList();
            this.FillReturnValue();
            this.panelRefinementDescription.Visible = false;
            this.listRefinement.Items.Clear();
            if (this.Message == null)
            {
                this.Enabled = false;
                this.txtMethodSelector.Text = null;
                this.txtMethodDocId.Text = null;
                this.synopsisControl.Text = null;
                this.descriptionControl.Text = null;
                this.errorsControl.Text = null;
                this.sourceEditControl.Source = null;
                this.panelRefinement.Visible = false;
                return;
            }

            this.Enabled = true;
            this.txtMethodSelector.Text = this.Message.Selector;
            this.txtMethodDocId.Text = this.Message.DocumentationId;
            this.synopsisControl.Html = this.Message.Synopsis;
            this.descriptionControl.Html = this.Message.DefinitionDescription;
            this.errorsControl.Html = this.Message.Errors;
            this.sourceEditControl.Source = this.Message.Source;
            this.panelRefinement.Visible = true;

            foreach (KeyValuePair<string, HtmlString> pair in this.Message.Refinement)
                this.AddRefinement(pair.Key, pair.Value);
        }

        private void MarkDirty()
        {
            this.Dirty = true;
        }

        private void MarkClean()
        {
            this.Dirty = false;
        }

        private void SetSelectorState()
        {
            bool ok = (this.Message == null) || (this.ValidateSelector() == null);
            this.txtMethodSelector.BackColor = ok ? SystemColors.Window : Color.LightSalmon;
        }

        private string ValidateSelector()
        {
            if (String.IsNullOrWhiteSpace(this.txtMethodSelector.Text))
                return "Message must have a selector";
            if (this.Protocol != null)
            {
                foreach (Definitions.Description.Message m in this.Protocol.Messages)
                {
                    if (m != this.Message)
                    {
                        if (m.Selector == this.txtMethodSelector.Text)
                            return "Message selector must be unuque";
                    }
                }
            }
            return null;
        }

        private string ValidateParameters()
        {
            HashSet<string> paramNames = new HashSet<string>();
            foreach (ListViewItem lvi in this.listParameters.Items)
            {
                ParameterInfo pi = (ParameterInfo)lvi.Tag;
                if (String.IsNullOrWhiteSpace(pi.Name))
                    return "Parameter name is empty";
                paramNames.Add(pi.Name);
                HashSet<string> prots = new HashSet<string>();
                foreach (string p in pi.Protocols)
                    prots.Add(p);
                if (prots.Count != pi.Protocols.Count)
                    return "Parameter " + pi.Name + " protocols duplicate";
            }
            if (paramNames.Count != this.listParameters.Items.Count)
                return "Parameter names duplicate";
            return this.ValidateParameterCount();
        }

        private string ValidateParameterCount()
        {
            string sel = this.txtMethodSelector.Text;
            if (String.IsNullOrWhiteSpace(sel))
                return "Invalid method selector";
            if (@"!%&*+,/<=>?@\~|-".Contains(sel[0]))
            {
                if (this.listParameters.Items.Count != 1)
                    return "Binary methods must have exactly one parameter";
                return null;
            }
            int cnt = sel.Count(c => c == ':');
            if (this.listParameters.Items.Count != cnt)
                return String.Format("Message #{0} expects {1} parameters", sel, cnt);
            return null;
        }

        private string ValidateReturnValue()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (ListViewItem lvi in this.listReturnValueProtocols.Items)
                set.Add((string)lvi.Tag);
            if (set.Count != this.listReturnValueProtocols.Items.Count)
                return "Return value protocols duplicate";
            return null;
        }

        private void txtMethodSelector_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
            this.SetSelectorState();
        }

        private void txtMethodDocId_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void checkBoxMessageAbstract_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }


        private void synopsisControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void errorsControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void MessageChanging(object sender, ValueChangingEventArgs<Definitions.Description.Message> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to #{0}?", this.txtMethodSelector.Text),
                "Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
                this.ProtocolChanged(this, e);
            else
                e.Cancel = true;
        }

        public bool Save()
        {
            if (this.Protocol == null)
                return false;

            string msg = this.ValidateSelector();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            msg = this.ValidateParameters();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            msg = this.ValidateReturnValue();
            if (msg != null)
            {
                MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            this.Message.Selector = this.txtMethodSelector.Text;
            this.Message.DocumentationId = this.txtMethodDocId.Text;
            this.Message.Synopsis = this.synopsisControl.Html;
            this.Message.DefinitionDescription = this.descriptionControl.Html;
            this.Message.Errors = this.errorsControl.Html;
            this.Message.Source = this.sourceEditControl.Source;
            
            // Refinement
            this.Message.Refinement.Clear();
            foreach (ListViewItem lvi in this.listRefinement.Items)
                this.Message.Refinement[((RefinementInfo)lvi.Tag).Protocol] = ((RefinementInfo)lvi.Tag).Description;
            // Parameters
            this.Message.Parameters.Clear();
            foreach (ListViewItem lvi in this.listParameters.Items)
                this.Message.Parameters.Add(((ParameterInfo)lvi.Tag).ToParameter(this.Message));
            // Return Value
            if (this.Message.ReturnValue == null)
                this.Message.ReturnValue = new ReturnValue(this.Message);
            if (this.comboReturnValueAliasing.SelectedIndex == 1)
                this.Message.ReturnValue.Aliasing = ReturnValue.AliasingEnum.State;
            else if (this.comboReturnValueAliasing.SelectedIndex == 2)
                this.Message.ReturnValue.Aliasing = ReturnValue.AliasingEnum.New;
            else
                this.Message.ReturnValue.Aliasing = ReturnValue.AliasingEnum.Unspecified;
            this.Message.ReturnValue.Description = this.returnValueDescriptionControl.Html;
            this.Message.ReturnValue.Protocols.Clear();
            foreach (ListViewItem lvi in this.listReturnValueProtocols.Items)
                this.Message.ReturnValue.Protocols.Add((string)lvi.Tag);

            bool remember = this.Updating;
            try
            {
                this.Updating = true;
                if (!this.Message.Protocol.Messages.Contains(this.Message))
                    this.Message.Protocol.Messages.Add(this.Message);
                this.MarkClean();
                this.MessageHolder.TriggerChanged(this.Message, this.Message);
                this.ProtocolHolder.TriggerChanged(this.Protocol, this.Protocol);
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

        #endregion

        #region Refinement

        private class RefinementInfo
        {
            public string Protocol;
            public HtmlString Description;

            public RefinementInfo(string protocol, HtmlString description)
            {
                this.Protocol = protocol;
                this.Description = description;
            }
        }

        private ListViewItem AddRefinement(string protocol, HtmlString description)
        {
            ListViewItem lvi = this.listRefinement.Items.Add(protocol);
            lvi.Name = protocol;
            lvi.SubItems.Add(description.Text);
            lvi.Tag = new RefinementInfo(protocol, description);
            return lvi;
        }

        private void buttonRefinementOK_Click(object sender, EventArgs e)
        {
            if (this.listRefinement.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = this.listRefinement.SelectedItems[0];
            ((RefinementInfo)lvi.Tag).Description = this.refinementDescription.Html;
            lvi.SubItems[1].Text = this.refinementDescription.Html.Text;

            this.panelRefinementDescription.Visible = false;
            this.panelRefinement.Visible = true;
            this.MarkDirty();
        }

        private void buttonRefinementCancel_Click(object sender, EventArgs e)
        {
            this.panelRefinementDescription.Visible = false;
            this.panelRefinement.Visible = true;
        }

        private void buttonAddRefinementProtocol_Click(object sender, EventArgs e)
        {
            Protocol p = this.comboRefinementProtocols.SelectedItem as Protocol;
            if (p == null)
                return;
            foreach (ListViewItem lvi in this.listRefinement.Items)
            {
                if (p.Name == lvi.Name)
                {
                    lvi.Selected = true;
                    return;
                }
            }
            this.AddRefinement(p.Name, new HtmlString()).Selected = true;
            this.MarkDirty();
        }

        private void buttonRemoveRefinement_Click(object sender, EventArgs e)
        {
            if (this.listRefinement.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = this.listRefinement.SelectedItems[0];
            this.listRefinement.Items.Remove(lvi);
            this.MarkDirty();
        }

        private void comboRefinementProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonAddRefinement.Enabled = (this.comboRefinementProtocols.SelectedItem is Protocol);
        }

        private void comboRefinementProtocols_Format(object sender, ListControlConvertEventArgs e)
        {
            e.Value = ((Protocol)e.ListItem).Name;
        }

        private void listRefinement_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRemoveRefinement.Enabled = (this.listRefinement.SelectedItems.Count == 1);
        }

        private void listRefinement_DoubleClick(object sender, EventArgs e)
        {
            if (this.listRefinement.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = this.listRefinement.SelectedItems[0];
            this.refinementDescription.Html = ((RefinementInfo)lvi.Tag).Description;
            this.panelRefinement.Visible = false;
            this.panelRefinementDescription.Visible = true;
        }

        #endregion

        #region Parameters

        private void FillParameterList()
        {
            this.listParameters.Items.Clear();
            if (this.Message == null)
                return;
            foreach (Parameter param in this.Message.Parameters)
            {
                ListViewItem lvi = this.listParameters.Items.Add(param.Name);
                lvi.SubItems.Add(String.Format("{0}", param.Aliasing));
                lvi.Name = param.Name;
                lvi.Tag = new ParameterInfo(param);
            }
        }

        private void FillProtocolCombos()
        {
            this.comboParameterProtocols.Items.Clear();
            this.comboParameterProtocols.Items.Add("ANY");
            this.comboReturnValueProtocols.Items.Clear();
            this.comboReturnValueProtocols.Items.Add("ANY");

            if (this.Protocol == null)
                return;
            this.comboParameterProtocols.Items.AddRange(
                this.Protocol.SystemDescription.Protocols.Select(p => p.Name).OrderBy(n => n).ToArray());
            this.comboReturnValueProtocols.Items.AddRange(
                this.Protocol.SystemDescription.Protocols.Select(p => p.Name).OrderBy(n => n).ToArray());
        }

        private void listParameters_DoubleClick(object sender, EventArgs e)
        {
            if (this.listParameters.SelectedItems.Count != 1)
                return;
            ParameterInfo pi = (ParameterInfo)this.listParameters.SelectedItems[0].Tag;
            this.unspecifiedToolStripMenuItem.Checked = false;
            this.capturedToolStripMenuItem.Checked = false;
            this.uncapturedToolStripMenuItem.Checked = false;
            if (pi.Aliasing == Parameter.AliasingEnum.Unspecified)
                this.unspecifiedToolStripMenuItem.Checked = true;
            else if (pi.Aliasing == Parameter.AliasingEnum.Captured)
                this.capturedToolStripMenuItem.Checked = true;
            else if (pi.Aliasing == Parameter.AliasingEnum.Uncaptured)
                this.uncapturedToolStripMenuItem.Checked = true;

            Rectangle rect = this.listParameters.SelectedItems[0].SubItems[1].Bounds;
            this.contextMenuParameterAliasing.Show(this.listParameters, rect.Left, rect.Bottom);
        }

        private void listParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillParameterProtocolsList();
        }

        private void comboParameterProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonAddParameterProtocol.Enabled = (this.comboParameterProtocols.SelectedItem != null);
        }

        private void buttonAddParameterProtocol_Click(object sender, EventArgs e)
        {
            if (this.comboParameterProtocols.SelectedItem == null)
                return;
            if (this.listParameters.SelectedItems.Count != 1)
                return;

            string prot = (string)this.comboParameterProtocols.SelectedItem;
            ParameterInfo pi = (ParameterInfo)this.listParameters.SelectedItems[0].Tag;
            if (!pi.Protocols.Contains(prot))
                pi.Protocols.Add(prot);
            this.FillParameterProtocolsList();
            this.listParameterProtocol.Items[prot].Selected = true;
            this.MarkDirty();
        }

        private void FillParameterProtocolsList()
        {
            this.listParameterProtocol.Items.Clear();
            if (this.listParameters.SelectedItems.Count != 1)
                return;
            ParameterInfo pi = (ParameterInfo)this.listParameters.SelectedItems[0].Tag;

            foreach (string prot in pi.Protocols)
            {
                ListViewItem lvi = this.listParameterProtocol.Items.Add(prot);
                lvi.Tag = prot;
                lvi.Name = prot;
            }
        }

        private void listParameterProtocol_DoubleClick(object sender, EventArgs e)
        {
            this.DeleteParameterProtocol();
        }

        private void listParameterProtocol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.None)
                return;
            if (e.KeyCode != Keys.Delete)
                return;
            this.DeleteParameterProtocol();
        }

        private void DeleteParameterProtocol()
        {
            if (this.listParameterProtocol.SelectedItems.Count != 1)
                return;
            if (this.listParameters.SelectedItems.Count != 1)
                return;
            string prot = (string)this.listParameterProtocol.SelectedItems[0].Tag;
            var dlgres = MessageBox.Show(String.Format("Remove Protocol {0}?", prot), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgres != DialogResult.Yes)
                return;
            ParameterInfo pi = (ParameterInfo)this.listParameters.SelectedItems[0].Tag;
            pi.Protocols.Remove(prot);
            this.FillParameterProtocolsList();
            this.MarkDirty();
        }

        private void listParameters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers != Keys.None)
                return;
            if (e.KeyCode == Keys.Delete)
            {
                if (this.listParameters.SelectedItems.Count != 1)
                    return;
                var dlgres = MessageBox.Show(String.Format(
                    "Delete parameter {0}?", this.listParameters.SelectedItems[0].Text), "Confirm",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgres != DialogResult.Yes)
                    return;
                this.listParameters.Items.Remove(this.listParameters.SelectedItems[0]);
                this.FillParameterProtocolsList();
                this.MarkDirty();
            }
            if (e.KeyCode == Keys.Insert)
            {
                int i = this.listParameters.Items.Count + 1;
                while (true)
                {

                    if (this.listParameters.Items["param" + i.ToString()] == null)
                        break;
                    i++;
                }
                ParameterInfo pi = new ParameterInfo("param" + i.ToString());
                ListViewItem lvi = this.listParameters.Items.Add(pi.Name);
                lvi.SubItems.Add(String.Format("{0}", pi.Aliasing));
                lvi.Name = pi.Name;
                lvi.Tag = pi;
                this.MarkDirty();
            }
        }

        private void unspecifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetParameterAliasing(Parameter.AliasingEnum.Unspecified);
        }

        private void capturedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetParameterAliasing(Parameter.AliasingEnum.Captured);
        }

        private void uncapturedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetParameterAliasing(Parameter.AliasingEnum.Uncaptured);
        }

        private void SetParameterAliasing(Parameter.AliasingEnum aliasing)
        {
            if (this.listParameters.SelectedItems.Count != 1)
                return;
            ListViewItem lvi = this.listParameters.SelectedItems[0];
            ParameterInfo pi = (ParameterInfo)lvi.Tag;
            pi.Aliasing = aliasing;
            lvi.SubItems[1].Text = String.Format("{0}", aliasing);
            this.MarkDirty();
        }

        private class ParameterInfo
        {
            public string Name;
            public readonly List<string> Protocols = new List<string>();
            public Parameter.AliasingEnum Aliasing;

            public ParameterInfo(string name)
            {
                this.Name = name;
                this.Aliasing = Parameter.AliasingEnum.Unspecified;
                this.Protocols = new List<string>();
            }

            public ParameterInfo(Parameter param)
            {
                this.Name = param.Name;
                this.Aliasing = param.Aliasing;
                this.Protocols = new List<string>(param.Protocols);
            }

            public Parameter ToParameter(Definitions.Description.Message message)
            {
                Parameter p = new Parameter(message);
                p.Aliasing = this.Aliasing;
                p.Name = this.Name;
                foreach(string prot in this.Protocols)
                    p.Protocols.Add(prot);
                return p;
            }
        }

        private void listParameters_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (this.Message.Parameters.Any(p => p.Name == e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            ListViewItem lvi = this.listParameters.Items[e.Item];
            lvi.Name = e.Label;
            ParameterInfo pi = lvi.Tag as ParameterInfo;
            if (pi == null)
                return;
            pi.Name = e.Label;
        }

        #endregion

        #region Return Value

        private List<string> ReturnValueProtocols;
        private HtmlString ReturnValueDescription;
        private ReturnValue.AliasingEnum ReturnValueAliasing;

        private void buttonAddReturnValueProtocol_Click(object sender, EventArgs e)
        {
            if (this.comboReturnValueProtocols.SelectedItem == null)
                return;

            string prot = (string)this.comboReturnValueProtocols.SelectedItem;
            if (!this.ReturnValueProtocols.Contains(prot))
                this.ReturnValueProtocols.Add(prot);
            this.FillReturnValueProtocolList();
            this.listReturnValueProtocols.Items[prot].Selected = true;
            this.MarkDirty();
        }

        private void returnValueDescriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void comboReturnValueAliasing_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void comboReturnValueProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonAddReturnValueProtocol.Enabled = (this.comboReturnValueProtocols.SelectedItem != null);
        }

        private void listReturnValueProtocols_DoubleClick(object sender, EventArgs e)
        {
            this.RemoveReturnValueProtocol();
        }

        private void listReturnValueProtocols_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.None) && (e.KeyCode == Keys.Delete))
                this.RemoveReturnValueProtocol();
        }

        private void RemoveReturnValueProtocol()
        {
            if (this.listReturnValueProtocols.SelectedItems.Count != 1)
                return;
            string prot = (string)this.listReturnValueProtocols.SelectedItems[0].Tag;
            var dlgres = MessageBox.Show(String.Format("Remove Protocol {0}?", prot), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgres != DialogResult.Yes)
                return;
            this.ReturnValueProtocols.Remove(prot);
            this.FillReturnValueProtocolList();
            this.MarkDirty();
        }



        private void FillReturnValueProtocolList()
        {
            this.listReturnValueProtocols.Items.Clear();

            foreach (string prot in this.ReturnValueProtocols)
            {
                ListViewItem lvi = this.listReturnValueProtocols.Items.Add(prot);
                lvi.Tag = prot;
                lvi.Name = prot;
            }
        }

        private void FillReturnValue()
        {
            this.ReturnValueProtocols = new List<string>();
            this.ReturnValueAliasing = ReturnValue.AliasingEnum.Unspecified;
            this.ReturnValueDescription = new HtmlString();

            if ((this.Message != null) && (this.Message.ReturnValue != null))
            {
                this.ReturnValueAliasing = this.Message.ReturnValue.Aliasing;
                this.ReturnValueDescription = this.Message.ReturnValue.Description;
                this.ReturnValueProtocols.AddRange(this.Message.ReturnValue.Protocols);
            }

            this.returnValueDescriptionControl.Html = this.ReturnValueDescription;
            if (this.ReturnValueAliasing == ReturnValue.AliasingEnum.State)
                this.comboReturnValueAliasing.SelectedIndex = 1;
            else if (this.ReturnValueAliasing == ReturnValue.AliasingEnum.New)
                this.comboReturnValueAliasing.SelectedIndex = 2;
            else
                this.comboReturnValueAliasing.SelectedIndex = 0;
            this.FillReturnValueProtocolList();
        }

        #endregion

    }
}
