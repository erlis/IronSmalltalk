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
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// Return statement node as described in X3J20 chapter "3.4.5.1 Return statement".
    /// Return statements cannot be followed by other statements.
    /// </summary>
    public partial class ReturnStatementNode : StatementNode
    {
        /// <summary>
        /// The up-arrow return operator.
        /// </summary>
        public ReturnOperatorToken Token { get; private set; }

        /// <summary>
        /// Create and initialize a new return statement node.
        /// </summary>
        /// <param name="parent">The parent node that defines the statement.</param>
        /// <param name="token">The up-arrow return operator.</param>
        protected internal ReturnStatementNode(IStatementParentNode parent, ReturnOperatorToken token)
            : base(parent)
        {
#if DEBUG
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            this.Token = token;
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="expression">Expression node defining the statement.</param>
        /// <param name="period">Optional period that terminates a statement.</param>
        protected internal void SetContents(ExpressionNode expression, SpecialCharacterToken period)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            this.Expression = expression;
            this.Period = period; // Null is OK here.
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            List<IParseNode> result = new List<IParseNode>();
            if (this.Expression != null) // In case of bad source code
                result.Add(this.Expression);
            return result;
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
            if (this.Period != null)
                result.Add(this.Period);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            if (this.Expression == null)
            {
                return "^?expression?";
            }
            else
            {
                if (this.Period == null)
                    return "^" + this.Expression.PrintString();
                else
                    return "^" + this.Expression.PrintString() + ".";
            }
        }
    }
}
