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
    public abstract class GlobalDefinitionNode : InterchangeElementNode
    {
        public StringToken GlobalName { get; private set; }

        public GlobalDefinitionNode()
        {
        }

        public GlobalDefinitionNode(StringToken globalName)
        {
            this.GlobalName = globalName; // Null OK ... in case error occured
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
            //   InterchangeFormatParser.ParseGlobalVariable/ConstantDefinition() should have reported the error.
            if (this.GlobalName == null)
                return this;

            GlobalDefinition definition = this.CreateDefinition(processor, sourceCodeService);
            this.Definfition = definition;
            // This may fail, but we don't care. If failed, it reported the error through its error sink.
            processor.FileInProcessor.FileInGlobal(definition);
            return this;
        }

        protected abstract GlobalDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService);

    }

    public partial class GlobalVariableDefinitionNode : GlobalDefinitionNode
    {
        public GlobalVariableDefinitionNode()
        {
        }

        public GlobalVariableDefinitionNode(StringToken globalName)
            : base(globalName)
        {
        }

        protected override GlobalDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService)
        {
            return new GlobalVariableDefinition(processor.CreateSourceReference(this.GlobalName.Value, this.GlobalName, sourceCodeService));
        }
    }

    public partial class GlobalConstantDefinitionNode : GlobalDefinitionNode
    {
        public GlobalConstantDefinitionNode()
        {
        }

        public GlobalConstantDefinitionNode(StringToken globalName)
            : base(globalName)
        {
        }

        protected override GlobalDefinition CreateDefinition(InterchangeFormatProcessor processor, ISourceCodeReferenceService sourceCodeService)
        {
            return new GlobalConstantDefinition(processor.CreateSourceReference(this.GlobalName.Value, this.GlobalName, sourceCodeService));
        }
    }
}
