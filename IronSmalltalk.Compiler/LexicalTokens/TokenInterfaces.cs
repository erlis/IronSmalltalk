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
using IronSmalltalk.Compiler.SemanticNodes;

namespace IronSmalltalk.Compiler.LexicalTokens
{
    /// <summary>
    /// Tokens that may construct a method selecor (identifiers, keywords, binary selectors)
    /// </summary>
    public interface IMethodSelectorToken : IToken
    {
        /// <summary>
        /// String value of the selector token.
        /// </summary>
        string Value { get; }
    }

    /// <summary>
    /// Interface indicating a literal token contains a numeric value.
    /// </summary>
    public interface INumberToken : ILiteralToken
    {
    }

    /// <summary>
    /// Tokens that are parsed inside a literal array as identifiers (identifier, keyword, unquoted selector).
    /// </summary>
    public interface ILiteralArrayIdentifierToken : IToken
    {
        string Value { get; }
    }

    /// <summary>
    /// Tokens that can be used to define parameters for primitive API calls.
    /// </summary>
    public interface IPrimitiveCallParameterToken : IToken
    {
        /// <summary>
        /// String value of the parameter token.
        /// </summary>
        string Value { get; }
    }
}
