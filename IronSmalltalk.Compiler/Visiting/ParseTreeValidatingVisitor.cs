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
    /// This class visits a parse tree node and validates if it is valid.
    /// A valid parse tree node follows the semantic rules defined in the X3J20 section 3.4.
    /// An invalid parse tree node is usually do to incomplete or somehow invalid source code;
    /// We do allow parsing of of invalid source code and return impartial nodes for use in
    /// development tools (e.g. dynamic code highlightning), but obviously such incomplete
    /// parse tree node cannot be used for generating runtime code - this is what this class tells us.
    /// </summary>
    public class ParseTreeValidatingVisitor : ParseTreeVisitor<bool>
    {
        /// <summary>
        /// Default instance of the validating visitor.
        /// </summary>
        public static readonly ParseTreeValidatingVisitor Current = new ParseTreeValidatingVisitor(); 

        /// <summary>
        /// Visits the Semantic Node. This is the default visit, in case no other implementation.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitSemanticNode(SemanticNode node)
        {
            // Naive brute force implementation
            foreach (IParseNode child in node.GetChildNodes())
            {
                if (child is SemanticNode)
                {
                    if (!((SemanticNode)child).Accept(this))
                        return false;
                }
            }

            return true; 
        }

        #region 3.4.1 Functions

        /// <summary>
        /// Visits the Temporary Variable node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitTemporaryVariable(TemporaryVariableNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        #endregion

        #region 3.4.2 Methods

        /// <summary>
        /// Visits the Method node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitMethod(MethodNode node)
        {
            if ((node.LeftBar == null) ^ (node.RightBar == null))
                return false;

            foreach (MethodArgumentNode arg in node.Arguments)
            {
                if (!arg.Accept(this))
                    return false;
            }

            foreach (TemporaryVariableNode tmp in node.Temporaries)
            {
                if (!tmp.Accept(this))
                    return false;
            }

            if ((node.Primitive != null) && !node.Primitive.Accept(this))
                return false;

            if ((node.Statements != null) && !node.Statements.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Method Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitMethodArgument(MethodArgumentNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.Token == null)
                return false;

            return true; 
        }

        #endregion

        #region 3.4.3 Initializers (Expressions)

        /// <summary>
        /// Visits the Initializer (Expression) node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitInitializer(InitializerNode node)
        {
            if ((node.LeftBar == null) ^ (node.RightBar == null))
                return false;

            foreach (TemporaryVariableNode tmp in node.Temporaries)
            {
                if (!tmp.Accept(this))
                    return false;
            }

            if ((node.Statements != null) && !node.Statements.Accept(this))
                return false;

            return true; 
        }

        #endregion

        #region 3.4.4 Blocks

        /// <summary>
        /// Visits the Block node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBlock(BlockNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.LeftBracket == null) || (node.RightBracket == null))
                return false;
            if ((node.LeftBar == null) ^ (node.RightBar == null))
                return false;

            if ((node.Arguments.Count > 0) && (node.ArgumentsBar == null))
                return false;

            foreach (BlockArgumentNode arg in node.Arguments)
            {
                if (!arg.Accept(this))
                    return false;
            }

            foreach (TemporaryVariableNode tmp in node.Temporaries)
            {
                if (!tmp.Accept(this))
                    return false;
            }

            if ((node.Statements != null) && !node.Statements.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Block Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBlockArgument(BlockArgumentNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Colon == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        #endregion

        #region 3.4.5 Statements

        /// <summary>
        /// Visits the Statement Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitStatementSequence(StatementSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Expression == null) || !node.Expression.Accept(this))
                return false;
            if ((node.NextStatement != null) && !node.NextStatement.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Return Statement node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitReturnStatement(ReturnStatementNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.Token == null)
                return false;
            if ((node.Expression == null) || !node.Expression.Accept(this))
                return false;

            return true; 
        }

        #endregion

        #region 3.4.5.2 Expressions

        /// <summary>
        /// Visits the Assignment node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitAssignment(AssignmentNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.AssignmentOperator == null)
                return false;
            if ((node.Target == null) || !node.Target.Accept(this))
                return false;
            if ((node.Expression == null) || !node.Expression.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Basic Expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBasicExpression(BasicExpressionNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Primary == null) || !node.Primary.Accept(this))
                return false;
            if ((node.Messages != null) && !node.Messages.Accept(this))
                return false;
            if ((node.CascadeMessages != null) && !node.CascadeMessages.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Cascade Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitCascadeMessageSequence(CascadeMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.Semicolon == null)
                return false;
            if ((node.Messages == null) || !node.Messages.Accept(this))
                return false;
            if ((node.NextCascade != null) && !node.NextCascade.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Parenthesized Expression node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitParenthesizedExpression(ParenthesizedExpressionNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.LeftParenthesis == null) ^ (node.RightParenthesis == null))
                return false;
            if ((node.Expression == null) || !node.Expression.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Variable Reference (as oposite to declaraion) node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitVariableReferencele(VariableReferenceleNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Assignment Target node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitAssignmentTarget(AssignmentTargetNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        #endregion

        #region 3.4.5.3 Messages Sequences

        /// <summary>
        /// Visits the Unary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitUnaryMessageSequence(UnaryMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Message == null) || !node.Message.Accept(this))
                return false;

            if ((node.NextMessage != null) && !node.NextMessage.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Unary-Binary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitUnaryBinaryMessageSequence(UnaryBinaryMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Message == null) || !node.Message.Accept(this))
                return false;

            if ((node.NextMessage != null) && !node.NextMessage.Accept(this))
                return false;
            
            return true; 
        }

        /// <summary>
        /// Visits the Unary-Binary-Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitUnaryBinaryKeywordMessageSequence(UnaryBinaryKeywordMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Message == null) || !node.Message.Accept(this))
                return false;

            if ((node.NextMessage != null) && !node.NextMessage.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Binary Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBinaryMessageSequence(BinaryMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Message == null) || !node.Message.Accept(this))
                return false;

            if ((node.NextMessage != null) && !node.NextMessage.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Binary-Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBinaryKeywordMessageSequence(BinaryKeywordMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Message == null) || !node.Message.Accept(this))
                return false;

            if ((node.NextMessage != null) && !node.NextMessage.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Keyword Message Sequence node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitKeywordMessageSequence(KeywordMessageSequenceNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Message == null) || !node.Message.Accept(this))
                return false;

            return true; 
        }

        #endregion

        #region 3.4.5.3 Messages

        /// <summary>
        /// Visits the Unary Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitUnaryMessage(UnaryMessageNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.SelectorToken == null)
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Binary Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBinaryMessage(BinaryMessageNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.SelectorToken == null)
                return false;
            if ((node.Argument == null) || !node.Argument.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Keyword Message node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitKeywordMessage(KeywordMessageNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.Arguments.Count < 1)
                return false;
            if (node.Arguments.Count != node.SelectorTokens.Count)
                return false;
            foreach (KeywordArgumentNode arg in node.Arguments)
            {
                if (!arg.Accept(this))
                    return false;
            }

            return true; 
        }

        /// <summary>
        /// Visits the Binary Message Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitBinaryArgument(BinaryArgumentNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Primary == null) || !node.Primary.Accept(this))
                return false;
            if ((node.Messages != null) && !node.Messages.Accept(this))
                return false;

            return true; 
        }

        /// <summary>
        /// Visits the Keyword Message Argument node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitKeywordArgument(KeywordArgumentNode node)
        {
            if (node.Parent == null)
                return false;

            if ((node.Primary == null) || !node.Primary.Accept(this))
                return false;
            if ((node.Messages != null) && !node.Messages.Accept(this))
                return false;
            
            return true; 
        }

        #endregion

        #region 3.4.6 Literals

        /// <summary>
        /// Visits the Small Integer Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitSmallIntegerLiteral(SmallIntegerLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Large Integer Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitLargeIntegerLiteral(LargeIntegerLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Float Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitFloatELiteral(FloatELiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Float Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitFloatDLiteral(FloatDLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Scaled Decimal Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitScaledDecimalLiteral(ScaledDecimalLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Character Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitCharacterLiteral(CharacterLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Identifier Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitIdentifierLiteral(IdentifierLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Selector Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitSelectorLiteral(SelectorLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the String Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitStringLiteral(StringLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Symbol Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitSymbolLiteral(SymbolLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if (node.Token == null)
                return false;
            return true; 
        }

        /// <summary>
        /// Visits the Array Literal node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitArrayLiteral(ArrayLiteralNode node)
        {
            if (node.Parent == null)
                return false;
            if ((node.ArrayToken == null) || (node.LeftParenthesis == null) || (node.RightParenthesis == null))
                return false;

            foreach (LiteralNode item in node.Elements)
            {
                if (!item.Accept(this))
                    return false;
            }

            return true; 
        }

        #endregion

        /// <summary>
        /// Visits the Primitive Call node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitPrimitiveCall(PrimitiveCallNode node)
        {
            if (node.Parent == null)
                return false;

            if (node.ApiConvention == null)
                return false;
            if ((node.OpeningDelimiter == null) || (node.ClosingDelimiter == null))
                return false;

            return true; 
        }
    }
}
