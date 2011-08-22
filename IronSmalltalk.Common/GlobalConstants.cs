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

namespace IronSmalltalk.Common
{
    /// <summary>
    /// Class containing global constants.
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary>
        /// Returns the names of the reserved identifiers (nil, true, false, self and super).
        /// </summary>
        public static readonly string[] ReservedIdentifiers = new string[] { "nil", "true", "false", "self", "super" };
    }
}
