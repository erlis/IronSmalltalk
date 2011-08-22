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
    /// Keyword token as described in X3J20 "3.5.4 Keywords".
    /// The Value property contains the keyword including the last ':' character.
    /// </summary>
    public class KeywordToken : IdentifierOrKeywordToken
    {
        /// <summary>
        /// Create a new keyword token.
        /// </summary>
        /// <param name="identifier">String value of the keyword. This includes the last ':' char.</param>
        public KeywordToken(string identifier)
            : base(identifier)
        {
        }
    }
}
