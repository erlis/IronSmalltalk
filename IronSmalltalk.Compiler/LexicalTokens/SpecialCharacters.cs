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
using IronSmalltalk.Compiler.LexicalAnalysis;

namespace IronSmalltalk.Compiler.LexicalTokens
{

    /// <summary>
    /// Token for characters that do not tokanize into the other tokens.
    /// Example; . ( ) | [ ] etc.
    /// </summary>
    public class SpecialCharacterToken : Token
    {
        /// <summary>
        /// Create a new Whitespace Token.
        /// </summary>
        /// <param name="value">The text contents (white spaces) for this token.</param>
        public SpecialCharacterToken(char value)
        {
            this.Value = value;
        }

        /// <summary>
        /// The text contents for this token.
        /// </summary>
        public char Value { get; private set; }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return this.Value.ToString(); }
        }
    }

    /// <summary>
    /// Special case token, then Preference.VerticalBar is set.
    /// </summary>
    public class VerticalBarToken : Token
    {
        /// <summary>
        /// Create and initialize a new VerticalBarToken.
        /// </summary>
        public VerticalBarToken()
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return LexicalConstants.VerticalBar.ToString(); }
        }
    }

    /// <summary>
    /// Special case token, then Preference.NegativeSign is set.
    /// </summary>
    public class NegativeSignToken : Token
    {
        /// <summary>
        /// Create and initialize a new NegativeSignToken.
        /// </summary>
        public NegativeSignToken()
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return LexicalConstants.NegativeSign.ToString(); }
        }
    }
}
