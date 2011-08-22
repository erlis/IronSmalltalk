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
    /// This partial class performs some lexical test on characters, 
    /// on definitions defined in "ANSI NCITS 319-1998 (R2007)" (X3J20).
    /// </summary>
    /// <remarks>
    /// The reason for having a seperate file is, in case there's a
    /// difference between .Net and the X3J20 definition, we only 
    /// have to change things here, and not go trough "business code" 
    /// in the compiler classes.
    /// </remarks>
    public partial class ScanResult
    {
        /// <summary>
        /// Test for [character], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsCharacter()
        {
            return !this.EndOfFile;
        }

        /// <summary>
        /// Test for [whitespace], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsWhitespace()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            if (this.EndOfFile)
                return false;

            return Char.IsWhiteSpace(this.Character);
        }

        /// <summary>
        /// Test for [digit], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsDigit()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character >= LexicalConstants.FirstDigit) && (this.Character <= LexicalConstants.LastDigit);
        }

        /// <summary>
        /// Test for [uppercaseAlphabetic], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsUppercaseAlphabetic()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character >= LexicalConstants.FirstUppercaseLetter) && (this.Character <= LexicalConstants.LastUppercaseLetter);
        }

        /// <summary>
        /// Test for [lowercaseAlphabetic], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsLowercaseAlphabetic()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character >= LexicalConstants.FirstLowercaseLetter) && (this.Character <= LexicalConstants.LastLowercaseLetter);
        }

        /// <summary>
        /// Test for [nonCaseLetter], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsNonCaseLetter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.NonCaseLetter);
        }

        /// <summary>
        /// Test for [letter], defined in "3.5.1 Character Categories".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsLetter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return Char.IsLetter(this.Character) || this.IsNonCaseLetter();
        }

        /// <summary>
        /// Test for [commentDelimiter], defined in "3.5.2 Comments".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsCommentDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.CommentDelimiter);
        }

        /// <summary>
        /// Test for [nonCommentDelimiter], defined in "3.5.2 Comments".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsNonCommentDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            if (this.EndOfFile)
                return false;
            return !this.IsCommentDelimiter();
        }

        /// <summary>
        /// Test for [binaryCharacter], defined in "3.5.5 Operators".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsBinaryCharacter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (LexicalConstants.BinaryCharacters.IndexOf(this.Character) != -1);
        }

        /// <summary>
        /// Test for [returnOperator], defined in "3.5.5 Operators".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsReturnOperator()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.ReturnOperator);
        }

        /// <summary>
        /// Test for [exponentLetter], defined in "3.5.6 Numbers".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsExponentLetter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (LexicalConstants.ExponentLetters.IndexOf(this.Character) != -1);
        }

        /// <summary>
        /// Test for [stringDelimiter], defined in "3.5.8 Quoted Strings".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsStringDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.StringDelimiter);
        }

        /// <summary>
        /// Test for character delimiter (prefix) as defined in "3.5.7 Quoted Character".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsCharacterDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.CharacterDelimiter);
        }

        /// <summary>
        /// Test for hashed string delimiter (prefix) as defined in "3.5.9 Hashed String".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsHashedStringDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.HashedStringDelimiter);
        }


        /// <summary>
        /// Test for quoted selector delimiter (prefix) as defined in "3.5.10 Quoted Selector".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsQuotedSelectorDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.QuotedSelectorDelimiter);
        }


        /// <summary>
        /// Test for first character in the assignment operator as defined in "3.5.5 Operators".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsAssignmentOperatorCharacter1()
        {
            // asignmentOperator ::= ':='
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.AssignmentOperatorCharacter1);
        }

        /// <summary>
        /// Test for second character in the assignment operator as defined in "3.5.5 Operators".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsAssignmentOperatorCharacter2()
        {
            // asignmentOperator ::= ':='
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.AssignmentOperatorCharacter2);
        }

        /// <summary>
        /// Test for the radix delimiter in integers, as defined in "3.5.6 Numbers".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsRadixDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.RadixDelimiter);
        }

        /// <summary>
        /// Test for the mantissa delimiter (float decimal separator), as defined in "3.5.6 Numbers".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsMantissaDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.MantissaDelimiter);
        }

        /// <summary>
        /// Test for the scaled decimals scale delimiter (the 's' in 12.34s4), as defined in "3.5.6 Numbers".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsScaledDecimalScaleDelimiter()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.ScaledDecimalScaleDelimiter);
        }

        /// <summary>
        /// Test for a decimal in the given numeric base.
        /// </summary>
        /// Base, between 2 and 36. 
        /// NB: For performance, we don't validate this param, so it better be correct!
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsNegativeSign()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.NegativeSign);
        }

        /// <summary>
        /// Test for a numeric negative sign.
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsDigitBase(int numericBase)
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            if (numericBase <= 10)
                return (this.Character >= LexicalConstants.FirstDigit) && (this.Character < (LexicalConstants.FirstDigit + numericBase));
            else
                // NB: X3J20 Standard only allows uppercase letters
                return ((this.Character >= LexicalConstants.FirstDigit) && (this.Character <= LexicalConstants.LastDigit))
                    || ((this.Character >= LexicalConstants.FirstLetterDigit) && (this.Character < (LexicalConstants.FirstLetterDigit + numericBase - 10)));
        }

        /// <summary>
        /// Test for the keyword postfix char, i.e. the ':' char at the end of keywords. See: "3.5.4 Keywords".
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsKeywordPostfix()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.KeywordPostfix);
        }

        /// <summary>
        /// Test for the vertical bar character.
        /// </summary>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public bool IsVerticalBar()
        {
            // NB: When, EOF, this.Character == '0x00', therefore it should return <false> here.
            return (this.Character == LexicalConstants.VerticalBar);
        }


        internal CharacterClassification Classify(Preference preference)
        {
            if (this.EndOfFile)
                return CharacterClassification.EndOfFile;
            if (this.IsLetter())
                return CharacterClassification.Letter;
            if (this.IsDigit())
                return CharacterClassification.Digit;
            if (((preference & Preference.VerticalBar) != 0) && this.IsVerticalBar())
                return CharacterClassification.VerticalBar;
            if (((preference & Preference.NegativeSign) != 0) && this.IsNegativeSign())
                return CharacterClassification.NegativeSign;
            if (this.IsBinaryCharacter())
                return CharacterClassification.BinaryCharacter;
            if (this.IsWhitespace())
                return CharacterClassification.Whitespace;
            if (this.IsCommentDelimiter())
                return CharacterClassification.CommentDelimiter;
            if (this.IsStringDelimiter())
                return CharacterClassification.StringDelimiter;
            if (this.IsAssignmentOperatorCharacter1())
                return CharacterClassification.AssignmentOperatorCharacter1;
            if (this.IsHashedStringDelimiter())
                return CharacterClassification.HashedDelimiter;
            if (this.IsCharacterDelimiter())
                return CharacterClassification.CharacterDelimiter;
            if (this.IsReturnOperator())
                return CharacterClassification.ReturnOperator;
            return CharacterClassification.Undefined;
        }
    }


    internal enum CharacterClassification
    {
        Whitespace,
        Digit,
        Letter,
        BinaryCharacter,
        CharacterDelimiter,
        CommentDelimiter,
        StringDelimiter,
        AssignmentOperatorCharacter1,
        HashedDelimiter,
        ReturnOperator,
        NegativeSign, // Special case, only if preferred insted of BinaryCharacter
        VerticalBar, // Special case, only if preferred insted of BinaryCharacter
        EndOfFile,
        Undefined
    }
}
