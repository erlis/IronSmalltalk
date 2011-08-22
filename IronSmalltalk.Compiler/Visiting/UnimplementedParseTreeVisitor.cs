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
using System.Text;
using IronSmalltalk.Compiler.SemanticNodes;

namespace IronSmalltalk.Compiler.Visiting
{
    /// <summary>
    /// Base class that implements the protocol for visiting
    /// the parse tree nodes defined in X3J20 section 3.4,
    /// i.e. the parse tree nodes for methods and initializers
    /// and that throws NotImplementedException for each method.
    /// This is usefull if only part of the parse tree needs to be
    /// visited and ensures the rest fails if accidently visited.
    /// </summary>
    public abstract class UnimplementedParseTreeVisitor<TResult> : IParseTreeVisitor<TResult>
    {
        /// <summary>
        /// Visits the Semantic Node. This is the default visit, in case no other implementation.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitSemanticNode(SemanticNode node)
        {
            throw new NotImplementedException();
        }

        #region 3.4.1 Functions

        /// <summary>
        /// Visits the Temporary Variable node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitTemporaryVariable(TemporaryVariableNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.2 Methods

        /// <summary>
        /// Visits the Method node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitMethod(MethodNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Method Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitMethodArgument(MethodArgumentNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.3 Initializers (Expressions)

        /// <summary>
        /// Visits the Initializer (Expression) node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitInitializer(InitializerNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.4 Blocks

        /// <summary>
        /// Visits the Block node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBlock(BlockNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Block Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBlockArgument(BlockArgumentNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.5 Statements

        /// <summary>
        /// Visits the Statement Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitStatementSequence(StatementSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Return Statement node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitReturnStatement(ReturnStatementNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.5.2 Expressions

        /// <summary>
        /// Visits the Assignment node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitAssignment(AssignmentNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Basic Expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBasicExpression(BasicExpressionNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Cascade Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitCascadeMessageSequence(CascadeMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Parenthesized Expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitParenthesizedExpression(ParenthesizedExpressionNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Variable Reference (as oposite to declaraion) node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitVariableReferencele(VariableReferenceleNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Assignment Target node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitAssignmentTarget(AssignmentTargetNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.5.3 Messages Sequences

        /// <summary>
        /// Visits the Unary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitUnaryMessageSequence(UnaryMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Unary-Binary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitUnaryBinaryMessageSequence(UnaryBinaryMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Unary-Binary-Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitUnaryBinaryKeywordMessageSequence(UnaryBinaryKeywordMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Binary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBinaryMessageSequence(BinaryMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Binary-Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBinaryKeywordMessageSequence(BinaryKeywordMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitKeywordMessageSequence(KeywordMessageSequenceNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.5.3 Messages

        /// <summary>
        /// Visits the Unary Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitUnaryMessage(UnaryMessageNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Binary Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBinaryMessage(BinaryMessageNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Keyword Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitKeywordMessage(KeywordMessageNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Binary Message Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitBinaryArgument(BinaryArgumentNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Keyword Message Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitKeywordArgument(KeywordArgumentNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 3.4.6 Literals

        /// <summary>
        /// Visits the Small Integer Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitSmallIntegerLiteral(SmallIntegerLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Large Integer Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitLargeIntegerLiteral(LargeIntegerLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Float Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitFloatELiteral(FloatELiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Float Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitFloatDLiteral(FloatDLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Scaled Decimal Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitScaledDecimalLiteral(ScaledDecimalLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Character Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitCharacterLiteral(CharacterLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Identifier Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitIdentifierLiteral(IdentifierLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Selector Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitSelectorLiteral(SelectorLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the String Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitStringLiteral(StringLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Symbol Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitSymbolLiteral(SymbolLiteralNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Visits the Array Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitArrayLiteral(ArrayLiteralNode node)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Visits the Primitive Call node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitPrimitiveCall(PrimitiveCallNode node)
        {
            throw new NotImplementedException();
        }
    }
}
