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
using System.Dynamic;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    /// <summary>
    /// Base class for IromSmalltalk's DynamicMetaObjectBinders.
    /// This class contains very little functionality, but is 
    /// used to logically group the binders.
    /// </summary>
    /// <remarks>
    /// Each concrete subclass will be responsible for implementing the Bind method.
    /// </remarks>
    public abstract class SmalltalkDynamicMetaObjectBinder : DynamicMetaObjectBinder
    {
        /// <summary>
        /// The Smalltalk Runtime that this binder belongs to.
        /// </summary>
        public SmalltalkRuntime Runtime { get; private set; }

        /// <summary>
        /// Create a new SmalltalkDynamicMetaObjectBinder.
        /// </summary>
        /// <param name="runtime">SmalltalkRuntine that this binder belongs to.</param>
        public SmalltalkDynamicMetaObjectBinder(SmalltalkRuntime runtime)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            this.Runtime = runtime;
        }
    }
}
