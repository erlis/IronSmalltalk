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
    /// Indicates an exception that occured due to a problem with the Smalltalk definitions,
    /// for example some problems with the source code that was being installed.
    /// </summary>
    [Serializable]
    public class SmalltalkDefinitionException : Exception
    {
        /// <summary>
        /// Creates and initializes a new exception.
        /// </summary>
        public SmalltalkDefinitionException() { }

        /// <summary>
        /// Creates and initializes a new exception with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SmalltalkDefinitionException(string message) : base(message) { }

        /// <summary>
        /// Creates and initializes a new exception with the specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null if no inner exception is specified.</param>
        public SmalltalkDefinitionException(string message, Exception inner) : base(message, inner) { }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the exception with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected SmalltalkDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
