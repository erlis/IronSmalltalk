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
    /// Statements node as described in X3J20 chapter "3.4.5 Statements".
    /// </summary>
    public abstract class StatementNode : SemanticNode
    {
        /// <summary>
        /// The parent node that defines the statement. 
        /// That can be a function (method, initializer or a block) or another statement.
        /// </summary>
        public IStatementParentNode Parent { get; private set; }

        /// <summary>
        /// Optional period that terminates a statement.
        /// </summary>
        public SpecialCharacterToken Period { get; protected set; }

        /// <summary>
        /// Expression node defining the statement.
        /// Under normal circumstances, the expression is always set. 
        /// Only if the source code is illegal can expression be null.
        /// </summary>
        public ExpressionNode Expression { get; protected set; }

        /// <summary>
        /// Create a new statement node.
        /// </summary>
        /// <param name="parent">The parent node that defines the statement.</param>
        protected StatementNode(IStatementParentNode parent)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }
    }

    /// <summary>
    /// Interface for parse nodes that can parent a statement node.
    /// That can be a function (method, initializer or a block) or another statement.
    /// </summary>
    public interface IStatementParentNode : IParseNode
    {
    }

    /// <summary>
    /// The StatementSequenceNode represents statenent optionally followed by another statement.
    /// Those are defined in X3J20 as "statements ::= expression ['.' [statements]]".
    /// </summary>
    public partial class StatementSequenceNode : StatementNode, IStatementParentNode
    {
        /// <summary>
        /// Optional statement node that follows this statement.
        /// </summary>
        public StatementNode NextStatement { get; private set; }

        /// <summary>
        /// Create and initialize a new statement node.
        /// </summary>
        /// <param name="parent">Parent node that defines the statement.</param>
        protected internal StatementSequenceNode(IStatementParentNode parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="expression">Expression node defining the statement.</param>
        /// <param name="period">Optional period that terminates a statement.</param>
        /// <param name="nextStatement">Optional statement node that follows this statement.</param>
        protected internal void SetContents(ExpressionNode expression, SpecialCharacterToken period, StatementNode nextStatement)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            this.Expression = expression;
            this.Period = period; // Null is OK here.
            this.NextStatement = nextStatement; // Null is OK here.
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
            if (this.NextStatement != null)
                result.Add(this.NextStatement);
            return result;
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
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
                return "?expression?";
            }
            else
            {
                if (this.Period == null)
                    return this.Expression.PrintString();
                else
                    return this.Expression.PrintString() + ".";
            }
        }
    }
}
