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
using IronSmalltalk.Runtime.Internal;

namespace IronSmalltalk.Runtime.Behavior
{
    public class CompiledMethod : IAnnotetable
    {
        public Symbol Selector { get; private set; }

        public IntermediateMethodCode Code { get; private set; }

        public CompiledMethod(Symbol selector, IntermediateMethodCode code)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (code == null)
                throw new ArgumentNullException("code");

            this.Selector = selector;
            this.Code = code;
        }

        /// <summary>
        /// Return the name of the method when this method is called from the outside world, i.e. dynamically from .Net code.
        /// </summary>
        public string NativeName
        {
            get
            {
                string name = null;
                if (this._annotations == null)
                    return null;
                this._annotations.TryGetValue("ist.runtime.native-name", out name);
                if (String.IsNullOrWhiteSpace(name))
                    return null;
                return name;
            }
        }

        /// <summary>
        /// Return the number of arguments that this method expects.
        /// </summary>
        public int NumberOfArguments
        {
            get
            {
                int args = 0;
                bool binary = this.Selector.Value.Length != 0;
                foreach(char c in this.Selector.Value)
                {
                    if (binary)
                    {
                        if (!@"!%&*+,/<=>?@\~|-".Contains(c))
                        binary = false;
                    } else 
                    {
                        if (c == ':')
                            args++;
                    }
                }
                if (binary)
                    return 1;
                else
                    return args;
            }
        }

        #region Annotations

        /// <summary>
        /// Annotations that may be added to the binding.
        /// </summary>
        private Dictionary<string, string> _annotations;

        /// <summary>
        /// The annotation pairs associated with the annotetable object.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Annotations
        {
            get
            {
                if (this._annotations == null)
                    return AnnotationsHelper.Empty;
                return this._annotations;
            }
        }

        /// <summary>
        /// Set (or overwrite) an annotation on the annotetable object.
        /// </summary>
        /// <param name="key">Key of the annotation.</param>
        /// <param name="value">Value or null to remove the annotation.</param>
        public void Annotate(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException();
            if (value == null)
            {
                if (this._annotations == null)
                    return;
                this._annotations.Remove(key);
            }
            else
            {
                if (this._annotations == null)
                    this._annotations = new Dictionary<string, string>();
                this._annotations[key] = value;
            }
        }

        #endregion
    }
}
