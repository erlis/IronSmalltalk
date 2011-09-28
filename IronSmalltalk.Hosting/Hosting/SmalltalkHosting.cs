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
using Microsoft.Scripting.Hosting;

namespace IronSmalltalk.Runtime.Hosting
{
    public static class SmalltalkHosting
    {
        public static LanguageSetup CreateLanguageSetup()
        {
            return new LanguageSetup(
                typeof(SmalltalkLanguageContext).AssemblyQualifiedName,
                "IronSmalltalk",
                new string[] { "IronSmalltalk", "Iron Smalltalk", "ist" },
                new string[] { "ist" });
        }
    }
}