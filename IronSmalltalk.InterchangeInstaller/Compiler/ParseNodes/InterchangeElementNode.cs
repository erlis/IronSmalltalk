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
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.Installer.Definitions;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public abstract class InterchangeUnitNode : InterchangeParseNode
    {
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
        public abstract InterchangeElementNode GetAnnotatedNode();


        /// <summary>
        /// File-in and process the actions contained in the node.
        /// </summary>
        /// <param name="processor">Interchange format processor responsible for the processing context.</param>
        /// <param name="parseErrorSink">Error sink for reporting parse errors.</param>
        /// <param name="sourceCodeService">Source code service that can convert tokens to source code span and reports issues.</param>
        /// <returns>Return an interchange unit node for annotation, usually just self.</returns>
        public abstract InterchangeUnitNode FileIn(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService);
    }

    public abstract class InterchangeElementNode : InterchangeUnitNode
    {
        protected static bool ValidateIdentifierString(string str)
        {
            // stringDelimiter identifier stringDelimiter
            // identifier ::= letter (letter | digit)*
            if (str == null)
                return true;

            if (str.Length == 0)
                return false; // Empty identifier

            ScanResult scanResult = new ScanResult();
            scanResult.SetResult(str[0]);
            if (!scanResult.IsLetter())
                return false; // First char non-letter

            foreach (char ch in str)
            {
                scanResult.SetResult(ch);
                if (!(scanResult.IsLetter() || scanResult.IsDigit()))
                    return false; // Non-letter or non-digit char
            }
            return true; // OK
        }

        /// <summary>
        /// Definition that represents this interchange element.
        /// This definition was added to the InstallerContext.
        /// We keep this definition to be able to add annotations to it.
        /// </summary>
        protected internal DefinitionBase Definfition { get; set; }

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
            return this;
        }
    }
}
