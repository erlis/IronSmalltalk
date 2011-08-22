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

namespace IronSmalltalk.Runtime.Bindings
{
    /// <summary>
    /// The SmalltalkNameScope contains globals, pools and class. 
    /// It is analogues to the SystemDictionary in classical implementation.
    /// </summary>
    /// <remarks>
    /// The SmalltalkNameScope is usually a global object that is references
    /// by the SmalltalkRuntime. In most cases, it is not needed to access 
    /// this object directly; The SmalltalkRuntime object contains helper 
    /// functions to access the items in the SmalltalkNameScope object. The
    /// SmalltalkNameScope is needed for fine-grade access mostly during 
    /// definition install (file-in).
    /// 
    /// The name scope contains the global binding, i.e. global variable, global
    /// constants, classes and shared pools. It may optionally have an outer scope,
    /// which is used when we want to have several layers of definitions. For example,
    /// IronSmalltalk may define the system behavior in a scope named the X3J20 documents
    /// as the "Extension Scope". This scope will be the outer-scope for the 
    /// "Global Scope" containing element defined by the end-user developers.
    /// The inner scope shadows elements in the outer scope. In some cases, the
    /// outer scope may refuse shadowing by an inner scope, if if decides that
    /// the element name is a protected name.
    /// 
    /// Element lookup is by name, the inner scope first looking within its
    /// own elements and then looking in the outer scope. The exeptions are
    /// the GetLocalXXX functions, which do not look into the outer scopes.
    /// 
    /// A SmalltalkNameScope is read-only and cannot normally be modified.
    /// Only the Definition-Installer can create and modify scopes.
    /// </remarks>
    public class SmalltalkNameScope
    {
        public SmalltalkNameScope OuterScope { get; private set; }
        public SmalltalkRuntime Runtime { get; private set; }
        public List<Symbol> ProtectedNames { get; private set; }
        public DiscreteBindingDictionary<ClassBinding> Classes { get; private set; }
        public DiscreteBindingDictionary<PoolBinding> Pools { get; private set; }
        public DiscreteBindingDictionary<GlobalVariableBinding> GlobalVariables { get; private set; }
        public DiscreteBindingDictionary<GlobalConstantBinding> GlobalConstants { get; private set; }

        public DiscreteBindingDictionary<ClassBinding> AllClasses { get; private set; }
        public DiscreteBindingDictionary<PoolBinding> AllPools { get; private set; }
        public DiscreteBindingDictionary<GlobalVariableOrConstantBinding> AllGlobalVariablesOrConstant { get; private set; }
        public DiscreteBindingDictionary<IDiscreteGlobalBinding> AllGlobals { get; private set; }

        public SmalltalkNameScope(SmalltalkRuntime runtime)
            : this(runtime, null)
        {
        }

        public SmalltalkNameScope(SmalltalkRuntime runtime, SmalltalkNameScope outerScope)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            this.Runtime = runtime;
            this.Classes = new DiscreteBindingDictionary<ClassBinding>(runtime, 50);
            this.GlobalConstants = new DiscreteBindingDictionary<GlobalConstantBinding>(runtime, 20);
            this.GlobalVariables = new DiscreteBindingDictionary<GlobalVariableBinding>(runtime, 20);
            this.Pools = new DiscreteBindingDictionary<PoolBinding>(runtime, 20);
            this.ProtectedNames = new List<Symbol>();
            this.OuterScope = outerScope;
        }

        public SmalltalkNameScope Copy()
        {
            SmalltalkNameScope result = new SmalltalkNameScope(this.Runtime, this.OuterScope);
            result.Classes.AddRange(this.Classes);
            result.GlobalConstants.AddRange(this.GlobalConstants);
            result.GlobalVariables.AddRange(this.GlobalVariables);
            result.Pools.AddRange(this.Pools);
            result.ProtectedNames.AddRange(this.ProtectedNames);
            return result; // NB: Result IS NOT write-protected.
        }

        public ClassBinding GetClassBinding(string name)
        {
            return this.GetClassBinding(this.Runtime.GetSymbol(name));
        }

        public ClassBinding GetClassBinding(Symbol name)
        {
            ClassBinding binding;
            this.Classes.TryGetValue(name, out binding);
            if ((binding == null) && (this.OuterScope != null))
                return this.OuterScope.GetClassBinding(name);
            return binding;
        }

        public PoolBinding GetPoolBinding(string name)
        {
            return this.GetPoolBinding(this.Runtime.GetSymbol(name));
        }

        public PoolBinding GetPoolBinding(Symbol name)
        {
            PoolBinding binding;
            this.Pools.TryGetValue(name, out binding);
            if ((binding == null) && (this.OuterScope != null))
                return this.OuterScope.GetPoolBinding(name);
            return binding;
        }

        public GlobalConstantBinding GetGlobalConstantBinding(string name)
        {
            return this.GetGlobalConstantBinding(this.Runtime.GetSymbol(name));
        }

        public GlobalConstantBinding GetGlobalConstantBinding(Symbol name)
        {
            GlobalConstantBinding binding;
            this.GlobalConstants.TryGetValue(name, out binding);
            if ((binding == null) && (this.OuterScope != null))
                return this.OuterScope.GetGlobalConstantBinding(name);
            return binding;
        }

        public GlobalVariableBinding GetGlobalVariableBinding(string name)
        {
            return this.GetGlobalVariableBinding(this.Runtime.GetSymbol(name));
        }

        public GlobalVariableBinding GetGlobalVariableBinding(Symbol name)
        {
            GlobalVariableBinding binding;
            this.GlobalVariables.TryGetValue(name, out binding);
            if ((binding == null) && (this.OuterScope != null))
                return this.OuterScope.GetGlobalVariableBinding(name);
            return binding;
        }

        public GlobalVariableOrConstantBinding GetGlobalVariableOrConstantBinding(string name)
        {
            return this.GetGlobalVariableOrConstantBinding(this.Runtime.GetSymbol(name));
        }

        public GlobalVariableOrConstantBinding GetGlobalVariableOrConstantBinding(Symbol name)
        {
            GlobalVariableOrConstantBinding result = this.GetLocalGlobalVariableOrConstantBinding(name);
            if ((result == null) && (this.OuterScope != null))
                return this.OuterScope.GetGlobalVariableOrConstantBinding(name);
            return result;
        }

        public GlobalVariableOrConstantBinding GetLocalGlobalVariableOrConstantBinding(string name)
        {
            return this.GetLocalGlobalVariableOrConstantBinding(this.Runtime.GetSymbol(name));
        }

        public GlobalVariableOrConstantBinding GetLocalGlobalVariableOrConstantBinding(Symbol name)
        {
            GlobalVariableBinding variableBinding;
            this.GlobalVariables.TryGetValue(name, out variableBinding);
            if (variableBinding != null)
                return variableBinding;
            GlobalConstantBinding constantBinding;
            this.GlobalConstants.TryGetValue(name, out constantBinding);
            if (constantBinding != null)
                return constantBinding;
            return null;
        }

        public IDiscreteGlobalBinding GetGlobalBinding(string name)
        {
            return this.GetGlobalBinding(this.Runtime.GetSymbol(name));
        }

        public IDiscreteGlobalBinding GetGlobalBinding(Symbol name)
        {
            IDiscreteGlobalBinding result = this.GetLocalGlobalBinding(name);
            if ((result == null) && (this.OuterScope != null))
                return this.OuterScope.GetGlobalBinding(name);
            return result;
        }

        public IDiscreteGlobalBinding GetLocalGlobalBinding(string name)
        {
            return this.GetLocalGlobalBinding(this.Runtime.GetSymbol(name));
        }

        public IDiscreteGlobalBinding GetLocalGlobalBinding(Symbol name)
        {
            ClassBinding classBinding;
            this.Classes.TryGetValue(name, out classBinding);
            if (classBinding != null)
                return classBinding;
            GlobalVariableBinding variableBinding;
            this.GlobalVariables.TryGetValue(name, out variableBinding);
            if (variableBinding != null)
                return variableBinding;
            GlobalConstantBinding constantBinding;
            this.GlobalConstants.TryGetValue(name, out constantBinding);
            if (constantBinding != null)
                return constantBinding;
            PoolBinding poolBinding;
            this.Pools.TryGetValue(name, out poolBinding);
            if (poolBinding != null)
                return poolBinding;
            return null;
        }

        public bool IsProtectedName(string name)
        {
            return this.IsProtectedName(this.Runtime.GetSymbol(name));
        }

        public bool IsProtectedName(Symbol name)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (this.ProtectedNames.Contains(name) && (this.GetLocalGlobalBinding(name) != null))
                return true;
            if (this.OuterScope == null)
                return false;
            return this.OuterScope.IsProtectedName(name);
        }

        internal void WriteProtect()
        {
            // Once write-protected, it cannot be unprotected to read-write state!
            lock (this)
            {
                this.Classes.WriteProtect();
                this.GlobalConstants.WriteProtect();
                this.GlobalVariables.WriteProtect();
                this.Pools.WriteProtect();

                this.AllClasses = new DiscreteBindingDictionary<ClassBinding>(this.Runtime,
                    this.Classes.Count + ((this.OuterScope == null) ? 0 : this.OuterScope.AllClasses.Count));
                this.AllGlobalVariablesOrConstant = new DiscreteBindingDictionary<GlobalVariableOrConstantBinding>(this.Runtime,
                    this.GlobalConstants.Count + this.GlobalVariables.Count + ((this.OuterScope == null) ? 0 : this.OuterScope.AllGlobalVariablesOrConstant.Count));
                this.AllPools = new DiscreteBindingDictionary<PoolBinding>(this.Runtime,
                    this.Pools.Count + ((this.OuterScope == null) ? 0 : this.OuterScope.AllPools.Count));
                this.AllGlobals = new DiscreteBindingDictionary<IDiscreteGlobalBinding>(this.Runtime);

                if (this.OuterScope != null)
                {
                    foreach (ClassBinding binding in this.OuterScope.AllClasses)
                        this.AllClasses.Add(binding);
                    foreach (GlobalVariableOrConstantBinding binding in this.OuterScope.AllGlobalVariablesOrConstant)
                        this.AllGlobalVariablesOrConstant.Add(binding);
                    foreach (PoolBinding binding in this.OuterScope.AllPools)
                        this.AllPools.Add(binding);
                    foreach (IDiscreteGlobalBinding binding in this.OuterScope.AllGlobals)
                        this.AllGlobals.Add(binding);
                }

                foreach (ClassBinding binding in this.Classes)
                {
                    if (this.AllClasses.ContainsKey(binding.Name))
                        this.AllClasses.Remove(binding.Name);
                    this.AllClasses.Add(binding);
                    if (this.AllGlobals.ContainsKey(binding.Name))
                        this.AllGlobals.Remove(binding.Name);
                    this.AllGlobals.Add(binding);
                }

                foreach (GlobalConstantBinding binding in this.GlobalConstants)
                {
                    if (this.AllGlobalVariablesOrConstant.ContainsKey(binding.Name))
                        this.AllGlobalVariablesOrConstant.Remove(binding.Name);
                    this.AllGlobalVariablesOrConstant.Add(binding);
                    if (this.AllGlobals.ContainsKey(binding.Name))
                        this.AllGlobals.Remove(binding.Name);
                    this.AllGlobals.Add(binding);
                }

                foreach (GlobalVariableBinding binding in this.GlobalVariables)
                {
                    if (this.AllGlobalVariablesOrConstant.ContainsKey(binding.Name))
                        this.AllGlobalVariablesOrConstant.Remove(binding.Name);
                    this.AllGlobalVariablesOrConstant.Add(binding);
                    if (this.AllGlobals.ContainsKey(binding.Name))
                        this.AllGlobals.Remove(binding.Name);
                    this.AllGlobals.Add(binding);
                }

                foreach (PoolBinding binding in this.Pools)
                {
                    if (this.AllPools.ContainsKey(binding.Name))
                        this.AllPools.Remove(binding.Name);
                    this.AllPools.Add(binding);
                    if (this.AllGlobals.ContainsKey(binding.Name))
                        this.AllGlobals.Remove(binding.Name);
                    this.AllGlobals.Add(binding);
                }

                this.AllClasses.WriteProtect();
                this.AllGlobals.WriteProtect();
                this.AllGlobalVariablesOrConstant.WriteProtect();
                this.AllPools.WriteProtect();
            }
        }
    }
}
