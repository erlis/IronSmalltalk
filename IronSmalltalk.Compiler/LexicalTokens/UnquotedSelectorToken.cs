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
    /// Unquoted selector token is similar to quoted selector tokens
    /// as described in X3J20 "3.5.10 Quoted Selector",
    /// except that unquoted selectors are only allowed inside literal arrays.
    /// 
    /// This is an extension to X3J20 lexical grammer, so we can read array literals.
    /// Example: #( with:with: ) .... should be read as: #( #with:with: )
    /// </summary>
    public class UnquotedSelectorToken : LiteralToken<string>, ILiteralArrayIdentifierToken
    {
        /// <summary>
        /// Create a new unquoted selector token.
        /// </summary>
        /// <param name="value">String value of the selector.</param>
        public UnquotedSelectorToken(string value)
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
            get { return LexicalConstants.QuotedSelectorDelimiter.ToString() + this.Value; }
        }
    }
}
