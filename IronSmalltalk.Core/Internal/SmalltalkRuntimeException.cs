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

namespace IronSmalltalk.Runtime.Internal
{
    /// <summary>
    /// Represents a Smalltalk runtime exception.
    /// </summary>
    [Serializable]
    public class SmalltalkRuntimeException : Exception
    {
        /// <summary>
        /// Creates and initializes a new exception.
        /// </summary>
        public SmalltalkRuntimeException() { }

        /// <summary>
        /// Creates and initializes a new exception with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SmalltalkRuntimeException(string message) : base(message) { }

        /// <summary>
        /// Creates and initializes a new exception with the specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null if no inner exception is specified.</param>
        public SmalltalkRuntimeException(string message, Exception inner) : base(message, inner) { }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the exception with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected SmalltalkRuntimeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
