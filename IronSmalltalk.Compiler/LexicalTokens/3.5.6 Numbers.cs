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
using System.Numerics;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.LexicalTokens
{
    /// <summary>
    /// Base class for number tokens as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    /// <typeparam name="TValue">Value type for the literal, usually int or other numeric type.</typeparam>
    public abstract class NumberToken<TValue> : LiteralToken<TValue>, INumberToken
    {
        /// <summary>
        /// Create a new numeric token.
        /// </summary>
        /// <param name="value">Literal token value.</param>
        protected NumberToken(TValue value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Base class for integer tokens as described in X3J20 chapter "3.5.6 Numbers".
    /// We've split integer into small and large integers due to .Net implementation issues.
    /// </summary>
    /// <typeparam name="TValue">Value type for the literal, int or other large integer type.</typeparam>
    public abstract class IntegerToken<TValue> : NumberToken<TValue>
    {
        /// <summary>
        /// Create a new literal token containing integer value.
        /// </summary>
        /// <param name="value">Literal token value.</param>
        public IntegerToken(TValue value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Small integer token as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    public class SmallIntegerToken : IntegerToken<int>
    {
        /// <summary>
        /// Creates a new small integer token.
        /// </summary>
        /// <param name="value">Value for the token.</param>
        public SmallIntegerToken(int value)
            : base(value)
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return this.Value.ToString("D", System.Globalization.CultureInfo.InvariantCulture); }
        }
    }

    /// <summary>
    /// Large integer token as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    public class LargeIntegerToken : IntegerToken<BigInteger>
    {
        /// <summary>
        /// Creates a new large integer token.
        /// </summary>
        /// <param name="value">Value for the token.</param>
        public LargeIntegerToken(BigInteger value)
            : base(value)
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return this.Value.ToString("D", System.Globalization.CultureInfo.InvariantCulture); }
        }
    }

    /// <summary>
    /// Floating point literal token as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    public abstract class FloatToken<TValue> : NumberToken<TValue>
    {
        /// <summary>
        /// Creates a new float token.
        /// </summary>
        /// <param name="value">Value for the token.</param>
        protected FloatToken(TValue value)
            : base(value)
        {
        }

    }

    /// <summary>
    /// Floating point FloatE literal token as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    public class FloatEToken : FloatToken<float>
    {
        /// <summary>
        /// Creates a new float token.
        /// </summary>
        /// <param name="value">Value for the token.</param>
        public FloatEToken(float value)
            : base(value)
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get
            {
                return this.Value.ToString("", System.Globalization.CultureInfo.InvariantCulture)
                    .Replace('E', 'e').Replace("e+", "e");
            }
        }
    }

    /// <summary>
    /// Floating point FloatD (or FloatQ) literal token as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    public class FloatDToken : FloatToken<double>
    {
        /// <summary>
        /// Creates a new float token.
        /// </summary>
        /// <param name="value">Value for the token.</param>
        public FloatDToken(double value)
            : base(value)
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get
            {
                return this.Value.ToString("", System.Globalization.CultureInfo.InvariantCulture)
                    .Replace('E', 'e').Replace("e+", "e").Replace('e', 'd');
            }
        }
    }

    /// <summary>
    /// Scaled decimal token as described in X3J20 chapter "3.5.6 Numbers"
    /// </summary>
    public class ScaledDecimalToken : NumberToken<BigDecimal>
    {
        /// <summary>
        /// Creates a new scaled decimal integer token.
        /// </summary>
        /// <param name="value">Value for the token.</param>
        public ScaledDecimalToken(BigDecimal value)
            : base(value)
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get
            {
                return this.Value.ToSourceString(); 
            }
        }
    }
}
