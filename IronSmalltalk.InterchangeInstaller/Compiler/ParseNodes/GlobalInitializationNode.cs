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
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Runtime.Installer.Definitions;
using IronSmalltalk.AstJitCompiler.Runtime;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This class handles initialization for both classes and globals.
    /// </remarks>
    public partial class GlobalInitializationNode : InterchangeElementNode
    {
        public IdentifierToken GlobalName { get; private set; }

        public GlobalInitializationNode(IdentifierToken globalName)
        {
            if (globalName == null)
                throw new ArgumentNullException();
            this.GlobalName = globalName;
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
            //   InterchangeFormatParser.ParseGlobalInitialization() should have reported the error.
            if (this.GlobalName == null)
                return this;

            // <globalValueInitialization> ::= <globalName> ’initializer’ <elementSeparator> 
            //      <variableInitializer> <elementSeparator>
            // <variableInitializer> ::= <initializer definition>

            ISourceCodeReferenceService methodSourceCodeService;
            InitializerNode initializer = processor.ParseInitializer(out methodSourceCodeService);
            if (initializer == null)
                return this; // Processor/Parser should have reported errors
            if (!initializer.Accept(IronSmalltalk.Compiler.Visiting.ParseTreeValidatingVisitor.Current))
            {
                // We expect the parser to have reported the errors. Just in case, we do it once more to the installer.
                // Bad practice here is to use the 'processor.SourcePosition', but we don't have anything better :-/
                if (processor.ErrorSink != null)
                    processor.ErrorSink.AddInterchangeError(processor.SourcePosition, processor.SourcePosition, "Invalid initializer source code.");
                return this;
            }

            GlobalInitializer definition = new GlobalInitializer(
                processor.CreateSourceReference(this.GlobalName.Value, this.GlobalName, sourceCodeService), 
                sourceCodeService,
                methodSourceCodeService,
                new AstIntermediateInitializerCode(initializer));
            this.Definfition = definition;
            // This may fail, but we don't care. If failed, it reported the error through its error sink.
            processor.FileInProcessor.FileInGlobalInitializer(definition);

            return this;
        }
    }
}
