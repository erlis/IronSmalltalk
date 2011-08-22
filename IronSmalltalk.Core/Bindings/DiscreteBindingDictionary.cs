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
using System.Collections;


namespace IronSmalltalk.Runtime.Bindings
{
    /// <summary>
    /// This is a dictionary of discrete binding items. It resembles a normal dictionary,
    /// but item can be accessed using both Strings or Symbols as the key. 
    /// It is also read-only to most clients, so they can't modify it and break consistency.
    /// </summary>
    /// <typeparam name="TItem">Type of the discrete binding.</typeparam>
    /// <remarks>
    /// Instances of DiscreteBindingDictionary are not synchronized. 
    /// This class is intended for read-only purposes. When the environment 
    /// is to be modified, the installer creates a copy of the dictionary 
    /// and replaces the reference to the whole dictionary (actually, to
    /// the namescope) in one atomic operation.
    /// </remarks>
    public class DiscreteBindingDictionary<TItem> : BindingDictionary<TItem>
        where TItem : IDiscreteBinding
    {
        public DiscreteBindingDictionary(SmalltalkRuntime runtime)
            : this(runtime, 100)
        {
        }

        public DiscreteBindingDictionary(SmalltalkRuntime runtime, int initialCapacity)
            : base(runtime, initialCapacity)
        {
        }
    }
}
