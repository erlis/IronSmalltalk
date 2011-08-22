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
using System.Collections.Generic;
using System.Linq;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.VseCompatibility
{
    /// <summary>
    /// Literal node that contains an array of byte integers (0 - 255).
    /// </summary>
    public class ByteArrayLiteralNode : LiteralNode
    {
        /// <summary>
        /// Hash token that defines the start of array literals.
        /// This property is always set to a legal value.
        /// </summary>
        public SpecialCharacterToken ArrayToken { get; private set; }

        /// <summary>
        /// Opening left bracket of the literal array.
        /// This property is always set to a legal value.
        /// </summary>
        public SpecialCharacterToken LeftBracket { get; private set; }

        /// <summary>
        /// Closing right bracket of the literal array.
        /// Usually it is set to a legal token, but it can be null in case of illegal source.
        /// </summary>
        public SpecialCharacterToken RightBracket { get; private set; }

        /// <summary>
        /// Collection with literal nodes that compose the elements of the literal array.
        /// The collection can be empty if the literal array contains no elements.
        /// </summary>
        public IList<SmallIntegerLiteralNode> Elements { get; private set; }

        /// <summary>
        /// Create and initialize a new literal array node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="arrayToken">Hash token that defines the start of array literals.</param>
        /// <param name="leftParenthesis">Opening left parenthesis of the literal array.</param>
        protected internal ByteArrayLiteralNode(ILiteralNodeParent parent, SpecialCharacterToken arrayToken,
            SpecialCharacterToken leftBracket)
            : base(parent)
        {
#if DEBUG
            if (!Parser.IsLiteralArrayPrefix(arrayToken))
                throw new ArgumentException("arrayToken");
            if (!VseCompatibleParser.IsOpeningByteArrayBracket(leftBracket))
                throw new ArgumentException("leftBracket");
#endif
            this.ArrayToken = arrayToken;
            this.LeftBracket = leftBracket;
            this.Elements = new List<SmallIntegerLiteralNode>();
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="elements">Collection with literal nodes that compose the elements of the literal array.</param>
        /// <param name="rightParenthesis">Closing right parenthesis of the literal array.</param>
        protected internal void SetContents(IEnumerable<SmallIntegerLiteralNode> elements, SpecialCharacterToken rightBracket)
        {
            if (elements == null)
                throw new ArgumentNullException("elements");
            if ((rightBracket != null) && !VseCompatibleParser.IsClosingByteArrayBracket(rightBracket)) // NB: We allow null value.
                throw new ArgumentException("rightBracket");
            this.RightBracket = rightBracket;
            foreach (SmallIntegerLiteralNode elem in elements)
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
            if (this.LeftBracket != null)
                result.Add(this.LeftBracket);
            if (this.RightBracket != null)
                result.Add(this.RightBracket);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("#[");
            bool first = true;
            foreach (LiteralNode elem in this.Elements)
            {
                if (!first)
                    sb.Append(" ");
                first = false;
                sb.Append(elem.PrintString());
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
