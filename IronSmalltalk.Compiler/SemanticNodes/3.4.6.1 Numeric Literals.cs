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
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// Parse node representing a numeric literal constant.
    /// </summary>
    /// <typeparam name="TToken">Type of the numeric value</typeparam>
    public abstract class NumericLiteralNode<TToken> : SingleValueLiteralNode<TToken>
        where TToken : INumberToken
    {
        /// <summary>
        /// Optional negative sign token indicating a negative numeric value.
        /// </summary>
        public NegativeSignToken NegativeSignToken { get; private set; }

        /// <summary>
        /// Create and initialize a new numeric literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        protected NumericLiteralNode(ILiteralNodeParent parent, TToken token, NegativeSignToken negativeSignToken)
            : base(parent, token)
        {
            this.NegativeSignToken = negativeSignToken; // OK with null.
        }


        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.NegativeSignToken != null)
                result.Add(this.NegativeSignToken);
            if (this.Token != null)
                result.Add(this.Token);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            if (this.NegativeSignToken == null)
                return base.PrintString();
            else
                return "-" + base.PrintString();
        }
    }

    /// <summary>
    /// Parse node representing a small integer literal constant.
    /// </summary>
    public partial class SmallIntegerLiteralNode : NumericLiteralNode<SmallIntegerToken>
    {
        /// <summary>
        /// Create and initialize a new small integer literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        public SmallIntegerLiteralNode(ILiteralNodeParent parent, SmallIntegerToken token, NegativeSignToken negativeSignToken)
            : base(parent, token, negativeSignToken)
        {
        }
    }

    /// <summary>
    /// Parse node representing a large integer literal constant.
    /// </summary>
    public partial class LargeIntegerLiteralNode : NumericLiteralNode<LargeIntegerToken>
    {
        /// <summary>
        /// Create and initialize a new large integer literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        protected internal LargeIntegerLiteralNode(ILiteralNodeParent parent, LargeIntegerToken token, NegativeSignToken negativeSignToken)
            : base(parent, token, negativeSignToken)
        {
        }
    }

    /// <summary>
    /// Parse node representing a float literal constant.
    /// </summary>
    public abstract partial class FloatLiteralNode<TToken> : NumericLiteralNode<TToken>
        where TToken : INumberToken
    {
        /// <summary>
        /// Create and initialize a new float literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        protected FloatLiteralNode(ILiteralNodeParent parent, TToken token, NegativeSignToken negativeSignToken)
            : base(parent, token, negativeSignToken)
        {
        }
    }

    /// <summary>
    /// Parse node representing a float-e literal constant.
    /// </summary>
    public partial class FloatELiteralNode : FloatLiteralNode<FloatEToken>
    {
        /// <summary>
        /// Create and initialize a new float literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        protected internal FloatELiteralNode(ILiteralNodeParent parent, FloatEToken token, NegativeSignToken negativeSignToken)
            : base(parent, token, negativeSignToken)
        {
        }
    }

    /// <summary>
    /// Parse node representing a float literal constant.
    /// </summary>
    public partial class FloatDLiteralNode : FloatLiteralNode<FloatDToken>
    {
        /// <summary>
        /// Create and initialize a new float literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        protected internal FloatDLiteralNode(ILiteralNodeParent parent, FloatDToken token, NegativeSignToken negativeSignToken)
            : base(parent, token, negativeSignToken)
        {
        }
    }

    /// <summary>
    /// Parse node representing a scaled decimal literal constant.
    /// </summary>
    public partial class ScaledDecimalLiteralNode : NumericLiteralNode<ScaledDecimalToken>
    {
        /// <summary>
        /// Create and initialize a new scaled decimal literal node.
        /// </summary>
        /// <param name="parent">Parent node that defines this literal node.</param>
        /// <param name="token">Token defining the value of the literal node.</param>
        /// <param name="negativeSignToken">Optional negative sign token indicating a negative numeric value.</param>
        protected internal ScaledDecimalLiteralNode(ILiteralNodeParent parent, ScaledDecimalToken token, NegativeSignToken negativeSignToken)
            : base(parent, token, negativeSignToken)
        {
        }
    }
}
