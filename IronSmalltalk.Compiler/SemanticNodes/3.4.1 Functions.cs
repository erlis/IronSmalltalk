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
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// Function node represents an executable code, i.e. method, initializer or block.
    /// </summary>
    public abstract class FunctionNode : SemanticNode, IStatementParentNode
    {
        /// <summary>
        /// Create and initialize a new FunctionNode.
        /// </summary>
        protected FunctionNode()
        {
            this.Temporaries = new List<TemporaryVariableNode>();
        }

        /// <summary>
        /// Left vertical bar "|" token, if temporary variable definition is present.
        /// </summary>
        public VerticalBarToken LeftBar { get; private set; }

        /// <summary>
        /// Right vertical bar "|" token, if temporary variable definition is present.
        /// </summary>
        public VerticalBarToken RightBar { get; private set; }

        /// <summary>
        /// A collection of temporary variable nodes.
        /// The collection is empty if no temporaries are defined.
        /// </summary>
        public List<TemporaryVariableNode> Temporaries { get; private set; }

        /// <summary>
        /// Optional executable statements defining the function.
        /// Statements may be null if no statements were present in the source code.
        /// </summary>
        public StatementNode Statements { get; private set; }

        /// <summary>
        /// Initializes the node after being parsed by the parser.
        /// </summary>
        /// <param name="leftBar">Left vertical bar "|" token, if temporary variable definition is present.</param>
        /// <param name="temporaries">A collection of temporary variable nodes.</param>
        /// <param name="rightBar">Right vertical bar "|" token, if temporary variable definition is present.</param>
        /// <param name="statements">Executable statements defining the function.</param>
        protected internal void SetContents(VerticalBarToken leftBar, IEnumerable<TemporaryVariableNode> temporaries, VerticalBarToken rightBar, StatementNode statements)
        {
            this.Temporaries.Clear();
            if (temporaries != null)
                this.Temporaries.AddRange(temporaries);
            this.LeftBar = leftBar; // Null if no temporaries 
            this.RightBar = rightBar; // Null if no temporaries or illegal source code
            this.Statements = statements; // OK with null

            if ((this.Temporaries.Count != 0) && (this.LeftBar == null))
                throw new ArgumentNullException("leftBar"); // Little late, but OK to throw the exception.
        }


        // Local scope = temps + args
        // Statement scope = local scope + outer scope

        // Activate(args...)
    }

    /// <summary>
    /// Temporary variable node represent temporary variable declaration inside a functions;
    /// that is vars inside the vertical bars in methods, blocks and intializers.
    /// </summary>
    public partial class TemporaryVariableNode : VariableNode
    {
        /// <summary>
        /// The function node that defines this temporary variable.
        /// </summary>
        public FunctionNode Parent { get; private set; }

        /// <summary>
        /// Create and initialize a new TemporaryVariableNode.
        /// </summary>
        /// <param name="parent">The function node that defines this temporary variable.</param>
        /// <param name="token">Identifier token containing the variable name.</param>
        protected internal TemporaryVariableNode(FunctionNode parent, IdentifierToken token)
            : base(token)
        {
#if DEBUG
            if (parent == null)
                throw new ArgumentNullException("parent");
#endif
            this.Parent = parent;
        }
    }
}
