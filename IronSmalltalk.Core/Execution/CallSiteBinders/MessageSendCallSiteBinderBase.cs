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
using System.Linq.Expressions;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    /// <summary>
    /// Base class for the dynamic message send binder. 
    /// This binder is responsible for binding the operationgs for message sends.
    /// </summary>
    public abstract class MessageSendCallSiteBinderBase : SmalltalkDynamicMetaObjectBinder
    {
        /// <summary>
        /// Selector of the message being sent.
        /// </summary>
        public Symbol Selector { get; private set; }

        /// <summary>
        /// Create a new MessageSendCallSiteBinderBase.
        /// </summary>
        /// <param name="runtime">SmalltalkRuntine that this binder belongs to.</param>
        /// <param name="selector">Selector of the message being sent.</param>
        public MessageSendCallSiteBinderBase(SmalltalkRuntime runtime, Symbol selector)
            : base(runtime)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            this.Selector = selector;
        }

        /// <summary>
        /// Performs the binding of the dynamic operation.
        /// </summary>
        /// <param name="target">The target of the dynamic operation.</param>
        /// <param name="args">An array of arguments of the dynamic operation.</param>
        /// <returns>The System.Dynamic.DynamicMetaObject representing the result of the binding.</returns>
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            // Look-up the method implementation for the given selector 
            // and generate an expression (runtime code) for that method.
            // This also returns the restriction needed for the PIC.
            BindingRestrictions restrictions;
            SmalltalkClass receiverClass;
            Expression expression;
            MethodLookupHelper.GetMethodInformation(this.Runtime,
                this.Selector,
                this.GetSuperLookupScope(),
                target.Value,
                target,
                args,
                out receiverClass,
                out restrictions,
                out expression);

            // If no code was generated, method is missing, and we must do #doesNotUnderstand. See DoesNotUnderstandCallSiteBinder. 
            if (expression == null)
            {
                DoesNotUnderstandCallSiteBinder binder = CallSiteBinderCache.GetCache(this.Runtime).CachedDoesNotUnderstandCallSiteBinder;
                if (binder == null)
                {
                    binder = new DoesNotUnderstandCallSiteBinder(this.Runtime);
                    CallSiteBinderCache.GetCache(this.Runtime).CachedDoesNotUnderstandCallSiteBinder = binder;
                }
                expression = Expression.Dynamic(binder, typeof(Object),
                    target.Expression,
                    Expression.Constant(this.Selector, typeof(object)),
                    Expression.NewArrayInit(typeof(object), args.Select(d => Expression.Convert(d.Expression, typeof(object))).ToArray()));
            }

            // Return a DynamicMetaObject with the generated code.
            // Important here are the restrictions, which ensure that as long as <self> is 
            // of the correct type, we can freely execute the code for the method we've just found.
            return new DynamicMetaObject(expression, target.Restrictions.Merge(restrictions));
        }

        /// <summary>
        /// For super sends, return the name of the class ABOVE which to start the method lookup.
        /// </summary>
        /// <returns>Return a class name or null to start the method lookup immedeately.</returns>
        protected virtual Symbol GetSuperLookupScope()
        {
            return null;
        }
    }
}
