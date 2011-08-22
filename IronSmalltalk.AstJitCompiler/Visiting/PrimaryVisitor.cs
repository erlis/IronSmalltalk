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
using System.Linq.Expressions;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Behavior;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    public class PrimaryVisitor : NestedEncoderVisitor<Expression>
    {
        public bool IsSuperSend { get; private set; }
        public bool IsConstant { get; private set; }
 
        public PrimaryVisitor(EncoderVisitor enclosingVisitor)
            : base(enclosingVisitor)
        {
            this.IsConstant = false;
        }

        public override Expression VisitBlock(BlockNode node)
        {
            return node.Accept(new BlockVisitor(this));
        }

        public override Expression VisitParenthesizedExpression(ParenthesizedExpressionNode node)
        {
            return node.Expression.Accept(new ExpressionVisitor(this));
        }

        public override Expression VisitVariableReferencele(VariableReferenceleNode node)
        {
            NameBinding target = this.GetBinding(node.Token.Value);
            if (target.IsErrorBinding)
                throw new BindingCodeGeneraionException(target, node);
            this.IsSuperSend = (node.Token.Value == SemanticConstants.Super);
            this.IsConstant = target.IsConstantValueBinding;
            return target.GenerateReadExpression(this);
        }

        // Encode the literal to a constant expression
        #region Literals

        public override Expression VisitArrayLiteral(ArrayLiteralNode node)
        {
            LiteralVisitor visitor = new LiteralVisitor(this);
            object[] result = new object[node.Elements.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = node.Elements[i].Accept(visitor);
            this.IsConstant = true;
            return Expression.Constant(result, typeof(object));
        }

        public override Expression VisitCharacterLiteral(CharacterLiteralNode node)
        {
            this.IsConstant = true;
            return Expression.Constant(node.Token.Value, typeof(object));
        }

        public override Expression VisitFloatELiteral(FloatELiteralNode node)
        {
            this.IsConstant = true;
            if (node.NegativeSignToken == null)
                return Expression.Constant(node.Token.Value, typeof(object));
            else
                return Expression.Constant(-node.Token.Value, typeof(object));
        }

        public override Expression VisitFloatDLiteral(FloatDLiteralNode node)
        {
            this.IsConstant = true;
            if (node.NegativeSignToken == null)
                return Expression.Constant(node.Token.Value, typeof(object));
            else
                return Expression.Constant(-node.Token.Value, typeof(object));
        }

        public override Expression VisitIdentifierLiteral(IdentifierLiteralNode node)
        {
            this.IsConstant = true;
            return base.VisitIdentifierLiteral(node); // Only nested in arrays
        }

        public override Expression VisitLargeIntegerLiteral(LargeIntegerLiteralNode node)
        {
            this.IsConstant = true;
            if (node.NegativeSignToken == null)
                return Expression.Constant(node.Token.Value, typeof(object));
            else
                return Expression.Constant(-node.Token.Value, typeof(object));
        }

        public override Expression VisitScaledDecimalLiteral(ScaledDecimalLiteralNode node)
        {
            this.IsConstant = true;
            if (node.NegativeSignToken == null)
                return Expression.Constant(node.Token.Value, typeof(object));
            else
                return Expression.Constant(-node.Token.Value, typeof(object));
        }

        public override Expression VisitSelectorLiteral(SelectorLiteralNode node)
        {
            // #asUppercase or #with:with: 
            this.IsConstant = true;
            return Expression.Constant(this.RootVisitor.Runtime.GetSymbol(node.Token.Value), typeof(object));
        }

        public override Expression VisitSmallIntegerLiteral(SmallIntegerLiteralNode node)
        {
            this.IsConstant = true;
            if (node.NegativeSignToken == null)
                return Expression.Constant(node.Token.Value, typeof(object));
            else
                return Expression.Constant(-node.Token.Value, typeof(object));
        }

        public override Expression VisitStringLiteral(StringLiteralNode node)
        {
            this.IsConstant = true;
            return Expression.Constant(node.Token.Value, typeof(object));
        }

        public override Expression VisitSymbolLiteral(SymbolLiteralNode node)
        {
            // #'asUppercase' or #'this is a test'
            this.IsConstant = true;
            return Expression.Constant(this.RootVisitor.Runtime.GetSymbol(node.Token.Value), typeof(object));
        }

        #endregion
    }
}
