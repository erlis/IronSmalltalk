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
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.Bindings;

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    /// <summary>
    /// Definition description of a global variable.
    /// </summary>
    public class GlobalVariableDefinition : GlobalDefinition
    {
        /// <summary>
        /// Creates a new definition description of a global variable.
        /// </summary>
        /// <param name="globalName">Name of the global.</param>
        public GlobalVariableDefinition(SourceReference<string> globalName)
            : base(globalName)
        {
        }

        /// <summary>
        /// Returns a System.String that represents the global definition object.
        /// </summary>
        /// <returns>A System.String that represents the global definition object.</returns>
        public override string ToString()
        {
            return String.Format("Global variable: '{0}'", this.Name.Value);
        }

        /// <summary>
        /// Create the binding object (association) for the global variable in the global name scope (system dictionary).
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <param name="name">Name of the global.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected override bool InternalCreateBinding(IInstallerContext installer, Symbol name)
        {
            // Everything else has been validate, so create a binding - so far pointing to null.
            installer.AddGlobalVariableBinding(new GlobalVariableBinding(name));
            return true;
        }
    }
}
