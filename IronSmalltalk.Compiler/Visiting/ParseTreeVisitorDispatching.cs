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
using IronSmalltalk.Compiler.Visiting;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    // ********************************************
    // *** File with partial classes that implement
    // *** the Parse-Tree-Visitor methods for
    // *** the Semantic Nodes (X3J20 3.4.x).
    // *** Moved here for logistical reasons.
    // ********************************************

    partial interface IPrimaryNode
    {
        TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor);
    }

    partial class SemanticNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public virtual TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitSemanticNode(this);
        }
    }

    #region 3.4.1 Functions

    partial class TemporaryVariableNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitTemporaryVariable(this);
        }
    }

    #endregion

    #region 3.4.2 Methods

    partial class MethodNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitMethod(this);
        }
    }

    partial class MethodArgumentNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitMethodArgument(this);
        }
    }

    #endregion

    #region 3.4.3 Initializers (Expressions)

    partial class InitializerNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitInitializer(this);
        }
    }

    #endregion

    #region 3.4.4 Blocks

    partial class BlockNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBlock(this);
        }
    }

    partial class BlockArgumentNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBlockArgument(this);
        }
    }

    #endregion

    #region 3.4.5 Statements

    partial class ReturnStatementNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitReturnStatement(this);
        }
    }

    partial class StatementSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitStatementSequence(this);
        }
    }

    #endregion

    #region 3.4.5.2 Expressions

    partial class AssignmentNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitAssignment(this);
        }
    }

    partial class ParenthesizedExpressionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitParenthesizedExpression(this);
        }
    }

    partial class BasicExpressionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBasicExpression(this);
        }
    }

    partial class VariableReferenceleNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitVariableReferencele(this);
        }
    }

    partial class AssignmentTargetNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitAssignmentTarget(this);
        }
    }

    partial class CascadeMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitCascadeMessageSequence(this);
        }
    }

    #endregion

    #region 3.4.5.3 Messages Sequences

    partial class BinaryArgumentNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBinaryArgument(this);
        }
    }

    partial class UnaryMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitUnaryMessageSequence(this);
        }
    }

    partial class KeywordArgumentNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitKeywordArgument(this);
        }
    }

    partial class UnaryBinaryMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitUnaryBinaryMessageSequence(this);
        }
    }

    partial class BinaryMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBinaryMessageSequence(this);
        }
    }

    partial class UnaryBinaryKeywordMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitUnaryBinaryKeywordMessageSequence(this);
        }
    }

    partial class BinaryKeywordMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBinaryKeywordMessageSequence(this);
        }
    }

    partial class KeywordMessageSequenceNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitKeywordMessageSequence(this);
        }
    }

    #endregion

    #region 3.4.5.3 Messages

    partial class KeywordMessageNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitKeywordMessage(this);
        }
    }

    partial class BinaryMessageNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitBinaryMessage(this);
        }
    }

    partial class UnaryMessageNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitUnaryMessage(this);
        }
    }

    #endregion

    #region 3.4.6 Literals

    partial class LargeIntegerLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitLargeIntegerLiteral(this);
        }
    }

    partial class FloatELiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitFloatELiteral(this);
        }
    }

    partial class FloatDLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitFloatDLiteral(this);
        }
    }

    partial class ScaledDecimalLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitScaledDecimalLiteral(this);
        }
    }

    partial class SmallIntegerLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitSmallIntegerLiteral(this);
        }
    }

    partial class CharacterLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitCharacterLiteral(this);
        }
    }

    partial class StringLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitStringLiteral(this);
        }
    }

    partial class SymbolLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitSymbolLiteral(this);
        }
    }

    partial class SelectorLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitSelectorLiteral(this);
        }
    }

    partial class ArrayLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitArrayLiteral(this);
        }
    }

    partial class IdentifierLiteralNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitIdentifierLiteral(this);
        }
    }

    #endregion

    partial class PrimitiveCallNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitPrimitiveCall(this);
        }
    }
}
