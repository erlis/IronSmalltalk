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
using IronSmalltalk.Compiler.Visiting;
using IronSmalltalk.Compiler.Interchange.Visiting;

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    // ********************************************
    // *** File with partial classes that implement
    // *** the Parse-Tree-Visitor methods for
    // *** the Interchange Nodes (X3J20 4.1).
    // *** Implemented here for logistical reasons.
    // ********************************************

    partial class InterchangeParseNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public virtual TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitInterchangeNode(this);
        }
    }

    partial class ProgramInitializationNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitProgramInitialization(this);
        }
    }

    partial class PoolVariableDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitPoolVariableDefinition(this);
        }
    }

    partial class PoolValueInitializationNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitPoolValueInitialization(this);
        }
    }

    partial class PoolDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitPoolDefinition(this);
        }
    }

    partial class PoolConstantDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitPoolConstantDefinition(this);
        }
    }

    partial class InterchangeVersionIdentifierNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitInterchangeVersionIdentifier(this);
        }
    }

    partial class InstanceMethodDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitInstanceMethodDefinition(this);
        }
    }

    partial class ClassMethodDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitClassMethodDefinition(this);
        }
    }

    partial class GlobalVariableDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitGlobalVariableDefinition(this);
        }
    }

    partial class GlobalInitializationNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitGlobalInitialization(this);
        }
    }

    partial class GlobalConstantDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitGlobalConstantDefinition(this);
        }
    }

    partial class ClassDefinitionNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitClassDefinition(this);
        }
    }

    partial class AnnotationNode
    {
        /// <summary>
        /// Implements the visitor pattern for parse nodes.
        /// Dispatches to the specific visit method for this node type. 
        /// For example, MethodNode calls the VisitMethod.
        /// </summary>
        /// <param name="visitor"></param>
        public override TResult Accept<TResult>(IInterchangeParseTreeVisitor<TResult> visitor)
        {
#if DEBUG
            if (visitor == null)
                throw new ArgumentNullException();
#endif
            return visitor.VisitAnnotation(this);
        }
    }
}
