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
    /// Block constructor node as described in X3J20 chapter "3.4.4 Blocks".
    /// </summary>
    public partial class BlockNode : FunctionNode, IPrimaryNode
    {
        /// <summary>
        /// Token for the left / opening square bracket of the block.
        /// </summary>
        public SpecialCharacterToken LeftBracket { get; private set; }

        /// <summary>
        /// Token for the right / closing square bracket of the block.
        /// </summary>
        public SpecialCharacterToken RightBracket { get; private set; }

        /// <summary>
        /// The vertical bar after the block arguments. This exists only if args are present.
        /// </summary>
        public VerticalBarToken ArgumentsBar { get; private set; }

        /// <summary>
        /// Collection of block arguments for this block.
        /// The calection is empty if no arguments are defined.
        /// </summary>
        public List<BlockArgumentNode> Arguments { get; private set; }

        /// <summary>
        /// The parent node, where this block node belongs to.
        /// </summary>
        public IPrimaryParentNode Parent { get; private set; }

        /// <summary>
        /// Create and intialize a block node.
        /// </summary>
        /// <param name="parent">The parent node to the block node.</param>
        /// <param name="token">Token for the left / opening square bracket of the block.</param>
        protected internal BlockNode(IPrimaryParentNode parent, SpecialCharacterToken token)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            this.Arguments = new List<BlockArgumentNode>();
            this.Parent = parent;
            this.LeftBracket = token;
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="arguments">Collection of block arguments for this block.</param>
        /// <param name="argumentsBar">The vertical bar after the block arguments.</param>
        /// <param name="leftBar">Left vertical bar "|" token, if temporary variable definition is present.</param>
        /// <param name="temporaries">A collection of temporary variable nodes.</param>
        /// <param name="rightBar">Right vertical bar "|" token, if temporary variable definition is present.</param>
        /// <param name="statements">Executable statements defining the function.</param>
        /// <param name="rightBracket">Token for the right / closing square bracket of the block.</param>
        protected internal void SetContents(IEnumerable<BlockArgumentNode> arguments, VerticalBarToken argumentsBar,
            VerticalBarToken leftBar, IEnumerable<TemporaryVariableNode> temporaries, VerticalBarToken rightBar, 
            StatementNode statements, SpecialCharacterToken rightBracket)
        {
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            this.SetContents(leftBar, temporaries, rightBar, statements);

            this.Arguments.Clear();
            this.Arguments.AddRange(arguments);
            this.ArgumentsBar = argumentsBar; // OK with null if no args. or illegal source
            this.RightBracket = rightBracket; // OK with null if illegal source
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            result.AddRange(this.Arguments.Cast<IParseNode>());
            result.AddRange(this.Temporaries.Cast<IParseNode>());
            if (this.Statements != null)
                result.Add(this.Statements);
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.LeftBracket != null)
                result.Add(this.LeftBracket); 
            if (this.ArgumentsBar != null)
                result.Add(this.ArgumentsBar);
            if (this.LeftBar != null)
                result.Add(this.LeftBar);
            if (this.RightBar != null)
                result.Add(this.RightBar);
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
            sb.Append("[");
            foreach(BlockArgumentNode arg in this.Arguments)
            {
                sb.Append(" :");
                if (arg.Token == null)
                    sb.Append("?name?");
                else
                    sb.Append(arg.Token.Value);
            }
            if (this.Arguments.Count != 0)
                sb.Append(" |");

            if (this.Temporaries.Count != 0)
            {
                sb.Append(" |");
                foreach(TemporaryVariableNode tmp in this.Temporaries)
                {
                    sb.Append(" ");
                    if (tmp.Token == null)
                        sb.Append("?name?");
                    else
                        sb.Append(tmp.Token.Value);
                }
                sb.Append(" |");
            }

            sb.Append(" ... ]"); // We don't print the statements 

            return sb.ToString();
        }
    }

    /// <summary>
    /// Block argument node represents an argument of a block closure.
    /// </summary>
    public partial class BlockArgumentNode : ArgumentNode
    {
        /// <summary>
        /// Block closure that defines the argument node.
        /// </summary>
        public BlockNode Parent { get; private set; }

        /// <summary>
        /// Colon that preceeds the argument name.
        /// </summary>
        public SpecialCharacterToken Colon { get; private set; }

        /// <summary>
        /// Create a new block argument.
        /// </summary>
        /// <param name="parent">Block closure that defines the argument node.</param>
        /// <param name="colon">Colon that preceeds the argument name.</param>
        /// <param name="token">Identifier token containing the name of the argument.</param>
        protected internal BlockArgumentNode(BlockNode parent, SpecialCharacterToken colon, IdentifierToken token)
            : base(token)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (colon == null)
                throw new ArgumentNullException("colon");
#endif
            this.Colon = colon;
            this.Parent = parent;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.Colon != null)
                result.Add(this.Colon);
            if (this.Token != null)
                result.Add(this.Token);
            return result;
        }
    }
}
