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

namespace IronSmalltalk.Runtime.CodeGeneration.BindingScopes
{
    /// <summary>
    /// Semantic constants, such as reserved identifiers, statement delimiters etc.,
    /// mostly defined in X3J20 "3.4 Method Grammar".
    /// </summary>
    public static class SemanticConstants
    {
        public const string Self = "self";
        public const string Super = "super";
        public const string Nil = "nil";
        public const string True = "true";
        public const string False = "false";
    }
}
