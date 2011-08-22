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

namespace IronSmalltalk.Runtime.Execution.Internals
{
    // TO-DO ... rename to better name and refactor
    public class RuntimeCodeGenerationErrors
    {
        public const string UndefinedBinding = "Undefined.";
        public const string PoolVariableNotUnique = "Duplicate pool variable or pool constant name found.";
        public const string DoesNotUnderstandMissing = "Could not find the #_doesNotUnderstand:arguments: method.";
    }
}
