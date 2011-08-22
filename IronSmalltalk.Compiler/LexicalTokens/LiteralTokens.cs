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

namespace IronSmalltalk.Compiler.LexicalTokens
{
    /// <summary>
    /// Base class for literal tokens. 
    /// </summary>
    /// <typeparam name="TValue">Value type for the literal, usually string, char, int or other numeric type.</typeparam>
    public abstract class LiteralToken<TValue> : Token, ILiteralToken
    {
        /// <summary>
        /// The value of the token.
        /// </summary>
        public TValue Value { get; protected set; }

        /// <summary>
        /// Creates and initializes a new literal token.
        /// </summary>
        /// <param name="value">Literal token value.</param>
        public LiteralToken(TValue value)
        {
            this.Value = value;
        }
    }

    /// <summary>
    /// Tokens that 'work well' with literal nodes.
    /// </summary>
    public interface ILiteralToken : IToken
    {
    }
}
