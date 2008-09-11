using IronSmalltalk.Compiler;
using Microsoft.Scripting;
using Microsoft.Scripting.Ast;
using Microsoft.Scripting.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IronSmalltalk
{
    /// <summary>
    /// Language context for IronSmalltalk.
    /// </summary>
    public class IronSmalltalkLanguageContext : LanguageContext
    {
        #region Variables

        /// <summary>
        /// Dummy span.
        /// I don't know what this is actually for yet.
        /// </summary>
        private SourceSpan _span = new SourceSpan(new SourceLocation(255, 255, 255), new SourceLocation(255, 255, 255));

        #endregion

        #region Constructors

        public IronSmalltalkLanguageContext(ScriptDomainManager manager)
            : base(manager)
        {
            Binder = new IronSmalltalkBinder(manager);

            // TO-DO: provide built-in functions here.
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parse a source code and return an AST, here only return the test AST.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>an AST</returns>
        public override LambdaExpression ParseSourceCode(CompilerContext context)
        {
            // Call the Smalltalk parser:
            Parser parser = new Parser(context.SourceUnit);
            parser.Parse();

            // Resturn the generated AST:
            return parser.Result;

            /*
            // Create lambda expression
            LambdaBuilder program = Utils.Lambda(typeof(object), "ASTDLRTest");
            
            // Add statements
            program.Body = Utils.Block(_span, GetStatements(program));
            
            // Return result
            return program.MakeLambda();
            */
        }

        /// <summary>
        /// Get statements.
        /// </summary>
        /// <returns>statements</returns>
        private List<Expression> GetStatements(LambdaBuilder program)
        {
            List<Expression> statements = new List<Expression>();

            #region n = 4; r = n;
            // n = 4
            Expression n = program.CreateLocalVariable("n", typeof(int));
            statements.Add(
                Expression.Assign(
                    n,
                    Expression.Constant(4)
                )
            );

            // r = n
            Expression r = program.CreateLocalVariable("r", typeof(int));
            statements.Add(
                Expression.Assign(
                    r,
                    n
                )
            );
            #endregion

            #region r = r * (n - 1);
            // r = r * (n - 1)
            statements.Add(
                Expression.Assign(
                    r,
                    Expression.Multiply(
                        r,
                        Expression.Subtract(
                            n,
                            Expression.Constant(1)
                        )
                    )
                )
            );
            #endregion

            #region Console.WriteLine(r);
            // Console.WriteLine(r);
            MethodInfo consoleWriteLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) });
            statements.Add(
                Expression.Call(
                    consoleWriteLine,
                    r
                )
            );
            #endregion

            #region while(n>1) { r = r * (n-1); n = n - 1; }
            // while(n>1) { r = r * (n-1); n = n - 1; }
            statements.Add(
                Utils.While(
                    Expression.GreaterThan(
                        n,
                        Expression.Constant(1)
                    ),
                    Expression.Block(
                        Expression.Assign(r, Expression.Multiply(r, Expression.Subtract(n, Expression.Constant(1)))),
                        Expression.Assign(n, Expression.Subtract(n, Utils.Constant(1)))
                    ),
                    Utils.Empty(_span)
                )
            );
            #endregion

            #region Console.WriteLine(fact(5));
            // fact = function(n) { ... }
            Expression fact = program.CreateLocalVariable("fact", typeof(object));
            statements.Add(
                Expression.Assign(
                    fact,
                    factorialExpression()
                )
            );

            // fact(5)
            Expression.Action.Call(
                this.Binder,
                typeof(int),
                Expression.CodeContext(),
                fact,
                Expression.Constant(5)
            );

            // Console.WriteLine(fact(5))
            statements.Add(
                Expression.Call(
                    consoleWriteLine,
                    Expression.Action.Call(
                        this.Binder,
                        typeof(int),
                        Expression.CodeContext(),
                        fact,
                        Expression.Constant(5)
                    )
                )
            );
            #endregion

            return statements;
        }

        /// <summary>
        /// Build lambda expression for the factorial function.
        /// </summary>
        /// <returns>the lambda expression</returns>
        private Expression factorialExpression()
        {
            // Create lambda expression
            LambdaBuilder function = Utils.Lambda(typeof(int), "fact");

            // Declare parameter
            Expression n = function.CreateParameter("n", typeof(int));

            // Create statements
            List<Expression> statements = new List<Expression>();

            // r = n
            Expression r = function.CreateLocalVariable("r", typeof(int));
            statements.Add(
                Expression.Assign(
                    r,
                    n
                )
            );

            // while(n>1) { r = r * (n-1); n = n - 1; }
            statements.Add(
                Utils.While(
                    Expression.GreaterThan(
                        n,
                        Expression.Constant(1)
                    ),
                    Expression.Block(
                        Expression.Assign(r, Expression.Multiply(r, Expression.Subtract(n, Expression.Constant(1)))),
                        Expression.Assign(n, Expression.Subtract(n, Utils.Constant(1)))
                    ),
                    Utils.Empty(_span)
                )
            );

            // return r
            statements.Add(
                Expression.Return(r)
            );

            // Add statements
            function.Body = Utils.Block(_span, statements);

            // Return result
            return function.MakeLambda();
        }

        #endregion
    }
}