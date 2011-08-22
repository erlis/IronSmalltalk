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
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// Base class for messages in a message sequence.
    /// </summary>
    public abstract class MessageNode : SemanticNode
    {
        /// <summary>
        /// The parent message sequence node that defines this message node.
        /// </summary>
        public MessageSequenceBase Parent { get; private set; }

        /// <summary>
        /// Create a new message node.
        /// </summary>
        /// <param name="parent">The parent message sequence node that defines this message node.</param>
        protected MessageNode(MessageSequenceBase parent)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }
    }

    /// <summary>
    /// Unary message parse node, defined in X3J20 as: unary_message ::= unarySelector.
    /// </summary>
    public partial class UnaryMessageNode : MessageNode
    {
        /// <summary>
        /// Token containing the message selector.
        /// Normally, this property contains a value. 
        /// Only in case of illegal source code will it be set to null.
        /// </summary>
        public IdentifierToken SelectorToken { get; private set; }

        /// <summary>
        /// Create and initialize a new unary message node.
        /// </summary>
        /// <param name="parent">The parent message sequence node that defines this message node.</param>
        /// <param name="token">Token containing the message selector.</param>
        protected internal UnaryMessageNode(MessageSequenceBase parent, IdentifierToken token)
            : base(parent)
        {
#if DEBUG
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            this.SelectorToken = token;
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
            if (this.SelectorToken != null)
                result.Add(this.SelectorToken);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            if (this.SelectorToken == null)
                return "?selector?";
            else
                return this.SelectorToken.Value;
        }
    }

    /// <summary>
    /// Binary message parse node, defined in X3J20 as: binary_message ::= binarySelector binary_argument.
    /// </summary>
    public partial class BinaryMessageNode : MessageNode
    {
        // <binary message> ::= binarySelector <binary argument>

        /// <summary>
        /// Token containing the message selector.
        /// Normally, this property contains a value. 
        /// Only in case of illegal source code will it be set to null.
        /// </summary>
        public BinarySelectorToken SelectorToken { get; private set; }

        /// <summary>
        /// Parse node containing the argument for the binary message.
        /// Normally, this property contains a value. 
        /// Only in case of illegal source code will it be set to null.
        /// </summary>
        public BinaryArgumentNode Argument { get; private set; }

        /// <summary>
        /// Create a new binary message node.
        /// </summary>
        /// <param name="parent">The parent message sequence node that defines this message node.</param>
        /// <param name="token">Token containing the message selector.</param>
        protected internal BinaryMessageNode(MessageSequenceBase parent, BinarySelectorToken token)
            : base(parent)
        {
#if DEBUG
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            this.SelectorToken = token;
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="argument">Parse node that is the argument for the binary message.</param>
        protected internal void SetContents(BinaryArgumentNode argument)
        {
            if (argument == null)
                throw new ArgumentNullException();
            this.Argument = argument;
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Argument != null)
                result.Add(this.Argument);
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.SelectorToken != null)
                result.Add(this.SelectorToken);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            string str;
            if (this.SelectorToken == null)
                str = "?selector?";
            else
                str = this.SelectorToken.Value;

            if (this.Argument == null)
                str = str + " ?argument?";
            else
                str = str + " " + this.Argument.PrintString();

            return str;
        }
    }

    /// <summary>
    /// Keyword message parse node, defined in X3J20 as: keyword_message ::= (keyword keyword_argument )+
    /// </summary>
    public partial class KeywordMessageNode : MessageNode
    {
        /// <summary>
        /// Collection of tokens composing the message selector.
        /// Normally, this property contains at least one value. 
        /// Only in case of illegal source code will it be empty.
        /// </summary>
        public List<KeywordToken> SelectorTokens { get; private set; }

        /// <summary>
        /// Collection of argument parse nodes containing the arguments for the keyword message.
        /// Normally, this property contains the same number of elements as the SelectorTokens property. 
        /// Only in case of illegal source code will have less elements or be empty.
        /// </summary>
        public List<KeywordArgumentNode> Arguments { get; private set; }

        /// <summary>
        /// Create a new keyword message node.
        /// </summary>
        /// <param name="parent">The parent message sequence node that defines this message node.</param>
        protected internal KeywordMessageNode(MessageSequenceNode parent)
            : base(parent)
        {
            this.SelectorTokens = new List<KeywordToken>();
            this.Arguments = new List<KeywordArgumentNode>();
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="selectorTokens">Collection of tokens composing the message selector.</param>
        /// <param name="arguments">Argument nodes containing the arguments for the keyword message.</param>
        protected internal void SetContents(IEnumerable<KeywordToken> selectorTokens, IEnumerable<KeywordArgumentNode> arguments)
        {
            if (selectorTokens == null)
                throw new ArgumentNullException("selectorTokens");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            this.SelectorTokens.Clear();
            this.Arguments.Clear();
            this.SelectorTokens.AddRange(selectorTokens);
            this.Arguments.AddRange(arguments);

            if (this.SelectorTokens.Count != this.Arguments.Count)
                throw new ArgumentException("Arguments selectorTokens and arguments must be of the same size.");

            if (this.SelectorTokens.Count == 0)
                throw new ArgumentException("Arguments selectorTokens and arguments must contain at least one element.");
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            result.AddRange(this.Arguments.Cast<IParseNode>());
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            result.AddRange(this.SelectorTokens.Cast<IToken>());
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            if ((this.Arguments.Count == 0) && (this.SelectorTokens.Count == 0))
                return "?selector? ?argument?";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < Math.Max(this.Arguments.Count, this.SelectorTokens.Count); i++)
            {
                if (i != 0)
                    sb.Append(" ");

                if (i >= this.SelectorTokens.Count)
                    sb.Append("?selector?");
                else
                    sb.Append(this.SelectorTokens[i].Value);

                sb.Append(" ");

                if (i >= this.Arguments.Count)
                    sb.Append("?argument?");
                else
                    sb.Append(this.Arguments[i].PrintString());
            }

            return sb.ToString();
        }
    }
}
