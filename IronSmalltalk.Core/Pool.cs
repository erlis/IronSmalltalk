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
using IronSmalltalk.Runtime.Bindings;

namespace IronSmalltalk.Runtime
{
    /// <summary>
    /// A Pool (a.k.a. Pool Dictionary) that contains pool variables and pool constants.
    /// </summary>
    public class Pool : DiscreteBindingDictionary<PoolVariableOrConstantBinding>
    {
        /// <summary>
        /// The name of the pool.
        /// </summary>
        public Symbol Name { get; private set; }

        /// <summary>
        /// Create a new pool with the given name within the given Smalltalk context.
        /// </summary>
        /// <param name="runtime">Smalltalk runtime that the pool belongs to.</param>
        /// <param name="name">Name of the pool.</param>
        public Pool(SmalltalkRuntime runtime, Symbol name)
            : base(runtime)
        {
            if (name == null)
                throw new ArgumentNullException();
            this.Name = name;
        }
    }
}
