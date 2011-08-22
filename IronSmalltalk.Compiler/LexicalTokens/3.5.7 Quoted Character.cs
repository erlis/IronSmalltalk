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
    /// Literal character token as described in X3J20 chapter "3.5.7 Quoted Character"
    /// </summary>
    public class CharacterToken : LiteralToken<char>
    {
        /// <summary>
        /// Creates a new character token.
        /// </summary>
        /// <param name="value">Character value for the token.</param>
        public CharacterToken(char value)
            : base(value)
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return LexicalConstants.CharacterDelimiter.ToString() + this.Value.ToString(); }
        }
    }
}
