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

namespace IronSmalltalk.Compiler.SemanticAnalysis
{
    /// <summary>
    /// Error messages the parser may report if it encounters illegal source code.
    /// </summary>
    internal static class SemanticErrors
    {
        public const string MissingClosingTempBar = "Missing | .";
        public const string UnexpectedCodeAfterMethodDefinition = "Unexpected token. Expected EOF.";
        public const string MissingMethodMessagePattern = "Missing message pattern.";
        public const string MissingMethodArgument = "Missing method argument.";
        public const string UnexpectedCodeAfterInitializer = "Unexpected token. Expected EOF.";
        public const string MissingBlockClosingArgsBar = "Missing closing | for block arguments.";
        public const string MissingBlockClosingBracket = "Missing closing ] for block.";
        public const string MissingBlockArgument = "Missing block argument.";
        public const string MissingStatement = "Empty statement or extra period.";
        public const string CodeAfterReturnStatement = "Unexpected code following return \"^\" statement.";
        public const string MissingExpression = "Expected expression.";
        public const string MissingPrimary = "Expected primary.";
        public const string MissingMessagePattern = "Expected message pattern.";
        public const string MissingExpressionClosingParenthesis = "Missing closing ) for expression.";
        public const string UnrecognizedLiteral = "Unrecognized literal.";
        public const string MissingLiteralArrayOpeningParenthesis = "Missing literal array opening parenthesis.";
        public const string MissingLiteralArrayClosingParenthesis = "Missing literal array closing parenthesis.";
        public const string MissingApiConvention = "Expected keyword.";
        public const string UnexpectedApiParameterToken = "Unexpected token. Expected string or identifier.";
    }
}
