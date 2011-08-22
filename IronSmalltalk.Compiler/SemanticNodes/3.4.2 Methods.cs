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
    /// Method definition node as described in X3J20 chapter "3.4.2 Method Definition".
    /// </summary>
    public partial class MethodNode : FunctionNode
    {
        /// <summary>
        /// Tokens for the selector parts of the method.
        /// </summary>
        public List<IMethodSelectorToken> SelectorParts { get; private set; }

        /// <summary>
        /// Collection of method arguments for this method.
        /// The collection is empty if no arguments were defined.
        /// </summary>
        public List<MethodArgumentNode> Arguments { get; private set; }

        /// <summary>
        /// Optional primitive API call to native code.
        /// This is an IronSmalltalk extension to X3J20.
        /// </summary>
        public PrimitiveCallNode Primitive { get; private set; }

        /// <summary>
        /// Create and intialize a method node.
        /// </summary>
        protected internal MethodNode()
        {
            this.SelectorParts = new List<IMethodSelectorToken>();
            this.Arguments = new List<MethodArgumentNode>();
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="selectorParts">Tokens for the selector parts of the method.</param>
        /// <param name="arguments">Collection of method arguments for this method.</param>
        protected internal void SetContents(IEnumerable<IMethodSelectorToken> selectorParts, IEnumerable<MethodArgumentNode> arguments)
        {
            this.SelectorParts.Clear();
            this.Arguments.Clear();
            if (selectorParts != null)
                this.SelectorParts.AddRange(selectorParts);
            if (arguments != null)
                this.Arguments.AddRange(arguments);

            if (this.SelectorParts.Count == 0)
                throw new ArgumentException("Arguments selectorParts contains no elements");
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="primitive">Optional primitive API call to native code.</param>
        protected internal void SetContents(PrimitiveCallNode primitive)
        {
            this.Primitive = primitive; // null is OK here.
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
            if (this.Primitive != null)
                result.Add(this.Primitive);
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
            result.AddRange(this.SelectorParts.Cast<IToken>());
            if (this.LeftBar != null)
                result.Add(this.LeftBar);
            if (this.RightBar != null)
                result.Add(this.RightBar);
            return result;
        }

        /// <summary>
        /// Returns the method selector, i.e. the name of the method.
        /// </summary>
        public string Selector
        {
            get
            {
                // Performance optimization - most methods have only one selector part.
                if (this.SelectorParts.Count == 1)
                    return this.SelectorParts[0].Value;
                // Build selector
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (IMethodSelectorToken part in this.SelectorParts)
                    sb.Append(part.Value);
                return sb.ToString();
            }
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            return this.Selector;
        }
    }

    /// <summary>
    /// Argument node represents a method or a block argument
    /// </summary>
    public abstract class ArgumentNode : VariableNode
    {
        // Constant Binding - cannot be changed

        /// <summary>
        /// Create a new argument node.
        /// </summary>
        /// <param name="token">Identifier token containing the name of the argument.</param>
        protected ArgumentNode(IdentifierToken token)
        {
            this.Token = token;
        }
    }

    /// <summary>
    /// Method argument node represents an argument of a method.
    /// </summary>
    public partial class MethodArgumentNode : ArgumentNode
    {
        /// <summary>
        /// Method node that defines the method argument.
        /// </summary>
        public MethodNode Parent { get; private set; }

        /// <summary>
        /// Create a new method argument.
        /// </summary>
        /// <param name="parent">Method node that defines the method argument.</param>
        /// <param name="token">Identifier token containing the name of the argument.</param>
        protected internal MethodArgumentNode(MethodNode parent, IdentifierToken token)
            : base(token)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }
    }
}
