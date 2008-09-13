using Microsoft.Scripting;
using Microsoft.Scripting.Ast;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IronSmalltalk.Compiler
{
    public class Parser
    {
        #region Variables

        /// <summary>
        /// Dummy span.
        /// I don't know what this is actually for yet.
        /// </summary>
        private SourceSpan _span = new SourceSpan(new SourceLocation(255, 255, 255), new SourceLocation(255, 255, 255));

        #endregion

        #region Variables

        private static Lexer _lexer;

        private Lexer.TokenStream _tokens;
        private LambdaExpression _result;
        private List<Expression> _expressions;
        private LambdaBuilder _program;

        #endregion

        #region Constructors

        static Parser()
        {
            _lexer = new Lexer("LexerGrammar.xml");
        }

        public Parser(SourceUnit source)
        {
            _tokens = _lexer.Tokenize(source.GetCode());
        }

        #endregion

        #region Properties

        public LambdaExpression Result
        {
            get
            {
                return _result;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parse source code, generating a LamdaExpression.
        /// </summary>
        /// <returns>False if there is an error in the source code.</returns>
        public bool Parse()
        {
            try
            {
                _program = Ast.Lambda("IronSmalltalk", typeof(object));
                ParseStatements();

                // The last expression is returned:
                _expressions[_expressions.Count - 1] = Ast.Return(_expressions[_expressions.Count - 1]);
                // ...of course, if a return statement is encountered before the end of the sequence,
                //    that expression will be returned instead.

                _program.Body = Ast.Block(_expressions);
                _result = _program.MakeLambda();
            }
            catch (Exception)
            {
                // TO DO: Report on the error encountered.
                return false;
            }

            return true;
        }

        private void ParseStatements()
        {
            _expressions = new List<Expression>();
            while (!_tokens.AtEnd)
            {
                _expressions.Add(ParseStatement());
            }
        }

        private Expression ParseStatement()
        {
            // First token in any statement should be a receiver (or a block, but we'll handle those later):
            Expression expr = ParseReceiver();

            // Should we just return the receiver?
            if (VerifyEndOfStatement())
            {
                return expr;
            }

            // TO DO: try to parse an assignment.

            // Parse messages to the receiver:
            expr = ParseMessages(expr);

            if (VerifyEndOfStatement())
            {
                return expr;
            }

            throw new Exception("End of statement expected.");
        }

        private Expression ParseReceiver()
        {
            Expression receiver;

            // Is the message receiver a primitive type?
            receiver = ParsePrimitiveReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            // Receiver must be a variable.
            receiver = ParseObjectReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            throw new Exception("Invalid receiver.");
        }

        private Expression ParsePrimitiveReceiver()
        {
            Expression receiver;

            receiver = ParseCharacterReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            receiver = ParseIntegerReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            receiver = ParseFloatReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            receiver = ParseSymbolReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            receiver = ParseStringReceiver();
            if (receiver != null)
            {
                return receiver;
            }

            // Must not be a primitive receiver.
            return null;
        }

        private Expression ParseCharacterReceiver()
        {
            Lexer.Token token = _tokens.Peek();

            if (token.Name == "Character")
            {
                token = _tokens.Read();
                return Ast.Constant(new SmallCharacter(token.Value));
            }

            return null;
        }

        private Expression ParseIntegerReceiver()
        {
            Lexer.Token token = _tokens.Peek();

            if (token.Name == "Integer")
            {
                token = _tokens.Read();
                return Ast.Constant(new SmallInteger(token.Value));
            }

            return null;
        }

        private Expression ParseFloatReceiver()
        {
            Lexer.Token token = _tokens.Peek();

            if (token.Name == "Float")
            {
                token = _tokens.Read();
                return Ast.Constant(new SmallFloat(token.Value));
            }

            return null;
        }

        private Expression ParseSymbolReceiver()
        {
            Lexer.Token token = _tokens.Peek();

            if (token.Name == "Symbol")
            {
                token = _tokens.Read();
                return Ast.Constant(new SmallSymbol(token.Value));
            }

            return null;
        }

        private Expression ParseStringReceiver()
        {
            Lexer.Token token = _tokens.Peek();

            if (token.Name == "String")
            {
                token = _tokens.Read();
                return Ast.Constant(new SmallString(token.Value));
            }

            return null;
        }

        private Expression ParseObjectReceiver()
        {
            Lexer.Token token = _tokens.Peek();

            if (token.Name != "Identifier")
            {
                return null;
            }

            throw new NotImplementedException("Non-primitive receivers are not yet implemented.");
        }

        private Expression ParseMessages(Expression receiver)
        {
            bool firstMessage = true;
            do
            {
                if (!firstMessage)
                {
                    // To continue sending messages to the receiver, you must have a message separator:
                    if (!VerifyMessageSeparator())
                    {
                        break;
                    }
                }

                receiver = ParseMessage(receiver);

                firstMessage = false;
            } while (!_tokens.AtEnd && (_tokens.Peek().Name == "MessageSeparator"));

            return receiver;
        }

        private Expression ParseMessage(Expression receiver)
        {
            if (_tokens.Peek().Name == "Identifier")
            {
                return ParseUnaryMessage(receiver);
            }
            if (_tokens.Peek().Name == "Selector")
            {
                return ParseBinaryMessage(receiver);
            }
            throw new Exception("Parser can only handle unary expressions.");
        }

        private Expression ParseUnaryMessage(Expression receiver)
        {
            Lexer.Token token = _tokens.Read();
            if (token.Name != "Identifier")
            {
                throw new Exception("Invalid unary selector.");
            }

            SmallSymbol unaryMessageSymbol = new SmallSymbol(string.Format("#{0}", token.Value));

            MethodInfo sendMessage = typeof(SmallObject).GetMethod("SendMessage", new Type[] { typeof(SmallSymbol) });

            receiver = Ast.Call(receiver, sendMessage, Ast.Constant(unaryMessageSymbol));

            return receiver;
        }

        private Expression ParseBinaryMessage(Expression receiver)
        {
            List<Lexer.Token> selectors = new List<Lexer.Token>();
            List<Lexer.Token> arguments = new List<Lexer.Token>();

            while (true)
            {
                if (_tokens.Peek().Name == "Selector")
                {
                    selectors.Add(_tokens.Read());
                }
                else
                {
                    throw new Exception("Selector was expected.");
                }

                if (_tokens.Peek().Name != "Selector")
                {
                    arguments.Add(_tokens.Read());
                }
                else
                {
                    throw new Exception("Argument was expected.");
                }
            }
        }

        private bool VerifyEndOfStatement()
        {
            if (_tokens.AtEnd)
            {
                return true;
            }
            if (_tokens.Peek().Name == "ExpressionSeparator")
            {
                _tokens.Read();
                return true;
            }
            return false;
        }

        private bool VerifyMessageSeparator()
        {
            if (_tokens.Peek().Name != "MessageSeparator")
            {
                return false;
            }
            _tokens.Read();
            return true;
        }

        /*
        private Expression ParseExpression()
        {
            Lexer.Token token = _tokens.Read();
            if (token.Name == "Identifier")
            {
                if (_tokens.Peek().Name == "Assign")
                {
                    return ParseAssign(token);
                }
            }
        }

        private Expression ParseAssign(Lexer.Token identifier)
        {
        }

        private Expression ParseLocalBlock()
        {
            if (_tokens.Read().Name != "LocalBlock")
            {
                throw new Exception("'|' expected.");
            }

            bool done = false;
            while (!done)
            {
                if (_tokens.AtEnd)
                {
                    throw new Exception("'|' expected.");
                }

                Lexer.Token token = _tokens.Read();
                switch (token.Name)
                {
                    case "LocalBlock":
                        done = true;
                        break;
                    case "Identifier":

                }
            }
        }
        */

        #endregion
    }
}
