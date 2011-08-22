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

namespace IronSmalltalk.Compiler.Interchange.ParseNodes
{
    public partial class InterchangeVersionIdentifierNode : InterchangeParseNode
    {
        public StringToken VersionId { get; private set; }

        public InterchangeVersionIdentifierNode()
        {
        }

        public InterchangeVersionIdentifierNode(StringToken versionId)
        {
            this.VersionId = versionId;
        }
    }
}
