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

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    /// <summary>
    /// CallSiteBinderCache caches the Smalltalk call-site-binders accross a SmalltalkRuntime.
    /// </summary>
    /// <remarks>
    /// One CallSiteBinderCache exists per SmalltalkRuntime and the cached binders in it
    /// belong to the same SmalltalkRuntime.
    /// </remarks>
    public class CallSiteBinderCache
    {
        public readonly CallSiteBinderCacheTable MessageSendCache = new CallSiteBinderCacheTable();
        public readonly CallSiteBinderCacheTable ConstantSendCache = new CallSiteBinderCacheTable();

        /// <summary>
        /// Cached ObjectClassCallSiteBinder.
        /// </summary>
        /// <remarks>
        /// Only one ObjectClassCallSiteBinder exists per SmalltalkRuntime.
        /// This is due to the fact that the ObjectClassCallSiteBinder does
        /// not contain any instance specific information.
        /// </remarks>
        public ObjectClassCallSiteBinder CachedObjectClassCallSiteBinder;

        /// <summary>
        /// Cached DoesNotUnderstandCallSiteBinder.
        /// </summary>
        /// <remarks>
        /// Only one DoesNotUnderstandCallSiteBinder exists per SmalltalkRuntime.
        /// This is due to the fact that the DoesNotUnderstandCallSiteBinder does
        /// not contain any instance specific information.
        /// </remarks>
        public DoesNotUnderstandCallSiteBinder CachedDoesNotUnderstandCallSiteBinder;

        public static CallSiteBinderCache GetCache(SmalltalkRuntime runtime)
        {
            if (runtime == null)
                throw new ArgumentNullException();

            object cache;
            runtime.ServicesCache.TryGetValue(typeof(CallSiteBinderCache), out cache);
            CallSiteBinderCache binderCache = cache as CallSiteBinderCache;
            if (binderCache == null)
            {
                binderCache = new CallSiteBinderCache();
                runtime.ServicesCache[typeof(CallSiteBinderCache)] = binderCache;
            }
            return binderCache;
        }
    }
}
