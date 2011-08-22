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
using System.Collections.Generic;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Compiler.LexicalAnalysis;
using System.IO;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public abstract partial class InterchangeParseNode : ParseNode
    {
        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            return new IParseNode[0];
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<LexicalTokens.IToken> GetTokens()
        {
            return new LexicalTokens.IToken[0];
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            return "";
        }
    }
}
