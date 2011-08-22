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
    /// Base class for definition describing objects that live in the Global / Smalltalk name scope.
    /// </summary>
    public abstract class GlobalBase : BindableDefinition
    {
        public SourceReference<string> Name { get; private set; }

        /// <summary>
        /// Create a new definition (description) of a global object.
        /// </summary>
        /// <param name="name">Name of the global.</param>
        protected GlobalBase(SourceReference<string> name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            this.Name = name;
        }

        /// <summary>
        /// Create the binding object (association) for the global.
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal abstract bool CreateGlobalBinding(IInstallerContext installer);

        /// <summary>
        /// Create the global object (and sets the value of the binding).
        /// </summary>
        /// <param name="installer">Context within which the global is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal abstract bool CreateGlobalObject(IInstallerContext installer);

        /// <summary>
        /// Validate that the definition of the global object does not break any rules set by the Smalltalk standard.
        /// </summary>
        /// <param name="installer">Context within which the validation is to be performed.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal abstract bool ValidateObject(IInstallerContext installer);

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

            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            Bindings.IDiscreteGlobalBinding binding = installer.GetLocalGlobalBinding(name);
            if (binding == null)
                return true; // An error, but we don't see the annotations as critical.

            installer.AnnotateObject(binding, this.Annotations);

            return true;
        }
    }
}
