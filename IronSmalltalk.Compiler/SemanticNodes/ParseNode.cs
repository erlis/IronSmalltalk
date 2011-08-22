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
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.SemanticAnalysis;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// The ParseNode is the root of all parse tree nodes.
    /// </summary>
    public abstract class ParseNode : IParseNode
    {
        /// <summary>
        /// Initializes a new ParseNode object.
        /// </summary>
        protected ParseNode()
        {
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public abstract IEnumerable<IParseNode> GetChildNodes();

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public abstract IEnumerable<IToken> GetTokens();

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public abstract string PrintString();

        /// <summary>
        /// Returns a String that represents the current parse node.
        /// </summary>
        /// <returns>A String that represents the current node.</returns>
        public override string ToString()
        {
            return this.GetType().Name + ": " + this.PrintString();
        }
    }

    /// <summary>
    /// This is the root class for all parse tree nodes defined in X3J20 3.4
    /// </summary>
    /// <remarks>
    /// We have this class so we can differentiate between parse nodes defines
    /// in X3J20 3.4 (methods, initializers) and ohter parse nodes that derive
    /// from the base ParseNode (e.g. the interchange nodes X3J20 4.1). 
    /// </remarks>
    public abstract partial class SemanticNode : ParseNode
    {
    }
}
