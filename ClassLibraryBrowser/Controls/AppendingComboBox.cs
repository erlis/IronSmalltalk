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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls
{
    public class AppendingComboBox : ComboBox
    {
        public string Separator { get; set; }

        public AppendingComboBox()
            : base()
        {
            this.Separator = " ";
        }

        public void SetChoices(IEnumerable<string> items)
        {
            string old = this.Text;
            try
            {
                this.Items.Clear();
                this.Items.AddRange(items.OrderBy(n => n).ToArray());
            }
            finally
            {
                this.Text = old;
            }
        }

        public void SetValues(IEnumerable<string> items)
        {
            this.SavedTextValue = String.Empty;
            this.CompiledText = null;
            this.Text = String.Join(this.Separator, items.OrderBy(n => n));
        }


        private string SavedTextValue = String.Empty;
        private string CompiledText = null;
        private bool DropDownOpened = false;

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            this.SavedTextValue = this.Text;
            this.DropDownOpened = true;
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            this.DropDownOpened = false;
            this.CommitItemSelection();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            this.CommitItemSelection();
        }

        protected override void OnSelectionChangeCommitted(EventArgs e)
        {
            base.OnSelectionChangeCommitted(e);
            this.CommitItemSelection();
        }

        private void CommitItemSelection()
        {
            if (this.DropDownOpened)
                return;
            string item = this.Text.Trim();
            if (String.IsNullOrWhiteSpace(item) || item.Contains(this.Separator) || (item == this.SavedTextValue))
            {
                //this.Text = this.SavedTextValue;
                return;
            }
            string[] items = (this.SavedTextValue + this.Separator + item).Split(new string[] { this.Separator }, StringSplitOptions.None);
            this.CompiledText = String.Join(this.Separator,
                items.Where(p => !String.IsNullOrWhiteSpace(p)).Distinct().OrderBy(p => p));
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if ((m.Msg == 0x111) && ((((int)m.WParam & 0xFFFF0000) >> 16) == 1) && (this.CompiledText != null))
            {
                string txt = this.CompiledText;
                this.CompiledText = null;
                this.Text = txt;
            }
        }
    }

    
}
