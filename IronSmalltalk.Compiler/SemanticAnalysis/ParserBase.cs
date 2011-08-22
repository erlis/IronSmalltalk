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
using System.IO;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.SemanticAnalysis
{
    public abstract class ParserBase : IScanErrorSink
    {
        /// <summary>
        /// Lexical scanner used by the parser to tokanize the source string.
        /// </summary>
        private Scanner Scanner;

        /// <summary>
        /// Optional sink object for reporting semantical errors encountered by the parser and
        /// source code syntax errors encountered by the scanner.
        /// </summary>
        public IParseErrorSink ErrorSink { get; set; }

        protected ParserBase()
        {
        }

        #region Helper Functions

        protected virtual void InitParser(TextReader reader)
        {
            this.Scanner = new Scanner(reader);
            this.Scanner.ErrorSink = this;
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="node">Parse node responsible / signaling the error.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected virtual void ReportParserError(IParseNode node, string parseErrorMessage, IToken offendingToken)
        {
            if (offendingToken == null)
                throw new ArgumentNullException("token");
            else
                this.ReportParserError(node, offendingToken.StartPosition, offendingToken.StopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="node">Parse node responsible / signaling the error.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected virtual void ReportParserError(IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken)
        {
            if (this.ErrorSink != null)
                this.ErrorSink.AddParserError(node, startPosition, stopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected virtual void ReportParserError(string parseErrorMessage, IToken offendingToken)
        {
            if (offendingToken == null)
                throw new ArgumentNullException("token");
            this.ReportParserError(offendingToken.StartPosition, offendingToken.StopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected virtual void ReportParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken)
        {
            if (this.ErrorSink != null)
                this.ErrorSink.AddParserError(startPosition, stopPosition, parseErrorMessage, offendingToken);            
        }

        protected virtual Token GetNextTokenxx(Preference preference)
        {
            Token token;

            if (this.ResidueToken != null)
            {
                token = this.ResidueToken;
                this.ResidueToken = null;
                return token;
            }

            do
            {
                token = this.Scanner.GetToken(preference);

                //if ((parent.Comments != null) && (token is CommentToken))
                //    parent.Comments.Add((CommentToken)token);

            } while (token is WhitespaceToken);
            return token;
        }

        protected Token ResidueToken;

        #endregion

        /// <summary>
        /// Report a syntax error encountered during scan of the source code.
        /// </summary>
        /// <param name="token">Token responsible for the problem.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="scanErrorMessage">Scan error message because of source code syntax error.</param>   
        void IScanErrorSink.AddScanError(IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
        {
            // Just forward the error to the outer sink.
            if (this.ErrorSink != null)
                this.ErrorSink.AddScanError(token, startPosition, stopPosition, scanErrorMessage);
        }
    }
}
