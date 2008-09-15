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
            if (_expressions.Count == 0)
            {
                _expressions.Add(Ast.Null());
            }
        }

        private Expression ParseStatement()
        {
            // First token in any statement should be a receiver (or a block, but we'll handle those later):
            Expression expr = ParseReceiver();

            // If the first token wasn't a receiver, then this has to be an assignment:
            if (expr == null)
            {
                return ParseAssignStatement();
            }

            if (_tokens.Peek().Name == "Assign")
            {
                // Assign a previously declared variable:
                return ParseAssignStatement(expr);
            }

            // Should we just return the receiver?
            if (VerifyEndOfStatement())
            {
                return expr;
            }

            // Parse messages to the receiver:
            expr = ParseMessages(expr);

            if (VerifyEndOfStatement())
            {
                return expr;
            }

            throw new Exception("End of statement expected.");
        }

        /// <summary>
        /// Assign a global variable.
        /// </summary>
        /// <returns></returns>
        private Expression ParseAssignStatement()
        {
            Lexer.Token nameToken = _tokens.Read();
            if (nameToken.Name != "Identifier")
            {
                throw new Exception("Identifier expected.");
            }
            string variableName = nameToken.Value;

            if (_tokens.Read().Name != "Assign")
            {
                throw new Exception("':=' expected.");
            }

            Expression value = ParseStatement();

            if (VerifyEndOfStatement())
            {
                return Ast.Call(
                        typeof(GlobalDictionary).GetMethod("Add", new Type[] { typeof(string), typeof(SmallObject) }),
                        Ast.Constant(variableName), value);
            }

            throw new Exception("End of statement expected.");
        }

        /// <summary>
        /// Assign a global variable that has already been created.
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        private Expression ParseAssignStatement(Expression receiver)
        {
            if (_tokens.Read().Name != "Assign")
            {
                throw new Exception("':=' expected.");
            }

            Expression value = ParseStatement();

            return Ast.Call(
                typeof(GlobalDictionary).GetMethod("Set", new Type[] { typeof(string), typeof(SmallObject) }),
                receiver, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Null if the next token set is not a receiver.</returns>
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

            return null;
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

            if (GlobalDictionary.Contains(token.Value))
            {
                _tokens.Read();
                return Ast.Call(
                        typeof(GlobalDictionary).GetMethod("Get", new Type[] { typeof(string) }),
                        Ast.Constant(token.Value));
            }

            return null;
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
            List<Expression> parameters = new List<Expression>();
            string selectorName = string.Empty;

            do
            {
                if (_tokens.Peek().Name == "Selector")
                {
                    selectorName += _tokens.Read().Value;
                }
                else
                {
                    throw new Exception("Selector was expected.");
                }

                Expression arg = ParseParameter();
                if (arg == null)
                {
                    throw new Exception("Parameter expected.");
                }
                else
                {
                    parameters.Add(arg);
                }
            } while (!_tokens.AtEnd && _tokens.Peek().Name == "Selector");

            SmallSymbol binaryMessageSymbol = new SmallSymbol(string.Format("#{0}", selectorName));

            MethodInfo sendMessage;
            if (parameters.Count == 1)
            {
                sendMessage = typeof(SmallObject).GetMethod("SendMessage", new Type[] { typeof(SmallSymbol), typeof(SmallObject) });
            }
            else
            {
                sendMessage = typeof(SmallObject).GetMethod("SendMessage", new Type[] { typeof(SmallSymbol), typeof(SmallObject[]) });
            }

            parameters.Insert(0, Ast.Constant(binaryMessageSymbol));

            receiver = Ast.Call(receiver, sendMessage, parameters.ToArray());

            return receiver;
        }

        /// <summary>
        /// Parse a message parameter.
        /// </summary>
        /// <returns></returns>
        private Expression ParseParameter()
        {
            if (_tokens.Peek().Name == "OpenParen")
            {
                return ParseSubStatement();
            }
            else
            {
                return ParseReceiver();
            }
        }

        /// <summary>
        /// Parse a receiver/message combination.
        /// </summary>
        /// <returns></returns>
        private Expression ParseSubStatement()
        {
            if (_tokens.Peek().Name != "OpenParen")
            {
                throw new Exception("'(' expected.");
            }
            _tokens.Read();

            Expression receiver = ParseReceiver();

            // Should we just return the receiver?
            if (_tokens.Peek().Name == "CloseParen")
            {
                _tokens.Read();
                return receiver;
            }

            // Parse messages to the receiver:
            receiver = ParseMessages(receiver);

            if (_tokens.Peek().Name != "CloseParen")
            {
                throw new Exception("')' expected.");
            }
            _tokens.Read();

            return receiver;
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
