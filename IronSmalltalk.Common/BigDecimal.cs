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
using System.Globalization;
using System.Numerics;
using System.Text;

namespace IronSmalltalk.Common
{
    /// <summary>
    /// Represents an arbitrarily large signed decimal.
    /// </summary>
    public struct BigDecimal: IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        /// <summary>
        /// Numerator containing the value of the decimal. 
        /// </summary>
        /// <example>
        /// The value 123.45600 has the numerator 12345600.
        /// </example>
        public readonly BigInteger Numerator;

        /// <summary>
        /// Denominator that is used in many math operations. It is 10^this.Scale (10 to the power of Scale).
        /// </summary>
        /// <example>
        /// The value 123.45600 has the denominator 100000.
        /// </example>
        public readonly BigInteger Denominator;

        /// <summary>
        /// An integer which represents the total number of digits used to represent 
        /// the fraction part of the decimal, including trailing zeroes.
        /// </summary>
        /// <example>
        /// The value 123.45600 has the scale 5. 
        /// The value 123 has the scale 0.
        /// The value 123.45s8 has the scale 8.
        /// </example>
        public readonly int Scale;

        /// <summary>
        /// The maximum number of fractional digits.
        /// </summary>
        /// <remarks>
        /// The implementation limit specified in X2J20 "3.6 Implementation Limits"
        /// is 30 fractional digits. Theoretically, we could use any, but we've 
        /// set the limit to 99.
        /// </remarks>
        public const int MaxScale = 99;

        /// <summary>
        /// Gets a value that represents the number 0 (zero) with a scale of 0.
        /// </summary>
        public static readonly BigDecimal Zero = new BigDecimal(0, 0);

        /// <summary>
        /// Gets a value that represents the number 1 (one) with a scale of 0.
        /// </summary>
        public static readonly BigDecimal One = new BigDecimal(1, 0);  

        /// <summary>
        /// Info: For some unexplained reason, we can't get the static constructor to run, 
        /// therefore we use explicit lazy initialization.
        /// </summary>
        private static BigInteger[] _CachedDenominators;

        /// <summary>
        /// Just some caches denominators to avoid calling too much BigInteger.Pow(10, scale).
        /// </summary>
        private static BigInteger[] CachedDenominators
        {
            get
            {
                if (BigDecimal._CachedDenominators == null)
                    BigDecimal._CachedDenominators = new BigInteger[] { 1, 10, 100, 1000, 10000, 100000 };
                return BigDecimal._CachedDenominators;
            }
        }

        /// <summary>
        /// Initializes a new instance of the BigDecimal structure using an integer value.
        /// </summary>
        /// <param name="value">An integer value.</param>
        public BigDecimal(int value)
            : this((BigInteger) value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BigDecimal structure using a BigInteger value.
        /// </summary>
        /// <param name="value">A BigInteger value.</param>
        public BigDecimal(BigInteger value)
            : this(value, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BigDecimal structure using a BigInteger numerator 
        /// value and a scale, which represents the number of digits in the denominator.
        /// The BigDecimal is capable of prepresenting the specified number of fractional digits.
        /// </summary>
        /// <param name="numerator">Numerator value.</param>
        /// <param name="scale">Number of denominator digits.</param>
        /// <example>
        /// new BigDecimal(1234500, 4) => 123.45s4
        /// </example>
        public BigDecimal(BigInteger numerator, int scale)
        {
            if (scale < 0)
                throw new ArgumentOutOfRangeException("scale");
            if (scale > BigDecimal.MaxScale)
                throw new ArgumentOutOfRangeException("scale");

            this.Numerator = numerator;
            this.Scale = scale;
            if (scale < BigDecimal.CachedDenominators.Length)
                this.Denominator = BigDecimal.CachedDenominators[scale];
            else
                this.Denominator = BigInteger.Pow(10, scale);
        }

        private BigDecimal(BigInteger numerator, BigInteger denominator, int scale)
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
            this.Scale = scale;
        }

        /// <summary>
        /// Initializes a new instance of the BigDecimal structure using a decimal value.
        /// </summary>
        /// <param name="value">A decimal value.</param>
        public BigDecimal(decimal value)
        {
            this.Scale = 0;
            decimal i = Decimal.Truncate(value);
            this.Numerator = new BigInteger(i);

            value = value - i;
            while (value != 0)
            {
                this.Scale++;
                value = value * 10;
                i = Decimal.Truncate(value);
                this.Numerator = this.Numerator * 10 + (int)i;
                value = value - i;
            }

            if (this.Scale > BigDecimal.MaxScale)
                throw new ArgumentOutOfRangeException();
            if (this.Scale < BigDecimal.CachedDenominators.Length)
                this.Denominator = BigDecimal.CachedDenominators[this.Scale];
            else
                this.Denominator = BigInteger.Pow(10, this.Scale);
        }

        /// <summary>
        /// Indicates whether the value of the current BigDecimal is BigDecimal.Zero.
        /// </summary>
        public bool IsZero
        {
            get { return this.Numerator.IsZero; }
        }

        /// <summary>
        /// Gets a number that indicates the sign (negative, positive, or zero) of the current BigDecimal.
        /// </summary>
        public int Sign
        {
            get { return this.Numerator.Sign; }
        }

        /// <summary>
        /// Indicates whether the value of the current BigDecimal is BigDecimal.One.
        /// </summary>
        public bool IsOne
        {
            get { return this.Numerator == this.Denominator; }
        }

        /// <summary>
        /// Converts the numeric value of the current BigDecimal object
        /// to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the current BigDecimal value.</returns>
        public override string ToString()
        {
            return this.ToSourceString();
        }

        /// <summary>
        /// Converts the numeric value of the current BigDecimal object
        /// to its equivalent string representation.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current BigDecimal value.</returns>
        public string ToString(IFormatProvider provider)
        {
            NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);
            BigInteger abs = BigInteger.Abs(this.Numerator);
            BigInteger digits;
            BigInteger integer = BigInteger.DivRem(abs, this.Denominator, out digits);
            
            StringBuilder str = new StringBuilder(100);
            if (this.Numerator.Sign < 0)
                str.Append(info.NegativeSign);
            str.Append(integer.ToString("R"));
            str.Append(info.NumberDecimalSeparator);
            string digitString = digits.ToString("R");
            for (int i = 0; i < this.Scale - digitString.Length; i++)
                str.Append('0');
            str.Append(digitString);
            str.Append('s');
            str.Append(this.Scale.ToString("D"));
            return str.ToString();
        }

        /// <summary>
        /// Converts the numeric value of the current BigDecimal object
        /// to its equivalent Smalltalk souce code string representation.
        /// </summary>
        /// <returns>The source code string representation of the current BigDecimal value.</returns>
        public string ToSourceString()
        {
            BigInteger abs = BigInteger.Abs(this.Numerator);
            BigInteger digits;
            BigInteger integer = BigInteger.DivRem(abs, this.Denominator, out digits);

            StringBuilder str = new StringBuilder(100);
            if (this.Numerator.Sign < 0)
                str.Append('-');
            str.Append(integer.ToString("R"));
            str.Append('.');
            string digitString = digits.ToString("R");
            for (int i = 0; i < this.Scale - digitString.Length; i++)
                str.Append('0');
            str.Append(digitString);
            str.Append('s');
            str.Append(this.Scale.ToString("D"));
            return str.ToString();
        }

        /// <summary>
        /// Returns the hash code for the current BigDecimal object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            if (this.Scale == BigDecimal.MaxScale)
                return this.Numerator.GetHashCode();
            // Convert numerator to max-precision ...
            BigInteger n = this.Numerator * BigDecimal.Deniminator(BigDecimal.MaxScale - this.Scale);
            return n.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current decimal is equal to another object.
        /// </summary>
        /// <param name="obj">An object to compare with this decimal.</param>
        /// <returns>True if the current decimal is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is BigDecimal))
                return false;
            return this == (BigDecimal)obj;
        }

        /// <summary>
        /// Indicates whether the current decimal is equal to another decimal.
        /// </summary>
        /// <param name="other">A decimal to compare with this decimal.</param>
        /// <returns>True if the current decimal is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(BigDecimal other)
        {
            return (this == other);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: Value Meaning Less than zero
        /// This object is less than the other parameter.Zero This object is equal to
        /// other. Greater than zero This object is greater than other.
        /// </returns>
        public int CompareTo(BigDecimal other)
        {
            return BigDecimal.BinaryOperation(this, other, (a, b, s) => a.CompareTo(b));
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows,
        /// or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings: Value Meaning Less than zero This instance
        /// is less than obj. Zero This instance is equal to obj. Greater than zero This
        /// instance is greater than obj.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is BigDecimal))
                throw new ArgumentException("Must be BigDecimal");
            return this.CompareTo((BigDecimal)obj);
        }

        /// <summary>
        /// Converts a BigDecimal to another BigDecimal with the given scale.
        /// </summary>
        /// <param name="scale">Number of denominator digits.</param>
        /// <returns>A BigDecimal which has the same value as this BigDecimal but with the given scale.</returns>
        public BigDecimal ToScale(int scale)
        {
            if (scale < 0)
                throw new ArgumentOutOfRangeException("scale");
            if (scale > BigDecimal.MaxScale)
                throw new ArgumentOutOfRangeException("scale");

            if (scale < this.Scale)
            {
                int m = this.Scale - scale;
                BigInteger multiplier;
                if (m < BigDecimal.CachedDenominators.Length)
                    multiplier = BigDecimal.CachedDenominators[m];
                else
                    multiplier = BigInteger.Pow(10, m);
                // NB: Not 100% sure how to handle rounding here. Examples:
                // -1.26s asScaledDecimal: 1 => -1.3s   OK
                // -1.25s asScaledDecimal: 1 => -1.2s   ISSUE! Rounds up to -1.2, but should we insted round to -1.3 to have symetric values?
                // -1.24s asScaledDecimal: 1 => -1.2s   OK
                // 1.24s asScaledDecimal: 1 => 1.2s     OK
                // 1.25s asScaledDecimal: 1 => 1.3s     OK, round up 1.3
                // 1.26s asScaledDecimal: 1 => 1.3s     OK 
                return BigDecimal.DivideRounded(this.Numerator, multiplier, scale);
            }
            else if (scale > this.Scale)
            {
                int m = scale - this.Scale;
                BigInteger multiplier;
                if (m < BigDecimal.CachedDenominators.Length)
                    multiplier = BigDecimal.CachedDenominators[m];
                else
                    multiplier = BigInteger.Pow(10, m);
                return new BigDecimal(this.Numerator * multiplier, scale);
            }
            return this;
        }

        #region Operations

        /// <summary>
        /// Performs a binary operation between two big decimals.
        /// </summary>
        /// <typeparam name="TResult">Type of the result of the binary operation.</typeparam>
        /// <param name="a">First operand of the binary operation.</param>
        /// <param name="b">Second operand of the binary operation.</param>
        /// <param name="op">Binary operation lambda function.</param>
        /// <returns>The value of the binary operation.</returns>
        /// <remarks>
        /// This function first converts both decimals to the same precision. 
        /// The operand with the highest precision dictates the outcoming precision.
        /// The lambda function is passed the numerators (and not decimals themself) 
        /// of the operands as well as the scale (precision). The numerators
        /// are of same precision and can be used directly for any operations.
        /// </remarks>
        private static TResult BinaryOperation<TResult>(BigDecimal a, BigDecimal b, Func<BigInteger, BigInteger, int, TResult> op)
        {
            if (a.Scale == b.Scale)
                return op(a.Numerator, b.Numerator, a.Scale);

            if (a.Scale < b.Scale)
            {
                int scale = b.Scale - a.Scale;
                BigInteger multiplier;
                if (scale < BigDecimal.CachedDenominators.Length)
                    multiplier = BigDecimal.CachedDenominators[scale];
                else
                    multiplier = BigInteger.Pow(10, scale);
                return op(a.Numerator * multiplier, b.Numerator, b.Scale);
            }
            else
            {
                int scale = a.Scale - b.Scale;
                BigInteger multiplier;
                if (scale < BigDecimal.CachedDenominators.Length)
                    multiplier = BigDecimal.CachedDenominators[scale];
                else
                    multiplier = BigInteger.Pow(10, scale);
                return op(a.Numerator, b.Numerator * multiplier, a.Scale);
            }
        }

        /// <summary>
        /// Performs a binary operation between two big decimals.
        /// </summary>
        /// <typeparam name="TResult">Type of the result of the binary operation.</typeparam>
        /// <param name="a">First operand of the binary operation.</param>
        /// <param name="b">Second operand of the binary operation.</param>
        /// <param name="op">Binary operation lambda function.</param>
        /// <returns>The value of the binary operation.</returns>
        /// <remarks>
        /// This function first converts both decimals to the same precision. 
        /// The operand with the highest precision dictates the outcoming precision.
        /// The lambda function is passed the numerators (and not decimals themself) 
        /// of the operands as well as the scale (precision). The numerators
        /// are of same precision and can be used directly for any operations.
        /// </remarks>
        private static TResult BinaryOperation<TResult>(BigDecimal a, BigDecimal b, Func<BigInteger, BigInteger, BigInteger, int, TResult> op)
        {
            return BigDecimal.BinaryOperation(a, b, delegate(BigInteger na, BigInteger nb, int s)
            {
                BigInteger d;
                if (s < BigDecimal.CachedDenominators.Length)
                    d = BigDecimal.CachedDenominators[s];
                else
                    d = BigInteger.Pow(10, s);
                return op(na, nb, d, s);
            });
        }

        private static BigInteger Deniminator(int scale)
        {
            if (scale < BigDecimal.CachedDenominators.Length)
                return BigDecimal.CachedDenominators[scale];
            else
                return BigInteger.Pow(10, scale);
        }

        /// <summary>
        /// Helper method we use when the numerator needs to be divided and the result to be rounded
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="divisor"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private static BigDecimal DivideRounded(BigInteger numerator, BigInteger divisor, int scale)
        {
            // Must handle rounding correctly, otherwise we get wrong rounding, e.g.: 0.5s * 0.5s => 0.2s (correct is 0.3s ... for 0.25s)
            // Rounding is up, so 0.5 => 1.0, -0.5 => 0.0
            BigInteger reminder;
            numerator = BigInteger.DivRem(numerator, divisor, out reminder);
            if (divisor.Sign < 0)
            {
                if ((reminder.Sign > 0) && ((-reminder << 1) < divisor))
                    numerator--;
                else if ((reminder.Sign < 0) && ((reminder << 1) <= divisor))
                    numerator++;
            }
            else
            {
                if ((reminder.Sign > 0) && ((reminder << 1) >= divisor))
                    numerator++;
                else if ((reminder.Sign < 0) && ((-reminder << 1) > divisor))
                    numerator--;
            }
            return new BigDecimal(numerator, scale);
        }

        /// <summary>
        /// Adds the values of two specified BigDecimal values.
        /// </summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The sum of left and right.</returns>
        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => new BigDecimal(a + b, s));
        }

        /// <summary>
        /// Decrements a BigDecimal value by 1.
        /// </summary>
        /// <param name="value">The value to decrement.</param>
        /// <returns>The value of the value parameter decremented by 1.</returns>
        public static BigDecimal operator --(BigDecimal value)
        {
            return value - BigDecimal.One;
        }

        /// <summary>
        /// Divides a specified BigDecimal value by another specified BigDecimal value.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <returns>The decimal result of the division.</returns>
        /// <remarks>The division rounds the value, example: 0.5s * 0.5s => 0.3s .... -0.5s * 0.5s => -0.2s</remarks>
        public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
        {
            return BigDecimal.BinaryOperation(dividend, divisor, (a, b, d, s) => BigDecimal.DivideRounded(a * d, b, s));
        }

        /// <summary>
        /// Returns a value that indicates whether the values of two BigDecimal values are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if the left and right parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => a == b);
        }

        //public static bool operator ==(BigDecimal left, Int64 right)
        //{
        //}

        //public static bool operator ==(Int64 left, BigDecimal right)
        //{
        //}

        //public static bool operator ==(BigDecimal left, UInt64 right)
        //{
        //}

        //public static bool operator ==(UInt64 left, BigDecimal right)
        //{
        //}

        /// <summary>
        /// Returns a value that indicates whether a BigDecimal value is greater than another BigDecimal value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is greater than right; otherwise, false.</returns>
        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            if (left.Sign > right.Sign)
                return true;
            if (left.Sign < right.Sign)
                return false;
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => a > b);
        }

        //public static bool operator >(BigDecimal left, Int64 right)
        //{
        //}

        //public static bool operator >(Int64 left, BigDecimal right)
        //{
        //}

        //public static bool operator >(BigDecimal left, UInt64 right)
        //{
        //}

        //public static bool operator >(UInt64 left, BigDecimal right)
        //{
        //}

        /// <summary>
        /// Returns a value that indicates whether a BigDecimal value is greater than or equal to another BigDecimal value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is greater than right; otherwise, false.</returns>
        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            if (left.Sign > right.Sign)
                return true;
            if (left.Sign < right.Sign)
                return false;
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => a >= b);
        }

        //public static bool operator >=(BigDecimal left, Int64 right)
        //{
        //}

        //public static bool operator >=(Int64 left, BigDecimal right)
        //{
        //}

        //public static bool operator >=(BigDecimal left, UInt64 right)
        //{
        //}

        //public static bool operator >=(UInt64 left, BigDecimal right)
        //{
        //}

        /// <summary>
        /// Returns a value that indicates whether two BigDecimal have different values.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left and right are not equal; otherwise, false.</returns>
        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            if (left.Sign != right.Sign)
                return true;
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => a != b);
        }

        //public static bool operator !=(BigDecimal left, Int64 right)
        //{
        //}

        //public static bool operator !=(Int64 left, BigDecimal right)
        //{
        //}

        //public static bool operator !=(BigDecimal left, UInt64 right)
        //{
        //}

        //public static bool operator !=(UInt64 left, BigDecimal right)
        //{
        //}

        /// <summary>
        /// Returns a value that indicates whether a BigDecimal value is less than another BigDecimal value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is less than right; otherwise, false.</returns>
        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            if (left.Sign < right.Sign)
                return true;
            if (left.Sign > right.Sign)
                return false;
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => a <= b);
        }

        //public static bool operator <(BigDecimal left, Int64 right)
        //{
        //}

        //public static bool operator <(Int64 left, BigDecimal right)
        //{
        //}

        //public static bool operator <(BigDecimal left, UInt64 right)
        //{
        //}

        //public static bool operator <(UInt64 left, BigDecimal right)
        //{
        //}

        /// <summary>
        /// Returns a value that indicates whether a BigDecimal value is less than or equal to another BigDecimal value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is less than or equal to right; otherwise, false.</returns>
        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            if (left.Sign < right.Sign)
                return true;
            if (left.Sign > right.Sign)
                return false;
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => a <= b);
        }

        //public static bool operator <=(BigDecimal left, Int64 right)
        //{
        //}

        //public static bool operator <=(Int64 left, BigDecimal right)
        //{
        //}

        //public static bool operator <=(BigDecimal left, UInt64 right)
        //{
        //}

        //public static bool operator <=(UInt64 left, BigDecimal right)
        //{
        //}

        /// <summary>
        /// Multiplies two specified BigDecimal values.
        /// </summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The product of left and right.</returns>
        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            if (left.IsZero || right.IsZero)
                return new BigDecimal(BigInteger.Zero, Math.Max(left.Scale, right.Scale));
            return BigDecimal.BinaryOperation(left, right, (a, b, d, s) => BigDecimal.DivideRounded(a * b, d, s));
        }

        /// <summary>
        /// Returns the remainder that results from division with two specified BigDecimal values.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <returns>The remainder that results from the division.</returns>
        public static BigDecimal operator %(BigDecimal dividend, BigDecimal divisor)
        {
            return BigDecimal.BinaryOperation(dividend, divisor, (a, b, s) => new BigDecimal(a % b, s));
        }

        //public static BigDecimal operator ~(BigDecimal value)
        //{
        //}

        /// <summary>
        /// Subtracts a BigDecimal value from another BigDecimal value.
        /// </summary>
        /// <param name="left">The value to subtract from (the minuend).</param>
        /// <param name="right">The value to subtract (the subtrahend).</param>
        /// <returns>The result of subtracting right from left.</returns>
        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            return BigDecimal.BinaryOperation(left, right, (a, b, s) => new BigDecimal(a - b, s));
        }

        /// <summary>
        /// Negates a specified BigDecimal value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of the value parameter multiplied by negative one (-1).</returns>
        public static BigDecimal operator -(BigDecimal value)
        {
            return new BigDecimal(-value.Numerator, value.Scale);
        }

        /// <summary>
        /// Increments a BigDecimal value by 1.
        /// </summary>
        /// <param name="value">The value to increment.</param>
        /// <returns>The value of the value parameter incremented by 1.</returns>
        public static BigDecimal operator ++(BigDecimal value)
        {
            return value + BigDecimal.One;
        }

        #endregion

        #region Conversion 

        #region Implicit Conversions

        /// <summary>
        /// Defines an implicit conversion of an unsigned byte to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(byte value)
        {
            return new BigDecimal(value);
        }

        /// <summary>
        /// Defines an implicit conversion of 16-bit signed integer to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(short value)
        {
            return new BigDecimal(value);
        }

        /// <summary>
        /// Defines an implicit conversion of 32-bit signed integer to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(int value)
        {
            return new BigDecimal(value);
        }

        /// <summary>
        /// Defines an implicit conversion of 64-bit signed integer to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(long value)
        {
            return new BigDecimal((BigInteger) value);
        }

        /// <summary>
        /// Defines an implicit conversion of a signed byte to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(sbyte value)
        {
            return new BigDecimal(value);
        }

        /// <summary>
        /// Defines an implicit conversion of 16-bit unsigned integer to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(ushort value)
        {
            return new BigDecimal(value);
        }

        /// <summary>
        /// Defines an implicit conversion of 32-bit unsigned integer to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(uint value)
        {
            return new BigDecimal((BigInteger)value);
        }

        /// <summary>
        /// Defines an implicit conversion of 64-bit unsigned integer to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(ulong value)
        {
            return new BigDecimal((BigInteger)value);
        }

        /// <summary>
        /// Defines an implicit conversion of decimal value to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(decimal value)
        {
            return new BigDecimal(value);
        }

        /// <summary>
        /// Defines an implicit conversion of BigInteger value to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static implicit operator BigDecimal(BigInteger value)
        {
            return new BigDecimal(value);
        }

        #endregion

        #region Explicit Conversions

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a decimal value.
        /// </summary>
        /// <param name="value">The value to convert to a decimal.</param>
        /// <returns>A decimal that contains the value of the value parameter.</returns>
        public static explicit operator decimal(BigDecimal value)
        {
            BigInteger i, r;
            i = BigInteger.DivRem(value.Numerator, value.Denominator, out r);
            // IntegerPart + FractionPart  ... FractionPart = Remainder / Denominator ... (rounding division)
            return ((decimal)i) + ((decimal)r) / ((decimal)value.Denominator);            
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a double-precision (64-bit) float value.
        /// </summary>
        /// <param name="value">The value to convert to a double.</param>
        /// <returns>A double that contains the value of the value parameter.</returns>
        public static explicit operator double(BigDecimal value)
        {
            BigInteger i, r;
            i = BigInteger.DivRem(value.Numerator, value.Denominator, out r);
            // IntegerPart + FractionPart  ... FractionPart = Remainder / Denominator ... (rounding division)
            return ((double)i) + ((double)r) / ((double)value.Denominator);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to an unsigned byte value.
        /// </summary>
        /// <param name="value">The value to convert to a byte.</param>
        /// <returns>A byte that contains the value of the value parameter.</returns>
        public static explicit operator byte(BigDecimal value)
        {
            return (byte)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a signed 16-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        public static explicit operator short(BigDecimal value)
        {
            return (short)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a signed 32-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        public static explicit operator int(BigDecimal value)
        {
            return (int)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a signed 64-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        public static explicit operator long(BigDecimal value)
        {
            return (long)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a signed byte value.
        /// </summary>
        /// <param name="value">The value to convert to a byte.</param>
        /// <returns>A byte that contains the value of the value parameter.</returns>
        public static explicit operator sbyte(BigDecimal value)
        {
            return (sbyte)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to an unsigned 16-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        public static explicit operator ushort(BigDecimal value)
        {
            return (ushort)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to an unsigned 32-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        public static explicit operator uint(BigDecimal value)
        {
            return (uint)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to an unsigned 64-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        public static explicit operator ulong(BigDecimal value)
        {
            return (ulong)((BigInteger)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a BigDecimal value to a signed System.Numerics.BigInteger value.
        /// </summary>
        /// <param name="value">The value to convert to an integer.</param>
        /// <returns>An integer that contains the value of the value parameter.</returns>
        /// <remarks>The return value is the integral part of the decimal value; fractional digits are truncated.</remarks>
        public static explicit operator BigInteger(BigDecimal value)
        {
            // We uses divI_t ... with truncation toward zero ... because System.Decimal does the same.
            return value.Numerator / value.Denominator;
        }

        /// <summary>
        /// Defines an explicit conversion of a single-precision (32-bit) float to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static explicit operator BigDecimal(float value)
        {
            return (BigDecimal)((double)value);
        }

        /// <summary>
        /// Defines an explicit conversion of a double-precision (64-bit) float to a BigDecimal value.
        /// </summary>
        /// <param name="value">The value to convert to a BigDecimal.</param>
        /// <returns>A BigDecimal that contains the value of the value parameter.</returns>
        public static explicit operator BigDecimal(double value)
        {
            double i = Math.Floor(value);
            double d = value - i;
            return (new BigDecimal((BigInteger)i)) + (new BigDecimal((decimal)d));
        }

        #endregion

        #endregion
    }
}
