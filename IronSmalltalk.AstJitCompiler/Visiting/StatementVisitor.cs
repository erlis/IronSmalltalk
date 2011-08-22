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
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Runtime.Behavior;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    public class StatementVisitor : NestedEncoderVisitor<List<Expression>>
    {
        public StatementVisitor(EncoderVisitor enclosingVisitor)
            : base(enclosingVisitor)
        {
            this.HasReturned = false;
        }

        public bool HasReturned { get; protected set; }
        protected readonly List<Expression> Expressions = new List<Expression>();

        public override List<Expression> VisitStatementSequence(StatementSequenceNode node)
        {
            if (this.HasReturned)
                throw new SemanticCodeGenerationException(CodeGenerationErrors.CodeAfterReturnStatement, node);

            Expression statementCode = node.Expression.Accept(new ExpressionVisitor(this));
            statementCode = this.RootVisitor.AddDebugInfo(statementCode, node.Expression);

            this.Expressions.Add(statementCode);

            if (node.NextStatement != null)
                node.NextStatement.Accept(this);

            return this.Expressions;
        }

        public override List<Expression> VisitReturnStatement(ReturnStatementNode node)
        {
            if (this.HasReturned)
                throw new SemanticCodeGenerationException(CodeGenerationErrors.CodeAfterReturnStatement, node);
            this.HasReturned = true;

            this.Expressions.Add(this.Return(node.Expression.Accept(new ExpressionVisitor(this))));

            return this.Expressions;
        }
    }
}
