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
using System.Collections.Concurrent;
using IronSmalltalk.Runtime.Execution.CallSiteBinders;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    public class CallSiteBinderCacheTable
    {
        /// <summary>
        /// Selectors of messages we concider common and worth caching agresively.
        /// </summary>
        /// <remarks>
        /// The list below was creating by examining an existing Smalltalk sourcecode
        /// and determining the most often sent messages (as number of call-sites).
        /// </remarks>
        public static readonly string[] CommonSelectors = new string[] {
            "=", "~=", "==", "~~", ">", ">=", "<", "<=",    // comparison operations 
            "+", "-", "*", "/", "\\\\", "//",               // arithmetic operations 
            "&", "|",                                       // logical operations 
            "@", ",",                                       // miscellaneous 
            "add:", "addAll:", "and:", "asString", "at:", "at:ifAbsent:", "at:put:", "atEnd", 
            "basicAt:", "basicAt:put:", "basicHash", "basicHash:", "basicNew", "basicNew:", "basicSize", 
            "between:and:", "class", "do:", "doesNotUnderstand:", "ensure:",
            "ifFalse:", "ifFalse:ifTrue:", "ifTrue:", "ifTrue:ifFalse:", 
            "isEmpty", "isNil", "key", "max:", "min:", "new", "new:", 
            "nextPut:", "nextPutAll:", "not", "notNil", "on:do:", "or:", "printOn:", "printString", 
            "propertyAt:", "propertyAt:ifAbsent:", "propertyAt:ifAbsentPut:", "propertyAt:put:", 
            "release", "size", "to:by:do:", "to:do:", "triggerEvent:", "value", "value:", "value:value:", 
            "vmInterrupt:", "when:send:to:", "when:send:to:with:", 
            "whileFalse", "whileFalse:", "whileTrue", "whileTrue:", "with:", "with:with:", "x", "y", "yourself"
        };

        private readonly WeakCallSiteBinderCache WeakCache = new WeakCallSiteBinderCache();
        private readonly ConcurrentDictionary<string, MessageSendCallSiteBinder> StrongCache;

        public CallSiteBinderCacheTable()
        {
            this.WeakCache = new WeakCallSiteBinderCache();
            this.StrongCache = new ConcurrentDictionary<string, MessageSendCallSiteBinder>();
            foreach (string selector in CallSiteBinderCacheTable.CommonSelectors)
                this.StrongCache[selector] = null;
        }

        public MessageSendCallSiteBinder GetBinder(string selector)
        {
            if (String.IsNullOrEmpty(selector))
                throw new ArgumentNullException();

            MessageSendCallSiteBinder result;
            this.StrongCache.TryGetValue(selector, out result);
            if (result != null)
                return result;

            return this.WeakCache.GetItem(selector);
        }

        public MessageSendCallSiteBinder AddBinder(MessageSendCallSiteBinder binder)
        {
            if (binder == null)
                throw new ArgumentNullException();

            MessageSendCallSiteBinder result;
            if (this.StrongCache.TryGetValue(binder.Selector.Value, out result))
            {
                // It is one of the common selectors
                if (result != null)
                    // Already cached
                    return result;
                // Cache it and return ...
                this.StrongCache.TryUpdate(binder.Selector.Value, binder, null);
                return this.StrongCache[binder.Selector.Value];
            }

            return this.WeakCache.AddItem(binder);
        }

        /// <summary>
        /// A weak dictionary of call-site-binders.
        /// </summary>
        /// <remarks>
        /// This is roughly based on the SymbolTable
        /// </remarks>
        internal class WeakCallSiteBinderCache
        {
            private ConcurrentDictionary<string, WeakReference> _contents;

            /// <summary>
            /// Create and initialize an empty weak table.
            /// </summary>
            public WeakCallSiteBinderCache()
            {
                // We expect very low concurrency on writing ... 
                // Use StringComparer.InvariantCulture ... because keys are case-sensitive etc.
                this._contents = new ConcurrentDictionary<string, WeakReference>(
                    Environment.ProcessorCount, 250, StringComparer.InvariantCulture);
            }

            /// <summary>
            /// Get a Call-Site-Binder that represent a message send with the given selector.
            /// </summary>
            /// <param name="value">String value of the selector.</param>
            /// <returns>An existing Call-Site-Binder or null if none.</returns>
            public MessageSendCallSiteBinder GetItem(string selector)
            {
                if (selector == null)
                    throw new ArgumentNullException();

                // 1. Try to get the CSB from the dictionary. There are good changes that:
                WeakReference reference;
                this._contents.TryGetValue(selector, out reference);
                if (reference == null)
                    return null;
                // 2. Get the CSB from the weak reference holding it
                return reference.Target as MessageSendCallSiteBinder;
            }

            public MessageSendCallSiteBinder AddItem(MessageSendCallSiteBinder binder)
            {
                if (binder == null)
                    throw new ArgumentNullException();

                // 1. Try to get the CSB from the dictionary. 
                binder.FinalizationManager = this;
                WeakReference reference = this._contents.GetOrAdd(binder.Selector.Value, na => new WeakReference(binder, false));
                // 2. Get the CSB from the weak reference holding it
                MessageSendCallSiteBinder result = reference.Target as MessageSendCallSiteBinder;
                // Once here, it can't be GC'ed.
                if (result != null)
                    // somebody else managed to put 
                    return result;
                reference.Target = binder;
                return binder;
            }

            /// <summary>
            /// A Call-Site-Binder was GC'ed. Remove the Call-Site-Binder info from the internal string-CSB dictionary.
            /// </summary>
            /// <param name="key">String value of the Call-Site-Binder that was GC'ed.</param>
            internal void InternalRemoveItem(string selector)
            {
                WeakReference reference;
                this._contents.TryGetValue(selector, out reference);
                if (reference == null)
                    return;

                // Check if the weak reference's Target reference a CSB. It may:
                //      a. CSB is null ... 1.a. ... CSB was GC'ed and GC set Target to null.
                //      b. CSB is NOT null ... 1.b. ... CSB was GC'ed and GC set Target to null,
                //         however, before this code managed to run, somebody requested a CSB with the
                //         same selector, and a new CSB object was created.
                //         Therefore, we cannot throw the weak reference away!
                MessageSendCallSiteBinder csb = reference.Target as MessageSendCallSiteBinder;
                if (csb == null)
                    // Remove the weak reference from the contents dictionary ... this is case a).
                    this._contents.TryRemove(selector, out reference);
            }
        }
    }
}
