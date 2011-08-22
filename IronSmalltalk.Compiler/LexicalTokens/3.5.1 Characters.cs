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

namespace IronSmalltalk.Compiler.LexicalTokens
{
    /// <summary>
    /// Whitespace token as described in X3J20 "3.5.1 Character Categories".
    /// </summary>
    public class WhitespaceToken : Token
    {
        /// <summary>
        /// Create a new Whitespace Token.
        /// </summary>
        /// <param name="value">The text contents (white spaces) for this token.</param>
        public WhitespaceToken(string value)
        {
            if (value == null)
                throw new ArgumentNullException();
            this.Value = value;
        }

        /// <summary>
        /// The text contents (white spaces) for this token.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return this.Value; }
        }
    }
}
