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
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.LexicalTokens
{
    /// <summary>
    /// A token object, modeling the description in "ANSI INCITS 319-1998 (R2007)",
    /// chapter "3.5 Lexical Grammer". 
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Source code start position. 
        /// </summary>
        /// <remarks>
        /// The position includes all characters of the token, even they may be excluded from the value.
        /// </remarks>
        SourceLocation StartPosition { get; }

        /// <summary>
        /// Source code end position.
        /// </summary>
        /// <remarks>
        /// The position includes all characters of the token, even they may be excluded from the value.
        /// </remarks>
        SourceLocation StopPosition { get; }

        /// <summary>
        /// In case there's a syntax error in the source file,
        /// this will contain a error message string.
        /// </summary>
        string ScanError { get; }

        /// <summary>
        /// Determines if the token is valid.
        /// Invalid tokens indicate that some syntax error was encountered.
        /// The scanner still return a Token, but the source file has syntax error in it!
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        string SourceString { get; }
    }
}
