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
    /// Token base class. The tokens here are described in "ANSI INCITS 319-1998 (R2007)",
    /// chapter "3.5 Lexical Grammer". The class hierarchy is modeled on this chapter.
    /// </summary>
    /// <remarks>
    /// IMPORTANT IMPLEMENTATION NOTES:
    /// The Token class is the base class for all tokens. The heirarchy below models roughly 
    /// the definitions in the X3J20 document.
    /// 
    /// HOWEVER:
    /// The class hieararchy has subclasses and common base classes. But we
    /// MUST AVOID having concrete classes inheriting from other concrete classes.
    /// 
    /// For example, QuotedSelectorToken could inherit from HashedStringToken,
    /// which in turn could inherit from StringToken. We DO NOT WANT THIS!!!!
    /// WE WANT A FLAT HIERARCHY, where the 3 classes are siblings at the same level.
    /// 
    /// RATIONALE:
    /// The parser ofter have to make choices depending on the type of token, 
    /// do one thing if string, other ting if selector token. If selector inherited
    /// from string, the parser would have to test someting like:
    ///     if ((token is StringToken) &amp; !(token is QuotedSelectorToken))
    /// ... which is cumbersome to write and prone to errors. We would like
    /// to simplify things for the users of the Token class hierarchy,
    /// therefore NO concrete classes inheritence. Keep it simple:
    ///     if (token is StringToken)
    /// 
    /// If we need common functionality, inject an abstract class above the concrete
    /// classes, or better, use interfaces!
    /// </remarks>
    public abstract class Token : IToken 
    {
        /// <summary>
        /// Create a new Token.
        /// </summary>
        protected Token()
        {
            this.StartPosition = SourceLocation.Invalid;
            this.StopPosition = SourceLocation.Invalid;
        }

        /// <summary>
        /// Set the values of the token; source code start, stop positions + opt. scan error message.
        /// </summary>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="scanError">Optional scan error message, in case of source code syntax error.</param>
        public void SetTokenValues(SourceLocation startPosition, SourceLocation stopPosition, string scanError)
        {
            if (!startPosition.IsValid)
                throw new ArgumentOutOfRangeException("startPosition");
            if (stopPosition < startPosition)
                throw new ArgumentOutOfRangeException("stopPosition");

            this.StartPosition = startPosition;
            this.StopPosition = stopPosition;
            this.ScanError = scanError;
        }

        /// <summary>
        /// Source code start position. 
        /// </summary>
        /// <remarks>
        /// The position includes all characters of the token, even they may be excluded from the value.
        /// If the token is artificially created (to fix source error), this property contains -1.
        /// </remarks>
        public SourceLocation StartPosition { get; private set; }

        /// <summary>
        /// source code end position.
        /// </summary>
        /// <remarks>
        /// The position includes all characters of the token, even they may be excluded from the value.
        /// If the token is artificially created (to fix source error), this property contains -1.
        /// </remarks>
        public SourceLocation StopPosition { get; private set; }

        /// <summary>
        /// In case there's a syntax error in the source file,
        /// this will contain a error message string.
        /// </summary>
        public string ScanError { get; private set; }

        /// <summary>
        /// Determines if the token is valid.
        /// Invalid tokens indicate that some syntax error was encountered.
        /// The scanner still return a Token, but the source file has syntax error in it!
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (this.ScanError == null);
            }
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public abstract string SourceString { get; }
    }

    /// <summary>
    /// End-of-file token. Indicates end of file.
    /// </summary>
    public class EofToken : Token
    {
        /// <summary>
        /// Create a new Enf-Of-File Token.
        /// </summary>
        public EofToken()
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return String.Empty; }
        }
    }
}
