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
using IronSmalltalk.Compiler.LexicalTokens;

namespace IronSmalltalk.Compiler.SemanticNodes
{
    /// <summary>
    /// Parse node representing primitive (API) call. This is an addition to the X3J20 standard.
    /// </summary>
    // <api call> ::= '<' <api convention> <api type> <api name> <api parameter>* <api result> '>'
    //      <api convention> ::= keyword
    //      <api type> ::= identifier | quotedString 
    //      <api name> ::= identifier | quotedString 
    //      <api parameter> ::= identifier | quotedString 
    //      <api result> ::= identifier | quotedString 
    // EXAMPLE: <static: Array Copy Array Array Int32>
    //      Array.Copy(Array arg1, Array arg2, Int32 arg3);
    // EXAMPLE: <static: 'System.Console' WriteLine string>
    //      System.Console(string arg1);
    // EXAMPLE: <call: 'System.String' ToUpper string>
    //      string this.ToUpper();
    public partial class PrimitiveCallNode : SemanticNode 
    {
        public MethodNode Parent { get; private set; }

        public BinarySelectorToken OpeningDelimiter { get; private set; }

        public BinarySelectorToken ClosingDelimiter { get; private set; }

        public KeywordToken ApiConvention { get; private set; }

        public IList<IPrimitiveCallParameterToken> ApiParameters { get; private set; }

        public PrimitiveCallNode(MethodNode parent)
        {
            this.Parent = parent;
            this.ApiParameters = new List<IPrimitiveCallParameterToken>();
        }

        // Initializes the node after being parsed by the parser.
        protected internal void SetContents(BinarySelectorToken openingDelimiter, BinarySelectorToken closingDelimiter,
            KeywordToken apiConvention, IEnumerable<IPrimitiveCallParameterToken> parameters)
        {
            this.OpeningDelimiter = openingDelimiter;
            this.ClosingDelimiter = closingDelimiter;
            this.ApiConvention = apiConvention;
            this.ApiParameters.Clear();
            foreach (IPrimitiveCallParameterToken param in parameters)
                this.ApiParameters.Add(param);
        }

        /// <summary>
        /// Get the child nodes directly defines in this node.
        /// </summary>
        /// <returns>An enumerable collection with the child nodes directly defines in this node.</returns>
        public override IEnumerable<IParseNode> GetChildNodes()
        {
            return new IParseNode[0];
        }

        /// <summary>
        /// Get all non-whitespace tokens that directly define the parse node.
        /// </summary>
        /// <returns>An enumerable collection with the tokens directly defining this node.</returns>
        public override IEnumerable<IToken> GetTokens()
        {
            List<IToken> result = new List<IToken>();
            if (this.OpeningDelimiter != null)
                result.Add(this.OpeningDelimiter);
            if (this.ApiConvention != null)
                result.Add(this.ApiConvention);
            foreach (IPrimitiveCallParameterToken param in this.ApiParameters)
                result.Add(param);
            if (this.ClosingDelimiter != null)
                result.Add(this.ClosingDelimiter);
            return result;
        }

        /// <summary>
        /// String representation of the parse node for diagnostic purposes.
        /// </summary>
        public override string PrintString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<");
            if (this.ApiConvention != null)
                sb.Append(this.ApiConvention.Value);
            foreach (Token token in this.ApiParameters)
                sb.AppendFormat(" {0}", token.SourceString);
            sb.Append(">");
            return sb.ToString();
        }
    }
}
