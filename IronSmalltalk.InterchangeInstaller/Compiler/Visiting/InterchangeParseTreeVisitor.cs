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
    /// Base class that implements the protocol for visiting
    /// the parse tree nodes defined in X3J20 section 4.1,
    /// i.e. the parse tree nodes from the interchange format specitication.
    /// </summary>
    public abstract class InterchangeParseTreeVisitor<TResult> : IInterchangeParseTreeVisitor<TResult>
    {
        /// <summary>
        /// Visits the Interchange Node. This is the default visit, in case no other implementation.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitInterchangeNode(InterchangeParseNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Interchange Version Identifier node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitInterchangeVersionIdentifier(InterchangeVersionIdentifierNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Class Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitClassDefinition(ClassDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Instance Method Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitInstanceMethodDefinition(InstanceMethodDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Class Method Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitClassMethodDefinition(ClassMethodDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Global Variable Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitGlobalVariableDefinition(GlobalVariableDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Global Constant Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitGlobalConstantDefinition(GlobalConstantDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Global Variable/Constant Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitGlobalInitialization(GlobalInitializationNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Pool Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitPoolDefinition(PoolDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Pool Variable Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitPoolVariableDefinition(PoolVariableDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Pool Constant Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitPoolConstantDefinition(PoolConstantDefinitionNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Pool Variable/Constant Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitPoolValueInitialization(PoolValueInitializationNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Program Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitProgramInitialization(ProgramInitializationNode node)
        {
            return default(TResult); // The default naive implementation
        }

        /// <summary>
        /// Visits the Annotation node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public virtual TResult VisitAnnotation(AnnotationNode node)
        {
            return default(TResult); // The default naive implementation
        }
    }
}
