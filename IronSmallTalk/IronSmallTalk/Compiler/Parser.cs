using Microsoft.Scripting;
using Microsoft.Scripting.Ast;
using System;
using System.Collections.Generic;

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
            if (expr == null)
            {
                throw new Exception("Invalid receiver.");
            }

            if (TryParseReturnReceiver(ref expr))
            {
                return expr;
            }
            // else, try to parse an assignment
            // else, do other stuff with the expression
            
            return expr;
        }

        private Expression ParseReceiver()
        {
            Lexer.Token token = _tokens.Read();
            if (token.Name == "Character")
            {
                return Ast.Constant(token.Value[1]);
            }
            return null;
        }

        private bool TryParseReturnReceiver(ref Expression expr)
        {
            if (_tokens.AtEnd)
            {
                expr = Ast.Return(expr);
                return true;
            }
            if (_tokens.Peek().Name == "ExpressionSeparator")
            {
                expr = Ast.Return(expr);
                _tokens.Read();
                return true;
            }
            return false;
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
