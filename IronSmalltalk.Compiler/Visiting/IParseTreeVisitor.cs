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
    /// Interface defining the protocol for visiting
    /// the parse tree nodes defined in X3J20 section 3.4,
    /// i.e. the parse tree nodes for methods and initializers.
    /// </summary>
    public interface IParseTreeVisitor<TResult>
    {
        /// <summary>
        /// Visits the Semantic Node. This is the default visit, in case no other implementation.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitSemanticNode(SemanticNode node);

        #region 3.4.1 Functions

        /// <summary>
        /// Visits the Temporary Variable node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitTemporaryVariable(TemporaryVariableNode node);

        #endregion
        
        #region 3.4.2 Methods

        /// <summary>
        /// Visits the Method node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitMethod(MethodNode node);

        /// <summary>
        /// Visits the Method Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitMethodArgument(MethodArgumentNode node);

        #endregion

        #region 3.4.3 Initializers (Expressions)

        /// <summary>
        /// Visits the Initializer (Expression) node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitInitializer(InitializerNode node);

        #endregion
        
        #region 3.4.4 Blocks

        /// <summary>
        /// Visits the Block node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBlock(BlockNode node);

        /// <summary>
        /// Visits the Block Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBlockArgument(BlockArgumentNode node);

        #endregion

        #region 3.4.5 Statements

        /// <summary>
        /// Visits the Statement Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitStatementSequence(StatementSequenceNode node);

        /// <summary>
        /// Visits the Return Statement node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitReturnStatement(ReturnStatementNode node);

        #endregion

        #region 3.4.5.2 Expressions

        /// <summary>
        /// Visits the Assignment node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitAssignment(AssignmentNode node);

        /// <summary>
        /// Visits the Basic Expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBasicExpression(BasicExpressionNode node);

        /// <summary>
        /// Visits the Cascade Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitCascadeMessageSequence(CascadeMessageSequenceNode node);

        /// <summary>
        /// Visits the Parenthesized Expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitParenthesizedExpression(ParenthesizedExpressionNode node);

        /// <summary>
        /// Visits the Variable Reference (as oposite to declaraion) node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitVariableReferencele(VariableReferenceleNode node);

        /// <summary>
        /// Visits the Assignment Target node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitAssignmentTarget(AssignmentTargetNode node);

        #endregion

        #region 3.4.5.3 Messages Sequences

        /// <summary>
        /// Visits the Unary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitUnaryMessageSequence(UnaryMessageSequenceNode node);

        /// <summary>
        /// Visits the Unary-Binary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitUnaryBinaryMessageSequence(UnaryBinaryMessageSequenceNode node);

        /// <summary>
        /// Visits the Unary-Binary-Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitUnaryBinaryKeywordMessageSequence(UnaryBinaryKeywordMessageSequenceNode node);

        /// <summary>
        /// Visits the Binary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBinaryMessageSequence(BinaryMessageSequenceNode node);

        /// <summary>
        /// Visits the Binary-Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBinaryKeywordMessageSequence(BinaryKeywordMessageSequenceNode node);

        /// <summary>
        /// Visits the Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitKeywordMessageSequence(KeywordMessageSequenceNode node);

        #endregion

        #region 3.4.5.3 Messages

        /// <summary>
        /// Visits the Unary Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitUnaryMessage(UnaryMessageNode node);

        /// <summary>
        /// Visits the Binary Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBinaryMessage(BinaryMessageNode node);

        /// <summary>
        /// Visits the Keyword Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitKeywordMessage(KeywordMessageNode node);

        /// <summary>
        /// Visits the Binary Message Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitBinaryArgument(BinaryArgumentNode node);

        /// <summary>
        /// Visits the Keyword Message Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitKeywordArgument(KeywordArgumentNode node);

        #endregion

        #region 3.4.6 Literals 

        /// <summary>
        /// Visits the Small Integer Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitSmallIntegerLiteral(SmallIntegerLiteralNode node);

        /// <summary>
        /// Visits the Large Integer Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitLargeIntegerLiteral(LargeIntegerLiteralNode node);

        /// <summary>
        /// Visits the FloatE Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitFloatELiteral(FloatELiteralNode node);

        /// <summary>
        /// Visits the FloatD Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitFloatDLiteral(FloatDLiteralNode node);

        /// <summary>
        /// Visits the Scaled Decimal Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitScaledDecimalLiteral(ScaledDecimalLiteralNode node);

        /// <summary>
        /// Visits the Character Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitCharacterLiteral(CharacterLiteralNode node);

        /// <summary>
        /// Visits the Identifier Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitIdentifierLiteral(IdentifierLiteralNode node);

        /// <summary>
        /// Visits the Selector Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitSelectorLiteral(SelectorLiteralNode node);

        /// <summary>
        /// Visits the String Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitStringLiteral(StringLiteralNode node);

        /// <summary>
        /// Visits the Symbol Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitSymbolLiteral(SymbolLiteralNode node);

        /// <summary>
        /// Visits the Array Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitArrayLiteral(ArrayLiteralNode node);

        #endregion

        /// <summary>
        /// Visits the Primitive Call node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitPrimitiveCall(PrimitiveCallNode node);

    }
}
