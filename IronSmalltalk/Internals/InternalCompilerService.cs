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
using IronSmalltalk.Runtime;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Internals
{
    /// <summary>
    /// Special compiler service for handling the IronSmalltalk class library.
    /// This is responsible for installing definitions into the extension-scope,
    /// as oposite to normal user code that ends up in the global scope.
    /// </summary>
    public class InternalCompilerService : CompilerService
    {
        public InternalCompilerService(SmalltalkRuntime runtime)
            : base(runtime)
        {
        }

        protected override InterchangeInstallerContext CreateInstallerContext()
        {
            return new InternalInstallerContext(this.Runtime);
        }
    }
}
