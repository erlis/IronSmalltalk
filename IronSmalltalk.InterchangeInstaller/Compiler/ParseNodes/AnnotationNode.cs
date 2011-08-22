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
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Annotation node is NOT an interchange elements, but we've it inherit 
    /// from the InterchangeElementNode for technical reasons inside the parser.
    /// </remarks>
    public partial class AnnotationNode : InterchangeUnitNode
    {
        public InterchangeElementNode AnnotatedNode { get; set; }
        public StringToken Key { get; set; }
        public StringToken Value { get; set; }

        public AnnotationNode()
        {
        }

        public AnnotationNode(InterchangeElementNode annotatedNode)
        {
            this.AnnotatedNode = annotatedNode; // Null is OK!
        }

        /// <summary>
        /// Get the node that a subsequent annotation node is to annotate.
        /// </summary>
        /// <example>
        /// Sequence of nodes:          Returns:
        ///     ClassDefinitionNode     ClassDefinitionNode, i.e. this
        ///     AnnotationNode          ClassDefinitionNode
        ///     AnnotationNode          ClassDefinitionNode
        /// </example>
        /// <returns>The interchange element node that an annotation node is to annotate.</returns>
        public override InterchangeElementNode GetAnnotatedNode()
        {
            return this.AnnotatedNode;
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

            if ((this.AnnotatedNode == null) || (this.Key == null) || (this.Value == null))
                return this;

            if (this.AnnotatedNode.Definfition == null)
                // Not normal, means there was bug in the code that was supposed to create the definition,
                // but we don't want to add too much trouble to the developer, so we silently ignore 
                // the annotation (to the buggy definition).
                return this; 

            this.AnnotatedNode.Definfition.Annotate(this.Key.Value, this.Value.Value);
            return this;
        }
    }
}
