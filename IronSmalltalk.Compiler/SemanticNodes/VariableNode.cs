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
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// A VariableNode is a base class for parse nodes representing some kind of a variable.
    /// That can be variable declaration, such as argument or temporary or 
    /// variable usage, such as assignment or reading.
    /// </summary>
    public abstract class VariableNode : SemanticNode
    {
        /// <summary>
        /// Identifier token containing the name of the variable.
        /// </summary>
        public IdentifierToken Token { get; protected set; }

        /// <summary>
        /// Create a new variable parse node.
        /// </summary>
        protected VariableNode()
        {
        }

        /// <summary>
        /// Create and initialize a new variable parse node.
        /// </summary>
        /// <param name="token">Identifier token containing the name of the variable.</param>
        protected VariableNode(IdentifierToken token)
        {
#if DEBUG
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            this.Token = token;
        }

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
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.Token != null)
                result.Add(this.Token);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            if (this.Token == null)
                return "?token?";
            else
                return this.Token.Value;
        }
    }
}
