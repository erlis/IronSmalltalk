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
using IronSmalltalk.Runtime.Execution.Dynamic;
using IronSmalltalk.Runtime;

namespace IronSmalltalk
{
    partial class SmalltalkRuntime : ISmalltalkDynamicMetaObjectProvider
    {
        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(System.Linq.Expressions.Expression parameter)
        {
            // Create the restrictions, which in pseudo-C# is defines as:
            //  SmalltalkClass cls = this.Class;    // Constant value  
            //  (self is SmalltalkObject) && (((SmalltalkObject) self).Class == cls);
            BindingRestrictions restrictions = BindingRestrictions.GetInstanceRestriction(parameter, this); 
            SmalltalkClass cls = this.NativeTypeClassMap.GetSmalltalkClass(typeof(SmalltalkRuntime));
            // If not explicitely mapped to a ST Class, fallback to the generic .Net mapping class.
            if (cls == null)
                cls = this.NativeTypeClassMap.Native;
            if (cls == null)
                cls = this.NativeTypeClassMap.Object;
            return new SmalltalkDynamicMetaObject(parameter, restrictions, cls, this);
        }

    }
}
