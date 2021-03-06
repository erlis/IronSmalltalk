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

namespace IronSmalltalk.Runtime.Execution.Internals
{
    /// <summary>
    /// Exception that occured during code generation and indicates some internal
    /// inconsistenct. Normally, this should not be thrown.
    /// </summary>
    public class InternalCodeGenerationException: CodeGenerationException
    {
        public InternalCodeGenerationException() { }
        public InternalCodeGenerationException(string message) : base(message) { }
        public InternalCodeGenerationException(string message, Exception inner) : base(message, inner) { }
#if !SILVERLIGHT
        protected InternalCodeGenerationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
