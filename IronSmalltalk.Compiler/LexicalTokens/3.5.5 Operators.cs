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
    /// Base class for operator tokens as described in X3J20 chapter "3.5.5 Operators".
    /// </summary>
    public abstract class OperatorToken : Token
    {
        /// <summary>
        /// Create a new operator token.
        /// </summary>
        protected OperatorToken()
            : base()
        {
        }
    }

    /// <summary>
    /// Return operator "^" token as described in X3J20 chapter "3.5.5 Operators". 
    /// </summary>
    public class ReturnOperatorToken : OperatorToken
    {
        /// <summary>
        /// Creates a new return operator token.
        /// </summary>
        public ReturnOperatorToken()
            : base()
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return LexicalConstants.ReturnOperator.ToString(); }
        }
    }

    /// <summary>
    /// Assignment operator ":=" token as described in X3J20 chapter "3.5.5 Operators". 
    /// </summary>
    public class AssignmentOperatorToken : OperatorToken
    {
        /// <summary>
        /// Creates a new assignment operator token.
        /// </summary>
        public AssignmentOperatorToken()
            : base()
        {
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return LexicalConstants.AssignmentOperator; }
        }
    }

    /// <summary>
    /// Binary operator token as described in X3J20 chapter "3.5.5 Operators". 
    /// </summary>
    public class BinarySelectorToken : OperatorToken, IMethodSelectorToken, ILiteralArrayIdentifierToken
    {
        /// <summary>
        /// Value of the binary operator, e.g. "~=".
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Creates a new binary operator token.
        /// </summary>
        /// <param name="selector">Value of the binary operator, e.g. "~=".</param>
        public BinarySelectorToken(string selector)
        {
            if (String.IsNullOrWhiteSpace(selector))
                throw new ArgumentNullException();
            this.Value = selector;
        }

        /// <summary>
        /// The (part of the) source code that the token represents.
        /// </summary>
        public override string SourceString
        {
            get { return this.Value; }
        }
    }
}
