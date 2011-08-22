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
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.LexicalAnalysis
{
    /// <summary>
    /// Interface defining sink for reporting lexical errors.
    /// </summary>
    public interface IScanErrorSink
    {
        /// <summary>
        /// Report a syntax error encountered during scan of the source code.
        /// </summary>
        /// <param name="token">Token responsible for the problem.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="scanErrorMessage">Scan error message because of source code syntax error.</param>      
        void AddScanError(IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage);
    }
}
