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
    /// A sequence of messages sent that are sent as cascade message to the primary of a basic expression.
    /// Cascade messages are defined in X3J20 "3.4.5.3 Messages" as: cascaded_messages ::= (';' messages)*
    /// </summary>
    public partial class CascadeMessageSequenceNode : SemanticNode, IMessageSequenceParentNode, ICascadeMessageSequenceParentNode
    {
        /// <summary>
        /// Token representing the semicolon used to delimit cascase message sequences.
        /// </summary>
        public SpecialCharacterToken Semicolon { get; private set; }

        /// <summary>
        /// Message or a sequence of messages sent to the primary of the expression.
        /// Normally this property is set to a legal parse node.
        /// Only in case of illegal source code will it be null.
        /// </summary>
        public MessageSequenceNode Messages { get; private set; }

        /// <summary>
        /// Optional cascade messages following this cascade message.
        /// </summary>
        public CascadeMessageSequenceNode NextCascade { get; private set; }

        /// <summary>
        /// The parent node that defines this cascade message node.
        /// </summary>
        public ICascadeMessageSequenceParentNode Parent { get; private set; }

        /// <summary>
        /// Create a new cascade message sequence.
        /// </summary>
        /// <param name="parent">Parent node that defines this cascade message node.</param>
        /// <param name="token">Token representing the semicolon used to delimit cascase message sequences.</param>
        protected internal CascadeMessageSequenceNode(ICascadeMessageSequenceParentNode parent, SpecialCharacterToken token)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            this.Parent = parent;
            this.Semicolon = token;
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="messages">Message or a sequence of messages sent to the primary of the expression.</param>
        /// <param name="nextCascade">Optional cascade messages following this cascade message.</param>
        protected internal void SetContents(MessageSequenceNode messages, CascadeMessageSequenceNode nextCascade)
        {
            if (messages == null)
                throw new ArgumentNullException("messages");
            this.Messages = messages;
            this.NextCascade = nextCascade; // OK with null
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Messages != null)
                result.Add(this.Messages);
            if (this.NextCascade != null)
                result.Add(this.NextCascade);
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.Semicolon != null)
                result.Add(this.Semicolon);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            string str;
            if (this.Semicolon == null)
                str = "?semicolon? ";
            else
                str = "; ";

            if (this.Messages == null)
                str = str + "?messages?";
            else
                str = str + this.Messages.PrintString();

            return str;
        }
    }

    /// <summary>
    /// Interface implemented by parse nodes that can parent a cascade message.
    /// </summary>
    public interface ICascadeMessageSequenceParentNode : IParseNode
    {
    }
}
