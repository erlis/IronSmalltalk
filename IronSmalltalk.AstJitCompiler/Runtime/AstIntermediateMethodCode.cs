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
using IronSmalltalk.Runtime;
using IronSmalltalk.Compiler.SemanticNodes;
using System.Linq.Expressions;
using System.Dynamic;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime.CodeGeneration.Visiting;
using IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Common;
using IronSmalltalk.Runtime.CodeGeneration.BindingScopes;

namespace IronSmalltalk.AstJitCompiler.Runtime
{
    public class AstIntermediateMethodCode : IntermediateMethodCode
    {
        public MethodNode ParseTree { get; private set; }

        public AstIntermediateMethodCode(MethodNode parseTree)
        {
            if (parseTree == null)
                throw new ArgumentNullException();
            this.ParseTree = parseTree;
        }

        public override MethodCompilationResult CompileClassMethod(SmalltalkRuntime runtime, SmalltalkClass cls, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope)
        {
            return this.CompileClassMethod(runtime, cls, self, arguments, superScope, runtime.GlobalScope);
        }

        private MethodCompilationResult CompileClassMethod(SmalltalkRuntime runtime, SmalltalkClass cls, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope, SmalltalkNameScope globalNameScope)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            if (cls == null)
                throw new ArgumentNullException("cls");
            if (self == null)
                throw new ArgumentNullException("self");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            MethodVisitor visitor = new MethodVisitor(runtime,
                BindingScope.ForClassMethod(cls, globalNameScope),
                ReservedScope.ForClassMethod(self.Expression),
                self,
                arguments,
                cls.Name);
            //visitor.SymbolDocument = Expression.SymbolDocument("<mod>", new Guid("{E1A254E3-FD2E-4D82-BAB3-D4E9B115154E}"), new Guid("{6A28E03C-E404-4190-A012-72B2CCE48DD5}"));
            Expression code = this.ParseTree.Accept(visitor);
            return new MethodCompilationResult(code, visitor.BindingRestrictions);
        }

        public override MethodCompilationResult CompileInstanceMethod(SmalltalkRuntime runtime, SmalltalkClass cls, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope)
        {
            return this.CompileInstanceMethod(runtime, cls, self, arguments, superScope, runtime.GlobalScope);
        }

        private MethodCompilationResult CompileInstanceMethod(SmalltalkRuntime runtime, SmalltalkClass cls, DynamicMetaObject self, DynamicMetaObject[] arguments, Symbol superScope, SmalltalkNameScope globalNameScope)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            if (cls == null)
                throw new ArgumentNullException("cls");
            if (self == null)
                throw new ArgumentNullException("self");
            if (arguments == null)
                throw new ArgumentNullException("arguments");
            MethodVisitor visitor = new MethodVisitor(runtime,
                BindingScope.ForInstanceMethod(cls, globalNameScope),
                (cls.Superclass == null) ?
                    ReservedScope.ForRootClassInstanceMethod(self.Expression) :
                    ReservedScope.ForInstanceMethod(self.Expression),
                self,
                arguments,
                cls.Name);
            Expression code = this.ParseTree.Accept(visitor);
            return new MethodCompilationResult(code, visitor.BindingRestrictions);
        }

        public override bool ValidateClassMethod(SmalltalkClass cls, SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink)
        {
            return this.Validate(cls, errorSink, (r, c, s, a, u) => this.CompileClassMethod(r, c, s, a, u, globalNameScope));
        }

        public override bool ValidateInstanceMethod(SmalltalkClass cls, SmalltalkNameScope globalNameScope, IIntermediateCodeValidationErrorSink errorSink)
        {
            return this.Validate(cls, errorSink, (r, c, s, a, u) => this.CompileInstanceMethod(r, c, s, a, u, globalNameScope));
        }

        private bool Validate(SmalltalkClass cls, IIntermediateCodeValidationErrorSink errorSink, Func<SmalltalkRuntime, SmalltalkClass, DynamicMetaObject, DynamicMetaObject[], Symbol, MethodCompilationResult> func)
        {
            return AstIntermediateMethodCode.Validate(this.ParseTree, errorSink, delegate()
            {
                DynamicMetaObject self = new DynamicMetaObject(Expression.Parameter(typeof(object), "self"), BindingRestrictions.Empty, null);
                DynamicMetaObject[] args = new DynamicMetaObject[this.ParseTree.Arguments.Count];
                for (int i = 0; i < args.Length; i++)
                    args[i] = new DynamicMetaObject(Expression.Parameter(typeof(object), String.Format("arg{0}", i)), BindingRestrictions.Empty, null);

                return func(cls.Runtime, cls, self, args, null);
            });
        }

        public static bool Validate<TResult>(SemanticNode rootNode, IIntermediateCodeValidationErrorSink errorSink, Func<TResult> compileFunc)
        {
            if (rootNode == null)
                throw new ArgumentNullException("rootNode");
            if (compileFunc == null)
                throw new ArgumentNullException("compileFunc");

            try
            {
                TResult result = compileFunc();
                if (result == null)
                    return AstIntermediateMethodCode.ReportError(errorSink, rootNode, "Could not compile method");
                return true;
            }
            catch (IronSmalltalk.AstJitCompiler.Internals.PrimitiveInvalidTypeException ex)
            {
                return AstIntermediateMethodCode.ReportError(errorSink, ex.Node, ex.Message);
            }
            catch (IronSmalltalk.AstJitCompiler.Internals.SemanticCodeGenerationException ex)
            {
                return AstIntermediateMethodCode.ReportError(errorSink, ex.Node, ex.Message);
            }
            catch (IronSmalltalk.Runtime.Internal.SmalltalkDefinitionException ex)
            {
                return AstIntermediateMethodCode.ReportError(errorSink, rootNode, ex.Message);
            }
            catch (IronSmalltalk.Runtime.Internal.SmalltalkRuntimeException ex)
            {
                return AstIntermediateMethodCode.ReportError(errorSink, rootNode, ex.Message);
            }
            catch (Exception ex)
            {
                return AstIntermediateMethodCode.ReportError(errorSink, rootNode, ex.Message);
            }
        }

        private static bool ReportError(IIntermediateCodeValidationErrorSink errorSink, SemanticNode node, string errorMessage)
        {
            if (errorSink != null)
            {
                SourceLocation start = SourceLocation.Invalid;
                SourceLocation end = SourceLocation.Invalid;
                if (node != null)
                {
                    var tokens = node.GetTokens();
                    if ((tokens != null) && tokens.Any())
                    {
                        start = tokens.Min(t => t.StartPosition);
                        end = tokens.Max(t => t.StopPosition);
                    }
                    foreach (var sn in node.GetChildNodes())
                    {
                        tokens = sn.GetTokens();
                        if ((tokens != null) && tokens.Any())
                        {
                            start = start.Min(tokens.Min(t => t.StartPosition));
                            end = end.Max(tokens.Max(t => t.StopPosition));
                        }
                    }
                }

                errorSink.ReportError(errorMessage, start, end);
            }
            return false;
        }
    }
}
