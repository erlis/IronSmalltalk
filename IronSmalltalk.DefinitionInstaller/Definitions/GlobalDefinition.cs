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
using System.Linq;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.Bindings;

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    /// <summary>
    /// Definition description of a global variable or a global constant.
    /// </summary>
    public abstract class GlobalDefinition : GlobalBase
    {
        /// <summary>
        /// Name of the global.
        /// </summary>
        public SourceReference<string> GlobalName {
            get { return this.Name; }
        }

        /// <summary>
        /// Creates a new definition description of a global variable or a global constant.
        /// </summary>
        /// <param name="globalName">Name of the global.</param>
        public GlobalDefinition(SourceReference<string> globalName)
            : base(globalName)
        {
        }

        /// <summary>
        /// Create the binding object (association) for the global variable or global constant in the global name scope (system dictionary).
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool CreateGlobalBinding(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 1. Check if the name is not complete garbage.
            if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(this.Name.Value))
                return installer.ReportError(this.Name, InstallerErrors.GlobalInvalidName);
            // 2. Check if there is already a global with that name in the inner name scope
            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            IDiscreteGlobalBinding existingBinding = installer.GetLocalGlobalBinding(name);
            if (existingBinding != null)
                return installer.ReportError(this.Name, InstallerErrors.GlobalNameNotUnique);
            // 3. No global defined in the inner scope, but chech the scopes (inner or outer) for protected names like Object etc.
            if (installer.IsProtectedName(name))
                return installer.ReportError(this.Name, InstallerErrors.GlobalNameProtected);
            if (IronSmalltalk.Common.GlobalConstants.ReservedIdentifiers.Contains(this.Name.Value))
                return installer.ReportError(this.Name, InstallerErrors.GlobalReservedName);

            // 4. OK ... create a binding - so far pointing to null.
            return this.InternalCreateBinding(installer, name);
        }

        /// <summary>
        /// Create the binding object (association) for the global variable or global constant in the global name scope (system dictionary).
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <param name="name">Name of the global.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected abstract bool InternalCreateBinding(IInstallerContext installer, Symbol name);

        /// <summary>
        /// Create the global object (and sets the value of the binding).
        /// </summary>
        /// <param name="installer">Context within which the global is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool CreateGlobalObject(IInstallerContext installer)
        {
            return true; // Do nothing - global variables/constants have initial value of null.
        }

        /// <summary>
        /// Validate that the definition of the global does not break any rules set by the Smalltalk standard.
        /// </summary>
        /// <param name="installer">Context within which the validation is to be performed.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool ValidateObject(IInstallerContext installer)
        {
            return true; // We did validate whatever is validatable when the global binding was created.
        }
    }



}
