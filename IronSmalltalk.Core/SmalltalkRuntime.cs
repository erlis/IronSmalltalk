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
using IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime;

namespace IronSmalltalk
{
    /// <summary>
    /// The Smalltalk runtime encapsulates all information necessary to execute Smalltalk code. 
    /// </summary>
    /// <remarks>
    /// The SmalltalkRuntime is somehow analogues to a classic Smalltalk image, however it differs 
    /// that it contains no development tools (compilers, installer, etc.) and contains only 
    /// live objects and behaviors.
    /// 
    /// It is an object and several runtimes can co-exist in the same AppDomain. Two or more runtimes
    /// should not interfeer with each other.
    /// 
    /// This is the root object for running any Smalltalk code. It contains many helper methods.
    /// </remarks>
    public partial class SmalltalkRuntime
    {
        /// <summary>
        /// Extension scope exists so that IronSmalltalk can provide predefined global name bindings.
        /// </summary>
        public SmalltalkNameScope ExtensionScope { get; private set; }

        /// <summary>
        /// The global definition scope contains names that are explicitly defined by the clients.
        /// </summary>
        public SmalltalkNameScope GlobalScope { get; private set; }

        /// <summary>
        /// Table of currently used / defined symbols.
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        /// <summary>
        /// Dictionary where services and users can cache whatever they wish.
        /// </summary>
        public Dictionary<object, object> ServicesCache { get; private set; }

        /// <summary>
        /// All classes currently defined in the system.
        /// </summary>
        public DiscreteBindingDictionary<ClassBinding> Classes
        {
            get
            {
#if DEBUG
                if (this.GlobalScope.AllClasses == null)
                    throw new InvalidOperationException("Should have called WriteProtect on the GlobalScope");
#endif
                return this.GlobalScope.AllClasses;
            }
        }

        /// <summary>
        /// All shared pools currently defined in the system.
        /// </summary>
        public DiscreteBindingDictionary<PoolBinding> Pools
        {
            get
            {
#if DEBUG
                if (this.GlobalScope.AllPools == null)
                    throw new InvalidOperationException("Should have called WriteProtect on the GlobalScope");
#endif
                return this.GlobalScope.AllPools;
            }
        }

        /// <summary>
        /// All global variables or global constants currently defined in the system.
        /// </summary>
        public DiscreteBindingDictionary<GlobalVariableOrConstantBinding> GlobalVariablesOrConstant
        {
            get
            {
#if DEBUG
                if (this.GlobalScope.AllGlobalVariablesOrConstant == null)
                    throw new InvalidOperationException("Should have called WriteProtect on the GlobalScope");
#endif
                return this.GlobalScope.AllGlobalVariablesOrConstant;
            }
        }

        /// <summary>
        /// All global objects (classes, pools, variables or constants) currently defined in the system.
        /// </summary>
        public DiscreteBindingDictionary<IDiscreteGlobalBinding> Globals
        {
            get
            {
#if DEBUG
                if (this.GlobalScope.AllGlobals == null)
                    throw new InvalidOperationException("Should have called WriteProtect on the GlobalScope");
#endif
                return this.GlobalScope.AllGlobals;
            }
        }

        /// <summary>
        /// This private (but for technical reasons public) property contains mapping between native types and Smalltalk classes.
        /// </summary>
        public readonly NativeTypeClassMap NativeTypeClassMap;

        /// <summary>
        /// Create a new Smalltalk Runtime.
        /// </summary>
        public SmalltalkRuntime()
        {
            this.SymbolTable = new SymbolTable(this);
            this.NativeTypeClassMap = new NativeTypeClassMap(this);
            this.ExtensionScope = new SmalltalkNameScope(this);
            GlobalConstantBinding smalltalk = new GlobalConstantBinding(this.GetSymbol("Smalltalk"));
            smalltalk.SetValue(this);
            this.ExtensionScope.GlobalConstants.Add(smalltalk);
            this.ExtensionScope.WriteProtect();
            this.GlobalScope = new SmalltalkNameScope(this, this.ExtensionScope);
            this.GlobalScope.WriteProtect();
            this.ServicesCache = new Dictionary<object, object>();
            this.ExtensionScope.ProtectedNames.AddRange(new string[] {
                "nil", "true", "false", "self", "super", "Object", "Smalltalk" }
                    .Select(str => this.GetSymbol(str)));
        }

        #region Helpers

        /// <summary>
        /// Get the Smalltalk class with the given name.
        /// </summary>
        /// <param name="name">Class name.</param>
        /// <returns>The Smalltalk class with the given name or null if none found.</returns>
        public SmalltalkClass GetClass(Symbol name)
        {
            ClassBinding binding = this.GlobalScope.GetClassBinding(name);
            if (binding != null)
                return binding.Value;
            else
                return null;
        }

        /// <summary>
        /// Get the Smalltalk class with the given name.
        /// </summary>
        /// <param name="name">Class name.</param>
        /// <returns>The Smalltalk class with the given name or null if none found.</returns>
        public SmalltalkClass GetClass(string name)
        {
            ClassBinding binding = this.GlobalScope.GetClassBinding(name);
            if (binding != null)
                return binding.Value;
            else
                return null;
        }

        /// <summary>
        /// Get the Smalltalk shared pool with the given name.
        /// </summary>
        /// <param name="name">Shared pool name.</param>
        /// <returns>The Smalltalk shared pool with the given name or null if none found.</returns>
        public Pool GetPool(Symbol name)
        {
            PoolBinding binding = this.GlobalScope.GetPoolBinding(name);
            if (binding != null)
                return binding.Value;
            else
                return null;
        }

        /// <summary>
        /// Get the Smalltalk shared pool with the given name.
        /// </summary>
        /// <param name="name">Shared pool name.</param>
        /// <returns>The Smalltalk shared pool with the given name or null if none found.</returns>
        public Pool GetPool(string name)
        {
            PoolBinding binding = this.GlobalScope.GetPoolBinding(name);
            if (binding != null)
                return binding.Value;
            else
                return null;
        }

        /// <summary>
        /// Get global variable or global constant with the given name.
        /// </summary>
        /// <param name="name">Global variable or global constant name.</param>
        /// <returns>Value of the global variable or global constant. If not found, null is returned.</returns>
        public object GetGlobal(Symbol name)
        {
            bool na;
            return this.GetGlobal(name, out na);
        }

        /// <summary>
        /// Get global variable or global constant with the given name.
        /// </summary>
        /// <param name="name">Global variable or global constant name.</param>
        /// <param name="exists">True if the global variable or global constant exists, otherwise false.</param>
        /// <returns>Value of the global variable or global constant. If not found, null is returned.</returns>
        public object GetGlobal(Symbol name, out bool exists)
        {
            GlobalVariableOrConstantBinding binding = this.GlobalScope.GetGlobalVariableOrConstantBinding(name);
            if (binding == null)
            {
                exists = false;
                return null;
            }
            else
            {
                exists = true;
                return binding.Value;
            }
        }

        /// <summary>
        /// Get global variable or global constant with the given name.
        /// </summary>
        /// <param name="name">Global variable or global constant name.</param>
        /// <returns>Value of the global variable or global constant. If not found, null is returned.</returns>
        public object GetGlobal(string name)
        {
            bool na;
            return this.GetGlobal(name, out na);
        }

        /// <summary>
        /// Get global variable or global constant with the given name.
        /// </summary>
        /// <param name="name">Global variable or global constant name.</param>
        /// <param name="exists">True if the global variable or global constant exists, otherwise false.</param>
        /// <returns>Value of the global variable or global constant. If not found, null is returned.</returns>
        public object GetGlobal(string name, out bool exists)
        {
            GlobalVariableOrConstantBinding binding = this.GlobalScope.GetGlobalVariableOrConstantBinding(name);
            if (binding == null)
            {
                exists = false;
                return null;
            }
            else
            {
                exists = true;
                return binding.Value;
            }
        }

        /// <summary>
        /// Set the value of the given global variable.
        /// </summary>
        /// <param name="name">Name of the global variable.</param>
        /// <param name="value">New value of the global variable.</param>
        /// <returns>True if the variable exists, otherwise false.</returns>
        public bool SetGlobal(Symbol name, object value)
        {
            GlobalVariableBinding binding = this.GlobalScope.GetGlobalVariableBinding(name);
            if (binding == null)
                return false;
            binding.Value = value;
            return true;
        }

        /// <summary>
        /// Set the value of the given global variable.
        /// </summary>
        /// <param name="name">Name of the global variable.</param>
        /// <param name="value">New value of the global variable.</param>
        /// <returns>True if the variable exists, otherwise false.</returns>
        public bool SetGlobal(string name, object value)
        {
            GlobalVariableBinding binding = this.GlobalScope.GetGlobalVariableBinding(name);
            if (binding == null)
                return false;
            binding.Value = value;
            return true;
        }

        /// <summary>
        /// Get or create a symbol that has the requested string value.
        /// </summary>
        /// <param name="value">String value of the symbol.</param>
        /// <returns>An unique symbol with the requested string value.</returns>
        public Symbol GetSymbol(string value)
        {
            if (value == null)
                throw new ArgumentNullException();

            return this.SymbolTable.GetSymbol(value);
        }
        
        #endregion

        /// <summary>
        /// This is internal metnod used by the Intaller to redefine the globals in the runtime.
        /// </summary>
        /// <param name="scope">New globals scope containing the extension (IronSmalltalk defined) globals.</param>
        public void SetExtensionScope(SmalltalkNameScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException();
            scope.WriteProtect();
            this.ExtensionScope = scope;
        }

        /// <summary>
        /// This is internal metnod used by the Intaller to redefine the globals in the runtime.
        /// </summary>
        /// <param name="scope">New globals scope containing the global (end-user defined) globals.</param>
        public void SetGlobalScope(SmalltalkNameScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException();
            scope.WriteProtect();
            this.GlobalScope = scope;
        }
    }
}
