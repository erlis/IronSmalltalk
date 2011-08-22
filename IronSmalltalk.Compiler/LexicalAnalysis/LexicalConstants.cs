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
    /// Lexical constants, such as characters and strings,
    /// mostly defined in X3J20 "3.5 Lexical Grammar".
    /// </summary>
    internal static class LexicalConstants
    {
        // 3.5.1 Character Categories
        public const char FirstLowercaseLetter = 'a';
        public const char LastLowercaseLetter = 'z';
        public const char FirstUppercaseLetter = 'A';
        public const char LastUppercaseLetter = 'Z';
        public const char NonCaseLetter = '_';

        // 3.5.2 Comments
        public const char CommentDelimiter = '"';

        // 3.5.4 Keywords
        public const char KeywordPostfix = ':';

        // 3.5.5 Operators
        public const string BinaryCharacters = @"!%&*+,/<=>?@\~|-";
        public const char ReturnOperator = '^';
        public const char AssignmentOperatorCharacter1 = ':';
        public const char AssignmentOperatorCharacter2 = '=';
        public const string AssignmentOperator = ":=";

        // 3.5.6 Numbers
        public const string ExponentLetters = "edq";
        public const char ExponentLettersFloatE = 'e';
        public const char ExponentLettersFloatD = 'd';
        public const char ExponentLettersFloatQ = 'q';
        public const char RadixDelimiter = 'r';
        public const char MantissaDelimiter = '.';
        public const char ScaledDecimalScaleDelimiter = 's';
        public const char NegativeSign = '-';
        public const char FirstDigit = '0';
        public const char LastDigit = '9';
        public const char FirstLetterDigit = 'A';

        // 3.5.7 Quoted Character
        public const char CharacterDelimiter = '$';

        // 3.5.8 Quoted Strings
        public const char StringDelimiter = '\'';

        // 3.5.9 Hashed String
        public const char HashedStringDelimiter = '#';

        // 3.5.10 Quoted Selector
        public const char QuotedSelectorDelimiter = '#';

        // Others
        public const char VerticalBar = '|';
    }
}
