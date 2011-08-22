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
using System.Runtime.Serialization;

namespace IronSmalltalk.Runtime.Execution.Internals
{
    /// <summary>
    /// Exception that indicates that attempt to resolve some .Net type during
    /// primitive call (method with primitive call to a .Net construct) 
    /// failed to resolve the .Net type it depends on.
    /// </summary>

    public class PrimitiveInvalidTypeException : RuntimeCodeGenerationException
    {
        public PrimitiveInvalidTypeException()
        {
        }
        public PrimitiveInvalidTypeException(string message)
            : base(message)
        {
        }
        public PrimitiveInvalidTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }

#if !SILVERLIGHT
        protected PrimitiveInvalidTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
