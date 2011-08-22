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
using IronSmalltalk.Runtime;
using System.Linq.Expressions;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime.Execution.Dynamic;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    /// <summary>
    /// Call-Site-Binder for nomal (non-super) dynamic message sends. 
    /// This binder is responsible for binding the operations for message sends.
    /// </summary>
    public class MessageSendCallSiteBinder : MessageSendCallSiteBinderBase 
    {
        /// <summary>
        /// Create a new MessageSendCallSiteBinder.
        /// </summary>
        /// <param name="runtime">SmalltalkRuntine that this binder belongs to.</param>
        /// <param name="selector">Selector of the message being sent.</param>
        /// <param name="nativeName">Name of the method that the target is asked to bind.</param>
        /// <param name="argumentCount">Number of method arguments.</param>
        public MessageSendCallSiteBinder(SmalltalkRuntime runtime, Symbol selector, string nativeName, int argumentCount)
            : base(runtime, selector)
        {
            if (argumentCount < 0)
                throw new ArgumentOutOfRangeException("argumentCount");
            this.NativeName = nativeName;
            this.ArgumentCount = argumentCount;
        }

        /// <summary>
        /// Performs the binding of the dynamic operation.
        /// </summary>
        /// <param name="target">The target of the dynamic operation.</param>
        /// <param name="args">An array of arguments of the dynamic operation.</param>
        /// <returns>The System.Dynamic.DynamicMetaObject representing the result of the binding.</returns>
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            // When binding a normal message send, we have two options
            if ((target is SmalltalkDynamicMetaObject) || (this.NativeName == null))
                // 1. If dealing with a Smalltalk object, don't do the normal BindInvokeMember and perform our logic directly
                return base.Bind(target, args);
            else
                // 2. For all other objects, ask the object to bing the operation and fallback to ST only if it's not successful.
                // NB: Maybe we wan't to do binding BEFORE the object, so we can shadow its methods and not vice versa.
                return target.BindInvokeMember(this.InvokeMemberBinder, args);
        }

        private DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }

        private DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            return base.Bind(target, args);
        }

        #region InvokeMemberBinder functionality

        private readonly string NativeName;
        private readonly int ArgumentCount;
        private MessageSendInvokeMemberBinder _invokeMemberBinder;
        private MessageSendInvokeMemberBinder InvokeMemberBinder
        {
            get
            {
                if (this._invokeMemberBinder == null)
                    this._invokeMemberBinder = new MessageSendInvokeMemberBinder(this);
                return this._invokeMemberBinder;
            }
        }

        private class MessageSendInvokeMemberBinder : InvokeMemberBinder
        {
            private readonly MessageSendCallSiteBinder Binder;

            public MessageSendInvokeMemberBinder(MessageSendCallSiteBinder binder)
                : base(binder.NativeName, false, MessageSendInvokeMemberBinder.GetCallInfo(binder.ArgumentCount))
            {
                this.Binder = binder;
            }

            public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
            {
                return this.Binder.FallbackInvoke(target, args, errorSuggestion);
            }

            public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
            {
                return this.Binder.FallbackInvokeMember(target, args, errorSuggestion);
            }

            #region Helpers

            private static CallInfo[] CachedCallInfos = new CallInfo[16];

            public static CallInfo GetCallInfo(int args)
            {
                if ((args >= 0) && (args < MessageSendInvokeMemberBinder.CachedCallInfos.Length) && (MessageSendInvokeMemberBinder.CachedCallInfos[args] != null))
                    return MessageSendInvokeMemberBinder.CachedCallInfos[args];

                List<string> argumentNames = new List<string>();
                argumentNames.Add("self");
                for (int i = 0; i < args; i++)
                    argumentNames.Add(String.Format("_arg_{0}", i));
                CallInfo result = new CallInfo(argumentNames.Count, argumentNames);
                if (args < MessageSendInvokeMemberBinder.CachedCallInfos.Length)
                    MessageSendInvokeMemberBinder.CachedCallInfos[args] = result;
                return result;
            }

            #endregion
        }

        #endregion

        #region Call-Site-Binder Cache Support

        private CallSiteBinderCacheTable.WeakCallSiteBinderCache _finalizationManager;

        internal CallSiteBinderCacheTable.WeakCallSiteBinderCache FinalizationManager
        {
            get
            {
                return this._finalizationManager;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                this._finalizationManager = value;
            }
        }

        ~MessageSendCallSiteBinder()
        {
            if (this._finalizationManager != null)
                this._finalizationManager.InternalRemoveItem(this.Selector.Value);
        }

        #endregion
    }
}
