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
using IronSmalltalk.Compiler.LexicalTokens;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// The ParseNode is the root of all parse tree nodes.
    /// </summary>
    public interface IParseNode
    {
        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        IEnumerable<IParseNode> GetChildNodes();

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        IEnumerable<IToken> GetTokens();

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        string PrintString();
    }
}
