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
using IronSmalltalk.Compiler.Interchange;
using IronSmalltalk.Interchange;
using IronSmalltalk.Common;

namespace TestPlayground
{
    public class ConsoleErrorSink : IInterchangeErrorSink
    {
        public void AddInterchangeError(SourceLocation startPosition, SourceLocation stopPosition, string errorMessage)
        {
            Console.WriteLine("InterchangeError [{0} - {1}]: {2}", startPosition, stopPosition, errorMessage);
        }

        public void AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
        {
            Console.WriteLine("ParseError [{0} - {1}]: {2}", startPosition, stopPosition, parseErrorMessage);
        }

        public void AddParserError(IronSmalltalk.Compiler.SemanticNodes.IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IronSmalltalk.Compiler.LexicalTokens.IToken offendingToken)
        {
            Console.WriteLine("ParseError [{0} - {1}]: {2}", startPosition, stopPosition, parseErrorMessage);
        }

        public void AddScanError(IronSmalltalk.Compiler.LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
        {
            Console.WriteLine("ScanError [{0} - {1}]: {2}", startPosition, stopPosition, scanErrorMessage);
        }
    }
}
