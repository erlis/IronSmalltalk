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
using IronSmalltalk.Runtime.Installer.Definitions;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public partial class PoolDefinitionNode : InterchangeElementNode
    {
        public StringToken PoolName { get; private set; }

        public PoolDefinitionNode()
        {
        }

        public PoolDefinitionNode(StringToken poollName)
        {
            if (poollName == null)
                throw new ArgumentNullException();
            this.PoolName = poollName;
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
            //   InterchangeFormatParser.ParsePoolDefinition() should have reported the error.
            if (this.PoolName == null)
                return this;

            PoolDefinition definition = new PoolDefinition(processor.CreateSourceReference(this.PoolName.Value, this.PoolName, sourceCodeService));
            this.Definfition = definition;
            // This may fail, but we don't care. If failed, it reported the error through its error sink.
            processor.FileInProcessor.FileInPool(definition);
            return this;
        }
    }
}
