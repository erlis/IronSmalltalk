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

namespace IronSmalltalk.Runtime
{
    partial class SmalltalkClass //: IDynamicMetaObjectProvider
    {
        //#region IDynamicMetaObjectProvider implementation

        //DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        //{
        //    return new SmalltalkClassDynamicMetaObject(parameter, BindingRestrictions.Empty, this);
        //}

        //public class SmalltalkClassDynamicMetaObject : DynamicMetaObject
        //{
        //    public SmalltalkClassDynamicMetaObject(Expression parameter, BindingRestrictions restrictions, SmalltalkClass cls)
        //        : base(parameter, restrictions, cls)
        //    {

        //    }

        //    public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        //    {
        //        return base.BindInvoke(binder, args);
        //    }

        //    public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        //    {
        //        //return new DynamicMetaObject(this.Expression, this.Restrictions.Merge(BindingRestrictions.GetInstanceRestriction(this.Expression, this.Value)));
        //        return base.BindInvokeMember(binder, args);
        //    }
        //}

        //#endregion
    }
}
