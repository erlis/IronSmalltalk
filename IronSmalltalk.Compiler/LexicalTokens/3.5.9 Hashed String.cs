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
    /// Hashed string token as described in X3J20 "3.5.9 Hashed String".
    /// </summary>
    /// <remarks>
    /// The string value excludes the preceeding '#' hash char, string delimiters or double/escape quotes.
    /// </remarks>
    public class HashedStringToken : LiteralToken<string>
    {
        /// <summary>
        /// Creates a new hashed string token.
        /// </summary>
        /// <param name="value">String value excluding the preceeding '#' hash char, string delimiters or double/escape quotes.</param>
        public HashedStringToken(string value)
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
                return LexicalConstants.HashedStringDelimiter.ToString() + LexicalConstants.StringDelimiter.ToString() +
                    this.Value.Replace(
                        LexicalConstants.StringDelimiter.ToString(),
                        LexicalConstants.StringDelimiter.ToString() + LexicalConstants.StringDelimiter.ToString())
                    + LexicalConstants.StringDelimiter.ToString();
            }
        }
    }
}
