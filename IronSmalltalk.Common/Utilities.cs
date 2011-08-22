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
    /// Utility class with common utility functions.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Validate that a given string is a legal identifier 
        /// according to the definition of X3J20 "3.5.3 Identifiers".
        /// </summary>
        /// <param name="value">String to be validated for being identifier.</param>
        /// <returns>True if the given string is an identifier, otherwise false.</returns>
        public static bool ValidateIdentifier(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return false;
            // This uses some of the logic in the lexical rules defined by the compiler,
            // but we want a lightweight function without too many dependencies,
            // so we've taken the liberty to hardcode some stuff in here.
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (!(Char.IsLetter(c) || (c == '_')))
                {
                    if ((i == 1) || (c < '0') || (c > '9'))
                        return false;
                }                
            }
            return true;
        }

    }

}
