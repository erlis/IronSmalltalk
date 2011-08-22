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
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime.Execution.CallSiteBinders;

namespace IronSmalltalk.Runtime.Execution.Dynamic
{
    public interface ISmalltalkDynamicMetaObjectProvider : IDynamicMetaObjectProvider
    {
    }

    public class SmalltalkDynamicMetaObject : DynamicMetaObject
    {
        private SmalltalkClass Class;

        public SmalltalkDynamicMetaObject(Expression expression, BindingRestrictions restrictions, SmalltalkClass cls, object value)
            : base(expression, restrictions, value)
        {
            if (cls == null)
                throw new ArgumentNullException("cls");
            this.Class = cls;
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            DynamicMetaObject result = this.PerformOperation(binder.Name, binder.IgnoreCase, binder.CallInfo.ArgumentCount, args);
            if (result != null)
                return result;
            return base.BindInvokeMember(binder, args);
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            DynamicMetaObject result = this.PerformOperation(binder.Name, binder.IgnoreCase, 0, null);
            if (result != null)
                return result;
            return base.BindGetMember(binder);
        }


        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            DynamicMetaObject result = this.PerformOperation(binder.Name, binder.IgnoreCase, 1, new DynamicMetaObject[] { value });
            if (result != null)
                return result;

            return base.BindSetMember(binder, value);
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            return base.BindGetIndex(binder, indexes);
        }

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            return base.BindSetIndex(binder, indexes, value);
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            return base.BindInvoke(binder, args);
        }

        public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
        {
            return base.BindUnaryOperation(binder);
        }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            return base.BindBinaryOperation(binder, arg);
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            return base.BindConvert(binder);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            HashSet<string> names = new HashSet<string>();
            //foreach (var behavior in this.Behaviors)
            //{
            //    foreach (string name in behavior.GetNativeMethodNames())
            //        names.Add(name);
            //}
            return names;
        }


        private DynamicMetaObject PerformOperation(string name, bool ignoreCase, int argumentCount, DynamicMetaObject[] args)
        {
            bool caseConflict = false;
            SmalltalkClass cls = this.Class;
            Symbol na = null;
            CompiledMethod method = MethodLookupHelper.LookupMethod(ref cls, ref na, delegate(SmalltalkClass c)
            {
                return c.InstanceBehavior.GetMethodByNativeName(name, argumentCount, ignoreCase, out caseConflict);
            });

            if (method != null)
            {
                if (args == null)
                    args = new DynamicMetaObject[0];
                var compilationResult = method.Code.CompileInstanceMethod(cls.Runtime, cls, this, args, null);
                return compilationResult.GetDynamicMetaObject(this.Restrictions);
            }
            return null;
        }
    }
}
