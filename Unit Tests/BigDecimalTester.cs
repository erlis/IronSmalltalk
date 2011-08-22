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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronSmalltalk.Common;
using System.Numerics;

namespace IronSmalltalk.UnitTesting
{
    [TestClass]
    public class BigDecimalTester
    {

        [TestMethod]
        public void TestCtor()
        {
            BigDecimal d = new BigDecimal();
            d = new BigDecimal(123);
            d = new BigDecimal(123, 12);
            try
            {
                d = new BigDecimal(123, -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            try
            {
                d = new BigDecimal(123, BigDecimal.MaxScale + 1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void TestEquality()
        {
            BigDecimal a = new BigDecimal(14121976);
            BigDecimal b = new BigDecimal(141219760000, 4);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.AreEqual(a, b);

            a = new BigDecimal(14121976, 4);
            b = new BigDecimal(1412.1976m);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.AreEqual(a, b);

            a = new BigDecimal(-14121976, 4);
            b = new BigDecimal(-1412.1976m);
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void TestConversion()
        {
            Assert.AreEqual((int)((BigDecimal)123), 123);
            Assert.AreEqual((int)((BigDecimal)(-123)), -123);
            Assert.AreEqual((long)((BigDecimal)12345678901234567L), 12345678901234567L);
            Assert.AreEqual((long)((BigDecimal)(-12345678901234567L)), -12345678901234567L);

            Assert.AreEqual((decimal)((BigDecimal)123.45m), 123.45m);
            Assert.AreEqual((decimal)((BigDecimal)123.5m), 123.5m);
            Assert.AreEqual((decimal)((BigDecimal)123.56m), 123.56m);
            Assert.AreEqual((decimal)((BigDecimal)(-123.45m)), -123.45m);
            Assert.AreEqual((decimal)((BigDecimal)(-123.5m)), -123.5m);
            Assert.AreEqual((decimal)((BigDecimal)(-123.56m)), -123.56m);

            Assert.AreEqual((decimal)((BigDecimal)201107130548123456.4519761214m), 201107130548123456.4519761214m);
            Assert.AreEqual((decimal)((BigDecimal)201107130548123456.519761214m), 201107130548123456.519761214m);
            Assert.AreEqual((decimal)((BigDecimal)201107130548123456.5619761214m), 201107130548123456.5619761214m);
            Assert.AreEqual((decimal)((BigDecimal)(-201107130548123456.4519761214m)), -201107130548123456.4519761214m);
            Assert.AreEqual((decimal)((BigDecimal)(-201107130548123456.519761214m)), -201107130548123456.519761214m);
            Assert.AreEqual((decimal)((BigDecimal)(-201107130548123456.5619761214m)), -201107130548123456.5619761214m);


            Assert.AreEqual((double)((BigDecimal)123.45), 123.45);
            Assert.AreEqual((double)((BigDecimal)123.5), 123.5);
            Assert.AreEqual((double)((BigDecimal)123.56), 123.56);
            Assert.AreEqual((double)((BigDecimal)(-123.45)), -123.45);
            Assert.AreEqual((double)((BigDecimal)(-123.5)), -123.5);
            Assert.AreEqual((double)((BigDecimal)(-123.56)), -123.56);

            Assert.AreEqual((double)((BigDecimal)201107130548123456.4519761214), 201107130548123456.4519761214);
            Assert.AreEqual((double)((BigDecimal)201107130548123456.519761214), 201107130548123456.519761214);
            Assert.AreEqual((double)((BigDecimal)201107130548123456.5619761214), 201107130548123456.5619761214);
            Assert.AreEqual((double)((BigDecimal)(-201107130548123456.4519761214)), -201107130548123456.4519761214);
            Assert.AreEqual((double)((BigDecimal)(-201107130548123456.519761214)), -201107130548123456.519761214);
            Assert.AreEqual((double)((BigDecimal)(-201107130548123456.5619761214)), -201107130548123456.5619761214);  
        }

        [TestMethod]
        public void TestOperations()
        {
            BigDecimal a = new BigDecimal(14121976, 3);
            BigDecimal b = new BigDecimal(19761214, 4);
            // a + 0 = a
            Assert.AreEqual((a + BigDecimal.Zero), a);
            // a - a = 0
            Assert.AreEqual(a - a, BigDecimal.Zero);
            // a + b = b + a
            Assert.AreEqual(a + b, b + a);
            // a * 1 = a
            Assert.AreEqual(a * BigDecimal.One, a);
            // a * b = b * a
            Assert.AreEqual(a * b, b * a);
            // a * 0 = 0
            Assert.AreEqual(a * BigDecimal.Zero, BigDecimal.Zero);
            // -(-a) = a
            Assert.AreEqual(-(-a), a);
            // a + a = a * 2
            Assert.AreEqual(a + a, a * new BigDecimal(2));

            BigDecimal r = a - b;
            Assert.AreEqual(r, new BigDecimal(121458546, 4));
            Assert.IsTrue((a - b) > BigDecimal.Zero);
            Assert.IsTrue((b - a) < BigDecimal.Zero);
        }
    }
}
