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
using IronSmalltalk.Compiler.Interchange.ParseNodes;

namespace IronSmalltalk.Compiler.Interchange.Visiting
{
    public class InterchangeParseTreeValidatingVisitor : InterchangeParseTreeVisitor<bool>
    {
        /// <summary>
        /// Default instance of the validating visitor.
        /// </summary>
        public static readonly InterchangeParseTreeValidatingVisitor Current = new InterchangeParseTreeValidatingVisitor(); 

        /// <summary>
        /// Visits the Interchange Node. This is the default visit, in case no other implementation.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitInterchangeNode(InterchangeParseNode node)
        {
            return true;
        }

        /// <summary>
        /// Visits the Interchange Version Identifier node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitInterchangeVersionIdentifier(InterchangeVersionIdentifierNode node)
        {
            if (node.VersionId == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Class Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitClassDefinition(ClassDefinitionNode node)
        {
            if (node.ClassInstanceVariableNames == null)
                return false;
            if (node.ClassName == null)
                return false;
            if (node.ClassVariableNames == null)
                return false;
            if (node.IndexedInstanceVariables == null)
                return false;
            if (node.InstanceVariableNames == null)
                return false;
            if (node.SharedPools == null)
                return false;
            if (node.SuperclassName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Instance Method Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitInstanceMethodDefinition(InstanceMethodDefinitionNode node)
        {
            if (node.ClassName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Class Method Definition  node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitClassMethodDefinition(ClassMethodDefinitionNode node)
        {
            if (node.ClassName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Global Variable Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitGlobalVariableDefinition(GlobalVariableDefinitionNode node)
        {
            if (node.GlobalName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Global Constant Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitGlobalConstantDefinition(GlobalConstantDefinitionNode node)
        {
            if (node.GlobalName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Global Variable/Constant Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitGlobalInitialization(GlobalInitializationNode node)
        {
            if (node.GlobalName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Pool Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitPoolDefinition(PoolDefinitionNode node)
        {
            if (node.PoolName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Pool Variable Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitPoolVariableDefinition(PoolVariableDefinitionNode node)
        {
            if (node.PoolName == null)
                return false;
            if (node.PoolVariableName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Pool Constant Definition node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitPoolConstantDefinition(PoolConstantDefinitionNode node)
        {
            if (node.PoolName == null)
                return false;
            if (node.PoolVariableName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Pool Variable/Constant Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitPoolValueInitialization(PoolValueInitializationNode node)
        {
            if (node.PoolName == null)
                return false;
            if (node.PoolVariableName == null)
                return false;
            return true;
        }

        /// <summary>
        /// Visits the Program Initialization node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitProgramInitialization(ProgramInitializationNode node)
        {
            return true;
        }

        /// <summary>
        /// Visits the Annotation node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        public override bool VisitAnnotation(AnnotationNode node)
        {
            if (node.AnnotatedNode == null)
                return false;
            if (node.Key == null)
                return false;
            if (node.Value == null)
                return false;
            return true;
        }

    }
}
