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
    /// Literal string token as described in X3J20 chapter "3.5.8 Quoted Strings"
    /// </summary>
    /// <remarks>
    /// The string value excludes the string delimiters or double/escape quotes.
    /// </remarks>
    public class StringToken : LiteralToken<string>, IPrimitiveCallParameterToken
    {
        /// <summary>
        /// Creates a new string token.
        /// </summary>
        /// <param name="value">String value excluding the string delimiters or double/escape quotes.</param>
        public StringToken(string value)
            : base(value)
        {
            if (value == null)
                throw new ArgumentNullException();
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get
            {
                return LexicalConstants.StringDelimiter.ToString() +
                    this.Value.Replace(
                        LexicalConstants.StringDelimiter.ToString(),
                        LexicalConstants.StringDelimiter.ToString() + LexicalConstants.StringDelimiter.ToString())
                    + LexicalConstants.StringDelimiter.ToString();
            }
        }
    }
}
