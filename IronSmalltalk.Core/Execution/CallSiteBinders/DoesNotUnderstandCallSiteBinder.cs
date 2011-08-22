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
using IronSmalltalk.Runtime.Execution.Internals;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    /// <summary>
    /// This is a special binding that is used to send the #_doesNotUnderstand:arguments: message 
    /// to inform the object that a message that it does not understand was send to it.
    /// </summary>
    /// <remarks>
    /// The DoesNotUnderstandCallSiteBinder is in many ways similar to the MessageSendCallSiteBinderBase.
    /// The exception is that it is hardcoded to send the #_doesNotUnderstand:arguments:,
    /// and does throw a runtime exception if the method is missing, because it can't do 
    /// a recursive #doesNotUnderstand: on #doesNotUnderstand:. Every object in the system
    /// must implement #_doesNotUnderstand:arguments:!
    /// 
    /// When the regular MessageSendCallSiteBinderBase experience a message send that
    /// the receiver does not understand, it auto-generates a method to send the
    /// #_doesNotUnderstand:arguments: to the receiver.  
    /// 
    /// Example:
    ///     anObject invalidMessageWith: arg1 with: arg2 with: arg3.
    ///     
    /// MessageSendCallSiteBinderBase auto-generates a method:
    ///     invalidMessageWith: arg1 with: arg2 with: arg3
    ///     
    ///     ^self _doesNotUnderstand: #invalidMessageWith:with:with:
    ///         arguments: (Array with: arg1 with: arg2 with: arg3).
    /// 
    /// The main difference is that the auto-generated code only exists in the PIC cache
    /// and the dynamic message send to  #_doesNotUnderstand:arguments: does not use
    /// the standard MessageSendCallSiteBinder but the special DoesNotUnderstandCallSiteBinder.
    /// </remarks>
    public class DoesNotUnderstandCallSiteBinder : SmalltalkDynamicMetaObjectBinder
    {
        /// <summary>
        /// Create a new DoesNotUnderstandCallSiteBinder.
        /// </summary>
        /// <param name="runtime">SmalltalkRuntine that this binder belongs to.</param>
        public DoesNotUnderstandCallSiteBinder(SmalltalkRuntime runtime)
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
#if DEBUG
            if ((args == null) || (args.Length != 2))
                throw new InvalidOperationException("The DoesNotUnderstandCallSiteBinder is special case and always requires 2 method arguments.");
#endif
            // Look-up the #_doesNotUnderstand:arguments: method.
            BindingRestrictions restrictions;
            SmalltalkClass receiverClass;
            Expression expression;
            MethodLookupHelper.GetMethodInformation(this.Runtime,
                this.Runtime.GetSymbol("_doesNotUnderstand:arguments:"),
                null,
                target.Value,
                target,
                args,
                out receiverClass,
                out restrictions,
                out expression);

            // Every class is supposed to implement the #_doesNotUnderstand:arguments:, if not, throw a runtime exception
            if (expression == null)
                throw new RuntimeCodeGenerationException(RuntimeCodeGenerationErrors.DoesNotUnderstandMissing);

            // Perform a standard cal to the #_doesNotUnderstand:arguments:
            return new DynamicMetaObject(expression, target.Restrictions.Merge(restrictions));
        }
    }
}
