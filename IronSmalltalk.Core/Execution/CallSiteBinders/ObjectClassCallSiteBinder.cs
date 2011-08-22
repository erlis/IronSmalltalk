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
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime;
using System.Linq.Expressions;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    /// <summary>
    /// This is a special binding that is used to get the class of an object.
    /// </summary>
    /// <remarks>
    /// It resembles functionally a normal DynamicMetaObjectBinder, except 
    /// that it never evaluates any dynamic code but when bound, it returns
    /// a constant which is the class of an object. The restrictions applied
    /// to the dynamic call site ensure that this works correctly. In other
    /// words, as long as the restrictions evaluate "true" for an object,
    /// the hard-coded class constant is returned, if not, back to the PIC cache.
    /// </remarks>
    public class ObjectClassCallSiteBinder : SmalltalkDynamicMetaObjectBinder
    {
        /// <summary>
        /// Create a new ObjectClassCallSiteBinder.
        /// </summary>
        /// <param name="runtime">SmalltalkRuntine that this binder belongs to.</param>
        public ObjectClassCallSiteBinder(SmalltalkRuntime runtime)
            : base(runtime)
        {
        }

        /// <summary>
        /// Performs the binding of the dynamic operation.
        /// </summary>
        /// <param name="target">The target of the dynamic operation.</param>
        /// <param name="args">An array of arguments of the dynamic operation.</param>
        /// <returns>The System.Dynamic.DynamicMetaObject representing the result of the binding.</returns>
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            BindingRestrictions restrictions;
            SmalltalkClass receiverClass = MethodLookupHelper.GetClassAndRestrictions(
                this.Runtime,
                target.Value,
                target,
                args,
                out restrictions);

#if DEBUG
            if (receiverClass == null)
                throw new InvalidOperationException("Should have found the Smalltalk class object - there is always supposed to be one.");
#endif

            // Create a dynamic expression that returns a constant with the class object.
            Expression expression = Expression.Constant(receiverClass, typeof(object));
            // Important here are the restrictions, which ensure that as long as <self> is 
            // of the correct type, we can freely return the class object constant.
            return new DynamicMetaObject(expression, target.Restrictions.Merge(restrictions));
        }
    }
}
