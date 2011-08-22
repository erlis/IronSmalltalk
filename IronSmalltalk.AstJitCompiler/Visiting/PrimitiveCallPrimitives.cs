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
using System.Linq.Expressions;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.AstJitCompiler.Runtime;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Execution.CallSiteBinders;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    partial class PrimitiveCallVisitor
    {
        private Expression GenerateBuildinPrimitive(string primitive, IList<string> parameters)
        {
            // ... huge list (switch) with primitive call (their names and code generation)

            //  **** Very common general operations ****
            if (primitive == "equals")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Equal(arg1, arg2));
            if (primitive == "referenceequals")
                return this.BinaryOperation(parameters, (arg1, arg2) => this.ReferenceEqual(arg1, arg2));
            if (primitive == "class")
                return this.ObjectClass(parameters);

            // **** Numeric Operations ****
            // ISO/IEC 10967 Integer Operations
            //  NB: No integer operation may wrap, i.e. they must overflow if needed!
            if (primitive == "eqI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Equal(arg1, arg2));
            if (primitive == "lssI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThan(arg1, arg2));
            if (primitive == "leqI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThanOrEqual(arg1, arg2));
            if (primitive == "gtrI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThan(arg1, arg2));
            if (primitive == "geqI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThanOrEqual(arg1, arg2));
            if (primitive == "addI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.AddChecked(arg1, arg2));
            if (primitive == "subI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.SubtractChecked(arg1, arg2));
            if (primitive == "mulI")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.MultiplyChecked(arg1, arg2));
            if (primitive == "divI_t")
                return this.BinaryOperation(parameters, (arg1, arg2, type1, type2) => this.DivideIntT(arg1, arg2, type1, type2));
            if (primitive == "divI_f")
                return this.BinaryOperation(parameters, (arg1, arg2, type1, type2) => this.DivideIntF(arg1, arg2, type1, type2));
            if (primitive == "remI_t")
                return this.BinaryOperation(parameters, (arg1, arg2, type1, type2) => this.ReminderIntT(arg1, arg2, type1, type2));
            if (primitive == "remI_f")
                return this.BinaryOperation(parameters, (arg1, arg2, type1, type2) => this.ReminderIntF(arg1, arg2, type1, type2));
            if (primitive == "negI")
                return this.UnaryOperation(parameters, arg => Expression.Negate(arg));
            // ISO/IEC 10967 Float Operations
            if (primitive == "eqF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Equal(arg1, arg2));
            if (primitive == "lssF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThan(arg1, arg2));
            if (primitive == "leqF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThanOrEqual(arg1, arg2));
            if (primitive == "gtrF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThan(arg1, arg2));
            if (primitive == "geqF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThanOrEqual(arg1, arg2));
            if (primitive == "addF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.AddChecked(arg1, arg2));
            if (primitive == "subF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.SubtractChecked(arg1, arg2));
            if (primitive == "mulF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.MultiplyChecked(arg1, arg2));
            if (primitive == "divF")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Divide(arg1, arg2));
            if (primitive == "negF")
                return this.UnaryOperation(parameters, arg => Expression.Negate(arg));
            // ISO/IEC 10967 Generic Operations ... for numbers that are not Integers or Floats.
            if (primitive == "eqG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Equal(arg1, arg2));
            if (primitive == "lssG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThan(arg1, arg2));
            if (primitive == "leqG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThanOrEqual(arg1, arg2));
            if (primitive == "gtrG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThan(arg1, arg2));
            if (primitive == "geqG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThanOrEqual(arg1, arg2));
            if (primitive == "addG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.AddChecked(arg1, arg2));
            if (primitive == "subG")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.SubtractChecked(arg1, arg2));
            if (primitive == "negG")
                return this.UnaryOperation(parameters, arg => Expression.Negate(arg));
            // Decimal operations .... not part of ISO/IEC 10967, but currently we assume same semantics
            if (primitive == "mulD")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.MultiplyChecked(arg1, arg2));
            if (primitive == "divD")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Divide(arg1, arg2));
            // Currently not used:
            //if (primitive == "addunchecked")
            //    return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Add(arg1, arg2));
            //if (primitive == "subtractunchecked")
            //    return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Subtract(arg1, arg2));
            //if (primitive == "multiplyunchecked")
            //    return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Multiply(arg1, arg2));
            //if (primitive == "leftshift")
            //    return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LeftShift(arg1, arg2));
            //if (primitive == "rightshift")
            //    return this.BinaryOperation(parameters, (arg1, arg2) => Expression.RightShift(arg1, arg2));
            //if (primitive == "power")
            //    return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Power(arg1, arg2));

            // **** Other Generic Operations ****
            //  NB: Don't use those for numeric types. It's not a technical error, but difficult to maintain if we find a bug or need to change something.
            if (primitive == "greaterthan")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThan(arg1, arg2));
            if (primitive == "greaterthanequal")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.GreaterThanOrEqual(arg1, arg2));
            if (primitive == "lessthan")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThan(arg1, arg2));
            if (primitive == "lessthanequal")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.LessThanOrEqual(arg1, arg2));
            if (primitive == "add")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.AddChecked(arg1, arg2));
            if (primitive == "subtract")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.SubtractChecked(arg1, arg2));
            if (primitive == "negate")
                return this.UnaryOperation(parameters, arg => Expression.Negate(arg));
            if (primitive == "cast")
                return this.UnaryOperation(parameters, arg => arg);
            if (primitive == "shift")
                return this.Shift(parameters);
            if (primitive == "bitwiseAnd")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.And(arg1, arg2));
            if (primitive == "bitwiseOr")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.Or(arg1, arg2));
            if (primitive == "bitwiseXor")
                return this.BinaryOperation(parameters, (arg1, arg2) => Expression.ExclusiveOr(arg1, arg2));
            if (primitive == "convert")
                return this.Convert(parameters, true);
            if (primitive == "uncheckedconvert")
                return this.Convert(parameters, false);
            return null;
        }

        private Expression UnaryOperation(IList<string> parameters, Func<Expression, Expression> func)
        {
            return this.UnaryOperation(parameters, (arg, type) => func(arg));
        }

        private Expression UnaryOperation(IList<string> parameters, Func<Expression, Type, Expression> func)
        {
            if (parameters == null)
                return null;
            if (parameters.Count != 1)
                return null;
            Type type = (parameters[0] == "self") ? null : NativeTypeClassMap.GetType(parameters[0]);
            Type[] types = new Type[] { type };
            List<Expression> args = this.GetArguments(types);
            return func(args[0], type);
        }

        private Expression BinaryOperation(IList<string> parameters, Func<Expression, Expression, Expression> func)
        {
            return this.BinaryOperation(parameters, (arg1, arg2, type1, type2) => func(arg1, arg2));
        }

        private Expression BinaryOperation(IList<string> parameters, Func<Expression, Expression, Type, Type, Expression> func)
        {
            if (parameters == null)
                return null;
            if (parameters.Count != 2)
                return null;
            Type type0 = (parameters[0] == "self") ? null : NativeTypeClassMap.GetType(parameters[0]);
            Type type1 = (parameters[1] == "self") ? null : NativeTypeClassMap.GetType(parameters[1]);
            Type[] types = new Type[] { type0, type1 };
            List<Expression> args = this.GetArguments(types);
            return func(args[0], args[1], type0, type1);
        }

        private Expression ReferenceEqual(Expression a, Expression b)
        {
            NameBinding trueBinding = this.GetBinding(SemanticConstants.True);
            NameBinding falseBinding = this.GetBinding(SemanticConstants.False);
            return PrimitiveCallVisitor.EncodeReferenceEquals(a, b, trueBinding.GenerateReadExpression(this), falseBinding.GenerateReadExpression(this));
        }

        /// <summary>
        /// Performs an ISO/IEC 10967 integer operation divI-t, i.e. division with truncation towards zero.
        /// </summary>
        /// <remarks>
        /// See ISO/IEC 10967:
        ///     4.1 Symbols (... about truncation)
        ///     5.1.3 Axioms
        /// Defined as:
        ///     divI-t(x,y): tr(x/y)
        ///     tr(x):      [x]     if (x >= 0)
        ///                 -[-x]   if (x < 0)
        /// Example:
        ///     divI-t( -3, 2) => -1
        ///     divI-t( -2, 2) => -1
        ///     divI-t( -1, 2) => 0
        ///     divI-t(  0, 2) => 0
        ///     divI-t(  1, 2) => 0
        ///     divI-t(  2, 2) => 1
        ///     divI-t(  3, 2) => 1
        /// </remarks>
        private Expression DivideIntT(Expression arg1, Expression arg2, Type type1, Type type2)
        {
            return Expression.Divide(arg1, arg2);
        }

        /// <summary>
        /// Performs an ISO/IEC 10967 integer operation divI-f, i.e. division with flooring truncation towards negative infinity.
        /// </summary>
        /// <remarks>
        /// See ISO/IEC 10967:
        ///     4.1 Symbols (... about truncation)
        ///     5.1.3 Axioms
        /// Defined as:
        ///     divI-f(x,y): [x/y]
        ///     [x]:    Largest integer where:  x-1 < [x] <= x
        /// Example:
        ///     divI-f( -3, 2) => -2
        ///     divI-f( -2, 2) => -1
        ///     divI-f( -1, 2) => -1
        ///     divI-f(  0, 2) => 0
        ///     divI-f(  1, 2) => 0
        ///     divI-f(  2, 2) => 1
        ///     divI-f(  3, 2) => 1
        /// </remarks>
        private Expression DivideIntF(Expression arg1, Expression arg2, Type type1, Type type2)
        {
            if (PrimitiveCallVisitor.IsUnsignedType(type1) && PrimitiveCallVisitor.IsUnsignedType(type2))
                return Expression.Divide(arg1, arg2);

            // C# pseudocode:
            //  if ((arg1 >= 0) ^ (arg2 >= 0))
            //      if ((arg1 % arg2) == 0)
            //          return arg1 / arg2;
            //      else
            //          return (arg1 / arg2) - 1;
            //  else
            //      return arg1 / arg2;
            Expression zero = Expression.Constant(PrimitiveCallVisitor.GetZero(type1), type1);
            Expression one = Expression.Constant(PrimitiveCallVisitor.GetOne(type1), type1);
            Expression division = Expression.Divide(arg1, arg2);
            Expression modulo = Expression.Modulo(arg1, arg2);
            return Expression.Condition(
                Expression.ExclusiveOr(
                    Expression.GreaterThanOrEqual(arg1, zero),
                    Expression.GreaterThanOrEqual(arg2, zero)),
                Expression.Condition(
                    Expression.Equal(modulo, zero),
                    division,
                    Expression.Subtract(division, one)),
                division);
        }

        /// <summary>
        /// Performs an ISO/IEC 10967 float operation divF, i.e. division with truncation towards zero.
        /// </summary>
        /// <remarks>
        /// See ISO/IEC 10967:
        ///     4.1 Symbols (... about truncation)
        ///     5.2.2 Operations
        /// </remarks>
        private Expression DivideFloat(Expression arg1, Expression arg2, Type type1, Type type2)
        {
            return Expression.Divide(arg1, arg2);
        }

        /// <summary>
        /// Performs an ISO/IEC 10967 operation rem-t, i.e. reminder with truncation towards zero.
        /// </summary>
        /// <remarks>
        /// See ISO/IEC 10967:
        ///     4.1 Symbols (... about truncation)
        ///     5.1.3 Axioms
        /// Defined as:
        ///     remI-t(x,y):     x - ( div-t(x, y) * y )
        /// </remarks>
        private Expression ReminderIntT(Expression arg1, Expression arg2, Type type1, Type type2)
        {
            return Expression.Modulo(arg1, arg2);
        }


        /// <summary>
        /// Performs an ISO/IEC 10967 operation rem-f, i.e. reminder with flooring truncation towards negative infinity.
        /// </summary>
        /// <remarks>
        /// See ISO/IEC 10967:
        ///     4.1 Symbols (... about truncation)
        ///     5.1.3 Axioms
        /// Defined as:
        ///     divI-f(x,y):     x - ( div-f(x, y) * y )
        /// </remarks>
        private Expression ReminderIntF(Expression arg1, Expression arg2, Type type1, Type type2)
        {
            if (PrimitiveCallVisitor.IsUnsignedType(type1) && PrimitiveCallVisitor.IsUnsignedType(type2))
                return Expression.Modulo(arg1, arg2);

            Expression zero1 = Expression.Constant(PrimitiveCallVisitor.GetZero(type1), type1);
            Expression zero2 = Expression.Constant(PrimitiveCallVisitor.GetZero(type2), type2);
            
            // C# pseudocode:
            //  if (arg1 >= 0)
            //      if (arg2 => 0)
            //          return arg1 % arg2;
            //      else 
            //          return -(arg1 % -arg2);
            //  else
            //      if (arg2 => 0)
            //          return -arg1 % arg2;
            //      else 
            //          return arg1 % arg2;  // same as: -(-arg1 % -arg2)
            Expression modulo = Expression.Modulo(                
                Expression.Condition(
                    Expression.GreaterThanOrEqual(arg1, zero1),
                    arg1, 
                    Expression.Negate(arg1)),
                arg2);
            return Expression.Condition(
                Expression.GreaterThanOrEqual(arg2, zero2),
                    modulo,
                    Expression.Negate(modulo));
        }

        private static bool IsUnsignedType(Type type)
        {
            if (type == typeof(ulong))
                return true;
            else if (type == typeof(uint))
                return true;
            else if (type == typeof(ushort))
                return true;
            else if (type == typeof(byte))
                return true;
            return false;
        }

        private static object GetZero(Type type)
        {
            return PrimitiveCallVisitor.GetValue(type, 0);
        }

        private static object GetOne(Type type)
        {
            return PrimitiveCallVisitor.GetValue(type, 1);
        }

        private static object GetValue(Type type, object value)
        {
            Expression expression = Expression.Convert(Expression.Constant(value, value.GetType()), type);
            var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)));
            var method = lambda.Compile();
            return method();
        }

        internal static Expression EncodeReferenceEquals(Expression a, Expression b, Expression trueValue, Expression falseValue)
        {
            return Expression.Condition(
                Expression.AndAlso(Expression.TypeIs(a, typeof(bool)), Expression.TypeIs(b, typeof(bool))),
                Expression.Condition(
                    Expression.Equal(a, b),
                    trueValue,
                    falseValue),
                Expression.Condition(
                    Expression.ReferenceEqual(a, b),
                    trueValue,
                    falseValue));
        }

        private Expression Convert(IList<string> parameters, bool convertChecked)
        {
            if (parameters == null)
                return null;
            if (parameters.Count != 1)
                return null;
            if (parameters.Count == 1)
            {
                Type type = NativeTypeClassMap.GetType(parameters[0]);
                Expression expr = this.GetArguments(new Type[] { type })[0];
                return expr;
            }


            if (parameters.Count != 2)
                return null;
            Type type1 = NativeTypeClassMap.GetType(parameters[0]);
            Type type2 = NativeTypeClassMap.GetType(parameters[1]);
            Type[] types = new Type[] { type1 };
            List<Expression> args = this.GetArguments(types);
            if (convertChecked)
                return Expression.ConvertChecked(args[0], type2);
            else
                return Expression.Convert(args[0], type2);
        }

        private Expression ObjectClass(IList<string> parameters)
        {
            if (parameters == null)
                return null;
            if (parameters.Count != 0)
                return null;

            NameBinding binding = this.MethodVisitor.GetBinding(SemanticConstants.Self);
            ObjectClassCallSiteBinder binder = this.RootVisitor.BinderCache.CachedObjectClassCallSiteBinder;
            if (binder == null)
            {
                binder = new ObjectClassCallSiteBinder(this.RootVisitor.Runtime);
                this.RootVisitor.BinderCache.CachedObjectClassCallSiteBinder = binder;
            }
            return Expression.Dynamic(binder, typeof(Object), binding.GenerateReadExpression(this));
        }

        private Expression Shift(IList<string> parameters)
        {
            if (parameters == null)
                return null;
            if (parameters.Count != 2)
                return null;
            // We don't support if the shift parameter is other than int
            if (NativeTypeClassMap.GetType(parameters[1]) != typeof(int))
                throw new PrimitiveInvalidTypeException(String.Format(CodeGenerationErrors.WrongShiftTypeName, parameters[1]));
            Type type = NativeTypeClassMap.GetType(parameters[0]);

            int bits;
            object zeroValue;
            if (type == typeof(System.Numerics.BigInteger))
                { bits = 0; zeroValue = System.Numerics.BigInteger.Zero; }
            else if (type == typeof(long))
                { bits = -63; zeroValue = (long)0; }
            else if (type == typeof(int))
                { bits = -31; zeroValue = (int)0; }
            else if (type == typeof(short))
                { bits = -15; zeroValue = (short)0; }
            else if (type == typeof(sbyte))
                { bits = -7; zeroValue = (sbyte)0; }
            else if (type == typeof(ulong))
                { bits = 64; zeroValue = (ulong)0; }
            else if (type == typeof(uint))
                { bits = 32; zeroValue = (uint)0; }
            else if (type == typeof(ushort))
                { bits = 16; zeroValue = (ushort)0; }
            else if (type == typeof(byte))
                { bits = 8; zeroValue = (byte)0; }
            else
                throw new PrimitiveInvalidTypeException(String.Format(CodeGenerationErrors.WrongShiftTypeName, parameters[0]));
            

            List<Expression> args = this.GetArguments(new Type[] { type, typeof(int) });
            Expression value = args[0];
            Expression shift = args[1];
            Expression leftShift;
            if (bits == 0)
            {
                // BigInteger never overflows .... so just shift away
                leftShift = Expression.LeftShift(value, shift);
            }
            else
            {
                // Some integer with limited precision ... must handle delicately with self-made overflow checks. 
                // NB: Logic here executes only if shift is greather than zero.
                // C# overflow check semantics ... example for int (32 bit):
                //  if (value < 0)
                //      throw constant OverflowException();         // Handle negatives in Smalltalk (X3J20 says "undefined" for this)
                //  if (shift >= 31)
                //      if (value == 0)                             // Zero is save to shift forever
                //          return 0;
                //      else
                //          throw constant OverflowException();     // Will definitely overflow
                //  if ((value >> (31 - shift)) == 0)               // Do test if there are significant bits that will overflow
                //      return value << shift;
                //  else
                //      throw constant OverflowException();         // Throw and let Smalltalk code to the work

                bool signed = bits < 0;
                bits = Math.Abs(bits);
                Expression zero = Expression.Constant(zeroValue, type);
                Expression overflow = Expression.Constant(new OverflowException(), typeof(OverflowException));
                Expression bitCount = Expression.Constant(bits, typeof(int));

                leftShift = Expression.Condition(
                    Expression.GreaterThanOrEqual(shift, bitCount),
                        Expression.Condition(
                            Expression.Equal(value, zero),
                                zero,
                                Expression.Throw(overflow, type)),
                        Expression.Condition(
                            Expression.Equal(Expression.RightShift(value, Expression.Subtract(bitCount, shift)), zero),
                                Expression.LeftShift(value, shift),
                                Expression.Throw(overflow, type)));
                if (signed)
                    leftShift = Expression.Condition(
                        Expression.LessThan(value, zero),
                            Expression.Throw(overflow, type),
                            leftShift);
            };

            // C# semantics:
            //  if (shift > 0)
            //      return checked(value << shift);     ... this can overflow, so see above for overflow checking
            //  else
            //      return value >> -shift;              ... this can never overflow ... so just do it
            return Expression.Condition(
                Expression.GreaterThan(shift, Expression.Constant(0, typeof(int))),
                leftShift,
                Expression.RightShift(value, Expression.Negate(shift)));
        }
    }
}
