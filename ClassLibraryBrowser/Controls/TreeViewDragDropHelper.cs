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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls
{
    public interface IDragDropClient
    {
        bool IsDropAllowed(TreeNode sourceNode, TreeNode targetNode);
        void CompleteDrop(TreeNode sourceNode, TreeNode targetNode);
    }

    /// <summary>
    /// Helper to display drag image of the dragged tree view item.
    /// </summary>
    /// <remarks>
    /// Loosely based on: http://www.codeproject.com/KB/tree/TreeViewDragDrop.aspx
    /// </remarks>
    public class TreeViewDragDropHelper
    {
        #region API declaraions

        [DllImport("comctl32.dll")]
        public static extern bool InitCommonControls();

        /// <summary>
        /// Begins dragging an image.
        /// </summary>
        /// <param name="himlTrack">Handle to the image list.</param>
        /// <param name="iTrack">Index of the image to drag.</param>
        /// <param name="dxHotspot">x-coordinate of the location of the drag position relative to the upper-left corner of the image.</param>
        /// <param name="dyHotspot">y-coordinate of the location of the drag position relative to the upper-left corner of the image.</param>
        /// <returns>Returns nonzero if successful, or zero otherwise.</returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ImageList_BeginDrag(IntPtr himlTrack, int iTrack, int dxHotspot, int dyHotspot);

        /// <summary>
        /// Moves the image that is being dragged during a drag-and-drop operation. 
        /// This function is typically called in response to a WM_MOUSEMOVE message.
        /// </summary>
        /// <param name="x">X-coordinate at which to display the drag image. 
        /// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <param name="y">Y-coordinate at which to display the drag image. 
        /// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <returns>Returns nonzero if successful, or zero otherwise.</returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ImageList_DragMove(int x, int y);

        /// <summary>
        /// Ends a drag operation.
        /// </summary>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        public static extern void ImageList_EndDrag();

        /// <summary>
        /// Displays the drag image at the specified position within the window.
        /// </summary>
        /// <param name="hwndLock">Handle to the window that owns the drag image.</param>
        /// <param name="x">X-coordinate at which to display the drag image. 
        /// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <param name="y">Y-coordinate at which to display the drag image. 
        /// The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <returns>Returns nonzero if successful, or zero otherwise.</returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

        /// <summary>
        /// Unlocks the specified window and hides the drag image, allowing the window to be updated.
        /// </summary>
        /// <param name="hwndLock">Handle to the window that owns the drag image.</param>
        /// <returns>Returns nonzero if successful, or zero otherwise.</returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ImageList_DragLeave(IntPtr hwndLock);

        /// <summary>
        /// Shows or hides the image being dragged.
        /// </summary>
        /// <param name="fShow">Value specifying whether to show or hide the image being dragged. 
        /// Specify <see langword="true"/> to show the image or <see langword="false"/> to hide the image.</param>
        /// <returns>Returns nonzero if successful, or zero otherwise. </returns>
        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ImageList_DragShowNolock([MarshalAs(UnmanagedType.Bool)] bool fShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern bool ImageList_GetIconSize(IntPtr himl, out int cx, out int cy);


        #endregion

        private class DragDropInformation
        {
            public TreeNode SourceNode { get; private set; }
            public IDragDropClient Client { get; private set; }

            public DragDropInformation(TreeNode sourceNode, IDragDropClient client)
            {
                if (sourceNode == null)
                    throw new ArgumentNullException("sourceNode");
                if (client == null)
                    throw new ArgumentNullException("client");
                this.SourceNode = sourceNode;
                this.Client = client;
            }
        }

        private class ScrollHelper : IDisposable
        {
            private readonly Timer ScrollTimer;
            private readonly TreeView TreeView;
            private const int SCROLL_AREA_WIDTH = 16;

            public ScrollHelper(TreeView treeView)
            {
                if (treeView == null)
                    throw new ArgumentNullException();
                this.TreeView = treeView;
                this.ScrollTimer = new Timer();
                this.ScrollTimer.Interval = 100;
                this.ScrollTimer.Tick += new EventHandler(this.ScrollTimer_Tick);
                this.ScrollTimer.Enabled = true;
            }
            private void ScrollTimer_Tick(object sender, EventArgs e)
            {
                Point p = this.TreeView.PointToClient(Cursor.Position);
                int h = this.TreeView.ClientSize.Height;
                if ((p.Y < 0) || (p.Y > h))
                    return;
                int d = Math.Min(ScrollHelper.SCROLL_AREA_WIDTH, h / 2);

                if (p.Y < d)
                    this.ScrollUp(p);
                else if (p.Y > (h - d))
                    this.ScrollDown(p);
            }

            private void ScrollUp(Point pt)
            {
                TreeNode visibleNode = this.TreeView.GetNodeAt(pt);
                if (visibleNode == null)
                    return;

                TreeNode showNode = visibleNode.PrevVisibleNode;
                if (showNode == null)
                {
                    showNode = visibleNode.PrevNode;
                    if (showNode == null)
                        showNode = visibleNode.Parent;
                }

                if (showNode != null)
                {
                    TreeViewDragDropHelper.ImageList_DragShowNolock(false);
                    showNode.EnsureVisible();
                    this.TreeView.Invalidate();
                    TreeViewDragDropHelper.ImageList_DragShowNolock(true);
                }
            }

            private void ScrollDown(Point pt)
            {
                TreeNode visibleNode = this.TreeView.GetNodeAt(pt);
                if (visibleNode == null)
                    return;

                TreeNode showNode = visibleNode.NextVisibleNode;
                if (showNode == null)
                {
                    showNode = visibleNode.NextNode;
                    if (showNode == null)
                    {
                        showNode = visibleNode.Parent;
                        if (showNode != null)
                            showNode = showNode.NextNode;
                    }
                }

                if (showNode != null)
                {
                    TreeViewDragDropHelper.ImageList_DragShowNolock(false);
                    showNode.EnsureVisible();
                    this.TreeView.Invalidate();
                    TreeViewDragDropHelper.ImageList_DragShowNolock(true);
                }
            }

            public void Dispose()
            {
                this.ScrollTimer.Enabled = false;
            }
        }

        public void DoDragDrop(IDragDropClient client, TreeNode node, DragDropEffects effects)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            if (node == null)
                throw new ArgumentNullException("node");

            using (ImageList imgList = this.CreateImageList(node))
            {
                TreeViewDragDropHelper.ImageList_BeginDrag(imgList.Handle, 0, 0, 0);

                try
                {
                    node.TreeView.DragDrop += new DragEventHandler(this.TreeView_DragDrop);
                    node.TreeView.DragEnter += new DragEventHandler(this.TreeView_DragEnter);
                    node.TreeView.DragLeave += new EventHandler(this.TreeView_DragLeave);
                    node.TreeView.DragOver += new DragEventHandler(this.TreeView_DragOver);

                    try
                    {
                        using(new ScrollHelper(node.TreeView))
                            node.TreeView.DoDragDrop(new DragDropInformation(node, client), effects);
                    }
                    finally
                    {
                        TreeView tree = node.TreeView;
                        if (tree != null)
                        {
                            tree.DragDrop -= new DragEventHandler(this.TreeView_DragDrop);
                            tree.DragEnter -= new DragEventHandler(this.TreeView_DragEnter);
                            tree.DragLeave -= new EventHandler(this.TreeView_DragLeave);
                            tree.DragOver -= new DragEventHandler(this.TreeView_DragOver);
                        }
                    }
                }
                finally
                {

                    TreeViewDragDropHelper.ImageList_EndDrag();
                }
            }
        }

        private ImageList CreateImageList(TreeNode node)
        {
            ImageList imgList = new ImageList();
            imgList.ColorDepth = ColorDepth.Depth32Bit;
            imgList.TransparentColor = Color.Transparent;
            imgList.ImageSize = new Size(
                node.Bounds.Size.Width + node.TreeView.Indent + 6,
                node.Bounds.Size.Height);

            using (Bitmap bmp = new Bitmap(imgList.ImageSize.Width, imgList.ImageSize.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.Clear(Color.White);

                    int idx = node.SelectedImageIndex;
                    if (idx < 0)
                        idx = node.TreeView.ImageList.Images.IndexOfKey(node.SelectedImageKey);
                    if (idx >= 0)
                        node.TreeView.ImageList.Draw(graphics, 0, 0, idx);

                    Font font = node.NodeFont;
                    if (font == null)
                        font = node.TreeView.Font;
                    graphics.DrawString(node.Text, font, Brushes.Black, node.TreeView.Indent, 0);
                }
                imgList.Images.Add(bmp);
                imgList.Handle.ToString(); // Forces creation of the img-list
            }

            return imgList;
        }

        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
            TreeView tree = (TreeView)sender;
            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeViewDragDropHelper.ImageList_DragEnter(((TreeView)sender).Handle, targetPoint.X, targetPoint.Y);
            //this.ScrollTimer.Enabled = true;
        }

        private void TreeView_DragLeave(object sender, EventArgs e)
        {
            //this.ScrollTimer.Enabled = false;
            TreeViewDragDropHelper.ImageList_DragLeave(((TreeView)sender).Handle);
        }

        private void TreeView_DragOver(object sender, DragEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = tree.GetNodeAt(targetPoint);
            DragDropInformation info = (DragDropInformation)e.Data.GetData(typeof(DragDropInformation));
            if (info == null)
                return;
            TreeViewDragDropHelper.ImageList_DragMove(targetPoint.X, targetPoint.Y);
            //TreeViewDragDropHelper.ImageList_DragMove(e.X, e.Y);
            TreeNode sourceNode = info.SourceNode;
            if (info.Client.IsDropAllowed(sourceNode, targetNode))
            {
                TreeViewDragDropHelper.ImageList_DragShowNolock(false);
                tree.SelectedNode = targetNode;
                e.Effect = e.AllowedEffect;
                TreeViewDragDropHelper.ImageList_DragShowNolock(true);
            }
            else
            {
                TreeViewDragDropHelper.ImageList_DragShowNolock(false);
                tree.SelectedNode = sourceNode;
                e.Effect = DragDropEffects.None;
                TreeViewDragDropHelper.ImageList_DragShowNolock(true);
            }
        }

        private void TreeView_DragDrop(object sender, DragEventArgs e)
        {
            //this.ScrollTimer.Enabled = false;
            TreeView tree = (TreeView)sender;
            Point targetPoint = tree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = tree.GetNodeAt(targetPoint);
            DragDropInformation info = (DragDropInformation)e.Data.GetData(typeof(DragDropInformation));
            if (info == null)
                return;
            TreeNode sourceNode = info.SourceNode;

            if (info.Client.IsDropAllowed(sourceNode, targetNode))
                info.Client.CompleteDrop(sourceNode, targetNode);
        }

    }
}
