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
using System.IO;
using System.Text;
using IronSmalltalk.Compiler.LexicalTokens;
using System.Numerics;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.LexicalAnalysis
{
    /// <summary>
    /// This is the Smalltalk lexical scanner.
    /// Most of the logic is described in X3J20 "3.5 Lexical Grammar".
    /// </summary>
    /// <remarks>
    /// See also Read Me file in Compiler folder for discussion on token syntax.
    /// </remarks>
    public class Scanner
    {
        private readonly CharReader Reader;
        private readonly StringBuilder Buffer;
        private readonly ScanResult ScanResult;
        private SourceLocation TokenStartPosition;
        private bool EofReturned = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public Scanner(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            this.Reader = new CharReader(reader);
            this.Buffer = new StringBuilder();
            this.ScanResult = new ScanResult();
        }

        private SourceLocation CurrentPosition
        {
            get { return new SourceLocation(this.Reader.CurrentPosition, this.Reader.CurrentLine, Math.Max(this.Reader.CurrentColumn, 1)); }
        }

        /// <summary>
        /// Optional sink object for reporting source code syntax errors encountered by the scanner.
        /// </summary>
        public IScanErrorSink ErrorSink { get; set; }

        /// <summary>
        /// The token returned by the last call to GetToken() function.
        /// </summary>
        /// <remarks>
        /// After calls to PutToken(), this property will be out-of-sync.
        /// </remarks>
        public Token CurrentToken { get; private set; }

        /// <summary>
        /// Get the next token.
        /// </summary>
        /// <returns>A token.</returns>
        public Token GetToken()
        {
            return this.GetToken(Preference.Default);
        }

        /// <summary>
        /// Get the next token.
        /// </summary>
        /// <param name="preference">Token preference, if other than the default.</param>
        /// <returns>A token.</returns>
        /// <remarks>
        /// If preference is other than Preference.Default, the user must not
        /// use PutToken() to put this token back in the scanner, since we have 
        /// no logic to convert the token to another type on next GetToken() call.
        /// If the token is returned to the scanner with PutToken(), the next call to
        /// GetToken() will return the token disregarding the Preference flags.
        /// </remarks>
        public Token GetToken(Preference preference)
        {
            this.CurrentToken = this.GetTokenInternal(preference);
            return this.CurrentToken;
        }

        private Token GetTokenInternal(Preference preference)
        {
            if (this.EofReturned)
                return null;

            this.Read();
            this.TokenStartPosition = this.CurrentPosition;
            CharacterClassification classification= this.ScanResult.Classify(preference);

            switch (classification)
            {
                case CharacterClassification.Whitespace:
                    return this.GetWhitespaces(); // Should never get here (handled above), but keep it just in case.
                case CharacterClassification.Digit:
                    return this.GetNumber();
                case CharacterClassification.Letter:
                    if ((preference & Preference.UnquotedSelector) != 0)
                        return this.GetUnquotedSelector();
                    else
                        return this.GetIdentifier();
                case CharacterClassification.BinaryCharacter:
                    return this.GetBinarySelector(preference);
                case CharacterClassification.NegativeSign:
                    return this.GetNegativeSign();
                case CharacterClassification.VerticalBar:
                    return this.ReturnSuccess(new VerticalBarToken());
                case CharacterClassification.CharacterDelimiter:
                    return this.GetQuotedCharacter();
                case CharacterClassification.CommentDelimiter:
                    return this.GetComment(); // Should never get here (handled above), but keep it just in case.
                case CharacterClassification.StringDelimiter:
                    return this.GetQuotedString();
                case CharacterClassification.ReturnOperator:
                    return this.GetReturnOperator();
                case CharacterClassification.EndOfFile:
                    this.EofReturned = true;
                    return this.ReturnSuccess(new EofToken());
                case CharacterClassification.AssignmentOperatorCharacter1:
                    if (this.PeekScanResult().IsAssignmentOperatorCharacter2())
                        return this.GetAssignmentOperator();
                    else
                        return this.GetSpecialCharacter();
                case CharacterClassification.HashedDelimiter:
                    ScanResult sc = this.PeekScanResult();
                    if (sc.IsStringDelimiter())
                        return this.GetHashedString();
                    else if (sc.IsLetter() || sc.IsBinaryCharacter())
                        return this.GetQuotedSelector();
                    else
                        return this.GetSpecialCharacter();
                case CharacterClassification.Undefined:
                default:
                    return this.GetSpecialCharacter();
            }
        }

        #region 3.5.2 Comments

        /// <summary>
        /// Returns a token for a comment, as defined in X3J20 "3.5.2 Comments"
        /// </summary>
        /// <returns>A comment token containing the comment text (excluding the delimitors).</returns>
        private CommentToken GetComment()
        {
            // commentDelimiter ::= '"'
            // nonCommentDelimiter ::= " any other char except commentDelimiter "
            // comment ::= commentDelimiter nonCommentDelimiter* commentDelimiter
            // Example:
            //  " this is a comment "
            // NB: The " quote cannot be nested. It's treated as two separate comments.

#if DEBUG
            if (!this.ScanResult.IsCommentDelimiter())
                throw new InvalidScannerOperationException("Expected \" char.");
#endif

            this.Buffer.Clear();
            // NB: The value of the comment excludes the delimiting quotes.
            while(true)
            {
                this.Read();
                if (this.ScanResult.EndOfFile)
                    return this.ReturnError(new CommentToken(this.Buffer.ToString()), LexicalErrors.MissingClosingComment);

                if (this.ScanResult.IsCommentDelimiter())
                    return this.ReturnSuccess(new CommentToken(this.Buffer.ToString()));

                this.Buffer.Append(this.ScanResult.Character);
            }
        }

        #endregion


        #region 3.5.3 Identifiers and 3.5.4 Keywords

        /// <summary>
        /// Returns a token for an identifier or keyword, as defined in X3J20 "3.5.3 Identifiers" and "3.5.4 Keywords"
        /// </summary>
        /// <returns>An identifier or keyword token containing the identifier value.</returns>
        private IdentifierOrKeywordToken GetIdentifier()
        {
            return this.GetIdentifier(true);
        }

        /// <summary>
        /// Returns a token for an identifier or keyword, as defined in X3J20 "3.5.3 Identifiers" and "3.5.4 Keywords"
        /// </summary>
        /// <param name="unadorned">Indicates an unadorned identifier. This is false, when we use this function to scan for quoted selectors.</param>
        /// <returns>An identifier or keyword token containing the identifier value.</returns>
        private IdentifierOrKeywordToken GetIdentifier(bool unadorned)
        {
            // identifier ::= letter (letter | digit)*
            // Example: 
            //  printString
            // keyword ::= identifier ':'
            // Example: 
            //  printOn:

            // X3J20 says: "An unadorned identifier is an identifier which is not immediately preceded 
            // by a '#'. If a ':' followed by an ' immediately follows an unadorned identifier, 
            // with no intervening white space, then the token is to be parsed as an identifier 
            // followed by an assignmentOperator not as an keyword followed by an '."
            // In other words: 
            // 1. Read identifier.
            // 2. Check for ':', then we have a keyword
            // 3. Check for ' after the ':', then it is not a keyword, but an identifier anyway. 

#if DEBUG
            if (!this.ScanResult.IsLetter())
                throw new InvalidScannerOperationException("Expected a letter.");
#endif

            this.Buffer.Clear();
            this.Buffer.Append(this.ScanResult.Character);
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsLetter() || this.ScanResult.IsDigit())
                {
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip();
                }
                else if (this.ScanResult.IsKeywordPostfix())
                {
                    // Tricky part ... are we keyword or identifier
                    char keywordPostfix = this.ScanResult.Character;
                    // An unadorned identifier is an identifier which is not preceded by a '#'
                    // ... so we don't have to look for := (assignment).
                    if (unadorned)
                    {
                        this.Mark(); // Mark the end of the identifier
                        this.Skip(); // Progress over the ':' char.
                        this.Peek(); // Look for '=' char.

                        if (this.ScanResult.IsAssignmentOperatorCharacter2())
                        {
                            // It was identifier folowed by assignment
                            this.Back(); // Go back to where the identifier ended, we marked with Mark().
                            return this.ReturnSuccess(new IdentifierToken(this.Buffer.ToString()));
                        }
                        this.Back();
                    }
                    // A keyword. It is identifier + ':' + something else, but not '='
                    this.Skip();
                    this.Buffer.Append(keywordPostfix);
                    return this.ReturnSuccess(new KeywordToken(this.Buffer.ToString()));
                }
                else
                {
                    // Just an identifier .... (not followed by ':')
                    return this.ReturnSuccess(new IdentifierToken(this.Buffer.ToString()));
                }
            }
        }

        #endregion


        #region 3.5.5 Operators

        /// <summary>
        /// Returns a token for a binary selector, as defined in X3J20 "3.5.5 Operators"
        /// </summary>
        /// <returns>A binary selector token.</returns>
        private BinarySelectorToken GetBinarySelector(Preference preference)
        {
            // binaryCharacter ::= '!' | '%' | '&'' | '*' | '+' | ',' | '/' | '<' | ' | '>' | '?' | '@' | '\' | '~' | '|' | '-'
            // binarySelector ::= binaryCharacter+

            // NB: X3J20 writes: "If a negative <number literal> follows a binary selector there must 
            // intervening white space." I read this as if the user have ommited the white space, 
            // then we regard this as a binary operator and a positive numner,e.g. "x --3" is 3 tokens: 
            // "x", "--", "3". The user can choose to write "x- -3", which gives: "x", "-", "-3".

#if DEBUG
            if (!this.ScanResult.IsBinaryCharacter())
                throw new InvalidScannerOperationException();
#endif

            this.Buffer.Clear();
            this.Buffer.Append(this.ScanResult.Character);
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsBinaryCharacter())
                {
                    if (((preference & Preference.NegativeSign) != 0) && this.ScanResult.IsNegativeSign())
                    {
                        // This is a negative sign ... is it followed by digits?
                        if (this.IsNegativeSign(true))
                            return this.ReturnSuccess(new BinarySelectorToken(this.Buffer.ToString()));
                    }
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip();
                }
                else
                {
                    return this.ReturnSuccess(new BinarySelectorToken(this.Buffer.ToString()));
                }
            }
        }

        /// <summary>
        /// Helper function to determine, if the current character is negative sign. 
        /// To quallify, the current character must be negative sign (minus),
        /// optionally followed by whitepsaces and MUST be followed by a digit.
        /// </summary>
        /// <returns></returns>
        private bool IsNegativeSign(bool currentIsPeak)
        {
            if (!this.ScanResult.IsNegativeSign())
                return false;
            char ch = this.ScanResult.Character;
            this.Mark();
            try
            {
                if (currentIsPeak)
                    this.Skip();
                while (true)
                {
                    this.Read();
                    if (this.ScanResult.IsDigit())
                    {
                        return true; // Digit 
                    }
                    else if (this.ScanResult.IsCommentDelimiter())
                    {
                        // Skip comments
                        this.Read();
                        while (!this.ScanResult.IsCommentDelimiter())
                            this.Read();
                    }
                    else if (!this.ScanResult.IsWhitespace())
                    {
                        return false; // Non comment or whitespace
                    }
                }
            }
            finally
            {
                this.Back();
                this.ScanResult.SetResult((int)ch);
            }
        }


        /// <summary>
        /// Returns a token for the return operator, as defined in X3J20 "3.5.5 Operators"
        /// </summary>
        /// <returns>A return operator token.</returns>
        private ReturnOperatorToken GetReturnOperator()
        {
            // returnOperator ::= '^'
#if DEBUG
            if (!this.ScanResult.IsReturnOperator())
                throw new InvalidScannerOperationException();
#endif

            return this.ReturnSuccess(new ReturnOperatorToken());
        }

        /// <summary>
        /// Returns a token for the assignment operator, as defined in X3J20 "3.5.5 Operators"
        /// </summary>
        /// <returns>An assignment operator token.</returns>
        private AssignmentOperatorToken GetAssignmentOperator()
        {
            // assignmentOperator ::= ':='

#if DEBUG
            if (!this.ScanResult.IsAssignmentOperatorCharacter1())
                throw new InvalidScannerOperationException();
#endif

            this.Read();
            if (this.ScanResult.IsAssignmentOperatorCharacter2())
                return this.ReturnSuccess(new AssignmentOperatorToken());
            else
                return this.ReturnError(new AssignmentOperatorToken(), LexicalErrors.InvalidAssignemtOperatorChar);
        }

        #endregion


        #region 3.5.6 Numbers

        /// <summary>
        /// Returns a token for the assignment operator, as defined in X3J20 "3.5.6 Numbers"
        /// </summary>
        /// <returns></returns>
        private Token GetNumber()
        {
            // ================ INTEGERS =====================
            // integer ::= decimalInteger | radixInteger
            // decimalInteger ::= digits
            // digits ::= digit+
            // radigInteger ::= radixSpecifier 'r' radixDigits
            // radixSpecifier ::= digits
            // radixDigits ::= (digit | uppercaseAlphabetic)+
            // ================ FLOATS =======================
            // float ::= mantissa [exponentLetter exponent]
            // mantissa ::= digits '.' digits
            // exponent ::= ['-'] decimalInteger
            // exponentLetter ::= 'e' | 'd' | 'q' 
            // ================ SCALED DECIMALS ==============
            // scaledDecimal ::= scaledMantissa 's' [fractionalDigits]
            // scaledMantissa ::= decimalInteger | mantissa
            // fractionalDigits ::= decimalInteger

#if DEBUG
            // All numbers MUST start with a digit (the '-' sign is handled elsewhere)
            if (!this.ScanResult.IsDigit())
                throw new InvalidScannerOperationException();
#endif

            this.Buffer.Clear();
            // .... therefore always append the first digit to the result buffer.
            this.Buffer.Append(this.ScanResult.Character);

            while (true)
            {
                // Read digits...
                this.Peek();
                if (this.ScanResult.IsDigit())
                {
                    // ... another digit, add it to the buffer.
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                }
                else
                {
                    // ... a non digit, must determine what to do now. Is it a special case/char?
                    if (this.ScanResult.IsRadixDelimiter())
                    {
                        // radigInteger ::= radixSpecifier 'r' radixDigits ... Example: 16r100
                        this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                        return this.GetRadixInteger(this.Buffer.ToString());
                    }
                    if (this.ScanResult.IsMantissaDelimiter())  // ... i.e. the decimal digit
                    {
                        this.Mark();
                        this.Skip();
                        if (this.PeekScanResult().IsDigit())
                        {
                            // Float or Scaled Dec. ... Example: 42.1 or 12.22e3 or 53.09s
                            this.Unmark();
                            return this.GetFloatOrScaledDecimal(this.Buffer.ToString());
                        }
                        else
                        {
                            this.Back();
                        }
                    }
                    if (this.ScanResult.IsScaledDecimalScaleDelimiter())    // ... i.e. the 's' character
                    {
                        // Scaled decimal in the form: decimalInteger 's' [fractionalDigits] ... Example: 123s or 123s3
                        this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                        return this.GetScaledDecimal(this.Buffer.ToString(), null);
                    }
                    
                    // OK, Must be a simple (base 10) integer. 
                    return this.GetIntegerToken(this.Buffer.ToString());
                }
            }
        }

        /// <summary>
        /// Helper function to GetNumber() for rading radix integer, e.g. 16r200.
        /// </summary>
        /// <remarks>
        /// If we get here, we are reading a "radigInteger", and we've read up to the 'r'.
        /// Here we process the "radixDigits" of the integer and return the integer token.
        /// </remarks>
        /// <param name="radixSpecifier">The "radixSpecifier" part, i.e. digits up to the 'r' delimiter.</param>
        /// <returns>An integer token for the radix integer.</returns>
        private Token GetRadixInteger(string radixSpecifier)
        {
            // ================ INTEGERS =====================
            // integer ::= decimalInteger | radixInteger
            // decimalInteger ::= digits
            // digits ::= digit+
            // radigInteger ::= radixSpecifier 'r' radixDigits
            // radixSpecifier ::= digits
            // radixDigits ::= (digit | uppercaseAlphabetic)+

#if DEBUG
            if (!this.ScanResult.IsRadixDelimiter())
                throw new InvalidScannerOperationException();
            if (String.IsNullOrWhiteSpace(radixSpecifier))
                throw new InvalidScannerOperationException();
#endif

            // Try to parse the <radixSpecifier> to integer.
            bool success;
            int radix = ConversionUtilities.ConvertSmallInteger(radixSpecifier, out success);
            // Check if successfull. It must be between 2 and 36 (both inclusive).
            if ((!success) || (radix < 2) || (radix > 36))
                return this.ReturnError(new SmallIntegerToken(0), String.Format(LexicalErrors.InvalidRadix, radixSpecifier));

            // Ok, start processing the <radixDigits> part.
            this.Read();
            if (!this.ScanResult.IsDigitBase(radix))
                return this.ReturnError(new SmallIntegerToken(radix), LexicalErrors.InvalidRadixInteger);

            // Add digits to the <radixDigits> buffer.
            this.Buffer.Clear();
            this.Buffer.Append(this.ScanResult.Character);
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsDigitBase(radix))
                {
                    // Another good digit ...
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                }
                else
                {
                    if (this.ScanResult.IsDigit() || this.ScanResult.IsUppercaseAlphabetic())
                    {
                        this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                        // The X3J20 standard does not allow the digits to be outside the radix range
                        return this.ReturnError(this.GetIntegerToken(this.Buffer.ToString(), radix),
                            String.Format(LexicalErrors.DigitTooBig,
                            radix, (radix <= 10) ? (radix - 1).ToString() : (LexicalConstants.FirstLetterDigit + radix - 11).ToString()));
                    }

                    // OK, it's not a digit or uppercase letter ... must be somethig else. We are done reading the integer.
                    return this.GetIntegerToken(this.Buffer.ToString(), radix);
                }
            }
        }

        /// <summary>
        /// Helper function to GetNumber() for rading floats or scaled decinals, e.g. 12.12e4 or 12.34s4.
        /// </summary>
        /// <remarks>
        /// If we get here, we are reading either a float or a scaled decimal that contains a decimal.
        /// In either case, and we've read up to the decimal separator '.' and need to process the decimal digits here.
        /// If we encounter 'e' | 'd' | 'q', we are dealing with float, and branch to the corresponding helper function.
        /// If we encounter 's', we are dealing with scaled decimal, and branch to the corresponding helper function.
        /// If we encounter anything else than decimals, we are done reading, and returm a float token.
        /// </remarks>
        /// <param name="integerDigits">The first "digits" part of the "mantissa", i.e. digits up to the '.' decimal separator.</param>
        /// <returns>A numeric token for the float or scaled decimal.</returns>
        private Token GetFloatOrScaledDecimal(string integerDigits)
        {
            // ================ FLOATS =======================
            // float ::= mantissa [exponentLetter exponent]
            // mantissa ::= digits '.' digits
            // exponent ::= ['-'] decimalInteger
            // exponentLetter ::= 'e' | 'd' | 'q' 
            // ================ SCALED DECIMALS ==============
            // scaledDecimal ::= scaledMantissa 's' [fractionalDigits]
            // scaledMantissa ::= decimalInteger | mantissa
            // fractionalDigits ::= decimalInteger

#if DEBUG
            if (!this.ScanResult.IsMantissaDelimiter())
                throw new InvalidScannerOperationException();
            if (String.IsNullOrWhiteSpace(integerDigits))
                throw new InvalidScannerOperationException();
#endif

            // Ok, start processing the second <digits> part of <mantissa>.
            this.Read();
            if (!this.ScanResult.IsDigit())
                return this.ReturnError(this.GetFloatToken(integerDigits, "0", "0", false, '\0'), LexicalErrors.InvalidFloatNoDecimalDigits);

            // Add digits to the <digits> buffer.
            this.Buffer.Clear();
            this.Buffer.Append(this.ScanResult.Character);
            // Below, we process the <mantissa> part in floats and scaled decimals.
            // Loop until we find non-digit, or 'e' (for float exponent) or 's' (for scaled decimal scale).
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsDigit())
                {
                    // Another good digit ...
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                }
                else
                {
                    // ... a non digit, must determine what to do now. Is it a special case/char?
                    if (this.ScanResult.IsExponentLetter())
                    {
                        // A float ... advancing to exponent part, Example 123.12e3
                        this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                        return this.GetFloat(integerDigits, this.Buffer.ToString());
                    }
                    if (this.ScanResult.IsScaledDecimalScaleDelimiter())
                    {
                        // A scaled decimal ... advancing to scale part, Example 123.12s4
                        this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                        return this.GetScaledDecimal(integerDigits, this.Buffer.ToString());
                    }

                    // The character 'is no good' for a number, so we are done reading.
                    // Since we didn't encounter 's' nor 'e', it must be float with exp 1.
                    return this.ReturnSuccess(this.GetFloatToken(integerDigits, this.Buffer.ToString(), "0", false, '\0'));
                }
            }

        }

        /// <summary>
        /// Helper function to GetNumber() and GetFloatOrScaledDecimal() for rading floats, e.g. 12.12e4.
        /// </summary>
        /// <remarks>
        /// If we get here, we are reading a float with exponent and we've done reading up to the exponent letter.
        /// This function reads the exponent and returns the float token.
        /// </remarks>
        /// <param name="integerDigits">The first "digits" part of the "mantissa", i.e. digits up to the '.' decimal separator.</param>
        /// <param name="decimalDigits">The second "digits" part of the "mantissa", i.e. digits after the '.' decimal separator up to the exponent letter.</param>
        /// <returns>A float token for the floating point number.</returns>
        private Token GetFloat(string integerDigits, string decimalDigits)
        {
            // float ::= mantissa [exponentLetter exponent]
            // mantissa ::= digits '.' digits
            // exponent ::= ['-'] decimalInteger
            // exponentLetter ::= 'e' | 'd' | 'q' 

#if DEBUG
            if (!this.ScanResult.IsExponentLetter())
                throw new InvalidScannerOperationException();
            if (String.IsNullOrWhiteSpace(integerDigits))
                throw new InvalidScannerOperationException();
            if (String.IsNullOrWhiteSpace(decimalDigits))
                throw new InvalidScannerOperationException();
#endif

            // Ok, start processing the <exponent> part.
            bool negativeExponent = false;
            char exponentLetter = this.ScanResult.Character;
            this.Read();
            if (this.ScanResult.IsNegativeSign())
            {
                negativeExponent = true;
                this.Read();
            }
            else if(!this.ScanResult.IsDigit())
                return this.ReturnError(this.GetFloatToken(integerDigits, decimalDigits, "0", negativeExponent, exponentLetter), LexicalErrors.InvalidFloatNoExponentDigits);

            // Add digits to the <exponent> buffer.
            this.Buffer.Clear();
            this.Buffer.Append(this.ScanResult.Character);
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsDigit())
                {
                    // Another good digit ...
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                }
                else
                {
                    // Non-digit. We are done reading the float.
                    return this.GetFloatToken(integerDigits, decimalDigits, this.Buffer.ToString(), negativeExponent, exponentLetter);
                }
            }
        }

        /// <summary>
        /// Helper function to GetNumber() and GetFloatOrScaledDecimal() for rading scaled decimals, e.g. 12.34s5.
        /// </summary>
        /// <remarks>
        /// If we get here, we are reading a scaled decimal and we've done reading up to the 's' letter.
        /// This function reads the fraction digits (if any) and returns the scaled decimal token.
        /// NB: The "decimalDigits" parameter may be null, if the "scaledMantissa" is of "decimalInteger" type.
        /// </remarks>
        /// <param name="integerDigits">The first "digits" part of the "scaledMantissa", i.e. digits up to the '.' decimal separator.</param>
        /// <param name="decimalDigits">The second "digits" part of the "scaledMantissa", i.e. digits after the '.' decimal separator up to the exponent letter.</param>
        /// <returns>A scaled decimal token for the number.</returns>
        private ScaledDecimalToken GetScaledDecimal(string integerDigits, string decimalDigits)
        {
            // scaledDecimal ::= scaledMantissa 's' [fractionalDigits]
            // scaledMantissa ::= decimalInteger | mantissa
            // fractionalDigits ::= decimalInteger

#if DEBUG
            if (!this.ScanResult.IsScaledDecimalScaleDelimiter())
                throw new InvalidScannerOperationException();
            if (String.IsNullOrWhiteSpace(integerDigits))
                throw new InvalidScannerOperationException();
#endif

            // Ok, start processing the optional <fractionalDigits> part.
            this.Buffer.Clear();
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsDigit())
                {
                    // Another good digit ...
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip(); // We've already filled the ScanResult with the Peek(), so just forward 1 position.
                }
                else
                {
                    // Non-digit. We are done reading the number.
                    return this.GetScaledDecimalToken(integerDigits, decimalDigits, this.Buffer.ToString());
                }
            }
        }

        /// <summary>
        /// Creates an IntegerToken for given digits in base 10.
        /// </summary>
        /// <param name="integerDigits">Integer digits string.</param>
        /// <returns>An integer token.</returns>
        private Token GetIntegerToken(string integerDigits)
        {
            bool success;
            // Try first if it is small integer ...
            int value = ConversionUtilities.ConvertSmallInteger(integerDigits, out success);
            if (success)
                return this.ReturnSuccess(new SmallIntegerToken(value));
            // ... then try large integer (it is not supposed to fail, since we provide clean input)
            BigInteger lValue = ConversionUtilities.ConvertLargeInteger(integerDigits);
            return this.ReturnSuccess(new LargeIntegerToken(lValue)); 
        }

        /// <summary>
        /// Creates an IntegerToken for given digits in a given numeric base.
        /// </summary>
        /// <param name="integerDigits">Integer digits string, e.g. for "16r200" this param contains "200".</param>
        /// <param name="radix">Numeric base, e.g. for "16r200" this param contains 16.</param>
        /// <returns>An integer token.</returns>
        private Token GetIntegerToken(string integerDigits, int radix)
        {
            bool success;
            // Try first if it is small integer ...
            int value = ConversionUtilities.ConvertSmallInteger(integerDigits, radix, out success);
            if (success)
                return this.ReturnSuccess(new SmallIntegerToken(value));
            // ... then try large integer (it is not supposed to fail, since we provide clean input)
            BigInteger lValue = ConversionUtilities.ConvertLargeInteger(integerDigits, radix);
            return this.ReturnSuccess(new LargeIntegerToken(lValue)); 
        }

        /// <summary>
        /// Create a FloatToken for the given float parts.
        /// </summary>
        /// <param name="integerDigits">Integer digits string, e.g. for "12.34e-5" this param contains "12".</param>
        /// <param name="decimalDigits">Decimal digits string, e.g. for "12.34e-5" this param contains "34".</param>
        /// <param name="exponentDigits">Exponent digits string, e.g. for "12.34e-5" this param contains "5".</param>
        /// <param name="negativeExponent">Is the exponent negative? E.g. for "12.34e-5" this param is set to true.</param>
        /// <returns>A float token.</returns>
        private Token GetFloatToken(string integerDigits, string decimalDigits, string exponentDigits, bool negativeExponent, char exponentLetter)
        {
#if DEBUG
            if ((LexicalConstants.ExponentLetters.IndexOf(exponentLetter) == -1) && (exponentLetter != '\0'))
                throw new InvalidScannerOperationException();
#endif

            string errorMessage = null;
            object value = ConversionUtilities.ConvertFloat(integerDigits, decimalDigits, exponentDigits, negativeExponent, exponentLetter, out errorMessage);
            if (value is double)
            {
                if (errorMessage == null)
                    return this.ReturnSuccess(new FloatDToken((double)value));
                else
                    return this.ReturnError(new FloatDToken((double)value), errorMessage);
            }
            else if (value is float)
            {
                if (errorMessage == null)
                    return this.ReturnSuccess(new FloatEToken((float)value));
                else
                    return this.ReturnError(new FloatEToken((float)value), errorMessage);
            }
            else
            {
                throw new InvalidScannerOperationException();
            }
        }

        /// <summary>
        /// Create a ScaledDecimalToken for the given scaled decimal parts.
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
        /// <returns>A scaled decimal token.</returns>
        private ScaledDecimalToken GetScaledDecimalToken(string integerDigits, string decimalDigits, string fractionalDigits)
        {
            // NB: decimalDigits may be null
            string errorMessage = null;
            BigDecimal value = ConversionUtilities.ConvertScaledDecimal(integerDigits, decimalDigits, fractionalDigits, out errorMessage);
            if (errorMessage == null)
                return this.ReturnSuccess(new ScaledDecimalToken(value));
            else
                return this.ReturnError(new ScaledDecimalToken(value), errorMessage); 
        }

        #endregion


        #region 3.5.7 Quoted Character

        /// <summary>
        /// Returns a token for a character, as defined in X3J20 "3.5.7 Quoted Character"
        /// </summary>
        /// <returns>A character token containing the character literal.</returns>
        private CharacterToken GetQuotedCharacter()
        {
            // quotedCharacter ::= '$' character
            // Example:
            //  $A

#if DEBUG
            if (!this.ScanResult.IsCharacterDelimiter())
                throw new InvalidScannerOperationException("Expected $ char.");
#endif

            this.Read();
            if (this.ScanResult.IsCharacter())
                return this.ReturnSuccess(new CharacterToken(this.ScanResult.Character));
            else
                return this.ReturnError(new CharacterToken(this.ScanResult.Character), LexicalErrors.MissingCharacter);
        }

        #endregion


        #region 3.5.8 Quoted Strings

        /// <summary>
        /// Returns a token for a string, as defined in X3J20 "3.5.8 Quoted Strings"
        /// </summary>
        /// <returns>A string token containing the string literal.</returns>
        private StringToken GetQuotedString()
        {
            // quotedString ::= stringDelimiter stringBody stringDelimiter
            // stringBody ::= (nonStringDelimiter | (stringDelimiter stringDelimiter))*
            // stringDelimiter ::= '''   " a single quote "
            // nonStringDelimiter ::= "  any character except stringDelimiter  "
            // Example: 
            //  'ABC DEF'   ... or  ... 'That''s super cool!'
            // NB: The returned token contains only the string value, no delimiters.

#if DEBUG
            if (!this.ScanResult.IsStringDelimiter())
                throw new InvalidScannerOperationException("Expected ' char.");
#endif

            this.Buffer.Clear();
            while (true)
            {
                this.Read();
                if (this.ScanResult.EndOfFile)
                    return this.ReturnError(new StringToken(this.Buffer.ToString()), LexicalErrors.MissingClosingStringQuote);
                if (this.ScanResult.IsStringDelimiter())
                {
                    if (this.PeekScanResult().IsStringDelimiter())
                        this.Skip(); // Double string delimiter.
                    else
                        return this.ReturnSuccess(new StringToken(this.Buffer.ToString()));
                }
                this.Buffer.Append(this.ScanResult.Character);
            }
        }

        #endregion


        #region 3.5.9 Hashed String

        /// <summary>
        /// Returns a token for a hashed string, as defined in X3J20 "3.5.9 Hashed Strings"
        /// </summary>
        /// <returns>A hashed string token containing the string literal.</returns>
        private HashedStringToken GetHashedString()
        {
            // hashedString ::= '#' quotedString
            // Example:
            //  #'A symbol with spaces'.
            // NB: The returned token contains only the string value, no delimiters.

#if DEBUG
            if (!this.ScanResult.IsHashedStringDelimiter())
                throw new InvalidScannerOperationException("Expected # char.");
#endif
            this.Read(); // Progress to the next char ... we expect to find ' here.
#if DEBUG
            if (!this.ScanResult.IsStringDelimiter())
                throw new InvalidScannerOperationException("Expected #' char sequence.");
#endif
            // Use the standard GetQuotedString() function to process everything after the # char.
            StringToken token = this.GetQuotedString();
            // Convert the StringToken to HashedStringToken
            HashedStringToken result = new HashedStringToken(token.Value);
            result.SetTokenValues(token.StartPosition, token.StopPosition, token.ScanError);
            return result;
        }

        #endregion


        #region 3.5.10 Quoted Selector

        /// <summary>
        /// Returns a token for a quoted selector, as defined in X3J20 "3.5.10 Quoted Selector"
        /// </summary>
        /// <returns>A keyword token containing the quoted selector value.</returns>
        private QuotedSelectorToken GetQuotedSelector()
        {
            // quotedSelector ::= '#' (unarySelector | binarySelector | keywordSelector)
            // keywordSelector ::= keyword+
            // Example: 
            //  #isNil
            // NB: The returned token contains only the string value, no '#' prefix.

#if DEBUG
            if (!this.ScanResult.IsQuotedSelectorDelimiter())
                throw new InvalidScannerOperationException("Expected # char.");
#endif

            this.Read(); // Progress to the next char ... we expect to find a letter or binary char here.

            if (this.ScanResult.IsLetter())
            {
                IdentifierOrKeywordToken token = this.GetIdentifier(false);
                if (!token.IsValid)
                    return this.ReturnError(new QuotedSelectorToken(token.Value), token.ScanError);
                if (token is KeywordToken)
                {
                    // keywordSelector
                    StringBuilder text = new StringBuilder();
                    text.Append(token.Value);
                    while (true)
                    {
                        // Look for more keywords
                        this.Peek();
                        if (this.ScanResult.IsLetter())
                        {
                            this.Mark(); // In case we get an identifier instead of keyword, we'll need to go back
                            this.Skip();
                            token = this.GetIdentifier(false);
                            if (!token.IsValid)
                            {
                                this.Unmark();
                                text.Append(token.Value);
                                return this.ReturnError(new QuotedSelectorToken(text.ToString()), token.ScanError);
                            }
                            if (!(token is KeywordToken))
                            {
                                // We hit an identifier (not keyword) following the last keyword. That's not for us!
                                this.Back(); // Back to where the last keyword ended.
                                return this.ReturnSuccess(new QuotedSelectorToken(text.ToString()));
                            }
                            // It is a keyword token, so append the keyword to the keyword selector text and continue scanning.
                            text.Append(token.Value);
                            this.Unmark();
                        }
                        else
                        {
                            // Not a keyword ... we are done with the keyword selector
                            return this.ReturnSuccess(new QuotedSelectorToken(text.ToString()));
                        }
                    }
                }
                else
                {
                    // unarySelector
                    return this.ReturnSuccess(new QuotedSelectorToken(token.Value));
                }
            }
            else if (this.ScanResult.IsBinaryCharacter())
            {
                // binarySelector
                BinarySelectorToken token = this.GetBinarySelector(Preference.Default);
                if (token.IsValid)
                    return this.ReturnSuccess(new QuotedSelectorToken(token.Value));
                else
                    return this.ReturnError(new QuotedSelectorToken(token.Value), token.ScanError);
            }
            else
            {
                throw new InvalidScannerOperationException("Expected a letter or binary character sequence.");
            }
        }

        #endregion


        #region 3.5.11 Separators

        private WhitespaceToken GetWhitespaces()
        {
#if DEBUG
            if (!this.ScanResult.IsWhitespace())
                throw new InvalidScannerOperationException();
#endif
            this.Buffer.Clear();
            this.Buffer.Append(this.ScanResult.Character);
            while (true)
            {
                this.Peek();
                if (this.ScanResult.IsWhitespace())
                {
                    this.Buffer.Append(this.ScanResult.Character);
                    this.Skip();
                }
                else
                {
                    return this.ReturnSuccess(new WhitespaceToken(this.Buffer.ToString()));
                }
            }
        }

        #endregion

        private SpecialCharacterToken GetSpecialCharacter()
        {
#if DEBUG
            CharacterClassification classification = this.ScanResult.Classify(Preference.Default);
            if ((classification != CharacterClassification.Undefined) && (classification != CharacterClassification.HashedDelimiter) && (classification != CharacterClassification.AssignmentOperatorCharacter1))
                throw new InvalidScannerOperationException();
#endif
            return this.ReturnSuccess(new SpecialCharacterToken(this.ScanResult.Character));
        }


        /// <summary>
        /// This is an extension to X3J20 lexical grammer, so we can read array literals.
        /// Example: #( with:with: ) .... should be read as: #( #with:with: )
        /// </summary>
        /// <returns>
        /// A unquoted selector token containing the selector value, if the token contains 2 or more keywords,
        /// or identifier token or keyword token if the token contains 0 or 1 keywords.
        /// </returns>
        private Token GetUnquotedSelector()
        {
            // unquotedSelector ::= (unarySelector | binarySelector | keywordSelector)
            // keywordSelector ::= keyword+
            // Example: 
            //  with:with:

#if DEBUG
            if (!this.ScanResult.IsLetter())
                throw new InvalidScannerOperationException("Expected letter.");
#endif

            IdentifierOrKeywordToken token = this.GetIdentifier(false);
            if (!token.IsValid)
                return token;
            if (!(token is KeywordToken))
                return token;

            // keywordSelector
            StringBuilder text = new StringBuilder();
            text.Append(token.Value);
            KeywordToken firstKeyword = (KeywordToken)token;
            while (true)
            {
                // Look for more keywords
                this.Peek();
                if (this.ScanResult.IsLetter())
                {
                    this.Mark(); // In case we get an identifier instead of keyword, we'll need to go back
                    this.Skip();
                    token = this.GetIdentifier(false);
                    if (!token.IsValid)
                    {
                        this.Unmark();
                        text.Append(token.Value);
                        return this.ReturnError(new UnquotedSelectorToken(text.ToString()), token.ScanError);
                    }

                    if (!(token is KeywordToken))
                    {
                        // We hit an identifier (not keyword) following the last keyword. That's not for us!
                        this.Back(); // Back to where the last keyword ended.
                        if (firstKeyword != null)
                            firstKeyword = null;
                        return this.ReturnSuccess(new UnquotedSelectorToken(text.ToString()));
                    }
                    // It is a keyword token, so append the keyword to the keyword selector text and continue scanning.
                    text.Append(token.Value);
                    this.Unmark();
                    firstKeyword = null;
                }
                else
                {
                    // Not a keyword ... we are done with the keyword selector
                    if (firstKeyword != null)
                        return firstKeyword;
                    return this.ReturnSuccess(new UnquotedSelectorToken(text.ToString()));
                }
            }
        }

        /// <summary>
        /// This is special case, where we think the current token might be a negative sign,
        /// so we must unsure it's followed by digits. If so, return a SpecialCharacterToken, 
        /// if not, return a BinarySelectorToken.
        /// </summary>
        /// <returns></returns>
        private Token GetNegativeSign()
        {
            if (this.IsNegativeSign(false))
                return this.ReturnSuccess(new NegativeSignToken());
            else
                return this.GetBinarySelector(Preference.NegativeSign);
        }

        private TToken ReturnSuccess<TToken>(TToken token)
            where TToken : Token
        {
#if DEBUG
            if (token == null)
                throw new ArgumentNullException("token");
#endif
            token.SetTokenValues(this.TokenStartPosition, this.CurrentPosition, null);
            return token;
        }

        private TToken ReturnError<TToken>(TToken token, string errorMessage)
            where TToken : Token
        {
#if DEBUG
            if (token == null)
                throw new ArgumentNullException("token");
            if (String.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("errorMessage");
#endif
            token.SetTokenValues(this.TokenStartPosition, this.CurrentPosition, errorMessage);
            if (this.ErrorSink != null)
                this.ErrorSink.AddScanError(token, this.TokenStartPosition, this.CurrentPosition, errorMessage);
            return token;
        }

        #region Text Reading and Scanning Functions

        /// <summary>
        /// Read a single character into the instance variable ScanResult and progress the text stream by one.
        /// </summary>
        private void Read()
        {
            this.ScanResult.SetResult(this.Reader.Read());
        }

        /// <summary>
        /// Read a single character and return a new ScanResult for it, progressing the text stream by one.
        /// </summary>
        /// <returns>ScanResult with the character that was read from the text stream.</returns>
        private ScanResult ReadScanResult()
        {
            return new ScanResult(this.Reader.Read());
        }

        /// <summary>
        /// Read a single character into the instance variable ScanResult without progressing the current text stream position.
        /// </summary>
        private void Peek()
        {
            this.ScanResult.SetResult(this.Reader.Peek());
        }

        /// <summary>
        /// Read a single character and return a new ScanResult for it, without progressing the current text stream position.
        /// </summary>
        /// <returns>ScanResult with the character that was read from the text stream.</returns>
        private ScanResult PeekScanResult()
        {
            return new ScanResult(this.Reader.Peek());
        }

        /// <summary>
        /// Progress the current text stream position by one.
        /// </summary>
        private void Skip()
        {
            this.Reader.Read();
        }

        /// <summary>
        /// Mark the current text stream position, to allow backtracking back to that position.
        /// </summary>
        private void Mark()
        {
            this.Reader.Mark();
        }

        /// <summary>
        /// Revert to a previously marked position in the text stream.
        /// </summary>
        private void Back()
        {
            this.Reader.Back();
        }

        /// <summary>
        /// Discard previously set mark.
        /// </summary>
        private void Unmark()
        {
            this.Reader.Unmark();
        }

        /// <summary>
        /// Helper class that wraps TextReader, but additionally adds Mark() and Back() functionality
        /// to be able to Mark() a position, read progressing in the stream and go Back() to the mark.
        /// </summary>
        /// <remarks>
        /// Not the best implementation, but simple. No nested Mark() operations supporten. 
        /// </remarks>
        private class CharReader
        {
            private readonly TextReader Reader;
            private Queue<int> TrackQueue;
            private Queue<int> BackTrackQueue;
            private int MarkedPosition;
            private int MarkedLine;
            private int MarkedColumn;
            private bool MarkedWasCarriageReturn;
            public int CurrentPosition { get; private set; }
            public int CurrentLine { get; private set; }
            public int CurrentColumn { get; private set; }
            private bool WasCarriageReturn;

            public CharReader(TextReader reader)
            {
                this.Reader = reader;
                this.CurrentPosition = -1;
                this.CurrentLine = 1;
                this.CurrentColumn = 0;
            }

            public int Read()
            {
                int result;
                if (this.BackTrackQueue != null)
                {
                    // Tracking back after a Back() command.
                    result = this.BackTrackQueue.Dequeue();
                    if (this.BackTrackQueue.Count == 0)
                        // Back track queue exhausted - start reading normally
                        this.BackTrackQueue = null;
                }
                else 
                {
                    // We only read from the stream if: we are not backtracking or BackTrackQueue is exhausted.
                    result = this.Reader.Read();
                }

                // We were send Mark() and asked to track data, so we can go Back() later.
                // NB: It is possible that we are both backtracking (reading from BackTrackQueue)
                // and tracking at the same time (writing to TrackQueue)
                if (this.TrackQueue != null)
                    this.TrackQueue.Enqueue(result);

                this.CurrentPosition++;

                // Line and column counting
                if (result == 13)
                {
                    this.CurrentLine++;
                    this.CurrentColumn = 0;
                    this.WasCarriageReturn = true;
                }
                else if (result == 10)
                {
                    if (!this.WasCarriageReturn)
                    {
                        this.CurrentLine++;
                        this.CurrentColumn = 0;
                    }
                    this.WasCarriageReturn = false;
                }
                else
                {
                    this.CurrentColumn++;
                    this.WasCarriageReturn = false;
                }

                return result;
            }

            public int Peek()
            {
                if (this.BackTrackQueue != null)
                    // Tracking back after a Back() command.
                    return this.BackTrackQueue.Peek();
                else
                    // Normal reading directly from the stream
                    return this.Reader.Peek();
            }

            public void Mark()
            {
                if (this.TrackQueue != null)
                    throw new InvalidScannerOperationException("We do not support nested transactions");
                this.TrackQueue = new Queue<int>(100);
                this.MarkedPosition = this.CurrentPosition;
                this.MarkedLine = this.CurrentLine;
                this.MarkedColumn = this.CurrentColumn;
                this.MarkedWasCarriageReturn = this.WasCarriageReturn;
            }

            public void Back()
            {
                if (this.TrackQueue == null)
                    throw new InvalidScannerOperationException("No mark to go back to.");
                this.BackTrackQueue = this.TrackQueue;
                this.TrackQueue = null;
                this.CurrentPosition = this.MarkedPosition;
                this.CurrentLine = this.MarkedLine;
                this.CurrentColumn = this.MarkedColumn;
                this.WasCarriageReturn = this.MarkedWasCarriageReturn;
            }

            public void Unmark()
            {
                if (this.TrackQueue == null)
                    throw new InvalidScannerOperationException("Not marked. Cannot unmark.");
                this.TrackQueue = null;
            }
        }

        #endregion

    }

    /// <summary>
    /// Scanner preference.
    /// </summary>
    [Flags]
    public enum Preference
    {
        /// <summary>
        /// Neither VerticalBar nor NegativeSign.
        /// </summary>
        Default = 0,

        /// <summary>
        /// If the next character is vertical bar ("|"), 
        /// return a VerticalBarToken instead of BinarySelectorToken.
        /// </summary>
        VerticalBar = 1,

        /// <summary>
        /// If the next character is negative sign ("-") followed by a digit,
        /// return a NegativeSignToken instead of BinarySelectorToken.
        /// </summary>
        NegativeSign = 2,
        
        /// <summary>
        /// Prefer unquoted selector to identifier or keyword token.
        /// Unquoted selectors are used exclusively inside array literals.
        /// </summary>
        UnquotedSelector = 4
    }
}
