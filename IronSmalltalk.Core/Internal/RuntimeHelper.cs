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
using System.Numerics;

namespace IronSmalltalk.Runtime.Internal
{
    /// <summary>
    /// Unility class with methods called by Smalltalk code.
    /// </summary>
    public static class RuntimeHelper
    {
        /// <summary>
        /// Evaluate a 0-argument block.
        /// </summary>
        /// <param name="block">Block to evaluate.</param>
        /// <returns>Block result.</returns>
        public static object BlockValue(Func<object> block)
        {
            return block();
        }

        /// <summary>
        /// Evaluate a 1-argument block.
        /// </summary>
        /// <param name="block">Block to evaluate.</param>
        /// <param name="arg">Block argument.</param>
        /// <returns>Block result.</returns>
        public static object BlockValue(Func<object, object> block, object arg)
        {
            return block(arg);
        }

        /// <summary>
        /// Evaluate a 2-argument block.
        /// </summary>
        /// <param name="block">Block to evaluate.</param>
        /// <param name="arg1">Block argument 1.</param>
        /// <param name="arg2">Block argument 2.</param>
        /// <returns>Block result.</returns>
        public static object BlockValue(Func<object, object, object> block, object arg1, object arg2)
        {
            return block(arg1, arg2);
        }

        /// <summary>
        /// Evaluate a block.
        /// </summary>
        /// <param name="block">Block to evaluate.</param>
        /// <param name="args">Block arguments.</param>
        /// <returns>Block result.</returns>
        public static object BlockValue(Delegate block, params object[] args)
        {
            return block.DynamicInvoke(args);
        }

        /// <summary>
        /// Calculates the Greatest Common Divisor.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>
        /// The largest non-negative integer that divides both operands with no remainder. 
        /// Answer 0 if both operands are zero.
        /// </returns>
        public static int GCD(int a, int b)
        {
            if ((a == 0) && (b == 0))
                return 0;

            if (a < 0)
                a = -a;
            if (b < 0)
                b = -b;

            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }
            return a;
        }

        /// <summary>
        /// Calculates the Greatest Common Divisor.
        /// </summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <returns>
        /// The largest non-negative integer that divides both operands with no remainder. 
        /// Answer 0 if both operands are zero.
        /// </returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            return BigInteger.GreatestCommonDivisor(a, b);
            //if ((a == 0) && (b == 0))
            //    return BigInteger.Zero;

            //if (a < 0)
            //    a = -a;
            //if (b < 0)
            //    b = -b;

            //while (b != 0)
            //{
            //    BigInteger r = BigInteger.Remainder(a, b);
            //    a = b;
            //    b = r;
            //}
            //return a;
        }

        /// <summary>
        /// Helper method for throwing exceptions.
        /// </summary>
        /// <param name="ex">Exception to be thrown.</param>
        /// <returns>Does not return.</returns>
        public static object Throw(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException();
            throw ex;
        }
    }
}
