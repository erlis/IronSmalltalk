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
using IronSmalltalk.Runtime.Installer.Definitions;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public abstract class PoolItemDefinitionNode : InterchangeElementNode
    {
        public IdentifierToken PoolName { get; private set; }
        public StringToken PoolVariableName { get; set; }

        public PoolItemDefinitionNode(IdentifierToken poolName)
        {
            if (poolName == null)
                throw new ArgumentNullException();
            this.PoolName = poolName;
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
            //   InterchangeFormatParser.ParsePoolVariable/ConstantDefinition() should have reported the error.
            if ((this.PoolName == null) || (this.PoolVariableName == null))
                return this;

            PoolValueDefinition definition = this.CreateDefinition(processor, sourceCodeService);
            this.Definfition = definition;
            // This may fail, but we don't care. If failed, it reported the error through its error sink.
            processor.FileInProcessor.FileInPoolVariable(definition);
            return this;
        }

        protected abstract PoolValueDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService);
    }

    public partial class PoolVariableDefinitionNode : PoolItemDefinitionNode
    {
        public PoolVariableDefinitionNode(IdentifierToken poolName)
            : base(poolName)
        {
        }

        protected override PoolValueDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService)
        {
            return new PoolVariableDefinition(
                processor.CreateSourceReference(this.PoolName.Value, this.PoolName, sourceCodeService),
                processor.CreateSourceReference(this.PoolVariableName.Value, this.PoolVariableName, sourceCodeService));
        }
    }

    public partial class PoolConstantDefinitionNode : PoolItemDefinitionNode
    {
        public PoolConstantDefinitionNode(IdentifierToken poolName)
            : base(poolName)
        {
        }

        protected override PoolValueDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService)
        {
            return new PoolConstantDefinition(
                processor.CreateSourceReference(this.PoolName.Value, this.PoolName, sourceCodeService),
                processor.CreateSourceReference(this.PoolVariableName.Value, this.PoolVariableName, sourceCodeService));
        }
    }
}
