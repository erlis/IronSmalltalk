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
using System.Linq.Expressions;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime;
using System.IO;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Runtime.CodeGeneration.Visiting;
using IronSmalltalk.Runtime.CodeGeneration;
using System.Collections;
using IronSmalltalk;
using IronSmalltalk.Interchange;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.AstJitCompiler.Runtime;
using IronSmalltalk.Common;

namespace TestPlayground
{
    public partial class WorkspaceTester : Form
    {
        public SmalltalkEnvironment Environment;
        public object LastResult;

        public WorkspaceTester()
        {
            InitializeComponent();
            this.Environment = new SmalltalkEnvironment();
            this.LastResult = null;
        }

        private void WorkspaceTester_Load(object sender, EventArgs e)
        {
            this.comboSelf.SelectedIndex = 0;
            this.textInstall.Text = Properties.Settings.Default.LastWorkspaceInstallSource;
            this.textEvaluate.Text = Properties.Settings.Default.LastWorkspaceEvalSource;
        }

        private void buttonCreateEnvironment_Click(object sender, EventArgs e)
        {
            this.Environment = new SmalltalkEnvironment();
            this.LastResult = null;
            this.textResultInstall.Text = null;
            this.textResultEvaluate.Text = null;
        }

        #region Reporting

        private class ErrorSink : IParseErrorSink, IInterchangeErrorSink, IInstallErrorSink
        {
            public bool HadErrors;
            private TextBox textResult;

            public ErrorSink(TextBox txtBox)
            {
                this.textResult = txtBox;
            }

            void IParseErrorSink.AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
            {
                this.ReportError(parseErrorMessage, startPosition, stopPosition);
            }

            void IParseErrorSink.AddParserError(IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
            {
                this.ReportError(parseErrorMessage, startPosition, stopPosition);
            }

            void IronSmalltalk.Compiler.LexicalAnalysis.IScanErrorSink.AddScanError(IronSmalltalk.Compiler.LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
            {
                this.ReportError(scanErrorMessage, startPosition, stopPosition);
            }

            void IInterchangeErrorSink.AddInterchangeError(SourceLocation startPosition, SourceLocation stopPosition, string errorMessage)
            {
                this.ReportError(errorMessage, startPosition, stopPosition);
            }

            void IInstallErrorSink.AddInstallError(string installErrorMessage, ISourceReference sourceReference)
            {
                this.ReportError(installErrorMessage, sourceReference.StartPosition, sourceReference.StopPosition);
            }

            private void ReportError(string message, SourceLocation start, SourceLocation end)
            {
                this.HadErrors = true;
                this.textResult.Text = this.textResult.Text +
                    String.Format("ERROR [{0} - {1}]: {2}\r\n", start, end, message);
            }
        }

        private void PrintResult(object val)
        {
            StringBuilder str = new StringBuilder();
            this.PrintResult(val, new HashSet<object>(), str, 0);
            this.textResultEvaluate.Text = str.ToString();
        }

        private void PrintResult(object val, ISet<object> recursionSet, StringBuilder str, int indent)
        {
            for (int i = 0; i < indent; i++)
		        str.Append('\t');

            if (recursionSet.Contains(val))
            {
                str.Append("!RECURSION!");
                return;
            }
            recursionSet.Add(val);

            if (val == null)
            {
                str.Append("nil");
                return;
            }
            if (val is Array)
            {
                this.PrintResult((Array)val, recursionSet, str, indent);
                return;
            }

            str.Append(val.GetType().Name);
            str.Append(" ");
            str.Append(val.ToString());

        }
        private void PrintResult(Array val, ISet<object> recursionSet, StringBuilder str, int indent)
        {
            str.Append("Array ");
            str.Append(val.GetType().Name);
            str.Append(" Length: ");
            str.Append(val.Length.ToString());
            str.Append(" #(");
            foreach (object elem in val)
            {
                str.AppendLine();
                this.PrintResult(elem, recursionSet, str, indent + 1);
            }
            str.Append(" )");
        }
        private void PrintResult(IEnumerable val, ISet<object> recursionSet, StringBuilder str, int indent)
        {
            str.Append("Enumerable ");
            str.Append(val.GetType().Name);
            str.Append(" Length: ");
            str.Append(" #(");
            foreach (object elem in val)
            {
                str.AppendLine();
                this.PrintResult(elem, recursionSet, str, indent + 1);
            }
            str.Append(" )");
        }

        #endregion


        private void buttonInstall_Click(object sender, EventArgs e)
        {
            if (this.Environment == null)
            {
                MessageBox.Show("First, create the environment.");
                return;
            }

            Properties.Settings.Default.LastWorkspaceInstallSource = this.textInstall.Text;
            Properties.Settings.Default.LastWorkspaceEvalSource = this.textEvaluate.Text;
            Properties.Settings.Default.Save();

            this.textResultInstall.Text = null;

            string txt = this.textInstall.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.textInstall.Text;

            this.Environment.CompilerService.InstallSource(txt, new ErrorSink(this.textResultInstall), new ErrorSink(this.textResultInstall));

            if (String.IsNullOrEmpty(this.textResultInstall.Text))
                this.textResultInstall.Text = "OK";
        }

        private void buttonJitExpression_Click(object sender, EventArgs e)
        {
            var lambda = this.JitExpression();
            if (lambda == null)
                return;
            this.PrintResult(lambda);
        }

        private void buttonEvalExpression_Click(object sender, EventArgs e)
        {
            object receiver = (this.comboSelf.SelectedIndex == 0) ? null : this.LastResult;
            var lambda = this.JitExpression();
            if (lambda == null)
                return;

          //AppDomain myDomain = AppDomain.CurrentDomain;
          //System.Reflection.AssemblyName asmName = new System.Reflection.AssemblyName();
          //asmName.Name = "MyDynamicAsm";
  
          //System.Reflection.Emit.AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(
          //                               asmName, 
          //                               System.Reflection.Emit.AssemblyBuilderAccess.RunAndSave);
  
          //System.Reflection.Emit.ModuleBuilder myModule = myAsmBuilder.DefineDynamicModule("MyDynamicAsm",
          //                                                          "MyDynamicAsm.dll");
  
          //System.Reflection.Emit.TypeBuilder myTypeBld = myModule.DefineType("MyDynamicType",
          //                                            System.Reflection.TypeAttributes.Public);           

          // System.Reflection.Emit.MethodBuilder myMthdBld = myTypeBld.DefineMethod(
          //                                     "TestMethod",
          //                                     System.Reflection.MethodAttributes.Public |
          //                                     System.Reflection.MethodAttributes.Static,
          //                                     typeof(object),
          //                                     new Type[] { typeof(SmalltalkRuntime), typeof(object) });
          // //lambda.CompileToMethod(myMthdBld);

          // // myAsmBuilder.Save("c:\\temp\\MyDynamicAsm.dll");

            var function = lambda.Compile();
            this.LastResult = function(this.Environment.Runtime, receiver);
            this.PrintResult(this.LastResult);
        }

        private Expression<Func<SmalltalkRuntime, object, object>> JitExpression()
        {
            if (this.Environment == null)
            {
                MessageBox.Show("First, create the environment.");
                return null;
            }

            Properties.Settings.Default.LastWorkspaceInstallSource = this.textInstall.Text;
            Properties.Settings.Default.LastWorkspaceEvalSource = this.textEvaluate.Text;
            Properties.Settings.Default.Save();

            this.textResultEvaluate.Text = null;

            string txt = this.textEvaluate.SelectedText;
            if (String.IsNullOrEmpty(txt))
                txt = this.textEvaluate.Text;
            StringReader reader = new StringReader(txt);

            ErrorSink errorSink = new ErrorSink(this.textResultEvaluate);
            Parser parser = new Parser();
            parser.ErrorSink = errorSink;
            InitializerNode node = parser.ParseInitializer(reader);
            if (errorSink.HadErrors)
                return null;

            AstIntermediateInitializerCode code = new AstIntermediateInitializerCode(node);
            var compilationResult = code.CompileGlobalInitializer(this.Environment.Runtime);
            if (compilationResult == null)
                return null;
            return compilationResult.ExecutableCode;
        }
    }
}
