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
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Runtime;
using IronSmalltalk.Compiler.SemanticAnalysis;
using System.Collections.Generic;
using IronSmalltalk.Runtime.Installer.Definitions;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public partial class ClassDefinitionNode : InterchangeElementNode
    {
        public StringToken ClassName { get; set; }
        public StringToken SuperclassName { get; set; }

        public HashedStringToken IndexedInstanceVariables { get; set; }
        public StringToken InstanceVariableNames { get; set; }
        public StringToken ClassVariableNames { get; set; }
        public StringToken ClassInstanceVariableNames { get; set; }
        public StringToken SharedPools { get; set; }

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
            //   InterchangeFormatParser.ParseClassDefinition() should have reported the error.
            if ((this.ClassName == null) || (this.SuperclassName == null) || (this.IndexedInstanceVariables == null)
                || (this.InstanceVariableNames == null) || (this.ClassVariableNames == null) || (this.SharedPools == null)
                || (this.ClassInstanceVariableNames == null))
                    return this;

            SmalltalkClass.InstanceStateEnum? instanceState = this.GetInstanceState(processor, parseErrorSink, sourceCodeService);
            if (instanceState == null)
                return this;

            IEnumerable<SourceReference<string>> civn = this.GetClassInstanceVariableNames(processor, parseErrorSink, sourceCodeService);
            if (civn == null)
                return this; // ParseIdentifierList() reported the error
            IEnumerable<SourceReference<string>> ivn = this.GetInstanceVariableNames(processor, parseErrorSink, sourceCodeService);
            if (ivn == null)
                return this; // ParseIdentifierList() reported the error
            IEnumerable<SourceReference<string>> cvn = this.GetClassVariableNames(processor, parseErrorSink, sourceCodeService);
            if (cvn == null)
                return this; // ParseIdentifierList() reported the error
            IEnumerable<SourceReference<string>> spn = this.GetSharedPoolNames(processor, parseErrorSink, sourceCodeService);
            if (spn == null)
                return this; // ParseIdentifierList() reported the error

            ClassDefinition definition = new ClassDefinition(
                processor.CreateSourceReference(this.ClassName.Value, this.ClassName, sourceCodeService),
                processor.CreateSourceReference(this.SuperclassName.Value, this.SuperclassName, sourceCodeService),
                processor.CreateSourceReference(instanceState.Value, this.IndexedInstanceVariables, sourceCodeService),
                civn, cvn, ivn, spn);
            this.Definfition = definition;
            // This may fail, but we don't care. If failed, it reported the error through its error sink.
            processor.FileInProcessor.FileInClass(definition); 

            return this;
        }

        protected virtual SmalltalkClass.InstanceStateEnum? GetInstanceState(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            if (this.IndexedInstanceVariables.Value == "byte")
                return SmalltalkClass.InstanceStateEnum.ByteIndexable;
            else if (this.IndexedInstanceVariables.Value == "object")
                return SmalltalkClass.InstanceStateEnum.ObjectIndexable;
            else if (this.IndexedInstanceVariables.Value == "none")
                return SmalltalkClass.InstanceStateEnum.NamedObjectVariables;
            else
            {
                parseErrorSink.AddParserError(this.IndexedInstanceVariables.StartPosition, this.IndexedInstanceVariables.StopPosition,
                    "Invalid instance state type. Allowed values are: #byte, #object and #none.", this.IndexedInstanceVariables);
                return null;
            }
        }

        protected virtual IEnumerable<SourceReference<string>> GetClassInstanceVariableNames(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            return processor.ParseIdentifierList(this.ClassInstanceVariableNames, parseErrorSink, sourceCodeService);
        }

        protected virtual IEnumerable<SourceReference<string>> GetInstanceVariableNames(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            return processor.ParseIdentifierList(this.InstanceVariableNames, parseErrorSink, sourceCodeService);
        }

        protected virtual IEnumerable<SourceReference<string>> GetClassVariableNames(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            return processor.ParseIdentifierList(this.ClassVariableNames, parseErrorSink, sourceCodeService);
        }

        protected virtual IEnumerable<SourceReference<string>> GetSharedPoolNames(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            return processor.ParseIdentifierList(this.SharedPools, parseErrorSink, sourceCodeService);
        }
    }
}
