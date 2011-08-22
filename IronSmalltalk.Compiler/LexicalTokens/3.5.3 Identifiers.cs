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
    /// Base class for IdentifierToken and KeywordToken.
    /// </summary>
    public abstract class IdentifierOrKeywordToken : Token, IMethodSelectorToken, ILiteralArrayIdentifierToken
    {
        /// <summary>
        /// String value of the identifier or keyword.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Create a new identifier or keyword token.
        /// </summary>
        /// <param name="value">String value of the identifier.</param>
        public IdentifierOrKeywordToken(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException();
            this.Value = value;
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return this.Value; }
        }
    }

    /// <summary>
    /// Identifier token as described in X3J20 "3.5.3 Identifiers".
    /// </summary>
    public class IdentifierToken : IdentifierOrKeywordToken, IPrimitiveCallParameterToken
    {
        /// <summary>
        /// Create a new identifier token.
        /// </summary>
        /// <param name="identifier">String value of the identifier.</param>
        public IdentifierToken(string identifier)
            : base(identifier)
        {
        }
    }
}
