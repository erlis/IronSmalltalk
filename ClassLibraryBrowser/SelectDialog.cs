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

namespace IronSmalltalk.Tools.ClassLibraryBrowser
{
    public partial class SelectDialogBase : Form
    {
        public SelectDialogBase()
        {
            InitializeComponent();
        }

        public bool CheckBoxes
        {
            get { return this.listView.CheckBoxes; }
            set { this.listView.CheckBoxes = value; }
        }

        // Columns

        public bool MultiSelect
        {
            get { return this.listView.MultiSelect; }
            set { this.listView.MultiSelect = value; }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.ButtonOk();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.ButtonCancel();
            this.Close();
        }

        private void SelectDialogBase_Load(object sender, EventArgs e)
        {
            this.FillList();
        }

        protected virtual void FillList()
        {
            this.listView.Items.Clear();
            this.listView.Columns.Clear();
        }

        protected virtual void ButtonOk()
        {
        }

        protected virtual void ButtonCancel()
        {
        }
    }

    public class SelectDialog<TItem> : SelectDialogBase
    {
        public IEnumerable<TItem> Items { get; set; }

        public IEnumerable<SelectDialogColumn<TItem>> Columns { get; set; }

        public IEnumerable<TItem> SelectedItems { get; set; }

        protected override void FillList()
        {
            base.FillList();
            if (this.Items == null)
                return;
            if (this.Columns == null)
                return;

            foreach (var def in this.Columns)
                this.listView.Columns.Add(def.Text, def.Width, def.Alignment).Tag = def;

            foreach (var item in this.Items)
            {
                ListViewItem lvi = null;
                foreach (var def in this.Columns)
                {
                    if (lvi == null)
                        lvi = this.listView.Items.Add(def.GetText(item));
                    else
                        lvi.SubItems.Add(def.GetText(item));
                }
                if (lvi == null)
                    lvi = this.listView.Items.Add(String.Empty);
                lvi.Tag = item;
                if ((this.SelectedItems != null) && this.SelectedItems.Contains(item))
                    lvi.Selected = true;
            }
        }

        protected override void ButtonCancel()
        {
            base.ButtonCancel();
            this.SelectedItems = null;
        }

        protected override void ButtonOk()
        {
            base.ButtonOk();
            List<TItem> selected = new List<TItem>();
            if (this.CheckBoxes)
            {
                foreach (ListViewItem lvi in this.listView.CheckedItems)
                    selected.Add((TItem)lvi.Tag);
            }
            else
            {
                foreach (ListViewItem lvi in this.listView.SelectedItems)
                    selected.Add((TItem)lvi.Tag);
            }
            this.SelectedItems = selected;
        }
    }

    public class SelectDialogColumn<TItem>
    {
        public string Text { get; set; }
        public int Width { get; set; }
        public HorizontalAlignment Alignment { get; set; }
        public Func<TItem, string> ColumnFunction { get; set; }

        public SelectDialogColumn(string text, int width, Func<TItem, string> columnFunction)
            : this(text, width, HorizontalAlignment.Left, columnFunction)
        {
        }

        public SelectDialogColumn(string text, int width, HorizontalAlignment alignment, Func<TItem, string> columnFunction)
        {
            this.Text = text;
            this.Width = width;
            this.Alignment = alignment;
            this.ColumnFunction = columnFunction;
        }

        public string GetText(TItem obj)
        {
            if (this.ColumnFunction == null)
            {
                if (obj == null)
                    return String.Empty;
                else
                    return obj.ToString();
            }
            else
            {
                return this.ColumnFunction(obj);
            }
        }
    }
}
