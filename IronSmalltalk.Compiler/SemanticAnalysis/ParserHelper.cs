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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronSmalltalk.Compiler.LexicalTokens;

namespace IronSmalltalk.Compiler.SemanticAnalysis
{
    /// <summary>
    /// Helper functions here perform some test on tokens, especially SpecialCharacterToken, 
    /// for certain cases described in "ANSI NCITS 319-1998 (R2007)" (X3J20).
    /// </summary>
    /// <remarks>
    /// The reason for having a seperate file is, in case there's a
    /// difference between .Net and the X3J20 definition, we only 
    /// have to change things here, and not go trough "business code" 
    /// in the compiler classes.
    /// </remarks>
    public partial class Parser
    {
        /// <summary>
        /// Test for block start bracket, defined in "3.4.4 Blocks".
        /// This is the opening square bracket "[" used for defining blocks.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsBlockStartDelimiter(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.BlockStartDelimiter);
        }

        /// <summary>
        /// Test for block end bracket, defined in "3.4.4 Blocks".
        /// This is the closing square bracket "]" used for defining blocks.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsBlockEndDelimiter(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.BlockEndDelimiter);
        }

        /// <summary>
        /// Test for block argument prefix, defined in "3.4.4 Blocks".
        /// This is the colon in block argument.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsBlockArgumentPrefix(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.BlockArgumentPrefix);
        }

        /// <summary>
        /// Test if the given token represents opening parenthesis, i.e. "(".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsOpeningParenthesis(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.OpeningParenthesis);
        }

        /// <summary>
        /// Test if the given token represents closing parenthesis, i.e. ")".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsClosingParenthesis(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.ClosingParenthesis);
        }

        /// <summary>
        /// Test if the given token represents a literal array prefix hash mark, i.e. "#".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsLiteralArrayPrefix(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.LiteralArrayPrefix);
        }


        /// <summary>
        /// Test statement delimiter, defined in "3.4.5 Statements".
        /// This is the period that follows expressions.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsStatementDelimiter(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.StatementDelimiter);
        }

        /// <summary>
        /// Test for cascade message delimiter, defined in "3.4.5.3 Messages".
        /// This is the semicolon delimiting expressions.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsCascadeDelimiter(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == SemanticConstants.CascadeDelimiter);
        }

        /// <summary>
        /// Test if the token is reference to the reserved identifier "self".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsIdentifierSelf(Token token)
        {
            IdentifierToken idtoken = token as IdentifierToken;
            if (idtoken == null)
                return false;
            return (idtoken.Value == SemanticConstants.Self);
        }

        /// <summary>
        /// Test if the token is reference to the reserved identifier "super".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsIdentifierSuper(Token token)
        {
            IdentifierToken idtoken = token as IdentifierToken;
            if (idtoken == null)
                return false;
            return (idtoken.Value == SemanticConstants.Super);
        }

        /// <summary>
        /// Test if the token is reference to the reserved identifier "nil".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsIdentifierNil(Token token)
        {
            IdentifierToken idtoken = token as IdentifierToken;
            if (idtoken == null)
                return false;
            return (idtoken.Value == SemanticConstants.Nil);
        }

        /// <summary>
        /// Test if the token is reference to the reserved identifier "self".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsIdentifierTrue(Token token)
        {
            IdentifierToken idtoken = token as IdentifierToken;
            if (idtoken == null)
                return false;
            return (idtoken.Value == SemanticConstants.True);
        }

        /// <summary>
        /// Test if the token is reference to the reserved identifier "self".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsIdentifierFalse(Token token)
        {
            IdentifierToken idtoken = token as IdentifierToken;
            if (idtoken == null)
                return false;
            return (idtoken.Value == SemanticConstants.False);
        }


        /// <summary>
        /// Test if the token is opening delimiter for Primitive API call statement.
        /// This is IronSmalltalk extension to the grammer.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsApiOpeningDelimiter(Token token)
        {
            BinarySelectorToken apitoken = token as BinarySelectorToken;
            if (apitoken == null)
                return false;
            return (apitoken.Value == SemanticConstants.PrimitiveOpeningDelimiter);
        }

        /// <summary>
        /// Test if the token is closing delimiter for Primitive API call statement.
        /// This is IronSmalltalk extension to the grammer.
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsApiClosingDelimiter(Token token)
        {
            BinarySelectorToken apitoken = token as BinarySelectorToken;
            if (apitoken == null)
                return false;
            return (apitoken.Value == SemanticConstants.PrimitiveClosingDelimiter);
        }
    }
}
