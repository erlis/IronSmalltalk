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

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    /// <summary>
    /// Definition description of a pool variable or pool constant.
    /// </summary>
    public abstract class PoolValueDefinition : BindableDefinition
    {
        /// <summary>
        /// Name of the Pool that owns the pool variable or pool constant.
        /// </summary>
        public SourceReference<string> PoolName { get; private set; }
        /// <summary>
        /// Name of the pool variable or pool constant.
        /// </summary>
        public SourceReference<string> VariableName { get; private set; }

        /// <summary>
        /// Creates a definition description of a pool variable or pool constant.
        /// </summary>
        /// <param name="poolName">Name of the Pool that owns the pool variable or pool constant.</param>
        /// <param name="variableName">Name of the pool variable or pool constant.</param>
        public PoolValueDefinition(SourceReference<string> poolName, SourceReference<string> variableName)
        {
            if (poolName == null)
                throw new ArgumentNullException("poolName");
            if (variableName == null)
                throw new ArgumentNullException("variableName");
            this.PoolName = poolName;
            this.VariableName = variableName;
        }

        /// <summary>
        /// Create a binding object (association) for the pool variable or pool constant in the pool that owns it.
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal abstract bool CreatePoolVariableBinding(IInstallerContext installer);

        /// <summary>
        /// Add annotations the the object being created.
        /// </summary>
        /// <param name="installer">Context which is performing the installation.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool AnnotateObject(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();

            if (!this.Annotations.Any())
                return true;

            Bindings.PoolBinding poolBinding = installer.GetPoolBinding(this.PoolName.Value);
            if ((poolBinding == null) || (poolBinding.Value == null))
                return true; // An error, but we don't see the annotations as critical.

            Bindings.PoolVariableOrConstantBinding binding;
            poolBinding.Value.TryGetValue(this.VariableName.Value, out binding);
            if (binding == null)
                return true; // An error, but we don't see the annotations as critical.

            installer.AnnotateObject(binding, this.Annotations);

            return true;
        }
    }
}
