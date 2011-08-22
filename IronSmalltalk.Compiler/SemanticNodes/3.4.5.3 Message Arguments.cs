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
    /// The BinaryArgumentNode parse node represents an argument of a binary message.
    /// It is defined in X3J20 as: binary_argument ::= primary unary_message*
    /// </summary>
    public partial class BinaryArgumentNode : SemanticNode, IMessageSequenceParentNode, IPrimaryParentNode
    {
        // <binary argument> ::= <primary> <unary message>*

        /// <summary>
        /// The parent message node that defines this message argument node.
        /// </summary>
        public BinaryMessageNode Parent { get; private set; }

        /// <summary>
        /// The primary of the argument.
        /// Under normal circumstances the primary is always present.
        /// Only in case of illegal source code is the primary set to null.
        /// </summary>
        public IPrimaryNode Primary { get; private set; }

        /// <summary>
        /// Optional message or a sequence of messages sent to the primary of the argument.
        /// </summary>
        public UnaryMessageSequenceNode Messages { get; private set; }

        /// <summary>
        /// Create a new binary argument node.
        /// </summary>
        /// <param name="parent">Parent message node that defines this message argument node.</param>
        protected internal BinaryArgumentNode(BinaryMessageNode parent)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="primary">Primary of the argument.</param>
        /// <param name="messages">Optional message or a sequence of messages sent to the primary.</param>
        protected internal void SetContents(IPrimaryNode primary, UnaryMessageSequenceNode messages)
        {
            if (primary == null)
                throw new ArgumentNullException("primary");
            this.Primary = primary;
            this.Messages = messages; // OK if this is null
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Primary != null)
                result.Add(this.Primary);
            if (this.Messages != null)
                result.Add(this.Messages);
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            return new IToken[0];
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            string str;
            if (this.Primary == null)
                str = "?primary?";
            else
                str = this.Primary.PrintString();

            if (this.Messages != null)
                str = str + " " + this.Messages.PrintString();

            return str;
        }
    }

    /// <summary>
    /// A sequence of messages starting with a unary message, 
    /// optionally followed by another unary message (sequence).
    /// This is an optional message sent to binary parameters.
    /// X2J20 definition: unary_message*
    /// </summary>
    public partial class UnaryMessageSequenceNode : MessageSequenceBase
    {
        // <unary message>*

        /// <summary>
        /// The first (current) message in the message sequence.
        /// Normally, this is set to a valid message node.
        /// Only if illegal source was parsed is this set to null.
        /// </summary>
        public UnaryMessageNode Message { get; private set; }

        /// <summary>
        /// Optional sequence of messages that may follow this message.
        /// </summary>
        public UnaryMessageSequenceNode NextMessage { get; private set; }

        /// <summary>
        /// Create a new message sequence starting with a unary message.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected internal UnaryMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="message">The first (current) message in the message sequence.</param>
        /// <param name="nextMessage">Optional sequence of messages that may follow this message.</param>
        protected internal void SetContents(UnaryMessageNode message, UnaryMessageSequenceNode nextMessage)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            this.Message = message;
            this.NextMessage = nextMessage; // OK with null.
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
    /// The KeywordArgumentNode parse node represents an argument of a keyword message.
    /// It is defined in X3J20 as: keyword_argument ::= primary unary_message* binary_message*
    /// </summary>
    public partial class KeywordArgumentNode : SemanticNode, IMessageSequenceParentNode, IPrimaryParentNode
    {
        /// <summary>
        /// The primary of the argument.
        /// Under normal circumstances the primary is always present.
        /// Only in case of illegal source code is the primary set to null.
        /// </summary>
        public IPrimaryNode Primary { get; private set; }

        /// <summary>
        /// Optional message or a sequence of messages sent to the primary of the argument.
        /// </summary>
        public BinaryOrBinaryUnaryMessageSequenceNode Messages { get; private set; }

        /// <summary>
        /// The parent message node that defines this message argument node.
        /// </summary>
        public KeywordMessageNode Parent { get; private set; }

        /// <summary>
        /// Create a new keyword message argument node.
        /// </summary>
        /// <param name="parent">Parent message node that defines this message argument node.</param>
        protected internal KeywordArgumentNode(KeywordMessageNode parent)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="primary">Primary of the argument.</param>
        /// <param name="messages">Optional message or a sequence of messages sent to the primary.</param>
        protected internal void SetContents(IPrimaryNode primary, BinaryOrBinaryUnaryMessageSequenceNode messages)
        {
            if (primary == null)
                throw new ArgumentNullException("primary");
            this.Primary = primary;
            this.Messages = messages; // null is OK.
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Primary != null)
                result.Add(this.Primary);
            if (this.Messages != null)
                result.Add(this.Messages);
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            return new IToken[0];
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            string str;
            if (this.Primary == null)
                str = "?primary?";
            else
                str = this.Primary.PrintString();

            if (this.Messages != null)
                str = str + " " + this.Messages.PrintString();

            return str;
        }
    }

    /// <summary>
    /// Base class for message (sequences) that may follow after keyword argument message.
    /// </summary>
    public abstract class BinaryOrBinaryUnaryMessageSequenceNode : MessageSequenceBase
    {
        /// <summary>
        /// Create a new message sequence.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected BinaryOrBinaryUnaryMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }
    }

    /// <summary>
    /// A sequence of messages starting with a unary message, 
    /// optionally followed by another unary or binary message (sequence).
    /// This is an optional message sent to keyword parameters.
    /// X2J20 definition: unary_message* binary_message*
    /// </summary>
    public partial class UnaryBinaryMessageSequenceNode : BinaryOrBinaryUnaryMessageSequenceNode
    {
        // <unary message>* <binary message>*

        /// <summary>
        /// The first (current) message in the message sequence.
        /// Normally, this is set to a valid message node.
        /// Only if illegal source was parsed is this set to null.
        /// </summary>
        public UnaryMessageNode Message { get; private set; }

        /// <summary>
        /// Optional sequence of messages that may follow this message.
        /// </summary>
        public BinaryOrBinaryUnaryMessageSequenceNode NextMessage { get; private set; }

        /// <summary>
        /// Create a new message sequence starting with a unary message.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected internal UnaryBinaryMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="message">The first (current) message in the message sequence.</param>
        /// <param name="nextMessage">Optional sequence of messages that may follow this message.</param>
        protected internal void SetContents(UnaryMessageNode message, BinaryOrBinaryUnaryMessageSequenceNode nextMessage)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            this.Message = message;
            this.NextMessage = nextMessage;
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
    /// A sequence of messages starting with a binary message, 
    /// optionally followed by another binary message (sequence).
    /// This is an optional message sent to keyword parameters.
    /// X2J20 definition: binary_message*
    /// </summary>
    public partial class BinaryMessageSequenceNode : BinaryOrBinaryUnaryMessageSequenceNode
    {
        //  <binary message>*

        /// <summary>
        /// The first (current) message in the message sequence.
        /// Normally, this is set to a valid message node.
        /// Only if illegal source was parsed is this set to null.
        /// </summary>
        public BinaryMessageNode Message { get; private set; }

        /// <summary>
        /// Optional sequence of messages that may follow this message.
        /// </summary>
        public BinaryMessageSequenceNode NextMessage { get; private set; }

        /// <summary>
        /// Create a new message sequence starting with a binary message.
        /// </summary>
        /// <param name="parent">Parent node that defines this parse node.</param>
        protected internal BinaryMessageSequenceNode(IMessageSequenceParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="message">The first (current) message in the message sequence.</param>
        /// <param name="nextMessage">Optional sequence of messages that may follow this message.</param>
        protected internal void SetContents(BinaryMessageNode message, BinaryMessageSequenceNode nextMessage)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            this.Message = message;
            this.NextMessage = nextMessage; // Null is OK.
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
}
