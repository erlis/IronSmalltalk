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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls
{
    public delegate void ListItemChangingEventHandler(object sender, ListItemChangingEventArgs e);

    public class ListItemChangingEventArgs : EventArgs
    {
        public ListViewItem Item { get; set; }


        public ListItemChangingEventArgs(ListViewItem item)
        {
            this.Item = item;
        }
    }

    //public class ListItemChangingEventArgs : EventArgs
    //{
    //    public ListViewItem Item { get; private set; }
    //    public int ItemIndex { get; private set; }
    //    public ListViewItemState OldState { get; private set; }
    //    public ListViewItemState NewState { get; private set; }
    //    public bool Cancel { get; set; }

    //    public ListItemChangingEventArgs()
    //    {
    //        this.Cancel = false;
    //    }

    //    public ListItemChangingEventArgs(ListViewItem item, int itemIndex, int oldState, int newState)
    //        : this()
    //    {
    //        this.Item = item;
    //        this.ItemIndex = itemIndex;
    //        this.OldState = (ListViewItemState) oldState;
    //        this.NewState = (ListViewItemState) newState;
    //    }

    //    public bool Equals(ListItemChangingEventArgs other)
    //    {
    //        if (other == null)
    //            return false;
    //        if (this.GetType() != other.GetType())
    //            return false;
    //        return (this.ItemIndex == other.ItemIndex)
    //            && (this.NewState == other.NewState)
    //            && (this.OldState == other.OldState);
    //    }

    //    public bool NewStateIsSelected
    //    {
    //        get { return (this.NewState & ListViewItemState.Selected) != 0; }
    //    }

    //    public bool OldStateIsSelected
    //    {
    //        get { return (this.OldState & ListViewItemState.Selected) != 0; }
    //    }

    //    public bool NewStateIsFocused
    //    {
    //        get { return (this.NewState & ListViewItemState.Focused) != 0; }
    //    }

    //    public bool OldStateIsFocused
    //    {
    //        get { return (this.OldState & ListViewItemState.Focused) != 0; }
    //    }

    //    public bool SelectionLost
    //    {
    //        get { return this.OldStateIsSelected && !this.NewStateIsSelected; }
    //    }

    //    public bool SelectionGained
    //    {
    //        get { return !this.OldStateIsSelected && this.NewStateIsSelected; }
    //    }

    //    public bool FocusLost
    //    {
    //        get { return this.OldStateIsFocused && !this.NewStateIsFocused; }
    //    }

    //    public bool FocusGained
    //    {
    //        get { return !this.OldStateIsFocused && this.NewStateIsFocused; }
    //    }
    //}

    //[Flags]
    //public enum ListViewItemState
    //{
    //    Focused = 1,
    //    Selected = 2,
    //    Cut = 4,
    //    DropHilited = 8,
    //    Activating = 0x20,
    //}

 
    public class VetoingListView : ListView 
    {
        public event ListItemChangingEventHandler ItemChanging;
        private int ListItemChangingVeto = 0;
        private Timer ListItemChangingTimer;
        private ListViewItem ListItemChangingRevortTo;

        public VetoingListView()
        {
            this.ListItemChangingTimer = new Timer();
            this.ListItemChangingTimer.Interval = 100;
            this.ListItemChangingTimer.Tick += new EventHandler(this.ListItemChangingTimer_Tick);
        }

        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            if (this.ItemChanging == null)
                return;
            if (this.ListItemChangingVeto > 0)
                return;
            if (e.Item == null)
                return;
            if (!e.IsSelected)
                return;

            ListViewItem lvi = e.Item;
            ListItemChangingEventArgs args = new ListItemChangingEventArgs(lvi);
            
            this.ItemChanging(this, args);
            if (lvi != args.Item)
            {
                this.ListItemChangingVeto = 2;
                this.ListItemChangingRevortTo = args.Item;
                this.ListItemChangingTimer.Start();
            }
            
            base.OnItemSelectionChanged(e);
        }

        private void ListItemChangingTimer_Tick(object sender, EventArgs e)
        {
            ListViewItem lvi = this.ListItemChangingRevortTo;
            var sel = this.SelectedItems;
            if (((lvi == null) && (sel.Count != 0)) || ((lvi != null) && ((sel.Count != 1) || (sel[0] != lvi))))
            {
                this.SelectedItems.Clear();
                if (lvi != null)
                {
                    lvi.Selected = true;
                    lvi.Focused = true;
                }
            }
            Application.DoEvents();
            Application.DoEvents();

            this.ListItemChangingVeto--;
            if (this.ListItemChangingVeto < 1)
                this.ListItemChangingTimer.Stop();
        }

        //protected override void  WndProc(ref Message m)
        //{
        //    if (this.ItemChanging != null)
        //    {
        //        if ((m.Msg == 0x4E) || (m.Msg == 0x204E)) // WM_NOTIFY
        //        {
        //            NMHDR nmh = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));
        //            if ((nmh.code == -100) && (nmh.hwndFrom == this.Handle)) // LVN_ITEMCHANGING
        //            {
        //                NMLISTVIEW nmlvi = (NMLISTVIEW)Marshal.PtrToStructure(m.LParam, typeof(NMLISTVIEW));
        //                if ((nmlvi.uChanged & 8) == 8)
        //                {
        //                    ListViewItem lvi = null;
        //                    if ((nmlvi.iItem >= 0) && (nmlvi.iItem < this.Items.Count))
        //                        lvi = this.Items[nmlvi.iItem];
        //                    ListItemChangingEventArgs e = new ListItemChangingEventArgs(
        //                        lvi, nmlvi.iItem, nmlvi.uOldState, nmlvi.uNewState);
        //                    this.ItemChanging(this, e);

        //                    if (e.Cancel)
        //                    {
        //                        m.Result = (IntPtr)1;
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //private struct NMHDR
        //{
        //    public IntPtr hwndFrom;
        //    public IntPtr idFrom;
        //    public int code;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //private struct NMLISTVIEW
        //{
        //    public NMHDR hdr;
        //    public int iItem;
        //    public int iSubItem;
        //    public int uNewState;
        //    public int uOldState;
        //    public int uChanged;
        //    public IntPtr lParam;
        //}
    }
}
