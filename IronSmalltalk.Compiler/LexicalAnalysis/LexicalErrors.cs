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

namespace IronSmalltalk.Compiler.LexicalAnalysis
{
    /// <summary>
    /// Error messages the lexical scanner may report if it encounters illegal source code.
    /// </summary>
    internal static class LexicalErrors
    {
        public const string MissingClosingComment = "Missing closing comment \" quote. Hit EOF.";
        public const string InvalidAssignemtOperatorChar = "Invalid assignment operator character. Expected := .";
        public const string InvalidRadix = "Invalid radix {0}. Radix must be between 2 and 36.";
        public const string InvalidRadixInteger = "Invalid radix integer. No digits found after radix specifier.";
        public const string DigitTooBig = "Digit too big. For radix {0}, digits must be between 0 and {1}.";
        public const string InvalidFloatNoDecimalDigits = "Invalid float number. No digits found after decimal separator.";
        public const string InvalidFloatNoExponentDigits = "Invalid float. No digits found after exponent.";
        public const string MissingCharacter = "Missing character. Hit EOF.";
        public const string MissingClosingStringQuote = "Missing closing ' quote in string literal. Hit EOF.";
    }
}
