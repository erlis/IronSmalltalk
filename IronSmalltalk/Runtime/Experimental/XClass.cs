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
using IronSmalltalk.Runtime.Installer.Definitions;

namespace IronSmalltalk.Runtime.Experimental
{
    public class XClass
    {
        public readonly string Name;

        private readonly XBinding _superclass;
        public XClass Superclass
        {
            get
            {
                if (this._superclass == null)
                    return null;
                return (XClass)this._superclass.Value;
            }
        }

        public readonly InstanceStateEnum InstanceState;

        public readonly ClassVariablesPool ClassVariables;

        public readonly ClassInstanceVariablesPool ClassInstanceVariables;

        public readonly ImportedPoolsPool ImportedPools;

        public readonly string[] InstanceVariableNames;

        public readonly Dictionary<string, InstanceMethodDefinition> InstanceMethods = new Dictionary<string, InstanceMethodDefinition>();

        public readonly Dictionary<string, ClassMethodDefinition> ClassMethods = new Dictionary<string, ClassMethodDefinition>();

        public XClass(ClassDefinition definition, XSystemDictionary smalltalk)
        {
            if ((definition == null) || (smalltalk == null))
                throw new ArgumentNullException();

            this.Name = definition.Name.Value;
            if (!String.IsNullOrWhiteSpace(definition.SuperclassName.Value) && (definition.SuperclassName.Value != "nil"))
                this._superclass = smalltalk.GetOrCreateBinding(definition.SuperclassName.Value);
            this.InstanceState = definition.InstanceState.Value;

            this.ClassVariables = new ClassVariablesPool(definition.ClassVariableNames);
            this.ClassInstanceVariables = new ClassInstanceVariablesPool(this, definition.ClassInstanceVariableNames);
            this.ImportedPools = new ImportedPoolsPool(smalltalk, definition.ImportedPoolNames);
            this.InstanceVariableNames = definition.InstanceVariableNames.Select(sr => sr.Value).ToArray();
        }

        public class ClassItemPool : XBindingScope
        {
        }
        public class ClassVariablesPool : XBindingScope
        {
            public ClassVariablesPool(IEnumerable<Installer.SourceReference<string>> items)
            {
                if (items == null)
                    throw new ArgumentNullException();
                foreach (var item in items)
                {
                    XBinding binding = this.CreateBinding(item.Value);
                    binding.MakeVariable();
                    binding.Value = null; // Bind it
                }
            }
        }
        public class ClassInstanceVariablesPool : XBindingScope
        {
            private readonly XClass Owner;

            public ClassInstanceVariablesPool(XClass owner)
            {
                if (owner == null)
                    throw new ArgumentNullException();
                this.Owner = owner;
            }

            public ClassInstanceVariablesPool(XClass owner, IEnumerable<Installer.SourceReference<string>> items)
                : this(owner)
            {
                if (items == null)
                    throw new ArgumentNullException();
                foreach (var item in items)
                {
                    XBinding binding = this.CreateBinding(item.Value);
                    binding.MakeVariable();
                    binding.Value = null; // Bind it
                }
            }

            public override XBinding GetBinding(string key)
            {
                XBinding result = this.InternalGetBinding(key);
                if (result != null)
                    return result;
                // Try the superclasses
                XClass super = this.Owner.Superclass;
                if (super == null)
                    return null;
                // Check if this is defined in a superclass
                result = super.ClassInstanceVariables.InternalGetBinding(key);
                if (result == null)
                    return null; // No definition exists in the superclasses.
                // The class-instance-variable is defined in a superclass, create a local binding.
                result = new XBinding(key);
                this.Bindings.Add(key, result);
                return result;
            }

            private XBinding InternalGetBinding(string key)
            {
                if (String.IsNullOrWhiteSpace(key))
                    throw new ArgumentNullException();
                XBinding result;
                if (this.Bindings.TryGetValue(key, out result))
                    return result;
                return null;
            }

            protected override XBinding GetOrCreateBinding(string key)
            {
                // We are happy with the base definition, which will create a local binding.
                // We allow our class to shadow (hide) variables from the superclass.
                return base.GetOrCreateBinding(key);
            }


        }
        public class ImportedPoolsPool : XBindingScope
        {
            public ImportedPoolsPool(XSystemDictionary smalltalk, IEnumerable<Installer.SourceReference<string>> items)
            {
                if ((items == null) || (smalltalk == null))
                    throw new ArgumentNullException();
                foreach (var item in items)
                {
                    XBinding binding = smalltalk.GetOrCreateBinding(item.Value);
                    this.Bindings.Add(binding.Key, binding);
                }
            }
        }
    }

    
}
