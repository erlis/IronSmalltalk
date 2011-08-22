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

namespace IronSmalltalk.Compiler.Interchange
{
    /// <summary>
    /// Error messages the interchange format reader may report if it encounters illegal source code.
    /// </summary>
    public class InterchangeFormatErrors
    {
        public const string MissingTerminalSeparator = "Missing terminal '!' separator.";
        public const string MissingInterchangeVersionIdentifier = "Missing version specification.";
        public const string InvalidVersionId = "Invalid version Id. Currently only '1.0' is supported.";
        public const string ExpectedInterchangeUnit = "Expected interchange unit / interchange element";
    }
}
