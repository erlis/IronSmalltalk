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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls
{
    public partial class DescriptionControl : UserControl
    {
        private HtmlString _text;

        public event EventHandler Changed;

        public DescriptionControl()
        {
            InitializeComponent();
            this.LoadWebControl(this.webControl);
        }

        private void LoadWebControl(WebBrowser wb)
        {
            wb.DocumentText = "<html><head><title>na</title></head><body></body></html>";
            wb.Document.ExecCommand("EditMode", false, "On");
            while (wb.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            dynamic body = wb.Document.Body.DomElement;
            dynamic style = body.style;
            style.fontFamily = "Microsoft Sans Serif";
            style.fontSize = "9pt";
            while (wb.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
        }


        public override string Text
        {
            get
            {
                return this.Html.Text;
            }
            set
            {
                this.Html = new HtmlString(value);
            }
        }

        public HtmlString Html
        {
            get
            {
                string html = this.webControl.Document.Body.InnerHtml;
                if (html == null)
                    return new HtmlString();
                return new HtmlString(html);
            }
            set
            {
                this.webControl.Document.Body.InnerHtml = value.Html;
                this._text = value;
            }
        }

        public bool ShowLabel
        {
            get
            {
                return this.labelDescription.Visible;
            }
            set
            {
                this.labelDescription.Visible = value;
                int spacing = (value ? 19 : 0);
                int height = (value ? 131 : 150);
                this.panelWebControl.Location = new Point(0, spacing);
                this.panelWebControl.Size = new Size(this.Width, this.Height - spacing);
            }
        }

        private void webControl_Validating(object sender, CancelEventArgs e)
        {
            if (this.Changed == null)
                return;
            string html1 = this._text.Html;
            string html2 = this.webControl.Document.Body.InnerHtml;
            if (html1 == html2)
                return;
            this.Changed(this, new EventArgs());
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.Enabled)
                this.webControl.Visible = true;
            else
                this.webControl.Visible = false;
            //        this.View.panelGlobalDescription.BackColor = SystemColors.Control;
            //        this.View.panelGlobalDescription.Enabled = false;
            //        this.View.wbGlobalDescription.Visible = false;
            //        this.View.panelGlobalDescription.BackColor = Color.Transparent;
        }
    }
}
