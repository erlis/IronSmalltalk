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

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    /// <summary>
    /// Definition that contains the description of an entity that is being installed.
    /// Definitions are a type of meta-object that will result in creation of the real 
    /// object they describe.
    /// </summary>
    public abstract class DefinitionBase
    {
        /// <summary>
        /// Annotations added to the definition.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Annotations
        {
            get { return this._annotations; }
        }

        /// <summary>
        /// Annotations added to the definition.
        /// </summary>
        private readonly Dictionary<string, string> _annotations = new Dictionary<string, string>();

        /// <summary>
        /// Annotate this definition.
        /// </summary>
        /// <param name="key">Annotation key.</param>
        /// <param name="value">Annotation value.</param>
        public virtual void Annotate(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            this._annotations[key] = value;
        }

        /// <summary>
        /// Add annotations the the object being created.
        /// </summary>
        /// <param name="installer">Context which is performing the installation.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal abstract bool AnnotateObject(IInstallerContext installer);
    }
}
