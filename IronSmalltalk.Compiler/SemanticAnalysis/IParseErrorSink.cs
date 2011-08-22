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
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.SemanticAnalysis
{
    /// <summary>
    /// Interface defining sink for reporting parsing (semantical) errors.
    /// </summary>
    public interface IParseErrorSink : IScanErrorSink
    {
        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        void AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken);

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="node">Parse node responsible / signaling the error.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        void AddParserError(IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken);
    }
}
