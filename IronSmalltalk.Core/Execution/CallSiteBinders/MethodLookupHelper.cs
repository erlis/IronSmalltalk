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
using IronSmalltalk.Runtime.Internal;
using System.Linq.Expressions;
using IronSmalltalk.Runtime.Behavior;
using System.Dynamic;
using IronSmalltalk.Runtime;
using System.Reflection;

namespace IronSmalltalk.Runtime.Execution.CallSiteBinders
{
    public static class MethodLookupHelper
    {
        /// <summary>
        /// This method is the core of the dynamic method lookup system.
        /// It determines the class of an object and looks-up the method implementation
        /// for a given method selector.
        /// </summary>
        /// <param name="runtime">Required.</param>
        /// <param name="selector">Required.</param>
        /// <param name="superLookupScope">Optional.</param>
        /// <param name="receiver">Optional.</param>
        /// <param name="self">Required.</param>
        /// <param name="arguments">Required (currently not used).</param>
        /// <param name="receiverClass">Must Return!</param>
        /// <param name="restrictions">Must Return!</param>
        /// <param name="executableCode">Return null if missing.</param>
        public static void GetMethodInformation(SmalltalkRuntime runtime, 
            Symbol selector, 
            Symbol superLookupScope, 
            object receiver,
            DynamicMetaObject self,
            DynamicMetaObject[] arguments, 
            out SmalltalkClass receiverClass,
            out BindingRestrictions restrictions, 
            out Expression executableCode)
        {
            restrictions = null;
            SmalltalkClass cls = null;

            // Special case for Smalltalk classes, because we want the class behavior first ...
            if (receiver is SmalltalkClass)
            {
                cls = (SmalltalkClass)receiver;
                if (cls.Runtime == runtime)
                {
                    receiverClass = runtime.NativeTypeClassMap.Class;
                    if (receiverClass == null)
                        receiverClass = runtime.NativeTypeClassMap.Object;
                    // Lookup method in class behavior
                    CompiledMethod mth = MethodLookupHelper.LookupClassMethod(selector, ref cls, ref superLookupScope);
                    if (mth != null)
                    {
                        // A class method, special restrictions
                        restrictions = BindingRestrictions.GetInstanceRestriction(self.Expression, receiver);
                        var compilationResult = mth.Code.CompileClassMethod(runtime, cls, self, arguments, superLookupScope);
                        if (compilationResult == null)
                        {
                            executableCode = null;
                        }
                        else
                        {
                            executableCode = compilationResult.ExecutableCode;
                            restrictions = compilationResult.MergeRestrictions(restrictions);
                        }
                        return;
                    }
                    // Not in class behavior ... fallback to instance / Object behavior
                    cls = receiverClass;
                    restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(SmalltalkClass));
                }
            }

            if ((cls == null) || (restrictions == null))
                cls = GetClassAndRestrictions(runtime, receiver, self, arguments, out restrictions);
            receiverClass = cls;

            // Look-up the method
            CompiledMethod method = MethodLookupHelper.LookupInstanceMethod(selector, ref cls, ref superLookupScope);
            if (method == null)
            {
                executableCode = null;
            }
            else
            {
                var compilationResult = method.Code.CompileInstanceMethod(runtime, cls, self, arguments, superLookupScope);
                if (compilationResult == null)
                {
                    executableCode = null;
                }
                else
                {
                    executableCode = compilationResult.ExecutableCode;
                    restrictions = compilationResult.MergeRestrictions(restrictions);
                }

            }
        }

        /// <summary>
        /// Look-up for an instance method implementation given a method selector and a class.
        /// </summary>
        /// <param name="selector">Method selector to look for.</param>
        /// <param name="cls">Class where to start searching for the method (unless superLookupScope) is set.</param>
        /// <param name="superLookupScope">If set, start the lookup from the superclass of this class.</param>
        /// <returns>Returns the compiled method for the given selector or null if none was found.</returns>
        public static CompiledMethod LookupInstanceMethod(Symbol selector, ref SmalltalkClass cls, ref Symbol superLookupScope)
        {
            return MethodLookupHelper.LookupMethod(ref cls, ref superLookupScope, delegate(SmalltalkClass c)
            {
                CompiledMethod method;
                if (c.InstanceBehavior.TryGetValue(selector, out method))
                    return method;
                return null;
            });
        }

        /// <summary>
        /// Look-up for a class method implementation given a method selector and a class.
        /// </summary>
        /// <param name="selector">Method selector to look for.</param>
        /// <param name="cls">Class where to start searching for the method (unless superLookupScope) is set.</param>
        /// <param name="superLookupScope">If set, start the lookup from the superclass of this class.</param>
        /// <returns>Returns the compiled method for the given selector or null if none was found.</returns>
        /// <remarks>If the method is not found, this functiond does not searches the instance side of the class.</remarks>
        public static CompiledMethod LookupClassMethod(Symbol selector, ref SmalltalkClass cls, ref Symbol superLookupScope)
        {
            return MethodLookupHelper.LookupMethod(ref cls, ref superLookupScope, delegate(SmalltalkClass c)
            {
                CompiledMethod method;
                if (c.ClassBehavior.TryGetValue(selector, out method))
                    return method;
                return null;
            });
        }

        /// <summary>
        /// Look-up a method implementation starting with the given class.
        /// </summary>
        /// <param name="cls">Class where to start searching for the method (unless superLookupScope) is set.</param>
        /// <param name="superLookupScope">If set, start the lookup from the superclass of this class.</param>
        /// <param name="lookupFunction">Function to perform the method lookup.</param>
        /// <returns>Returns the compiled method for the given selector or null if none was found.</returns>
        public static CompiledMethod LookupMethod(ref SmalltalkClass cls, ref Symbol superLookupScope, Func<SmalltalkClass, CompiledMethod> lookupFunction)
        {
            if (lookupFunction == null)
                throw new ArgumentNullException("lookupFunction");

            while (cls != null)
            {
                if (superLookupScope == null)
                {
                    CompiledMethod method = lookupFunction(cls);
                    if (method != null)
                        return method;
                }
                else
                {
                    if (cls.Name == superLookupScope)
                        superLookupScope = null;
                }
                cls = cls.Superclass;
            }

            // No method ... no luck;
            return null;
        }

        /// <summary>
        /// This core method determines the class of an object.
        /// </summary>
        /// <param name="runtime">Required: SmalltalkRuntime containing the Smalltalk classes.</param>
        /// <param name="receiver">Optional: Object whos class is to be determined.</param>
        /// <param name="self">Required: Expression for the receiver.</param>
        /// <param name="arguments">Required: Currently not used.</param>
        /// <param name="restrictions">Restrictions for the given receiver.</param>
        /// <returns>The SmalltalkClass for the given receiver. This always returns an object (unless the given SmalltalkRuntime is inconsistent).</returns>
        public static SmalltalkClass GetClassAndRestrictions(SmalltalkRuntime runtime, 
            object receiver,
            DynamicMetaObject self,
            DynamicMetaObject[] arguments, 
            out BindingRestrictions restrictions)
        {
            SmalltalkClass cls;
            // Special case handling of null, so it acts like first-class-object.
            if (receiver == null)
            {
                cls = runtime.NativeTypeClassMap.UndefinedObject;
                // If not explicitely mapped to a ST Class, fallback to the generic .Net mapping class.
                if (cls == null)
                    cls = runtime.NativeTypeClassMap.Native;
                if (cls == null)
                    cls = runtime.NativeTypeClassMap.Object;
                restrictions = BindingRestrictions.GetInstanceRestriction(self.Expression, null);
            }
            // Smalltalk objects ... almost every objects ends up here.
            else if (receiver is SmalltalkObject)
            {
                SmalltalkObject obj = (SmalltalkObject)receiver;
                cls = obj.Class;
                if (cls.Runtime == runtime)
                {
                    FieldInfo field = typeof(SmalltalkObject).GetField("Class", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);
                    if (field == null)
                        throw new InvalidOperationException();
                    restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(SmalltalkObject));
                    restrictions = restrictions.Merge(BindingRestrictions.GetExpressionRestriction(
                        Expression.ReferenceEqual(Expression.Field(Expression.Convert(self.Expression, typeof(SmalltalkObject)), field), Expression.Constant(cls))));
                }
                else
                {
                    // A smalltalk object, but from different runtime
                    cls = null; // Let block below handle this.
                    restrictions = null;
                }
            }
            else if (receiver is Symbol)
            {
                Symbol symbol = (Symbol)receiver;
                SymbolTable manager = symbol.Manager;
                if (manager.Runtime == runtime)
                {
                    cls = runtime.NativeTypeClassMap.Symbol;
                    FieldInfo field = typeof(Symbol).GetField("Manager", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public);
                    if (field == null)
                        throw new InvalidOperationException();
                    restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(Symbol));
                    restrictions = restrictions.Merge(BindingRestrictions.GetExpressionRestriction(
                        Expression.ReferenceEqual(Expression.Field(Expression.Convert(self.Expression, typeof(Symbol)), field), Expression.Constant(manager))));
                }
                else
                {
                    // A smalltalk object, but from different runtime
                    cls = null; // Let block below handle this.
                    restrictions = null;
                }
            }
            else if (receiver is Pool)
            {
                Pool pool = (Pool)receiver;

                if (pool.Runtime == runtime)
                {
                    cls = runtime.NativeTypeClassMap.Pool;
                    PropertyInfo prop = typeof(Pool).GetProperty("Runtime", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.GetProperty,
                        null, typeof(SmalltalkRuntime), new Type[0], null);
                    if (prop == null)
                        throw new InvalidOperationException();
                    restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(Pool));
                    restrictions = restrictions.Merge(BindingRestrictions.GetExpressionRestriction(
                        Expression.ReferenceEqual(Expression.Property(Expression.Convert(self.Expression, typeof(Pool)), prop), Expression.Constant(runtime))));
                }
                else
                {
                    // A smalltalk object, but from different runtime
                    cls = null; // Let block below handle this.
                    restrictions = null;
                }
            }
            // Common FCL type mapping (bool, int, string, etc) to first-class-object.
            else if (receiver is bool)
            {
                Expression restrictionTest;
                if ((bool)receiver)
                {
                    cls = runtime.NativeTypeClassMap.True;
                    restrictionTest = Expression.IsTrue(Expression.Convert(self.Expression, typeof(bool)));
                }
                else
                {
                    cls = runtime.NativeTypeClassMap.False;
                    restrictionTest = Expression.IsFalse(Expression.Convert(self.Expression, typeof(bool)));
                }
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(bool))
                    .Merge(BindingRestrictions.GetExpressionRestriction(restrictionTest));
            }
            else if (receiver is int)
            {
                cls = runtime.NativeTypeClassMap.SmallInteger;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(int));
            }
            else if (receiver is string)
            {
                cls = runtime.NativeTypeClassMap.String;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(string));
            }
            else if (receiver is char)
            {
                cls = runtime.NativeTypeClassMap.Character;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(char));
            }
            else if (receiver is double)
            {
                cls = runtime.NativeTypeClassMap.Float;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(double));
            }
            else if (receiver is decimal)
            {
                cls = runtime.NativeTypeClassMap.SmallDecimal;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(decimal));
            }
            else if (receiver is System.Numerics.BigInteger)
            {
                cls = runtime.NativeTypeClassMap.BigInteger;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(System.Numerics.BigInteger));
            }
            else if (receiver is IronSmalltalk.Common.BigDecimal)
            {
                cls = runtime.NativeTypeClassMap.BigDecimal;
                restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(IronSmalltalk.Common.BigDecimal));
            }
            // Special case for Smalltalk classes, because we want the class behavior ...
            else if (receiver is SmalltalkClass)
            {
                cls = (SmalltalkClass)receiver;
                if (cls.Runtime == runtime)
                {
                    cls = runtime.NativeTypeClassMap.Class;
                    if (cls == null)
                        cls = runtime.NativeTypeClassMap.Object;

                    if (cls == null)
                        restrictions = null;
                    else
                        // NB: Restriction below are no good for For class behavior.
                        restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, typeof(SmalltalkClass)); 
                }
                else
                {
                    // A smalltalk object, but from different runtime
                    cls = null; // Let block below handle this.
                    restrictions = null;
                }
            }
            // Some .Net type that's neither IronSmalltalk object nor any on the known (hardcoded) types.
            else
            {
                cls = null; // Let block below handle this.
                restrictions = null;
            }

            // In case of any of the known (hardcoded) types has no registered Smalltalk class, 
            // fallback to the generic .Net type to Smalltalk class mapping.
            if (cls != null)
            {
                return cls;
            }
            else
            {
                Type type = receiver.GetType();
                cls = runtime.NativeTypeClassMap.GetSmalltalkClass(type);
                // If not explicitely mapped to a ST Class, fallback to the generic .Net mapping class.
                if (cls == null)
                    cls = runtime.NativeTypeClassMap.Native;
                if (restrictions == null)
                    restrictions = BindingRestrictions.GetTypeRestriction(self.Expression, type);
                return cls;
            }
        }

    }
}
