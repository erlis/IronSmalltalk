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
    /// A sequence of message nodes.
    /// </summary>
    /// <remarks>
    /// The MessageSequenceNode class represents a sequence of message nodes.
    /// Example of this is the Smalltalk expression: 'abc' asUppercase first codePoint + 1.
    /// This has a sequence of 4 messages: #asUppercase, #first, #codePoint and #'+' (binary plus).
    /// 
    /// IronSmalltalk uses a linked list sequence to represent the sequence of message.
    /// The combination of legal sequences is definex in X3J20 "3.4.5.3 Messages".
    /// 
    /// Following sequences (subclasses) are identified:
    /// - UnaryBinaryKeywordMessageSequenceNode : unary_message+ binary_message* [keyword_message] 
    ///     ... a unary message, followed by another message (sequence)  ... an MessageSequenceNode
    /// - BinaryKeywordMessageSequenceNode : binary_message+ [keyword_message]
    ///     ... a binary message, followed by binary or keyword message (sequence) ... an BinaryKeywordOrKeywordMessageSequenceNode 
    /// - KeywordMessageSequenceNode : keyword_message
    ///     ... a keyword message.
    ///     
    /// - UnaryMessageSequenceNode : unary_message* 
    ///     ... a unary message, followed by a unuary message (sequence) ... as part of binary arguments
    /// - UnaryBinaryMessageSequenceNode : unary_message* binary_message*
    ///     ... a unary message, followed by a unuary or binary message (sequence) ... as part of keyword arguments
    /// - BinaryMessageSequenceNode : binary_message*
    ///     ... a binary message, followed by a binary message (sequence) ... as part of keyword arguments       
    /// 
    /// Following abstract classes are defined to group and generalize specific message sequences:
    /// - MessageSequenceNode : UnaryBinaryKeywordMessageSequenceNode, BinaryKeywordMessageSequenceNode, KeywordMessageSequenceNode
    /// - BinaryKeywordOrKeywordMessageSequenceNode: BinaryKeywordMessageSequenceNode, KeywordMessageSequenceNode
    /// - BinaryOrBinaryUnaryMessageSequenceNode: BinaryMessageSequenceNode, UnaryBinaryMessageSequenceNode
    /// </remarks>
    public abstract class MessageSequenceBase : SemanticNode, IMessageSequenceParentNode
    {
        /// <summary>
        /// The parent node that defines this parse node.
        /// </summary>
        public IMessageSequenceParentNode Parent { get; private set; }

        /// <summary>
        /// Create a new message sequence.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected MessageSequenceBase(IMessageSequenceParentNode parent)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            return new IToken[0];
        }
    }

    /// <summary>
    /// A sequence of message nodes.
    /// This class is base class for all message sequences that can be used by an expression.
    /// </summary>
    public abstract class MessageSequenceNode : MessageSequenceBase
    {
        /// <summary>
        /// Create a new message sequence.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected MessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }
    }

    /// <summary>
    /// An interface implemented by parse nodes that can be parent node to a message sequence.
    /// </summary>
    public interface IMessageSequenceParentNode : IParseNode
    {
    }

    /// <summary>
    /// A sequence of messages starting with a unary message, 
    /// optionally followed by another message (sequence).
    /// X2J20 definition: unary_message+ binary_message* [keyword_message]
    /// </summary>
    public partial class UnaryBinaryKeywordMessageSequenceNode : MessageSequenceNode
    {
        // <unary message>+ <binary message>* [<keyword message>]

        /// <summary>
        /// The first (current) message in the message sequence.
        /// Normally, this is set to a valid message node.
        /// Only if illegal source was parsed is this set to null.
        /// </summary>
        public UnaryMessageNode Message { get; private set; }

        /// <summary>
        /// Optional sequence of messages that may follow this message.
        /// </summary>
        public MessageSequenceNode NextMessage { get; private set; }

        /// <summary>
        /// Create a new message sequence starting with a unary message.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected internal UnaryBinaryKeywordMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="message">The first (current) message in the message sequence.</param>
        /// <param name="nextMessage">Optional sequence of messages that may follow this message.</param>
        protected internal void SetContents(UnaryMessageNode message, MessageSequenceNode nextMessage)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            this.Message = message;
            this.NextMessage = nextMessage; // null is OK
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Message != null)
                result.Add(this.Message);
            if (this.NextMessage != null)
                result.Add(this.NextMessage);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            string str;
            if (this.Message == null)
                str = "?message?";
            else
                str = this.Message.PrintString();

            if (this.NextMessage != null)
                str = str + " " + this.NextMessage.PrintString();

            return str;
        }
    }

    /// <summary>
    /// Base class for message (sequences) that may follow after binary message.
    /// </summary>
    public abstract class BinaryKeywordOrKeywordMessageSequenceNode : MessageSequenceNode
    {
        /// <summary>
        /// Create a new message sequence.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected BinaryKeywordOrKeywordMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }
    }

    /// <summary>
    /// A sequence of messages starting with a binary message, 
    /// optionally followed by another binary or keyword message (sequence).
    /// X2J20 definition: binary_message+ [keyword_message]
    /// </summary>
    public partial class BinaryKeywordMessageSequenceNode : BinaryKeywordOrKeywordMessageSequenceNode
    {
        // <binary message>+ [<keyword message>]

        /// <summary>
        /// The first (current) message in the message sequence.
        /// Normally, this is set to a valid message node.
        /// Only if illegal source was parsed is this set to null.
        /// </summary>
        public BinaryMessageNode Message { get; private set; }

        /// <summary>
        /// Optional sequence of messages that may follow this message.
        /// </summary>
        public BinaryKeywordOrKeywordMessageSequenceNode NextMessage { get; private set; }

        /// <summary>
        /// Create a new message sequence starting with a binary message.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected internal BinaryKeywordMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="message">The first (current) message in the message sequence.</param>
        /// <param name="nextMessage">Optional sequence of messages that may follow this message.</param>
        protected internal void SetContents(BinaryMessageNode message, BinaryKeywordOrKeywordMessageSequenceNode nextMessage)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            this.Message = message;
            this.NextMessage = nextMessage; // null is OK
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Message != null)
                result.Add(this.Message);
            if (this.NextMessage != null)
                result.Add(this.NextMessage);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            string str;
            if (this.Message == null)
                str = "?message?";
            else
                str = this.Message.PrintString();

            if (this.NextMessage != null)
                str = str + " " + this.NextMessage.PrintString();

            return str;
        }
    }

    /// <summary>
    /// A sequence of messages starting with a keyword message.
    /// Technically, this is not a sequence, since no messages may follow it,
    /// but we've kept the sequence nomenclature.
    /// X2J20 definition: keyword_message
    /// </summary>
    public partial class KeywordMessageSequenceNode : BinaryKeywordOrKeywordMessageSequenceNode
    {
        // <keyword message>

        /// <summary>
        /// The first (current) message in the message sequence.
        /// Normally, this is set to a valid message node.
        /// Only if illegal source was parsed is this set to null.
        /// </summary>
        public KeywordMessageNode Message { get; private set; }

        /// <summary>
        /// Create a new message sequence with a keyword message.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected internal KeywordMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="message">The first (current) message in the message sequence.</param>
        protected internal void SetContents(KeywordMessageNode message)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            this.Message = message;
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Message != null)
                result.Add(this.Message);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            if (this.Message == null)
                return "?message?";
            else
                return this.Message.PrintString();
        }
    }

}
