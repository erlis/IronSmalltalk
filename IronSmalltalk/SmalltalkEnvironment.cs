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
//using IronSmalltalk.Compiler;
using System.Linq;
using IronSmalltalk.Internals;
using IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Runtime.Internal;
using System.Collections.Generic;
using IronSmalltalk.Runtime;

namespace IronSmalltalk
{
    public class SmalltalkEnvironment
    {
        public SmalltalkRuntime Runtime { get; private set; }

        public CompilerService CompilerService { get; private set; }

        public SmalltalkEnvironment()
        {
            this.Runtime = new SmalltalkRuntime();
            this.CompilerService = new CompilerService(this.Runtime);
        }

        public Symbol GetSymbol(string value)
        {
            if (value == null)
                throw new ArgumentNullException();

            return this.Runtime.GetSymbol(value);
        }

        public SmalltalkClass GetClass(string name)
        {
            ClassBinding binding = this.Runtime.GlobalScope.GetClassBinding(name);
            if (binding == null)
                return null;
            return binding.Value;
        }

        public SmalltalkClass GetClass(Symbol name)
        {
            ClassBinding binding = this.Runtime.GlobalScope.GetClassBinding(name);
            if (binding == null)
                return null;
            return binding.Value;
        }

        public Pool GetPool(string name)
        {
            PoolBinding binding = this.Runtime.GlobalScope.GetPoolBinding(name);
            if (binding == null)
                return null;
            return binding.Value;
        }

        public Pool GetPool(Symbol name)
        {
            PoolBinding binding = this.Runtime.GlobalScope.GetPoolBinding(name);
            if (binding == null)
                return null;
            return binding.Value;
        }

        public object GetGlobal(string name)
        {
            object value;
            if (!this.TryGetGlobal(name, out value))
                throw new KeyNotFoundException(name);
            return value;
        }

        public object GetGlobal(Symbol name)
        {
            if (name == null)
                throw new ArgumentNullException();
            object value;
            if (!this.TryGetGlobal(name, out value))
                throw new KeyNotFoundException(name.Value);
            return value;
        }

        public bool TryGetGlobal(string name, out object value)
        {
            GlobalVariableOrConstantBinding binding = this.Runtime.GlobalScope.GetGlobalVariableOrConstantBinding(name);
            if ((binding == null) || !binding.HasBeenSet)
            {
                value = null;
                return false;
            }
            value = binding.Value;
            return true;
        }

        public bool TryGetGlobal(Symbol name, out object value)
        {
            GlobalVariableOrConstantBinding binding = this.Runtime.GlobalScope.GetGlobalVariableOrConstantBinding(name);
            if ((binding == null) || !binding.HasBeenSet)
            {
                value = null;
                return false;
            }
            value = binding.Value;
            return true;
        }

        public void SetGlobal(string name, object value)
        {
            if (!this.TrySetGlobal(name, value))
                throw new KeyNotFoundException(name);
        }

        public void SetGlobal(Symbol name, object value)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (!this.TrySetGlobal(name, value))
                throw new KeyNotFoundException(name.Value);
        }

        public bool TrySetGlobal(string name, object value)
        {
            GlobalVariableBinding binding = this.Runtime.GlobalScope.GetGlobalVariableBinding(name);
            if ((binding == null) || binding.IsConstantBinding)
                return false;
            binding.Value = value;
            return true;
        }

        public bool TrySetGlobal(Symbol name, object value)
        {
            GlobalVariableBinding binding = this.Runtime.GlobalScope.GetGlobalVariableBinding(name);
            if ((binding == null) || binding.IsConstantBinding)
                return false;
            binding.Value = value;
            return true;
        }
    }
}
