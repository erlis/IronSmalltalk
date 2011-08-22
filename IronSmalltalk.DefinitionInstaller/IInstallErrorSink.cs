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

namespace IronSmalltalk.Runtime.Installer
{
    /// <summary>
    /// Interface used for reporting errors during definition installation.
    /// </summary>
    public interface IInstallErrorSink
    {

        /// <summary>
        /// Report an error that occurred during installation of a definition, typically because it didn't pass validation rules.
        /// </summary>
        /// <param name="installErrorMessage">Error message because of installer validation error.</param>
        /// <param name="sourceReference">Reference to the source code that caused the error.</param>
        void AddInstallError(string installErrorMessage, ISourceReference sourceReference);
    }
}
