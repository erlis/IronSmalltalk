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
    /// Parse node representing a literal constant.
    /// </summary>
    public abstract class LiteralNode : SemanticNode, IPrimaryNode, ILiteralNodeParent
    {
        /// <summary>
        /// Parent node that defines this literal node.
        /// </summary>
        public ILiteralNodeParent Parent { get; private set; }

        /// <summary>
        /// Create and initialize a new literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines the literal node.</param>
        protected LiteralNode(ILiteralNodeParent parent)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }
    }

    /// <summary>
    /// Interface for nodes that can be parent to a literal node.
    /// That is either basic expression nodes or array literals.
    /// </summary>
    public interface ILiteralNodeParent : IParseNode
    {
    }

    /// <summary>
    /// Literal node that contains a single constant value,
    /// i.e. literals except for array literals.
    /// </summary>
    /// <typeparam name="TToken">Type of the lireral token.</typeparam>
    public abstract class SingleValueLiteralNode<TToken> : LiteralNode
        where TToken : ILiteralToken
    {
        /// <summary>
        /// Token defining the literal node.
        /// </summary>
        public TToken Token { get; private set; }

        /// <summary>
        /// Create and initialize a new single value literal node.
        /// </summary>
        /// <param name="parent">The parent node that defines the literal node.</param>
        /// <param name="token">Token defining the literal node.</param>
        protected SingleValueLiteralNode(ILiteralNodeParent parent, TToken token)
            : base(parent)
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
                return this.Token.SourceString;
        }
    }
}
