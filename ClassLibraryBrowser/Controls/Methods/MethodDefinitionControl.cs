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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;
using IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes;
using IronSmalltalk.Common;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Methods
{
    public partial class MethodDefinitionControl : UserControl
    {
        #region Value Holders

        private ValueHolder<Class> _class;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<Class> ClassHolder
        {
            get
            {
                return this._class;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._class != null)
                {
                    this._class.Changed -= new ValueChangedEventHandler<Class>(this.ClassChanged);
                    this._class.Changing -= new ValueChangingEventHandler<Definitions.Implementation.Class>(this.ClassChanging);
                }
                this._class = value;
                this._class.Changing += new ValueChangingEventHandler<Definitions.Implementation.Class>(this.ClassChanging);
                this._class.Changed += new ValueChangedEventHandler<Class>(this.ClassChanged);
            }
        }

        private ValueHolder<MethodType> _methodType;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<MethodType> MethodTypeHolder
        {
            get
            {
                return this._methodType;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._methodType != null)
                {
                    this._methodType.Changed -= new ValueChangedEventHandler<MethodType>(this.MethodTypeChanged);
                    this._methodType.Changing -= new ValueChangingEventHandler<MethodType>(this.MethodTypeChanging);
                }
                this._methodType = value;
                this._methodType.Changed += new ValueChangedEventHandler<MethodType>(this.MethodTypeChanged);
                this._methodType.Changing += new ValueChangingEventHandler<MethodType>(this.MethodTypeChanging);
            }
        }

        private ValueHolder<string> _protocol;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<string> ProtocolNameHolder
        {
            get
            {
                return this._protocol;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._protocol != null)
                {
                    this._protocol.Changed -= new ValueChangedEventHandler<string>(this.ProtocolChanged);
                    this._protocol.Changing -= new ValueChangingEventHandler<string>(this.ProtocolChanging);
                }
                this._protocol = value;
                this._protocol.Changed += new ValueChangedEventHandler<string>(this.ProtocolChanged);
                this._protocol.Changing += new ValueChangingEventHandler<string>(this.ProtocolChanging);
            }
        }

        private ValueHolder<string> _method;

        [Browsable(false)]
        [System.ComponentModel.ReadOnly(true)]
        public ValueHolder<string> MethodNameHolder
        {
            get
            {
                return this._method;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (this._method != null)
                {
                    this._method.Changed -= new ValueChangedEventHandler<string>(this.MethodNameChanged);
                    this._method.Changing -= new ValueChangingEventHandler<string>(this.MethodNameChanging);
                }
                this._method = value;
                this._method.Changed += new ValueChangedEventHandler<string>(this.MethodNameChanged);
                this._method.Changing += new ValueChangingEventHandler<string>(this.MethodNameChanging);
            }
        }


        #region Value Accessors

        public Class Class
        {
            get
            {
                if (this.ClassHolder == null)
                    return null;
                return this.ClassHolder.Value;
            }
        }

        public string ProtocolName
        {
            get
            {
                if (this.ProtocolNameHolder == null)
                    return null;
                return this.ProtocolNameHolder.Value;
            }
        }

        public MethodType MethodType
        {
            get
            {
                if (this.MethodTypeHolder == null)
                    return MethodType.Instance;
                else
                    return this.MethodTypeHolder.Value;
            }
        }

        public string MethodName
        {
            get
            {
                if (this.MethodNameHolder == null)
                    return null;
                return this.MethodNameHolder.Value;
            }
        }

        #endregion

        #endregion

        public MethodDefinitionControl()
        {
            InitializeComponent();
        }

        #region Fill

        private void ClassChanged(object sender, ValueChangedEventArgs<Class> e)
        {
            e.Transaction.AddAction(this.FillView);
        }

        private void MethodTypeChanged(object sender, ValueChangedEventArgs<MethodType> e)
        {
            e.Transaction.AddAction(this.FillView);

        }

        private void ProtocolChanged(object sender, ValueChangedEventArgs<string> e)
        {
            e.Transaction.AddAction(this.FillView);
        }

        private void MethodNameChanged(object sender, ValueChangedEventArgs<string> e)
        {
            e.Transaction.AddAction(this.FillView);
        }

        private void FillView()
        {
            bool remember = this.Updating;
            this.Updating = true;
            try
            {
                this.txtMethodHeader.Text = String.Format("{0}, {1}, P:{2}, #{3}, Native: {4}",
                    (this.Class == null) ? "" : this.Class.Name,
                    this.MethodType,
                    this.ProtocolName,
                    this.MethodName,
                    this.GetNativeName());
                if (this.Dirty)
                    this.txtMethodHeader.BackColor = Color.Red;
                else
                    this.txtMethodHeader.BackColor = SystemColors.Control;

                string src = this.GetMethodSource();
                this.txtSourceCode.ReadOnly = (src == null);
                this.txtSourceCode.Text = src;

                string name = this.GetNativeName();
                this.txtNativeName.ReadOnly = (name == null);
                this.txtNativeName.Text = name;

                string doc = this.GetDocumentation();
                if (doc == null)
                    this.descriptionControl.Html = new Definitions.HtmlString();
                else
                    this.descriptionControl.Html = new Definitions.HtmlString(doc);
                this.descriptionControl.Enabled = (doc != null);

                this.descriptionControlCopy.Html = this.descriptionControl.Html;
                this.descriptionControlCopy.Enabled = this.descriptionControl.Enabled;

                this.Dirty = false;
            }
            finally
            {
                this.Updating = remember;
            }
        }

        #endregion

        #region Save

        public bool Dirty { get; set; }
        private bool Updating = false;

        private void MarkDirty()
        {
            this.Dirty = true;
        }

        private void MarkClean()
        {
            this.Dirty = false;
        }



        private void ClassChanging(object sender, ValueChangingEventArgs<Class> e)
        {
            this.OnChanging(sender, e);
        }

        private void MethodTypeChanging(object sender, ValueChangingEventArgs<MethodType> e)
        {
            this.OnChanging(sender, e);
        }

        private void ProtocolChanging(object sender, ValueChangingEventArgs<string> e)
        {
            this.OnChanging(sender, e);
        }

        private void MethodNameChanging(object sender, ValueChangingEventArgs<string> e)
        {
            this.OnChanging(sender, e);
        }

        private void OnChanging<TItem>(object sender, ValueChangingEventArgs<TItem> e)
        {
            if (!this.Dirty)
                return;
            if (e.Cancel)
                return;
            if (this.Updating)
                return;

            var dlgResult = MessageBox.Show(
                String.Format("Do you want to save changes to {0}?", this.MethodName),
                "Method", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
                e.Cancel = !this.Save();
            else if (dlgResult == DialogResult.No)
            //this.AllProtocolsChanged(this, e);
            { }
            else
                e.Cancel = true;
        }

        public bool Save()
        {
            bool remember = this.Updating;
            try
            {
                this.Updating = true;

                if ((this.Class != null) && (this.MethodType != null))
                {
                    string errors;
                    string selector = MethodDefinitionControl.GetSelector(this.txtSourceCode.Text, out errors);
                    if (selector == null)
                    {
                        MessageBox.Show(errors, "Could not save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    ChangTransaction.Perform(delegate(ChangTransaction t)
                    {
                        ISet<Method> methods;
                        if (this.MethodType == Classes.MethodType.Class)
                            methods = this.Class.ClassMethods;
                        else
                            methods = this.Class.InstanceMethods;
                        Method method = methods.FirstOrDefault(m => m.Selector == selector);
                        if (method == null)
                        {
                            method = new Method(this.Class);
                            method.Selector = selector;
                            methods.Add(method);
                            this.txtNativeName.Text = MethodDefinitionControl.GenerateNativeName(selector);
                        }

                        method.Source = this.txtSourceCode.Text;
                        method.Annotations.Replace("ist.runtime.native-name", this.txtNativeName.Text);

                        this.ProtocolNameHolder.TriggerChanged(this.ProtocolName, this.ProtocolName);
                        this.MethodNameHolder.Value = selector;
                        this.MethodNameHolder.TriggerChanged(selector, selector);
                    });
                }

                //if (!this.SystemImplementation.GlobalItems.Contains(this.Global))
                //    this.SystemImplementation.GlobalItems.Add(this.Global);
                this.MarkClean();
                //this.SystemImplementation.GlobalItems.TriggerChanged();
                //this.GlobalHolder.TriggerChanged(this.Global, this.Global);
            }
            finally
            {
                this.Updating = remember;
            }
            return true;
        }

        #endregion

        public static string GetSelector(string source, out string errors)
        {
            var errorSink = new ParseErrorSink();
            var parser = new IronSmalltalk.Compiler.SemanticAnalysis.Parser();
            parser.ErrorSink = errorSink;
            var node = parser.ParseMethod(new System.IO.StringReader(source));
            if (errorSink.Errors.Count != 0)
            {
                errors = String.Join("\r\n", errorSink.Errors);
                return null;
            }
            errors = null;
            return node.Selector;
        }

        private class ParseErrorSink : IronSmalltalk.Compiler.SemanticAnalysis.IParseErrorSink
        {
            public readonly List<string> Errors = new List<string>();
            void Compiler.SemanticAnalysis.IParseErrorSink.AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, Compiler.LexicalTokens.IToken offendingToken)
            {
                this.Errors.Add(parseErrorMessage);
            }

            void Compiler.SemanticAnalysis.IParseErrorSink.AddParserError(Compiler.SemanticNodes.IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, Compiler.LexicalTokens.IToken offendingToken)
            {
                this.Errors.Add(parseErrorMessage);
            }

            void Compiler.LexicalAnalysis.IScanErrorSink.AddScanError(Compiler.LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
            {
                this.Errors.Add(scanErrorMessage);
            }
        }

        private void txtMethodHeader_DoubleClick(object sender, EventArgs e)
        {
            this.Dirty = true;
            this.txtMethodHeader.BackColor = Color.Red;
        }

        private void txtSourceCode_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private string GetMethodSource()
        {
            if (String.IsNullOrWhiteSpace(this.MethodName))
                return null;
            if (this.Class == null)
                return null;
            if (this.MethodType == null)
                return null;

            Method mth;
            if (this.MethodType == Classes.MethodType.Class)
                mth = this.Class.ClassMethods.FirstOrDefault(m => m.Selector == this.MethodName);
            else
                mth = this.Class.InstanceMethods.FirstOrDefault(m => m.Selector == this.MethodName);

            if ((mth == null) || String.IsNullOrWhiteSpace(mth.Source))
                return this.GenerateMethodPrototype();
            if (mth.Source == null)
                return String.Empty;
            else
                return mth.Source;
        }

        private string GetNativeName()
        {
            if (String.IsNullOrWhiteSpace(this.MethodName))
                return null;
            if (this.Class == null)
                return null;
            if (this.MethodType == null)
                return null;

            Method mth;
            if (this.MethodType == Classes.MethodType.Class)
                mth = this.Class.ClassMethods.FirstOrDefault(m => m.Selector == this.MethodName);
            else
                mth = this.Class.InstanceMethods.FirstOrDefault(m => m.Selector == this.MethodName);

            if (mth == null)
                return null;
            string anno = mth.Annotations.GetFirst("ist.runtime.native-name");
            if (anno == null)
                return String.Empty;
            return anno;
        }

        private string GenerateMethodPrototype()
        {
            if (String.IsNullOrWhiteSpace(this.MethodName))
                return null;
            if (this.Class == null)
                return null;

            var msg = MethodHelper.GetMessageForMethod(this.MethodName, this.MethodType, this.Class, this.ProtocolName);
            return MethodHelper.BuildMethodHeader(this.MethodName, msg);
        }

        private string GetDocumentation()
        {
            if (String.IsNullOrWhiteSpace(this.MethodName))
                return null;
            if (this.Class == null)
                return null;
            if (this.MethodType == null)
                return null;

            Method mth;
            if (this.MethodType == Classes.MethodType.Class)
                mth = this.Class.ClassMethods.FirstOrDefault(m => m.Selector == this.MethodName);
            else
                mth = this.Class.InstanceMethods.FirstOrDefault(m => m.Selector == this.MethodName);

            var msg = MethodHelper.GetMessageForMethod(this.MethodName, this.MethodType, this.Class, this.ProtocolName);
            return MethodHelper.BuildDocumentation(msg, 0);
        }

        private void descriptionControl_Changed(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        private void txtSourceCode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Alt) && (e.KeyCode == Keys.S))
            {
                if (this.Save())
                    e.SuppressKeyPress = true;
            }
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.A))
            {
                this.txtSourceCode.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        private void MethodDefinitionControl_Load(object sender, EventArgs e)
        {
            this.splitContainer.SplitterDistance = this.Width * 3 / 4;
        }

        private void txtNativeName_TextChanged(object sender, EventArgs e)
        {
            if (this.Updating)
                return;
            this.MarkDirty();
        }

        public static string GenerateNativeName(string selector)
        {
            if (selector == null)
                return null;
            if (selector.Length == 0)
                return selector;
            if (selector[0] == '_')
                return null;
            int idx = selector.IndexOf(':');
            if (idx == 0)
                return selector;
            if (idx > 0)
                selector = selector.Substring(0, idx);
            return selector.Substring(0, 1).ToUpper() + selector.Substring(1);
        }
    }
}
