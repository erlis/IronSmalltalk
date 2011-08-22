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
using System.Reflection;

namespace IronSmalltalk.Runtime.Internal
{
    /// <summary>
    /// Home context of a block closure.
    /// </summary>
    /// <remarks>
    /// The home context is an object used as a marker to identify the unique method activation.
    /// </remarks>
    public class HomeContext : Object
    {
    }

    /// <summary>
    /// Result of a non-local return of a block closure.
    /// </summary>
    /// <remarks>
    /// A block closure with explicit (non-local) return throws an instance of BlockResult.
    /// The BlockResult contains the actual result and the HomeContext of the block.
    /// </remarks>
    public class BlockResult //: Exception
    {
        /// <summary>
        /// HomeContext identifying which mehtod actication created the block.
        /// </summary>
        public readonly HomeContext HomeContext;

        /// <summary>
        /// Value being returned.
        /// </summary>
        public object Value;

        /// <summary>
        /// Internal. The FieldInfo of the BlockResult.Value field.
        /// </summary>
        public static readonly FieldInfo ValueField;

        /// <summary>
        /// Internal. The FieldInfo of the BlockResult.HomeContext field.
        /// </summary>
        public static readonly FieldInfo HomeContextField;

        /// <summary>
        /// Internal. The ConstructorInfo of the BlockResult.ctor() constructor.
        /// </summary>
        public static readonly ConstructorInfo ConstructorInfo;

        /// <summary>
        /// Initializes the cached reflection information (used by the JIT compiler).
        /// </summary>
        static BlockResult()
        {
            BlockResult.ValueField = typeof(BlockResult).GetField("Value", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);
            if (BlockResult.ValueField == null)
                throw new InvalidOperationException("Could not find BlockResult.Value field.");
            BlockResult.HomeContextField = typeof(BlockResult).GetField("HomeContext", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);
            if (BlockResult.HomeContextField == null)
                throw new InvalidOperationException("Could not find BlockResult.HomeContextField field.");
            BlockResult.ConstructorInfo = typeof(BlockResult).GetConstructor(BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance,
                null, new Type[] { typeof(HomeContext), typeof(object) }, null);
            if (BlockResult.ConstructorInfo == null)
                throw new InvalidOperationException("Could not find BlockResult.ctor(HomeContext, object) field.");
        }

        /// <summary>
        /// Create a new block result.
        /// </summary>
        /// <param name="homeContext">HomeContext identifying which mehtod actication created the block.</param>
        /// <param name="value">Value being returned.</param>
        public BlockResult(HomeContext homeContext, object value)
            //: base("Method has already returned")
        {
#if DEBUG
            if (homeContext == null)
                throw new ArgumentNullException("homeContext");
#endif
            this.HomeContext = homeContext;
            this.Value = value;
        }
    }
}
