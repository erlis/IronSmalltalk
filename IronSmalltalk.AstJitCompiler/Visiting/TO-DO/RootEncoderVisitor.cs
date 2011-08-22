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
using IronSmalltalk.Compiler.Visiting;
using IronSmalltalk.Compiler.SemanticNodes;
using System.Linq.Expressions;
using IronSmalltalk.AstJitCompiler.Runtime;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Common;
using IronSmalltalk.Runtime.CodeGeneration.BindingScopes;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Execution.CallSiteBinders;
using IronSmalltalk.Runtime.Execution.Internals;
using IronSmalltalk.Runtime.Behavior;
using System.Dynamic;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    public abstract class RootEncoderVisitor<TResult, TNode> : EncoderVisitor<TResult>, IRootFunctionVisitor
        where TNode : FunctionNode
    {
        /// <summary>
        /// Binding lookup scope with reserved identifiers, e.g. "true", "false", "nil" etc.
        /// </summary>
        public BindingScope ReservedScope { get; private set; }

        /// <summary>
        /// Binding lookup scope with locally defined identifiers, e.g. arguments and temporary variables.
        /// </summary>
        protected BindingScope LocalScope { get; private set; }

        /// <summary>
        /// Binding lookup scope for identifiers of globals and similar, e.g. global variables, class or instance variables, pool variables etc.
        /// </summary>
        protected BindingScope GlobalScope { get; private set; }

        /// <summary>
        /// Collection of temporary variables bindings. We need this to define the vars in the AST block.
        /// </summary>
        protected readonly List<TemporaryBinding> Temporaries = new List<TemporaryBinding>();

        /// <summary>
        /// SymbolDocument needed for storing information necessary to emit 
        /// debugging symbol information for a source file.
        /// 
        /// If this property is set, the generator will emit debug(able) code.
        /// </summary>
        public SymbolDocumentInfo SymbolDocument { get; set; }

        /// <summary>
        /// Gets or sets binding restrictions to be applied together with the visited executable code.
        /// </summary>
        public BindingRestrictions BindingRestrictions { get; set; }

        /// <summary>
        /// Create a new root-function (method or initializer) visitor.
        /// </summary>
        /// <param name="runtime">Smalltalk runtime responsible for running the code.</param>
        /// <param name="globalScope">Binding lookup scope with global identifiers, e.g. globals, class variables, instance variables etc.</param>
        /// <param name="reservedScope">Binding lookup scope with reserved identifiers, e.g. "true", "false", "nil" etc.</param>
        protected RootEncoderVisitor(SmalltalkRuntime runtime, BindingScope globalScope, BindingScope reservedScope)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            if (globalScope == null)
                throw new ArgumentNullException("globalScope");
            if (reservedScope == null)
                throw new ArgumentNullException("reservedScope");

            this._runtime = runtime;
            this.GlobalScope = globalScope;
            this.ReservedScope = reservedScope;
            this.LocalScope = new BindingScope();
            this._returnLabel = null; // Lazy init
            this._homeContext = null; // Lazy init
        }

        protected internal override IRootFunctionVisitor RootVisitor
        {
            get { return this; }
        }

        #region interface IRootFunctionVisitor

        private LabelTarget _returnLabel;
        protected LabelTarget ReturnLabel
        {
            get
            {
                if (this._returnLabel == null)
                    this._returnLabel = Expression.Label(typeof(object), "return");
                return this._returnLabel;
            }
        }

        private readonly SmalltalkRuntime _runtime;
        public SmalltalkRuntime Runtime
        {
            get { return this._runtime; }
        }

        private CallSiteBinderCache _binderCache;
        public CallSiteBinderCache BinderCache
        {
            get
            {
                if (this._binderCache == null)
                    this._binderCache = CallSiteBinderCache.GetCache(this.Runtime);
                return this._binderCache;
            }
        }

        public abstract Symbol SuperLookupScope { get; }

        private Expression _homeContext;
        private ParameterExpression _homeContextVariable;
        public Expression HomeContext
        {
            get
            {
                if (this._homeContext == null)
                {
                    System.Reflection.ConstructorInfo ctor = typeof(HomeContext).GetConstructor(new Type[0]);
                    if (ctor == null)
                        throw new InternalCodeGenerationException(CodeGenerationErrors.InternalError);

                    // Semantics are as follows:
                    // .... = ( (_homeContext == null) ? (_homeContext = new HomeContext()) : _homeContext );
                    ParameterExpression homeContextVariable = Expression.Variable(typeof(HomeContext), "_homeContext");
                    this._homeContext = Expression.Condition(
                        Expression.ReferenceEqual(homeContextVariable, Expression.Constant(null, typeof(HomeContext))),
                        Expression.Assign(homeContextVariable, Expression.New(ctor)),
                        homeContextVariable,
                        typeof(HomeContext));
                    this._homeContextVariable = homeContextVariable;
                }
                return this._homeContext;
            }
        }

        #endregion

        protected Expression InternalVisitFunction(TNode node)
        {
            if (node == null)
                throw new ArgumentNullException();
            if (!node.Accept(Compiler.Visiting.ParseTreeValidatingVisitor.Current))
                throw new SemanticCodeGenerationException(CodeGenerationErrors.InvalidCode, node);

            this.DefineArguments(node);
            this.DefineTemporaries(node);

            StatementVisitor na;
            List<Expression> expressions = this.GenerateExpressions(node, out na);

#if DEBUG
            if (expressions.Count == 0)
                throw new InternalCodeGenerationException("We did expect at least ONE expression");

            foreach (var expr in expressions)
            {
                if (expr.Type != typeof(object))
                    throw new InternalCodeGenerationException(String.Format("Expression does not return object! \n{0}", expr));
            }
#endif

            Expression result;
            if ((this.Temporaries.Count == 0) && (expressions.Count == 1))
                result = expressions[0];
            else
                result = Expression.Block(this.Temporaries.Select(binding => binding.Expression), expressions);

            // Somebody requested explicit return
            if (this._returnLabel != null)
                result = Expression.Label(this._returnLabel, result);

            // Somebody requested the home context used for block returns ... we must handle this correcty with try ... catch ...
            if (this._homeContext != null) 
            {
                // Semantics:
                // HomeContext homeContext = new HomeContext();     ... however, this is lazy init'ed
                // try
                // {
                //      return <<result>>;
                // } 
                // .... this is how we would like to have it ... if we could. CLR limitations do not allow filters in dynamic methos ....
                // catch (BlockResult blockResult) where (blockResult.HomeContext == homeContext)       ... the where semantics are not part of C#
                // {
                //      return blockResult.Result;
                // }
                // .... therefore the following implementation ....
                // catch (BlockResult blockResult)
                // {
                //      if (blockResult.HomeContext == homeContext)
                //          return blockResult.Result;
                //      else
                //          throw;
                // }

                ParameterExpression blockResult = Expression.Parameter(typeof(BlockResult), "blockResult");
                CatchBlock catchBlock = Expression.Catch(
                    blockResult,
                    Expression.Condition(
                        Expression.ReferenceEqual(Expression.Field(blockResult, BlockResult.HomeContextField), this._homeContextVariable),
                        Expression.Field(blockResult, BlockResult.ValueField),
                        Expression.Rethrow(typeof(object))));

                result = Expression.Block(new ParameterExpression[] { _homeContextVariable }, Expression.TryCatch(result, catchBlock));
            }

            //result = Expression.TryCatch(result, Expression.Catch(typeof(ArgumentNullException), Expression.Constant("Null Ex!", typeof(object))));

            return result;
        }

        protected abstract void DefineArguments(TNode node);

        protected void DefineTemporaries(FunctionNode node)
        {
            foreach (TemporaryVariableNode tmp in node.Temporaries)
                this.DefineTemporary(tmp.Token.Value);
        }

        protected virtual List<Expression> GenerateExpressions(TNode node, out StatementVisitor visitor)
        {
            List<Expression> expressions = null;
            visitor = new StatementVisitor(this);

            if (node.Statements != null)
                expressions = node.Statements.Accept(visitor);

            if (expressions == null)
                expressions = new List<Expression>();
            return expressions;
        }

        protected internal override NameBinding GetBinding(string name)
        {
            NameBinding result = this.ReservedScope.GetBinding(name);
            if (result != null)
                return result;

            result = this.LocalScope.GetBinding(name);
            if (result != null)
                return result;

            result = this.GlobalScope.GetBinding(name);
            if (result != null)
                return result;

            return new ErrorBinding(name);
        }

        protected internal override Expression Return(Expression value)
        {
            return Expression.Return(this.ReturnLabel, value, typeof(object));
        }

        public Expression AddDebugInfo(Expression expression, int startLine, int startColumn, int endLine, int endColumn)
        {
            if (this.SymbolDocument == null)
                return expression;

            DebugInfoExpression debugInfo = Expression.DebugInfo(this.SymbolDocument, startLine, startColumn, endLine, endColumn);
            return Expression.Block(debugInfo, expression);
        }

        public Expression AddDebugInfo(Expression expression, IParseNode node)
        {
            var tokens = node.GetTokens().Concat(node.GetChildNodes().SelectMany(n => n.GetTokens()));
            SourceLocation start = tokens.Min(t => t.StartPosition);
            SourceLocation end = tokens.Min(t => t.StopPosition);
            return this.AddDebugInfo(expression, start.Line, start.Column, end.Line, end.Column);
        }

        #region Helpers 

        protected void DefineTemporary(string name)
        {
            TemporaryBinding temporary = new TemporaryBinding(name);
            this.Temporaries.Add(temporary);
            this.LocalScope.DefineBinding(temporary);
        }

        protected void DefineArgument(string name)
        {
            this.DefineArgument(new ArgumentBinding(name));
        }

        protected void DefineArgument(string name, Expression expression)
        {
            this.DefineArgument(new ArgumentBinding(name, expression));
        }

        protected void DefineArgument(ArgumentBinding argument)
        {
            if (argument == null)
                throw new ArgumentNullException();
            this.LocalScope.DefineBinding(argument);
        }

        #endregion
    }

    public interface IRootFunctionVisitor
    {
        SmalltalkRuntime Runtime { get; }
        CallSiteBinderCache BinderCache { get; }
        Symbol SuperLookupScope { get; }
        Expression HomeContext { get; }
        BindingScope ReservedScope { get; }
        BindingRestrictions BindingRestrictions { get; set; }
        Expression AddDebugInfo(Expression expression, int startLine, int startColumn, int endLine, int endColumn);
        Expression AddDebugInfo(Expression expression, IParseNode node);
    }

}
