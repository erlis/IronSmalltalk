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
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.LexicalAnalysis;
using System.Collections.Generic;

namespace IronSmalltalk.Compiler.VseCompatibility
{
    public class VseCompatibleParser : Parser
    {
        protected override LiteralNode ParseLiteral(ILiteralNodeParent parent, Token token)
        {
            if ((parent is ArrayLiteralNode) && Parser.IsOpeningParenthesis(token))
            {
                // Stupid VSE allows declarations of arrays like: #( 1 2 ( 3 4 ) 5 6),
                // which is identical to: #( 1 2 #( 3 4 ) 5 6).
                // Only the inner (child) arrays may omit the hash prefix.
                // Here we emulate this and create a 'fake' hash token.
                // The fake hash token gets the same source positions and the parenthesis token.
                SpecialCharacterToken hash = new SpecialCharacterToken(SemanticConstants.LiteralArrayPrefix);
                hash.SetTokenValues(token.StartPosition, token.StopPosition, null);
                this.ResidueToken = token;
                return this.ParseArrayLiteral(parent, hash);
            }

            if (!Parser.IsLiteralArrayPrefix(token))
                return base.ParseLiteral(parent, token);

            Token token2 = this.GetNextTokenxx(Preference.Default);
            if (VseCompatibleParser.IsOpeningByteArrayBracket(token2))
                return this.ParseByteArrayLiteral(parent, (SpecialCharacterToken) token, (SpecialCharacterToken)token2);

            this.ResidueToken = token2;
            return base.ParseLiteral(parent, token);
        }

        protected ByteArrayLiteralNode ParseByteArrayLiteral(ILiteralNodeParent parent, SpecialCharacterToken arrayToken, SpecialCharacterToken leftBracket)
        {
            // PARSE: <array literal> ::= '#[' <number literal>* ']'

            List<SmallIntegerLiteralNode> elements = new List<SmallIntegerLiteralNode>();
            ByteArrayLiteralNode result = new ByteArrayLiteralNode(parent, arrayToken, leftBracket);

            // Process tokens inside the array ...
            while (true)
            {
                // ... get next token in the array ...
                Token token = this.GetNextTokenxx(Preference.NegativeSign);

                // Is this closing parenthesis? 
                if (VseCompatibleParser.IsClosingByteArrayBracket(token))
                {
                    // Closing parenthesis ... done with the array, return litral array node.
                    result.SetContents(elements, (SpecialCharacterToken)token);
                    this.ResidueToken = null;
                    return result;
                }

                if (token is EofToken)
                {
                    // Unfinished source code ... return 
                    result.SetContents(elements, null);
                    this.ReportParserError(parent, "Missing literal byte array closing bracket.", token);
                    this.ResidueToken = token;
                    return result;
                }

                // PARSE: <numeric liteal> 
                if (token is SmallIntegerToken)
                {
                    elements.Add(new SmallIntegerLiteralNode(result, (SmallIntegerToken)token, null));
                }
                else if (token is NegativeSignToken)
                {
                    NegativeSignToken negativeSign = (NegativeSignToken)token;
                    token = this.GetNextTokenxx(Preference.NegativeSign);
                    if (token is SmallIntegerToken)
                    {
                        elements.Add(new SmallIntegerLiteralNode(result, (SmallIntegerToken)token, negativeSign));
                    }
                    else
                    {
                        this.ReportParserError(parent, "Unrecognized literal.", token);
                        this.ResidueToken = token;
                        result.SetContents(elements, null);
                        return result;
                    }
                }
                else
                {
                    this.ReportParserError(parent, "Unrecognized literal.", token);
                    this.ResidueToken = token;
                    result.SetContents(elements, null);
                    return result;
                }
            }
        }

        /// <summary>
        /// Test if the given token represents opening bracket of a byte array, i.e. "]".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsOpeningByteArrayBracket(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == '[');
        }

        /// <summary>
        /// Test if the given token represents closing bracket of a byte array, i.e. "]".
        /// </summary>
        /// <param name="token">Token to perform the test on.</param>
        /// <returns>True, if the test succeeds, otherwise false.</returns>
        public static bool IsClosingByteArrayBracket(Token token)
        {
            SpecialCharacterToken sctoken = token as SpecialCharacterToken;
            if (sctoken == null)
                return false;
            return (sctoken.Value == ']');
        }
    }
}
