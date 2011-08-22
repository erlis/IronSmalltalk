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
using System.IO;
using IronSmalltalk.Compiler.Interchange.ParseNodes;
using IronSmalltalk.Interchange;
using IronSmalltalk.Runtime;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.Installer;

namespace IronSmalltalk.Compiler.Interchange
{
    public class InterchangeFormatParserIST10 : InterchangeFormatParser
    {
        public InterchangeFormatParserIST10(TextReader reader)
            : base(reader)
        {
        }

        protected override ParseNodes.ClassDefinitionNode CreateClassDefinitionNode()
        {
            return new ClassDefinitionNodeIST10();
        }
    }

    public class ClassDefinitionNodeIST10 : ClassDefinitionNode
    {
        protected override SmalltalkClass.InstanceStateEnum? GetInstanceState(InterchangeFormatProcessor processor, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            if (this.IndexedInstanceVariables.Value == "native")
                return SmalltalkClass.InstanceStateEnum.Native;
            return base.GetInstanceState(processor, parseErrorSink, sourceCodeService);
        }
    }
}
