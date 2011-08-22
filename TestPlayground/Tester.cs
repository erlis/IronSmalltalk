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

namespace TestPlayground
{
    public class Tester
    {
        public static readonly Tester Current = new Tester();

        public int Field;
        public int Property { get; set; }

        public static int StaticField;
        public static int StaticProperty { get; set; }

        public static readonly string FullName = typeof(Tester).FullName;
        public static readonly string TypeName = typeof(Tester).AssemblyQualifiedName;

        private string[] Vals = new string[10];
        public string this[int index]
        {
            get
            {
                return this.Vals[index];
            }
            set
            {
                this.Vals[index] = value;
            }
        }

        public override string ToString()
        {
            return "The Tester!";
        }
    }
}
