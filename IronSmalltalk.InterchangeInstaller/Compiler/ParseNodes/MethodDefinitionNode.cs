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
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Runtime.Installer.Definitions;
using IronSmalltalk.AstJitCompiler.Runtime;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public abstract class MethodDefinitionNode : InterchangeElementNode
    {
        public IdentifierToken ClassName { get; private set; }

        public MethodDefinitionNode(IdentifierToken className)
        {
            if (className == null)
                throw new ArgumentNullException();
            this.ClassName = className;
        }

        /// <summary>
        /// File-in and process the actions contained in the node.
        /// </summary>
        /// <param name="processor">Interchange format processor responsible for the processing context.</param>
        /// <param name="parseErrorSink">Error sink for reporting parse errors.</param>
        /// <param name="sourceCodeService">Source code service that can convert tokens to source code span and reports issues.</param>
        /// <returns>Return an interchange unit node for annotation, usually just self.</returns>
        public override InterchangeUnitNode FileIn(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");
            if (parseErrorSink == null)
                throw new ArgumentNullException("parseErrorSink");
            if (sourceCodeService == null)
                throw new ArgumentNullException("sourceCodeService");
            // ALL instance vars must be set. If one is missing, then source code bug, and 
            //   InterchangeFormatParser.ParseInstance/ClassMethodDefinition() should have reported the error.
            if (this.ClassName == null)
                return this;

            // <methodDefinition> ::= <className> ’method’ <elementSeparator>
            //      <method definition> <elementSeparator>
            // <classMethodDefinition> ::= <className> ’classMethod’ <elementSeparator>
            //      <method definition> <elementSeparator>

            ISourceCodeReferenceService methodSourceCodeService;
            MethodNode method = processor.ParseMethod(out methodSourceCodeService);
            if (method == null)
                return this; // Processor/Parser should have reported errors
            if (!method.Accept(IronSmalltalk.Compiler.Visiting.ParseTreeValidatingVisitor.Current))
            {
                // We expect the parser to have reported the errors. Just in case, we do it once more to the installer.
                // Bad practice here is to use the 'processor.SourcePosition', but we don't have anything better :-/
                if (processor.ErrorSink != null)
                    processor.ErrorSink.AddInterchangeError(processor.SourcePosition, processor.SourcePosition, "Invalid method source code.");
                return this;
            }

            MethodDefinition definition = this.CreateDefinition(processor, sourceCodeService, methodSourceCodeService, method);
            this.Definfition = definition;
            // This may fail, but we don't care. If failed, it reported the error through its error sink.
            processor.FileInProcessor.FileInMethod(definition);

            return this;
        }

        protected abstract MethodDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService, ISourceCodeReferenceService methodSourceCodeService, MethodNode parseTree);
    }

    public partial class InstanceMethodDefinitionNode : MethodDefinitionNode
    {
        public InstanceMethodDefinitionNode(IdentifierToken className)
            : base(className)
        {
        }

        protected override MethodDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService, ISourceCodeReferenceService methodSourceCodeService, MethodNode parseTree)
        {
            return new InstanceMethodDefinition(
                processor.CreateSourceReference(this.ClassName.Value, this.ClassName, sourceCodeService),
                processor.CreateSourceReference(parseTree.Selector, parseTree.SelectorParts[0].StartPosition, parseTree.SelectorParts.Last().StopPosition, sourceCodeService),
                sourceCodeService,
                methodSourceCodeService,
                new AstIntermediateMethodCode(parseTree));
        }
    }

    public partial class ClassMethodDefinitionNode : MethodDefinitionNode
    {
        public ClassMethodDefinitionNode(IdentifierToken className)
            : base(className)
        {
        }

        protected override MethodDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService, ISourceCodeReferenceService methodSourceCodeService, MethodNode parseTree)
        {
            return new ClassMethodDefinition(
                processor.CreateSourceReference(this.ClassName.Value, this.ClassName, sourceCodeService),
                processor.CreateSourceReference(parseTree.Selector, parseTree.SelectorParts[0].StartPosition, parseTree.SelectorParts.Last().StopPosition, sourceCodeService),
                sourceCodeService,
                methodSourceCodeService,
                new AstIntermediateMethodCode(parseTree));
        }
    }
}
