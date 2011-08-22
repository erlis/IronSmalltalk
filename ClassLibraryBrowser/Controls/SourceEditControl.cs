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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls
{
    public partial class SourceEditControl : UserControl
    {
        public event EventHandler Changed;

        public SourceEditControl()
        {
            InitializeComponent();
        }

        public string Source
        {
            get { return this.richTextBox.Text; }
            set { this.richTextBox.Text = value; }
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.Changed != null)
                this.Changed(sender, e);
        }
    }
}
