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
using IronSmalltalk.Compiler.Interchange.ParseNodes;

namespace IronSmalltalk.Compiler.Interchange.Visiting
{
    /// <summary>
    /// Interface defining the protocol for visiting
    /// the parse tree nodes defined in X3J20 section 4.1,
    /// i.e. the parse tree nodes from the interchange format specitication.
    /// </summary>
    public interface IInterchangeParseTreeVisitor<TResult>
    {
        /// <summary>
        /// Visits the Interchange Node. This is the default visit, in case no other implementation.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitInterchangeNode(InterchangeParseNode node);

        /// <summary>
        /// Visits the Interchange Version Identifier node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitInterchangeVersionIdentifier(InterchangeVersionIdentifierNode node);

        /// <summary>
        /// Visits the Class Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitClassDefinition(ClassDefinitionNode node);

        /// <summary>
        /// Visits the Instance Method Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitInstanceMethodDefinition(InstanceMethodDefinitionNode node);

        /// <summary>
        /// Visits the Class Method Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitClassMethodDefinition(ClassMethodDefinitionNode node);

        /// <summary>
        /// Visits the Global Variable Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitGlobalVariableDefinition(GlobalVariableDefinitionNode node);

        /// <summary>
        /// Visits the Global Constant Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitGlobalConstantDefinition(GlobalConstantDefinitionNode node);

        /// <summary>
        /// Visits the Global Variable/Constant Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitGlobalInitialization(GlobalInitializationNode node);

        /// <summary>
        /// Visits the Pool Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitPoolDefinition(PoolDefinitionNode node);

        /// <summary>
        /// Visits the Pool Variable Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitPoolVariableDefinition(PoolVariableDefinitionNode node);

        /// <summary>
        /// Visits the Pool Constant Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitPoolConstantDefinition(PoolConstantDefinitionNode node);

        /// <summary>
        /// Visits the Pool Variable/Constant Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitPoolValueInitialization(PoolValueInitializationNode node);

        /// <summary>
        /// Visits the Program Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitProgramInitialization(ProgramInitializationNode node);

        /// <summary>
        /// Visits the Annotation node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        TResult VisitAnnotation(AnnotationNode node);
    }
}
