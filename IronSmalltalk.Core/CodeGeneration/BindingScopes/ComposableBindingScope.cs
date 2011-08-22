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
using IronSmalltalk.Runtime.CodeGeneration.Bindings;

namespace IronSmalltalk.Runtime.CodeGeneration.BindingScopes
{
    public abstract class ComposableBindingScope : BindingScope
    {
        public BindingScope OuterScope { get; set; }

        public ComposableBindingScope()
            : this(null, null)
        {
        }

        public ComposableBindingScope(BindingScope outerScope)
            : this(outerScope, null)
        {
        }

        public ComposableBindingScope(BindingScope outerScope, IEnumerable<NameBinding> bindings)
            : base(bindings)
        {
            this.OuterScope = outerScope;
        }

        public override NameBinding GetBinding(string name)
        {
            // Try to see if we've already resolved this binding
            NameBinding result = base.GetBinding(name);
            if (result != null)
                return result;

            // Try to resolve it ...
            result = this.ResolveBinding(name);
            if (result != null)
            {
                this.DefineBinding(result); // Cache it for next usage
                return result;
            }

            // No luck ... try outer 
            if (this.OuterScope != null)
                return this.OuterScope.GetBinding(name);
            else
                return null;
        }

        protected virtual NameBinding ResolveBinding(string name)
        {
            return null; // Default, do nothig ... subclasses can implement this
        }
    }
}
