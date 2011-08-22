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
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// Literal node that contains an array of literal constant nodes.
    /// </summary>
    public partial class ArrayLiteralNode : LiteralNode
    {
        /// <summary>
        /// Hash token that defines the start of array literals.
        /// This property is always set to a legal value.
        /// </summary>
        public SpecialCharacterToken ArrayToken { get; private set; }

        /// <summary>
        /// Opening left parenthesis of the literal array.
        /// This property is always set to a legal value.
        /// </summary>
        public SpecialCharacterToken LeftParenthesis { get; private set; }
        
        /// <summary>
        /// Closing right parenthesis of the literal array.
        /// Usually it is set to a legal token, but it can be null in case of illegal source.
        /// </summary>
        public SpecialCharacterToken RightParenthesis { get; private set; }

        /// <summary>
        /// Collection with literal nodes that compose the elements of the literal array.
        /// The collection can be empty if the literal array contains no elements.
        /// </summary>
        public IList<LiteralNode> Elements { get; private set; }

        /// <summary>
        /// Create and initialize a new literal array node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="arrayToken">Hash token that defines the start of array literals.</param>
        /// <param name="leftParenthesis">Opening left parenthesis of the literal array.</param>
        protected internal ArrayLiteralNode(ILiteralNodeParent parent, SpecialCharacterToken arrayToken,
            SpecialCharacterToken leftParenthesis)
            : base(parent)
        {
#if DEBUG
            if (!Parser.IsLiteralArrayPrefix(arrayToken))
                throw new ArgumentException("arrayToken");
            if (!Parser.IsOpeningParenthesis(leftParenthesis))
                throw new ArgumentException("leftParenthesis");
#endif
            this.ArrayToken = arrayToken;
            this.LeftParenthesis = leftParenthesis;
            this.Elements = new List<LiteralNode>();
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="elements">Collection with literal nodes that compose the elements of the literal array.</param>
        /// <param name="rightParenthesis">Closing right parenthesis of the literal array.</param>
        protected internal void SetContents(IEnumerable<LiteralNode> elements, SpecialCharacterToken rightParenthesis)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");
            if ((rightParenthesis != null) &&  !Parser.IsClosingParenthesis(rightParenthesis)) // NB: We allow null value.
                throw new ArgumentException("rightParenthesis");
            this.RightParenthesis = rightParenthesis;
            foreach (LiteralNode elem in elements)
                this.Elements.Add(elem);
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            result.AddRange(this.Elements.Cast<IParseNode>());
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.ArrayToken != null)
                result.Add(this.ArrayToken);
            if (this.LeftParenthesis != null)
                result.Add(this.LeftParenthesis);
            if (this.RightParenthesis != null)
                result.Add(this.RightParenthesis);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("#(");
            bool first = true;
            foreach (LiteralNode elem in this.Elements)
            {
                if (!first)
                    sb.Append(" ");
                first = false;
                sb.Append(elem.PrintString());
            }
            sb.Append(")");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Special case, used only with arrays. 
    /// This is in case ppl. write: #(true asUppercase).
    /// </summary>
    public partial class IdentifierLiteralNode : LiteralNode
    {
        /// <summary>
        /// Token defining the literal node.
        /// </summary>
        public ILiteralArrayIdentifierToken Token { get; private set; }

        /// <summary>
        /// Create and initialize a new identifier literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        protected internal IdentifierLiteralNode(ILiteralNodeParent parent, ILiteralArrayIdentifierToken token)
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
