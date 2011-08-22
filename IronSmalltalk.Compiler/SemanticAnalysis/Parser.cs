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
using System.IO;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.SemanticNodes;

namespace IronSmalltalk.Compiler.SemanticAnalysis
{
    /// <summary>
    /// This is the Smalltalk parser.
    /// Most of the logic is described in X3J20 "3.4 Method Grammar".
    /// </summary>
    /// <remarks>
    /// The Parser object can be used only once and cannot be reused after a parse operation has completed.
    /// This is because the Parse object keeps state about the parse operation, e.g. any errors that
    /// may have been encountered during parsing of the source code.
    /// </remarks>
    public partial class Parser : ParserBase
    {
        /// <summary>
        /// Create a new parser.
        /// </summary>
        public Parser()
            : base()
        {
        }

        /// <summary>
        /// Parse a method definition, as defined in X3J20 "3.4.2 Method Definition".
        /// </summary>
        /// <param name="reader">Text reader containing the source code to be parsed.</param>
        /// <returns>A parse node for the method definition.</returns>
        public MethodNode ParseMethod(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            this.InitParser(reader);
            return this.ParseMethod();
        }

        /// <summary>
        /// Parse a method definition, as defined in X3J20 "3.4.3 Initializer Definition".
        /// </summary>
        /// <param name="reader">Text reader containing the source code to be parsed.</param>
        /// <returns>A parse node for the initializer definition.</returns>
        public InitializerNode ParseInitializer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            this.InitParser(reader);
            return this.ParseInitializer();
        }

        #region 3.4.1 Functions

        protected virtual ParseTemporariesResult ParseTemporaries(FunctionNode function, Token token)
        {
            // PARSE: <temporaries> ::= '|' <temporary variable list> '|'
            ParseTemporariesResult result = new ParseTemporariesResult();
            if (!(token is VerticalBarToken))
            {
                this.ResidueToken = token;
                return result;
            }

            result.LeftBar = (VerticalBarToken)token;
            while (true)
            {
                token = this.GetNextTokenxx(Preference.VerticalBar);
                if (token is VerticalBarToken)
                {
                    // Done with temp variables.
                    result.RightBar = (VerticalBarToken)token;
                    return result;
                }
                if (token is IdentifierToken)
                {
                    result.Temporaries.Add(new TemporaryVariableNode(function, (IdentifierToken)token));
                }
                else
                {
                    this.ReportParserError(function, SemanticErrors.MissingClosingTempBar, token);
                    this.ResidueToken = token;
                    return result;
                }
            }
        }

        protected class ParseTemporariesResult
        {
            public VerticalBarToken LeftBar;
            public List<TemporaryVariableNode> Temporaries = new List<TemporaryVariableNode>();
            public VerticalBarToken RightBar;
        }

        #endregion

        #region 3.4.2 Method Definition

        protected virtual MethodNode ParseMethod()
        {
            // PARSE X3J20: <method definition> ::= <message pattern> [<temporaries> ] [<statements>] 
            // NB: We've extended the method definition to allow API calls into the .Net Framework, 
            // PARSE: <method definition> ::= <message pattern> [<temporaries>] [<api call>] [<statements>] 
            MethodNode result = this.ParseMessagePattern();
            Token token = this.GetNextTokenxx(Preference.NegativeSign | Preference.VerticalBar);

            // PARSE: [<temporaries>] 
            ParseTemporariesResult ptr = this.ParseTemporaries(result, token);

            // PARSE [<api call>]
            token = this.GetNextTokenxx(Preference.NegativeSign);
            if (Parser.IsApiOpeningDelimiter(token))
            {
                result.SetContents(this.ParseApiCall(result, (BinarySelectorToken)token));
                token = this.GetNextTokenxx(Preference.NegativeSign);
            }

            // PARSE: <statements>
            StatementNode statements = this.ParseStatement(result, token);

            // Should be done.
            token = this.GetNextTokenxx(Preference.Default);
            if (!(token is EofToken))
                this.ReportParserError(result, SemanticErrors.UnexpectedCodeAfterMethodDefinition, token);

            result.SetContents(ptr.LeftBar, ptr.Temporaries, ptr.RightBar, statements);
            return result;
        }

        protected virtual PrimitiveCallNode ParseApiCall(MethodNode parent, BinarySelectorToken token)
        {
            PrimitiveCallNode result = new PrimitiveCallNode(parent);
            List<IPrimitiveCallParameterToken> parameters = new List<IPrimitiveCallParameterToken>();

            Token apiConvention = this.GetNextTokenxx(Preference.Default);
            if (!(apiConvention is KeywordToken))
                this.ReportParserError(result, SemanticErrors.MissingApiConvention, apiConvention);

            while(true)
            {
                Token tkn = this.GetNextTokenxx(Preference.Default);
                if (tkn is IPrimitiveCallParameterToken)
                {
                    parameters.Add((IPrimitiveCallParameterToken)tkn);
                }
                else if (Parser.IsApiClosingDelimiter(tkn))
                {

                    result.SetContents(token, tkn as BinarySelectorToken, apiConvention as KeywordToken, parameters);
                    return result;
                } 
                else
                {
                    this.ReportParserError(result, SemanticErrors.UnexpectedApiParameterToken, tkn);
                    this.ResidueToken = tkn;

                    result.SetContents(token, tkn as BinarySelectorToken, apiConvention as KeywordToken, parameters);
                    return result;
                }
            }        
        }

        /// <summary>
        /// Parse the method message pattern.
        /// </summary>
        protected virtual MethodNode ParseMessagePattern()
        {
            // PARSE: <message pattern> ::= <unary pattern> | <binary pattern> | <keyword pattern>
            //      <unary pattern> ::= unarySelector
            //      <binary pattern> ::= binarySelector <method argument>
            //      <keyword pattern> ::= (keyword <method argument>)+
            MethodNode result = new MethodNode();
            List<IMethodSelectorToken> selectorParts = new List<IMethodSelectorToken>();
            List<MethodArgumentNode> arguments = new List<MethodArgumentNode>();

            Token token = this.GetNextTokenxx(Preference.Default);

            // PARSE: <message pattern> ::= <unary pattern> | <binary pattern> | <keyword pattern>
            if (token is KeywordToken)
            {
                // <keyword pattern> ::= (keyword <method argument>)+
                selectorParts.Add((IMethodSelectorToken)token);
                token = this.GetNextTokenxx(Preference.Default);
                arguments.Add(this.ParseMethodArgument(result, token));
                while (true)
                {
                    token = this.GetNextTokenxx(Preference.VerticalBar | Preference.NegativeSign);
                    if (token is KeywordToken)
                    {
                        selectorParts.Add((IMethodSelectorToken)token);
                        token = this.GetNextTokenxx(Preference.Default);
                        arguments.Add(this.ParseMethodArgument(result, token));
                    }
                    else
                    {
                        result.SetContents(selectorParts, arguments);
                        this.ResidueToken = token; // temp vars or statement
                        return result;
                    }
                }
            }
            else if (token is BinarySelectorToken)
            {
                //  <binary pattern> ::= binarySelector <method argument>
                selectorParts.Add((IMethodSelectorToken)token);
                token = this.GetNextTokenxx(Preference.Default);
                arguments.Add(this.ParseMethodArgument(result, token));
                result.SetContents(selectorParts, arguments);
                return result;
            }
            else if (token is IdentifierToken)
            {
                //  <unary pattern> ::= unarySelector
                selectorParts.Add((IMethodSelectorToken)token);
                result.SetContents(selectorParts, arguments);
                return result;
            }
            else
            {
                this.ReportParserError(result, SemanticErrors.MissingMethodMessagePattern, token);
                this.ResidueToken = token;
                return result;
            }
        }

        protected virtual MethodArgumentNode ParseMethodArgument(MethodNode parent, Token token)
        {
            // PARSE: identifier      ... NB: X3J20 bug - definition missing ... but this one is easy.
            if (!(token is IdentifierToken))
            {
                this.ReportParserError(parent, SemanticErrors.MissingMethodArgument, token);
                this.ResidueToken = token;
                // NB: MethodArgumentNode must be able to handle null for arg. name token.
                return new MethodArgumentNode(parent, null);
            }
            return new MethodArgumentNode(parent, (IdentifierToken)token);
        }

        #endregion

        #region 3.4.3 Initializer Definition

        protected virtual InitializerNode ParseInitializer()
        {
            // PARSE: <initializer definition> ::= [<temporaries>] [<statements>]
            InitializerNode result = new InitializerNode();
            Token token = this.GetNextTokenxx(Preference.VerticalBar | Preference.NegativeSign);
            
            // PARSE: [<temporaries>] 
            ParseTemporariesResult ptr = this.ParseTemporaries(result, token);

            // PARSE: <statements>
            token = this.GetNextTokenxx(Preference.NegativeSign);
            StatementNode statements = this.ParseStatement(result, token);

            // Should be done.
            token = this.GetNextTokenxx(Preference.Default);
            if (!(token is EofToken))
                this.ReportParserError(result, SemanticErrors.UnexpectedCodeAfterInitializer, token);

            result.SetContents(ptr.LeftBar, ptr.Temporaries, ptr.RightBar, statements);
            return result;
        }

        #endregion

        #region 3.4.4 Blocks

        protected virtual BlockNode ParseBlock(IPrimaryParentNode parent, SpecialCharacterToken leftBracket)
        {
            // PARSE: <block constructor> ::= '[' <block body> ']'
            //      <block body> ::= [<block argument>* '|'] [<temporaries>] [<statements>]
            //      <block argument> ::= ':' identifier
            BlockNode result = new BlockNode(parent, leftBracket);

            // PARSE: [<block argument>* '|'] 
            // ISSUE: It this a bug in X3J20. Shouldn't it be: [<block argument>+ '|']
            // The current definition allows blocks like: [ | ] ... or ... [ | self doStuff ]  ... or ... [ | | temp | temp := self something ]
            // We assume X3J20 bug and expect: [<block argument>+ '|'] 
            VerticalBarToken argumentsBar = null;
            List<BlockArgumentNode> arguments = new List<BlockArgumentNode>();
            
            Token token = this.GetNextTokenxx(Preference.VerticalBar | Preference.NegativeSign);
            // Check if the block has arguments
            if (Parser.IsBlockArgumentPrefix(token))
            {
                // ... yes arguments ... parse them ...
                while (true)
                {

                    if (Parser.IsBlockArgumentPrefix(token))
                    {
                        // <block argument>
                        arguments.Add(this.ParseBlockArgument(result, (SpecialCharacterToken)token));
                    }
                    else if (token is VerticalBarToken)
                    {
                        // The '|' after the arguments.
                        argumentsBar = (VerticalBarToken)token;
                        token = this.GetNextTokenxx(Preference.NegativeSign | Preference.VerticalBar);
                        break; // Done with block arguments
                    }
                    else
                    {
                        this.ReportParserError(result, SemanticErrors.MissingBlockClosingArgsBar, token);
                        break;
                    }
                    // Get the next token
                    token = this.GetNextTokenxx(Preference.VerticalBar | Preference.NegativeSign);
                }
            }

            // PARSE: [<temporaries>] 
            ParseTemporariesResult ptr = this.ParseTemporaries(result, token);

            // PARSE: <statements>
            token = this.GetNextTokenxx(Preference.NegativeSign);
            StatementNode statements = this.ParseStatement(result, token);

            // Ensure the block is properly closed.
            SpecialCharacterToken rightBracket = null;
            token = this.GetNextTokenxx(Preference.Default) ;
            if (!Parser.IsBlockEndDelimiter(token))
            {
                this.ReportParserError(result, SemanticErrors.MissingBlockClosingBracket, token);
                this.ResidueToken = token;
            }
            else
            {
                rightBracket = (SpecialCharacterToken)token;
            }

            result.SetContents(arguments, argumentsBar, ptr.LeftBar, ptr.Temporaries, ptr.RightBar, statements, rightBracket);
            return result;
        }

        protected virtual BlockArgumentNode ParseBlockArgument(BlockNode parent, SpecialCharacterToken colon)
        {
            // PARSE: <block argument> ::= ':' identifier
            Token token = this.GetNextTokenxx(Preference.Default);
            if (!(token is IdentifierToken))
            {
                this.ReportParserError(parent, SemanticErrors.MissingBlockArgument, token);
                this.ResidueToken = token;
                // NB: BlockArgumentNode must be able to handle null for arg. name token.
                return new BlockArgumentNode(parent, colon, null);
            }
            return new BlockArgumentNode(parent, colon, (IdentifierToken)token);
        }

        #endregion

        #region 3.4.5 Statements

        protected virtual StatementNode ParseStatement(IStatementParentNode parent, Token token)
        {
            // PARSE: <statements> ::= 
            //      (<return statement> ['.'] ) | 
            //      (<expression> ['.' [<statements>]])

            if ((token is EofToken) || Parser.IsBlockEndDelimiter(token))
            {
                this.ResidueToken = token;
                return null;
            }                

            if (token is ReturnOperatorToken)
                return this.ParseReturnStatement(parent, (ReturnOperatorToken)token);
            else
                return this.ParseStatementSequence(parent, token);
        }

        protected virtual StatementSequenceNode ParseStatementSequence(IStatementParentNode parent, Token token)
        {
            // PARSE: (<expression> ['.' [<statements>]])
            StatementSequenceNode result = new StatementSequenceNode(parent);

            ExpressionNode expression = this.ParseExpression(result, token);
            if (expression == null)
                this.ReportParserError(result, SemanticErrors.MissingExpression, token);

            SpecialCharacterToken period = null;
            token = this.GetNextTokenxx(Preference.Default);
            if (Parser.IsStatementDelimiter(token))
            {
                period = (SpecialCharacterToken)token;
            }
            else
            {
                result.SetContents(expression, null, null);
                this.ResidueToken = token;
                return result;
            }

            // ['.' [<statements>]])
            token = this.GetNextTokenxx(Preference.NegativeSign);
            if (token is EofToken)
            {
                result.SetContents(expression, period, null);
                this.ResidueToken = token;
                return result;
            }
            if (Parser.IsBlockEndDelimiter(token))
            {
                result.SetContents(expression, period, null);
                this.ResidueToken = token;
                return result;
            }
            else if (Parser.IsStatementDelimiter(token))
            {
                this.ReportParserError(result, SemanticErrors.MissingStatement, token);
                result.SetContents(expression, period, null);
                this.ResidueToken = token;
                return result;
            }

            StatementNode nextStatement = this.ParseStatement(result, token);
            result.SetContents(expression, period, nextStatement);
            return result;
        }

        protected virtual ReturnStatementNode ParseReturnStatement(IStatementParentNode parent, ReturnOperatorToken returnOperator)
        {
            // PARSE: <return statement> ::= returnOperator <expression>
            // Also: <statements> ::= <return statement> [’.’]
            ReturnStatementNode result = new ReturnStatementNode(parent, returnOperator);

            Token token = this.GetNextTokenxx(Preference.NegativeSign);
            ExpressionNode expression = this.ParseExpression(result, token);
            if (expression == null)
                this.ReportParserError(result, SemanticErrors.MissingExpression, token);

            SpecialCharacterToken period = null;
            token = this.GetNextTokenxx(Preference.Default);
            if (Parser.IsStatementDelimiter(token))
            {
                period = (SpecialCharacterToken)token;
                token = this.GetNextTokenxx(Preference.Default);
            }

            result.SetContents(expression, period);

            // Returns statement ... should have reached the end of the statement.
            this.ResidueToken = token;
            if (token is EofToken)
                return result; // OK
            else if (Parser.IsBlockEndDelimiter(token))
                return result; // OK;
            else if (Parser.IsStatementDelimiter(token))
                this.ReportParserError(result, SemanticErrors.MissingStatement, token);
            else
                this.ReportParserError(result, SemanticErrors.CodeAfterReturnStatement, token);
            return result;
        }

        #endregion

        #region 3.4.5.2 Expressions

        // <expression> ::=
        //      <assignment> |
        //      <basic expression>
        // <assignment> ::= <assignment target> assignmentOperator <expression>
        // <basic expression> ::= <primary> [<messages> <cascaded messages>]
        // <assignment target> := identifier
        // <primary> ::=
        //      identifier |
        //      <literal> |
        //      <block constructor> |
        //      ( '(' <expression> ')' )

        protected virtual ExpressionNode ParseExpression(SemanticNode parent, Token token)
        {
            // Tricky ... first try for <assignment>
            if (token is IdentifierToken)
            {
                // In here, we either have an <assignment target> or <primary> of a <basic expression>
                IdentifierToken identifier = (IdentifierToken)token;

                // Try to check for assignmentOperator
                token = this.GetNextTokenxx(Preference.Default);
                if (token is AssignmentOperatorToken)
                    // OK, it's <assignment>
                    return this.ParseAssignment(parent, identifier, (AssignmentOperatorToken)token);

                // Must recover ... it is a <basic expression> anyway.
                // PARSE: identifier [<messages> <cascaded messages>]
                BasicExpressionNode result = new BasicExpressionNode(parent);
                this.ParseBaseicExpressionMessages(result, new VariableReferenceleNode(result, identifier), token);
                return result;
            };
            return this.ParseBasicExpression(parent, token);
        }

        protected virtual AssignmentNode ParseAssignment(SemanticNode parent, IdentifierToken identifier, AssignmentOperatorToken assignmentOperator)
        {
            // PARSE: <assignment> ::= <assignment target> assignmentOperator <expression>
            //      <assignment target> := identifier
            AssignmentNode result = new AssignmentNode(parent, identifier, assignmentOperator);

            Token token = this.GetNextTokenxx(Preference.NegativeSign);
            ExpressionNode expression = this.ParseExpression(result, token);

            if (expression == null)
                this.ReportParserError(result, SemanticErrors.MissingExpression, token);
            else
                result.SetContents(expression);

            return result;
        }

        protected virtual BasicExpressionNode ParseBasicExpression(SemanticNode parent, Token token)
        {
            // PARSE: <basic expression> ::= <primary> [<messages> <cascaded messages>]
            BasicExpressionNode result = new BasicExpressionNode(parent);

            IPrimaryNode primary = this.ParsePrimary(result, token);
            if (primary == null)
            {
                this.ReportParserError(result, SemanticErrors.MissingPrimary, token);
                return result;
            }

            token = this.GetNextTokenxx(Preference.Default);
            this.ParseBaseicExpressionMessages(result, primary, token);
            return result;
        }

        protected virtual void ParseBaseicExpressionMessages(BasicExpressionNode expression, IPrimaryNode primary, Token token)
        {
            MessageSequenceNode messages = this.ParseMessages(expression, token);
            if (messages == null)
            {
                expression.SetContents(primary, null, null);
                return;
            }

            token = this.GetNextTokenxx(Preference.Default);
            CascadeMessageSequenceNode cascadeMessages = this.ParseCascadeMessageSequenceNode(expression, token);

            expression.SetContents(primary, messages, cascadeMessages);
        }

        protected virtual IPrimaryNode ParsePrimary(IPrimaryParentNode parent, Token token)
        {
            // PARSE: <primary> ::= identifier | <literal> | <block constructor> | ( '(' <expression> ')' )
            if (token is IdentifierToken)
                return new VariableReferenceleNode(parent, (IdentifierToken)token);
            else if (Parser.IsBlockStartDelimiter(token))
                return this.ParseBlock(parent, (SpecialCharacterToken)token);
            else if (Parser.IsOpeningParenthesis(token))
                return this.ParseParenthesizedExpression(parent, (SpecialCharacterToken)token);
            else
                return this.ParseLiteral(parent, token);
        }

        protected virtual ParenthesizedExpressionNode ParseParenthesizedExpression(IPrimaryParentNode parent, SpecialCharacterToken leftParenthesis)
        {
            // PARSE: '(' <expression> ')'
            ParenthesizedExpressionNode result = new ParenthesizedExpressionNode(parent, leftParenthesis);

            Token token = this.GetNextTokenxx(Preference.NegativeSign);
            ExpressionNode expression = this.ParseExpression(result, token);

            if (expression == null)
            {
                this.ReportParserError(result, SemanticErrors.MissingExpression, token);
                return result;
            }

            // Ensure the parenthese are properly closed.
            token = this.GetNextTokenxx(Preference.Default);
            SpecialCharacterToken rightParenthesis = null;
            if (Parser.IsClosingParenthesis(token))
            {
                rightParenthesis = (SpecialCharacterToken)token;
            }
            else
            {
                this.ReportParserError(result, SemanticErrors.MissingExpressionClosingParenthesis, token);
                this.ResidueToken = token;
            }

            result.SetContents(expression, rightParenthesis);
            return result;
        }

        #endregion

        #region 3.4.5.3 Messages

        /// <summary>
        /// Parses a message sequence as defined in X3J20 "3.4.5.3 Messages".
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="token">First token of the message node. If the token is message token, it is returned in the residueToken parameter.</param>
        /// <returns>Message sequence node or null if the given token is not a message token.</returns>
        /// <remarks>
        /// This mehtod does not registers errors if the given token is not a message token.
        /// The 'offending' token is returned in the residueToken parameter, and it is 
        /// responsibility of the called to determine what to do.
        /// </remarks>
        protected virtual MessageSequenceNode ParseMessages(IMessageSequenceParentNode parent, Token token)
        {
            return this.ParseMessages(parent, token, MessageType.All);
        }

        protected virtual MessageSequenceNode ParseMessages(IMessageSequenceParentNode parent, Token token, MessageType type)
        {
            // <messages> ::=
            //      (<unary message>+ <binary message>* [<keyword message>] ) |
            //      (<binary message>+ [<keyword message>] ) |
            //      <keyword message>
            // <unary message> ::= unarySelector
            // <binary message> ::= binarySelector <binary argument>
            // <binary argument> ::= <primary> <unary message>*
            // <keyword message> ::= (keyword <keyword argument> )+
            // <keyword argument> ::= <primary> <unary message>* <binary message>*

            if ((token is IdentifierToken) && ((type & MessageType.Unary) != 0))
                // (<unary message>+ <binary message>* [<keyword message>] )
                return this.ParseUnaryBinaryKeywordMessageSequence(parent, (IdentifierToken)token);

            if ((token is BinarySelectorToken) && ((type & MessageType.Binary) != 0))
                // (<binary message>+ [<keyword message>] ) 
                return this.ParseBinaryKeywordMessageSequence(parent, (BinarySelectorToken)token);

            if ((token is KeywordToken) && ((type & MessageType.Keyword) != 0))
                // <keyword message>
                return this.ParseKeywordMessageSequence(parent, (KeywordToken)token);

            this.ResidueToken = token; // Not for us ... let others give it a try.
            return null;
        }

        protected virtual UnaryBinaryKeywordMessageSequenceNode ParseUnaryBinaryKeywordMessageSequence(IMessageSequenceParentNode parent, IdentifierToken selector)
        {
            // PARSE: // <unary message>+ <binary message>* [<keyword message>]
            UnaryBinaryKeywordMessageSequenceNode result = new UnaryBinaryKeywordMessageSequenceNode(parent);

            // NB: ParseUnaryMessage() cannot fail, so we don't check result 
            UnaryMessageNode message = this.ParseUnaryMessage(result, selector);

            Token token = this.GetNextTokenxx(Preference.Default);
            MessageSequenceNode nextMessage = this.ParseMessages(result, token, MessageType.All);

            result.SetContents(message, nextMessage);
            return result;
        }

        protected virtual BinaryKeywordMessageSequenceNode ParseBinaryKeywordMessageSequence(IMessageSequenceParentNode parent, BinarySelectorToken selector)
        {
            // PARSE: <binary message>+ [<keyword message>]
            BinaryKeywordMessageSequenceNode result = new BinaryKeywordMessageSequenceNode(parent);

            // NB: ParseBinaryMessage() cannot fail, so we don't check result 
            BinaryMessageNode message = this.ParseBinaryMessage(result, selector);

            Token token = this.GetNextTokenxx(Preference.Default);
            BinaryKeywordOrKeywordMessageSequenceNode nextMessage = (BinaryKeywordOrKeywordMessageSequenceNode)
                this.ParseMessages(result, token, MessageType.Binary | MessageType.Keyword);

            result.SetContents(message, nextMessage);
            return result;
        }

        protected virtual KeywordMessageSequenceNode ParseKeywordMessageSequence(IMessageSequenceParentNode parent, KeywordToken token)
        {
            // PARSE: <keyword message>
            KeywordMessageSequenceNode result = new KeywordMessageSequenceNode(parent);

            // NB: ParseKeywordMessage() cannot fail, so we don't check result 
            KeywordMessageNode message = this.ParseKeywordMessage(result, token);

            result.SetContents(message);
            return result;
        }

        protected virtual UnaryMessageNode ParseUnaryMessage(MessageSequenceBase parent, IdentifierToken token)
        {
            // PARSE: <unary message> ::= unarySelector
            return new UnaryMessageNode(parent, token);
        }

        protected virtual BinaryMessageNode ParseBinaryMessage(MessageSequenceBase parent, BinarySelectorToken selector)
        {
            // PARSE: <binary message> ::= binarySelector <binary argument>
            BinaryMessageNode result = new BinaryMessageNode(parent, selector);

            Token token = this.GetNextTokenxx(Preference.NegativeSign);
            // Parse the binary argument ... ParseBinaryArgument() does not fail and reports errors self.
            BinaryArgumentNode argument = this.ParseBinaryArgument(result, token);

            if (argument != null)
                result.SetContents(argument);
            return result;
        }

        protected virtual KeywordMessageNode ParseKeywordMessage(MessageSequenceNode parent, KeywordToken selector)
        {
            // PARSE: <keyword message> ::= (keyword <keyword argument> )+
            KeywordMessageNode result = new KeywordMessageNode(parent);

            List<KeywordToken> selectorTokens = new List<KeywordToken>();
            List<KeywordArgumentNode> arguments = new List<KeywordArgumentNode>();

            Token token = this.GetNextTokenxx(Preference.NegativeSign);
            selectorTokens.Add(selector);
            arguments.Add(this.ParseKeywordArgument(result, token)); // NB: ParseKeywordArgument() may not fail.

            token = this.GetNextTokenxx(Preference.Default);

            while (token is KeywordToken)
            {
                selectorTokens.Add((KeywordToken)token);
                token = this.GetNextTokenxx(Preference.NegativeSign);
                arguments.Add(this.ParseKeywordArgument(result, token)); // NB: ParseKeywordArgument() may not fail.

                token = this.GetNextTokenxx(Preference.Default);
            }

            result.SetContents(selectorTokens, arguments);
            this.ResidueToken = token;
            return result;
        }

        protected virtual BinaryArgumentNode ParseBinaryArgument(BinaryMessageNode parent, Token token)
        {
            // PARSE: <binary argument> ::= <primary> <unary message>*
            BinaryArgumentNode result = new BinaryArgumentNode(parent);

            IPrimaryNode primary = this.ParsePrimary(result, token);
            if (primary == null)
            {
                this.ReportParserError(result, SemanticErrors.MissingPrimary, token);
                return result;
            }

            token = this.GetNextTokenxx(Preference.Default);

            UnaryMessageSequenceNode messages = null;
            if (token is IdentifierToken)
                // <unary message>*
                messages = this.ParseUnaryMessageSequence(result, (IdentifierToken)token);
            else
                this.ResidueToken = token;

            result.SetContents(primary, messages);
            return result;
        }

        protected virtual KeywordArgumentNode ParseKeywordArgument(KeywordMessageNode parent, Token token)
        {
            // PARSE: <keyword argument> ::= <primary> <unary message>* <binary message>*
            KeywordArgumentNode result = new KeywordArgumentNode(parent);

            IPrimaryNode primary = this.ParsePrimary(result, token);
            if (primary == null)
            {
                this.ReportParserError(result, SemanticErrors.MissingPrimary, token);
                return result;
            }

            token = this.GetNextTokenxx(Preference.Default);

            BinaryOrBinaryUnaryMessageSequenceNode messages = null;
            if (token is IdentifierToken)
                // <unary message>*
                messages = this.ParseUnaryBinaryMessageSequence(result, (IdentifierToken)token);
            else if (token is BinarySelectorToken)
                messages = this.ParseBinaryMessageSequence(result, (BinarySelectorToken)token);
            else
                this.ResidueToken = token;

            result.SetContents(primary, messages);
            return result;
        }

        protected virtual UnaryMessageSequenceNode ParseUnaryMessageSequence(IMessageSequenceParentNode parent, IdentifierToken selector)
        {
            // PARSE: <unary message>*
            UnaryMessageSequenceNode result = new UnaryMessageSequenceNode(parent);

            // NB: ParseUnaryMessage() cannot fail, so we don't check result 
            UnaryMessageNode message = this.ParseUnaryMessage(result, selector);

            Token token = this.GetNextTokenxx(Preference.Default);

            UnaryMessageSequenceNode nextMessage = null;
            if (token is IdentifierToken)
                nextMessage = this.ParseUnaryMessageSequence(result, (IdentifierToken)token);
            else
                this.ResidueToken = token;

            result.SetContents(message, nextMessage);
            return result;
        }

        protected virtual UnaryBinaryMessageSequenceNode ParseUnaryBinaryMessageSequence(IMessageSequenceParentNode parent, IdentifierToken selector)
        {
            // PARSE: <unary message>* <binary message>*
            UnaryBinaryMessageSequenceNode result = new UnaryBinaryMessageSequenceNode(parent);

            // NB: ParseUnaryMessage() cannot fail, so we don't check result 
            UnaryMessageNode message = this.ParseUnaryMessage(result, selector);

            Token token = this.GetNextTokenxx(Preference.Default);

            BinaryOrBinaryUnaryMessageSequenceNode nextMessage = null;
            if (token is IdentifierToken)
                // <unary message>*
                nextMessage = this.ParseUnaryBinaryMessageSequence(result, (IdentifierToken)token);
            else if (token is BinarySelectorToken)
                // <binary message>*
                nextMessage = this.ParseBinaryMessageSequence(result, (BinarySelectorToken)token);
            else
                this.ResidueToken = token;

            result.SetContents(message, nextMessage);
            return result;
        }

        protected virtual BinaryMessageSequenceNode ParseBinaryMessageSequence(IMessageSequenceParentNode parent, BinarySelectorToken selector)
        {
            // PARSE: <binary message>*
            BinaryMessageSequenceNode result = new BinaryMessageSequenceNode(parent);

            // NB: ParseBinaryMessage() cannot fail, so we don't check result 
            BinaryMessageNode message = this.ParseBinaryMessage(result, selector);

            Token token = this.GetNextTokenxx(Preference.Default);

            BinaryMessageSequenceNode nextMessage = null;
            if (token is BinarySelectorToken)
                nextMessage = this.ParseBinaryMessageSequence(result, (BinarySelectorToken)token);
            else
                this.ResidueToken = token;

            result.SetContents(message, nextMessage);
            return result;
        }

        protected virtual CascadeMessageSequenceNode ParseCascadeMessageSequenceNode(ICascadeMessageSequenceParentNode parent, Token semicolon)
        {
            // PARSE: <cascaded messages> ::= (';' <messages>)*
            if (!Parser.IsCascadeDelimiter(semicolon))
            {
                this.ResidueToken = semicolon;
                return null; // Not a cascade message ... return null.
            }
            CascadeMessageSequenceNode result = new CascadeMessageSequenceNode(parent, (SpecialCharacterToken)semicolon);

            Token token = this.GetNextTokenxx(Preference.Default);
            MessageSequenceNode messages = this.ParseMessages(result, token);

            if (messages == null)
            {
                this.ReportParserError(result, SemanticErrors.MissingMessagePattern, token);
                return result;
            }

            token = this.GetNextTokenxx(Preference.Default);

            CascadeMessageSequenceNode nextCascade = null;
            if (Parser.IsCascadeDelimiter(token))
                nextCascade = this.ParseCascadeMessageSequenceNode(result, (SpecialCharacterToken)token);
            else
                this.ResidueToken = token;

            result.SetContents(messages, nextCascade);
            return result;
        }

        [Flags]
        protected enum MessageType
        {
            Unary = 1,
            Binary = 2,
            Keyword = 4,
            All = 7,
        }

        #endregion

        #region 3.4.6 Literals

        /// <summary>
        /// Parse a literal node as described in X3J20 "3.4.6 Literals".
        /// </summary>
        /// <remarks>
        /// If the given token is not a legal token for a literal node,
        /// it is simply returned in the residueToken output parameter, 
        /// and the function returns null.
        /// </remarks>
        /// <param name="parent">Parent node that defines the literal node.</param>
        /// <param name="token">First token of the literal node.</param>
        /// <returns>A literal node, or null in case of non-literal token.</returns>
        protected virtual LiteralNode ParseLiteral(ILiteralNodeParent parent, Token token)
        {
            // PARSE: <literal> ::= <number literal> | <string literal> | <character literal> | 
            //      <symbol literal> | <selector literal> | <array literal>

            // <string literal> ::= quotedString
            if (token is StringToken)   // 'example'
                return new StringLiteralNode(parent, (StringToken)token);
            // <symbol literal> ::= hashedString
            if (token is HashedStringToken) // #'example'
                return new SymbolLiteralNode(parent, (HashedStringToken)token);
            // <character literal> ::= quotedCharacter
            if (token is CharacterToken)    // $e
                return new CharacterLiteralNode(parent, (CharacterToken)token);
            // <selector literal> ::= quotedSelector
            if (token is QuotedSelectorToken)   // #example ..or.. #example: ..or.. #example:example: ..or.. #+
                return new SelectorLiteralNode(parent, (QuotedSelectorToken)token);

            // <number literal> ::= ['-'] <number>
            if (token is NegativeSignToken)
                return this.ParseNegativeNumericLiteralNode(parent, (NegativeSignToken)token);
            // <number> ::= integer | float | scaledDecimal
            if (token is SmallIntegerToken)
                return new SmallIntegerLiteralNode(parent, (SmallIntegerToken)token, null);
            if (token is LargeIntegerToken)
                return new LargeIntegerLiteralNode(parent, (LargeIntegerToken)token, null);
            if (token is FloatEToken)
                return new FloatELiteralNode(parent, (FloatEToken)token, null);
            if (token is FloatDToken)
                return new FloatDLiteralNode(parent, (FloatDToken)token, null);
            if (token is ScaledDecimalToken)
                return new ScaledDecimalLiteralNode(parent, (ScaledDecimalToken)token, null);

            // <array literal> ::= '#(' <array element>* ')'
            // <array element> ::= <literal> | identifier
            if (Parser.IsLiteralArrayPrefix(token))
                return this.ParseArrayLiteral(parent, (SpecialCharacterToken)token);

            this.ResidueToken = token;
            return null;
        }

        protected virtual LiteralNode ParseNegativeNumericLiteralNode(ILiteralNodeParent parent, NegativeSignToken negativeSign)
        {
            // PARSE: <number> ::= integer | float | scaledDecimal
            Token token = this.GetNextTokenxx(Preference.NegativeSign);

            if (token is SmallIntegerToken)
                return new SmallIntegerLiteralNode(parent, (SmallIntegerToken)token, negativeSign);
            if (token is LargeIntegerToken)
                return new LargeIntegerLiteralNode(parent, (LargeIntegerToken)token, negativeSign);
            if (token is FloatEToken)
                return new FloatELiteralNode(parent, (FloatEToken)token, negativeSign);
            if (token is FloatDToken)
                return new FloatDLiteralNode(parent, (FloatDToken)token, negativeSign);
            if (token is ScaledDecimalToken)
                return new ScaledDecimalLiteralNode(parent, (ScaledDecimalToken)token, negativeSign);

            // We don't know what that is.... the negative sign token thrown away and lost :-/
            this.ReportParserError(parent, SemanticErrors.UnrecognizedLiteral, token);
            this.ResidueToken = token;
            return null;
        }

        protected virtual ArrayLiteralNode ParseArrayLiteral(ILiteralNodeParent parent, SpecialCharacterToken arrayToken)
        {
            // PARSE: <array literal> ::= '#(' <array element>* ')'
            // <array element> ::= <literal> | identifier

            Token token = this.GetNextTokenxx(Preference.Default);
            if (!Parser.IsOpeningParenthesis(token))
            {
                this.ReportParserError(parent, SemanticErrors.MissingLiteralArrayOpeningParenthesis, token);
                // The hash token is thrown away and lost :-/   ... only the current token is passed on.
                this.ResidueToken = token;
                return null; 
            }
            List<LiteralNode> elements = new List<LiteralNode>();
            ArrayLiteralNode result = new ArrayLiteralNode(parent, arrayToken, (SpecialCharacterToken)token);

            // Process tokens inside the array ...
            while (true)
            {
                // ... get next token in the array ...
                token = this.GetNextTokenxx(Preference.NegativeSign | Preference.UnquotedSelector);

                // Is this closing parenthesis? 
                if (Parser.IsClosingParenthesis(token))
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
                    this.ReportParserError(parent, SemanticErrors.MissingLiteralArrayClosingParenthesis, token);
                    this.ResidueToken = token;
                    return result;
                }

                // PARSE: <array element> ::= <literal> | identifier
                if (token is ILiteralArrayIdentifierToken)
                {
                    // identifier ... special case only inside arrays.
                    elements.Add(new IdentifierLiteralNode(result, (ILiteralArrayIdentifierToken)token));
                }
                else
                {
                    // ... it's not identifier, so it must be an literal
                    LiteralNode element = this.ParseLiteral(result, token);
                    if (element == null)
                    {
                        // Report error in souce code ... here, it must be a literal
                        this.ReportParserError(result, SemanticErrors.UnrecognizedLiteral, token);
                        result.SetContents(elements, null);
                        return result;
                    }
                    else
                    {
                        // ... add the child element to the array elements.
                        elements.Add(element);
                    }
                }
            }
        }

        #endregion
    }
}
