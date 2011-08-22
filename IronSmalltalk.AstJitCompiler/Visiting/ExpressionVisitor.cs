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
using IronSmalltalk.Compiler.SemanticNodes;
using System.Linq.Expressions;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.AstJitCompiler.Visiting;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Behavior;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    public class ExpressionVisitor : NestedEncoderVisitor<Expression>
    {
        public bool IsConstant { get; private set; }

        public ExpressionVisitor(EncoderVisitor enclosingVisitor)
            : base(enclosingVisitor)
        {
            this.IsConstant = false;
        }

        public override Expression VisitParenthesizedExpression(ParenthesizedExpressionNode node)
        {
            ExpressionVisitor visitor = new ExpressionVisitor(this);
            Expression result = node.Expression.Accept(visitor);
            this.IsConstant = visitor.IsConstant;
            return result;
        }

        public override Expression VisitAssignment(AssignmentNode node)
        {
            NameBinding target = this.GetBinding(node.Target.Token.Value);
            if (target.IsErrorBinding)
                throw new BindingCodeGeneraionException(target, node);
            if (!(target is IAssignableBinding))
                throw new BindingCodeGeneraionException(CodeGenerationErrors.AssigningToConstant, node);
            
            ExpressionVisitor visitor = new ExpressionVisitor(this);
            Expression value = node.Expression.Accept(visitor);
            this.IsConstant = visitor.IsConstant;

            return ((IAssignableBinding)target).GenerateAssignExpression(value, this);
        }

        public override Expression VisitBasicExpression(BasicExpressionNode node)
        {
            // First, try to encode inline loop
            Expression result = this.EncodeInlinedLoop(node);
            if (result != null)
                return result;
            // X3J20 definition: basic_expression ::= primary [messages cascaded_messages].
            PrimaryVisitor visitor = new PrimaryVisitor(this);
            result = node.Primary.Accept(visitor);
            this.IsConstant = visitor.IsConstant;

            if (node.Messages != null)
            {
                MessageVisitor messageVisitor = new MessageVisitor(this, result, visitor.IsSuperSend, this.IsConstant, (node.CascadeMessages != null));
                result = node.Messages.Accept(messageVisitor);
                this.IsConstant = false;    // After first message send, we are no longer a constant

                // Cascade messages ... this is handled by telling the message-visitor above that we have cascade messages 
                // and need the receiver for cascade messages. The message-visitor has created a temporary variable for us 
                // that contains the result of the next-last message (i.e. the receiver for cascade messages). It has
                // also noted if the send for that receiver was a super-send and if it is constant (for optimization).
                CascadeMessageSequenceNode cascade = node.CascadeMessages;
                if (cascade != null)
                {
                    List<Expression> cascadeExpressions = new List<Expression>();
                    cascadeExpressions.Add(result);
                    while (cascade != null)
                    {
                        // Loop and process each cascade in the cascade chain. There is nothing special here.
                        // The receiver, self/super send etc. is determined from the visitor of the original message send.
                        // We create new message-visitor for each cascade iteration, which has fresh receiver 
                        // and other parameters. The result of the cascade messages is added to an expression list,
                        // which is converted to expression block. The block fulfills the semantics that the last
                        // expression in the list caries the result of the evaluation (same as required for sacsades).
                        MessageVisitor cascadeVisitor = new MessageVisitor(this,
                            messageVisitor.CascadeReceiver, messageVisitor.CascadeSuperSend, messageVisitor.CascadeConstantReceiver, false);
                        cascadeExpressions.Add(cascade.Messages.Accept(cascadeVisitor));
                        cascade = cascade.NextCascade;
                    }
                    result = Expression.Block(typeof(object), new ParameterExpression[] { messageVisitor.CascadeReceiver }, cascadeExpressions);
                }
            }
            else if (visitor.IsSuperSend)
            {
                throw new SemanticCodeGenerationException(CodeGenerationErrors.SuperNotFollowedByMessage, node);
            }

            return result;
        }

        private Expression EncodeInlinedLoop(BasicExpressionNode node)
        {
            if (node.CascadeMessages != null)
                return null;
            if (node.Messages == null)
                return null;
            BlockNode conditionBlock = node.Primary as BlockNode;
            if (conditionBlock == null)
                return null;
            if (conditionBlock.Arguments.Count != 0)
                return null;

            UnaryBinaryKeywordMessageSequenceNode unarySeq = node.Messages as UnaryBinaryKeywordMessageSequenceNode;
            if (unarySeq != null)
            {
                string selector = unarySeq.Message.SelectorToken.Value;
                if ((selector != "whileTrue") && (selector != "whileFalse"))
                    return null;
                return this.EncodeInlineWhile(conditionBlock, null, (selector == "whileFalse"));
            }
            KeywordMessageSequenceNode keywordSeq = node.Messages as KeywordMessageSequenceNode;
            if (keywordSeq != null)
            {
                if (keywordSeq.Message.SelectorTokens.Count != 1)
                    return null;
                string selector = keywordSeq.Message.SelectorTokens[0].Value;
                if ((selector != "whileTrue:") && (selector != "whileFalse:"))
                    return null;
                if (keywordSeq.Message.Arguments.Count != 1)
                    return null;
                KeywordArgumentNode arg = keywordSeq.Message.Arguments[0];
                if (arg.Messages != null)
                    return null;
                BlockNode valueBlock = arg.Primary as BlockNode;
                if (valueBlock == null)
                    return null;
                if (valueBlock.Arguments.Count != 0)
                    return null;
                return this.EncodeInlineWhile(conditionBlock, valueBlock, (selector == "whileFalse:"));
            }
            return null;
        }

        private Expression EncodeInlineWhile(BlockNode conditionBlock, BlockNode valueBlock, bool whileFalse)
        {
            NameBinding nilBinding = this.GetBinding(SemanticConstants.Nil);
            Expression conditionExpression = Expression.Convert(conditionBlock.Accept(new InlineBlockVisitor(this)), typeof(bool));
            // No, it's not an error ... if the <conditionExpression> evaluates to true, we terminate
            if (whileFalse)
                conditionExpression = Expression.IsTrue(conditionExpression);   
            else
                conditionExpression = Expression.IsFalse(conditionExpression);
            LabelTarget exitLabel = Expression.Label(typeof(object));
            GotoExpression exitLoop = Expression.Break(exitLabel, nilBinding.GenerateReadExpression(this));

            Expression loop;
            if (valueBlock == null)
                loop = Expression.IfThen(conditionExpression, exitLoop);
            else
                loop = Expression.IfThenElse(conditionExpression, exitLoop, valueBlock.Accept(new InlineBlockVisitor(this)));

            return Expression.Loop(loop, exitLabel);
        }
    }
}
