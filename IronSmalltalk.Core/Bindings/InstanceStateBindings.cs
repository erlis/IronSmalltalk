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

namespace IronSmalltalk.Runtime.Bindings
{
    /// <summary>
    /// Binding of a instance variable. The binding itself is not a discrete binding 
    /// and does not contain the value if the variable. Instead it contains the index
    /// int the instance variables array that is attached to each object and that
    /// contains the actual values.
    /// </summary>
    public class InstanceVariableBinding : Binding<int>
    {
        public InstanceVariableBinding(Symbol name)
            : base(name)
        {
            this._value = -1;
        }

        internal void SetValue(int value)
        {
            this._value = value;
        }
    }

    /// <summary>
    /// Binding of a class-instance variable. The binding itself is not a discrete binding 
    /// and does not contain the value if the variable. Instead it contains the index
    /// int the instance variables array that is attached to each object and that
    /// contains the actual values.
    /// </summary>
    public class ClassInstanceVariableBinding : Binding<int>
    {
        public ClassInstanceVariableBinding(Symbol name)
            : base(name)
        {
            this._value = -1;
        }

        internal void SetValue(int value)
        {
            this._value = value;
        }
    }
}
