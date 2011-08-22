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
using IronSmalltalk.Common;
using IronSmalltalk.Compiler.LexicalAnalysis;

namespace IronSmalltalk.Compiler
{
    /// <summary>
    /// Helper class for literal conversion.
    /// </summary>
    public static class ConversionUtilities
    {
        public static int ConvertSmallInteger(string digits, out bool success)
        {
            int result;
            success = Int32.TryParse(digits, out result);
            return result;
        }

        public static BigInteger ConvertLargeInteger(string digits)
        {
            return ConversionUtilities.ParseBigInteger(digits, false);
        }

        public static int ConvertSmallInteger(string digits, int digitBase, out bool success)
        {
            if (digitBase == 10)
                return ConversionUtilities.ConvertSmallInteger(digits, out success);

            // Use the LargeInteger conversion, and see if it succeeds.
            BigInteger value = ConversionUtilities.ConvertLargeInteger(digits, digitBase);
            if ((value >= Int32.MinValue) && (value <= Int32.MaxValue))
            {
                success = true;
                return (int)value;
            }
            else
            {
                success = false;
                return 0;
            }
        }

        public static BigInteger ConvertLargeInteger(string digits, int digitBase)
        {
            if (digitBase == 10)
                return ConversionUtilities.ConvertLargeInteger(digits);
            // Hmm .... should we do base-16 ourself or pass to the CLR? May be CLR is faster.
            if (digitBase == 16)
                return ConversionUtilities.ParseBigInteger(digits, true);

            // Parse manually
            BigInteger value = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                char ch = digits[i];
                int digitValue;
                if ((ch >= LexicalConstants.FirstDigit) && (ch <= LexicalConstants.LastDigit))
                    digitValue = (int)(ch - LexicalConstants.FirstDigit);
                else
                    digitValue = (int)(ch - LexicalConstants.FirstLetterDigit);
                if (digitValue > digitBase)
                    throw new FormatException();
                value = (value * digitBase) + digitValue;
            }
            return value;
        }


        public static object ConvertFloat(string integerDigits, string decimalDigits, string exponentDigits, bool negativeExponent, char exponentLetter, out string errorMessage)
        {
#if DEBUG
            if ((LexicalConstants.ExponentLetters.IndexOf(exponentLetter) == -1) && (exponentLetter != '\0'))
                throw new InvalidScannerOperationException();
#endif
            // We use the lazy approach here - convert to string, let the .Net do the job.
            string str = String.Format("{0}.{1}e{2}{3}", integerDigits, decimalDigits, (negativeExponent ? "-" : ""), exponentDigits);

            if (exponentLetter == LexicalConstants.ExponentLettersFloatE)
            {
                float result;
                if (Single.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                    errorMessage = null;
                else
                    errorMessage = String.Format("Invalid float {0}.", str.Replace('e', exponentLetter));
                return result;
            }
            else if ((exponentLetter == LexicalConstants.ExponentLettersFloatD) || (exponentLetter == LexicalConstants.ExponentLettersFloatQ))
            {
                double result;
                if (Double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                    errorMessage = null; // OK
                else
                    errorMessage = String.Format("Invalid float {0}.", str.Replace('e', exponentLetter));
                return result;
            }
            else
            {
                // Must try our best here ...
                float floatE;
                double floatD;
                bool okE = Single.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out floatE);
                bool okD = Double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out floatD);
                if (okE & okD)
                {
                    // X3J20: 3.4.6.1. (page 23 first / second chapter)
                    errorMessage = null;
                    // NB: I find this X3J20 requirement stupid, I would rather return the double every time.
                    // Good for both ... return one with most precision
                    if (((double)floatE) == floatD)
                        return floatE;  // No precision loss ... return the smalles float
                    else
                        return floatD;  // Loss of precision ... return the one with the largest precision.
                }
                else if (okE)
                {
                    errorMessage = null;
                    return floatE;
                }
                else if (okD)
                {
                    errorMessage = null;
                    return floatD;
                }
                else
                {
                    errorMessage = String.Format("Invalid float {0}.", str);
                    return floatD; // Not used, but must return something.
                }
            }
        }

        /// <summary>
        /// Convert the given decimal part strings to a BigDecimal object.
        /// </summary>
        /// <param name="integerDigits">
        /// Integer digits string, e.g. for "12.34s5" this param contains "12".
        /// </param>
        /// <param name="decimalDigits">
        /// Optional. Decimal digits string, e.g. for "12.34s5" this param contains "34", for "12s5" is is null or empty str.
        /// </param>
        /// <param name="fractionalDigits">
        /// Optional. Fraction digits string, e.g. for "12.34s5" this param contains "5", for "12.34s" is is null or empty str.
        /// </param>
        /// <param name="errorMessage">Return error message/description.</param>
        /// <returns>Return BigDecimal object.</returns>
        public static BigDecimal ConvertScaledDecimal(string integerDigits, string decimalDigits, string fractionalDigits, out string errorMessage)
        {
            // NB: decimalDigits may be null. fractionalDigits may be null or empty.

            // NB: X3J20 says: "If fractionalDigits is present it specifies that the representation used
            // for the number must allow for a number of digits to the right of the decimal point that is equal to the
            // numeric value of fractionalDigits." As I read this, it must allow minimum number of digits, but
            // it is not required to truncate the number to the given fraction of digits if the mantissa contains
            // more digits than the fraction part. So we leave them alone.

            // 1. The integer part .... simple, just convert to BigInteger
            BigInteger integer;
            if (!ConversionUtilities.TryParseBigInteger(integerDigits, false, out integer))
                goto Error;

            // 2. The decimal part ... check for garbage and convert to BigInteger
            if (decimalDigits == null)
                decimalDigits = String.Empty;
            foreach(char ch in decimalDigits)
            {
                if ((ch < LexicalConstants.FirstDigit || ch > LexicalConstants.LastDigit))
                    goto Error;
            }
            BigInteger decimals = 0;
            if (decimalDigits.Length > 0)
                decimals = ConversionUtilities.ParseBigInteger(decimalDigits, false); // Should not fail ... we just checked for garbage

            // 3. The scale part ... check for garbage and convert to integer
            if (fractionalDigits == null)
                fractionalDigits = String.Empty;
            foreach(char ch in fractionalDigits)
            {
                if ((ch < LexicalConstants.FirstDigit || ch > LexicalConstants.LastDigit))
                    goto Error;
            }
            int scale = 0;
            if ((fractionalDigits.Length != 0) && !Int32.TryParse(fractionalDigits, out scale))
                goto Error;
            scale = Math.Max(scale, decimalDigits.Length); // X3J20 says minimum number of digits, but we allow more if explicitely given.
            if ((scale < 0) || (scale > BigDecimal.MaxScale))
                goto Error;

            // Convert things ...
            decimals = decimals * BigInteger.Pow(10, scale - decimalDigits.Length);
            BigInteger numerator = (integer * BigInteger.Pow(10, scale)) + decimals;

            errorMessage = null; // OK
            return new BigDecimal(numerator, scale);

            Error:
            errorMessage = String.Format("Invalid scaled decimal {0}{1}s{2}.", 
                integerDigits, 
                String.IsNullOrWhiteSpace(decimalDigits) ? "" : String.Format(".{0}", decimalDigits),
                fractionalDigits);
            return BigDecimal.Zero;
        }


        /// <summary>
        /// Parsing of BigIntegers.
        /// </summary>
        /// <param name="digits">Digits to parse</param>
        /// <param name="isHex">Are the digits base 16 or base 10?</param>
        /// <returns>The number value</returns>
        private static BigInteger ParseBigInteger(string digits, bool isHex)
        {
#if !SILVERLIGHT
            if (isHex)
                return BigInteger.Parse(digits, NumberStyles.HexNumber, null);
            else
                return BigInteger.Parse(digits);
#else
            // Silverlight doesn't have Parse() method, so we must do this by hand.
            BigInteger result;
            if (ConversionUtilities.TryParseBigInteger(digits, isHex, out result))
                return result;
            throw new FormatException();
#endif
        }

        /// <summary>
        /// Parsing of BigIntegers.
        /// </summary>
        /// <param name="digits">Digits to parse</param>
        /// <param name="isHex">Are the digits base 16 or base 10?</param>
        /// <param name="result">The number value.</param>
        /// <returns>True if successful, otherwise false.</returns>
        private static bool TryParseBigInteger(string digits, bool isHex, out BigInteger result)
        {
#if !SILVERLIGHT
            if (isHex)
                return BigInteger.TryParse(digits, NumberStyles.HexNumber, null, out result);
            else
                return BigInteger.TryParse(digits, out result);
#else
            // Silverlight doesn't have Parse() method, so we must do this by hand.
            // In here, we (hopefully) provide functionality similar to BigInteger.TryParse(...).
            // We handle exactly what the Parser and Compiler need and we are not 100% compatible.
            // Alg. can be improved ...
            result = BigInteger.Zero;
            if (digits == null)
                return false;
            NumberFormatInfo info = NumberFormatInfo.GetInstance(null);
            digits = digits.Trim(); // Remove whitespaces ... NumberStyles.AllowLeadingWhite and NumberStyles.AllowTrailingWhite
            if (digits.Length == 0)
                return false;
            // Handle NumberStyles.AllowLeadingSign logic
            bool negative = false;
            if (digits.StartsWith(info.NegativeSign))
            {
                negative = true;
                digits.Substring(info.NegativeSign.Length).Trim();
            }
            else if (digits.StartsWith(info.PositiveSign))
            {
                digits.Substring(info.PositiveSign.Length).Trim();
            }
            // Parse the number
            string allowedChars = isHex ? "0123456789ABCDEFabcdef" : "0123456789";
            BigInteger value = 0;
            foreach (char ch in digits)
            {
                string digit = ch.ToString();
                if (!allowedChars.Contains(digit))
                    return false;
                int num = isHex ? Int32.Parse(digit, NumberStyles.HexNumber, null) : Int32.Parse(digit);
                value = value * 10 + num;
            }

            if (negative)
                value = -value;
            result = value;
            return true;
#endif
        }
    }
}
