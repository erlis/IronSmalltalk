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

namespace IronSmalltalk.Compiler.SemanticAnalysis
{
    /// <summary>
    /// Exception thrown only if we have internal bug in the workings of the Parser.
    /// </summary>
    [Serializable]
    public class InvalidParserOperationException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the InvalidParserOperationException class.
        /// </summary>
        public InvalidParserOperationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidParserOperationException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidParserOperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidParserOperationException class
        /// with a specified error message and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException
        /// parameter is not a null reference (Nothing in Visual Basic), the current
        /// exception is raised in a catch block that handles the inner exception.
        /// </param>
        public InvalidParserOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the InvalidParserOperationException class with serialized data. 
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        [System.Security.SecuritySafeCritical]
        protected InvalidParserOperationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }  
}
