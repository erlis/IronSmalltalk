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
    /// Definition description of a Shared Pool.
    /// </summary>
    public class PoolDefinition : GlobalBase
    {
        /// <summary>
        /// Name of the Pool.
        /// </summary>
        public SourceReference<string> PoolName
        {
            get { return this.Name; }
        }

        /// <summary>
        /// Creates a new definition description of a Shared Pool.
        /// </summary>
        /// <param name="poolName">Name of the Pool.</param>
        public PoolDefinition(SourceReference<string> poolName)
            : base(poolName)
        {
        }

        /// <summary>
        /// Returns a System.String that represents the shared pool definition object.
        /// </summary>
        /// <returns>A System.String that represents the shared pool definition object.</returns>
        public override string ToString()
        {
            return String.Format("Pool named: '{0}'", this.Name.Value);
        }

        /// <summary>
        /// Create the binding object (association) for the pool in the global name scope (system dictionary).
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool CreateGlobalBinding(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 1. Check if the name is not complete garbage.
            if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(this.Name.Value))
                return installer.ReportError(this.Name, InstallerErrors.PoolInvalidName);
            // 2. Check if there is already a global with that name in the inner name scope
            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            IDiscreteGlobalBinding existingBinding = installer.GetLocalGlobalBinding(name);
            if (existingBinding != null)
                return installer.ReportError(this.Name, InstallerErrors.PoolNameNotUnique);
            // 3. No global defined in the inner scope, but chech the scopes (inner or outer) for protected names like Object etc.
            if (installer.IsProtectedName(name))
                return installer.ReportError(this.Name, InstallerErrors.PoolNameProtected);
            if (IronSmalltalk.Common.GlobalConstants.ReservedIdentifiers.Contains(this.Name.Value))
                return installer.ReportError(this.Name, InstallerErrors.PoolReservedName);

            // 4. OK ... create a binding - so far pointing to null.
            installer.AddPoolBinding(new PoolBinding(name));
            return true;
        }

        /// <summary>
        /// Create the pool object (and sets the value of the binding).
        /// </summary>
        /// <param name="installer">Context within which the pool is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool CreateGlobalObject(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 1. Get the binding to the global 
            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            PoolBinding binding = installer.GetLocalPoolBinding(name);
            // 2. Check consistency that we didn't mess up in the implementation.
            if (binding == null)
                throw new InvalidOperationException("Should have found a binding, because CreateGlobalBinding() created it!");
            if (binding.Value != null)
                throw new InvalidOperationException("Should be an empty binding, because CreateGlobalBinding() complained if one already existed!");
            // 3. Create the pool object.
            binding.SetValue(new Pool(installer.Runtime, name));
            return true;
        }

        /// <summary>
        /// Validate that the definition of the pool object does not break any rules set by the Smalltalk standard.
        /// </summary>
        /// <param name="installer">Context within which the validation is to be performed.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool ValidateObject(IInstallerContext installer)
        {
            return true; // We did validate whatever is validatable when the pool binding was created.
        }
    }
}
