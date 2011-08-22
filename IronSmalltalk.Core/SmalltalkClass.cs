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
using IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Runtime.Behavior;

namespace IronSmalltalk.Runtime
{
    /// <summary>
    /// An object representing a Smalltalk class. It describes the properties and behavior
    /// of the class and the instances of that class.
    /// </summary>
    public partial class SmalltalkClass 
    {
        /// <summary>
        /// The SmalltalkRuntime that this class belongs to.
        /// </summary>
        public SmalltalkRuntime Runtime { get; private set; }

        /// <summary>
        /// Global name of the class object.
        /// </summary>
        /// <remarks>
        /// The binding in the global name scope is a constant binding.
        /// It is erroneous if there is any other global definitions of this name within the program. 
        /// IronSmalltalk: We interpret that the binding must be unique within it's namescope,
        /// i.e. it's OK to have the same global names in an outer scope (the ExtensionScope).
        /// It is erroneous if the class name is one of the reserved identifiers, "true", "false",
        /// "nil", "self" or "super".
        /// </remarks>
        public Symbol Name { get; private set; }

        /// <summary>
        /// Global binding to the class definition from which this definition inherits.
        /// </summary>
        /// <remarks>
        /// It is erroneous if superclass name is not the class name of another class definition
        /// whose binding exists in the global scope of this program.
        /// 
        /// If the superclass name is absent then this class has no inherited behavior. 
        /// It is erroneous if the superclass name is the same as the class name or if 
        /// superclass name is the name of a class that directly or indirectly specifies 
        /// class name as its superclass name.
        /// </remarks>
        public ClassBinding SuperclassBinding { get; private set; }


        /// <summary>
        /// Identifies the class definition from which this definition inherits.
        /// </summary>
        public SmalltalkClass Superclass
        {
            get { return (this.SuperclassBinding == null) ? null : this.SuperclassBinding.Value; }
        }

        /// <summary>
        /// List of bindings to instance variables directly defined by the class.
        /// </summary>
        /// <remarks>
        /// It is erroneous for the same identifier to occur more than once in the sequence of instance variable names. 
        /// It is erroneous if any of the instance variable names is the same identifier as an instance variable name 
        /// or class variable name defined by any superclass. 
        /// It is erroneous for an instance variable name to also appear as a class variable name. 
        /// It is erroneous if an instance variable name is one of the reserved identifiers, 
        /// "true", "false", "nil", "self" or "super".
        /// </remarks>
        public BindingDictionary<InstanceVariableBinding> InstanceVariableBindings { get; private set; }

        /// <summary>
        /// List of bindings to class instance variables directly defined by the class.
        /// </summary>
        /// <remarks>
        /// See also remarks for InstanceVariableNames.
        /// </remarks>
        public BindingDictionary<ClassInstanceVariableBinding> ClassInstanceVariableBindings { get; private set; }

        /// <summary>
        /// List of names of class variables directly defined by the class.
        /// </summary>
        /// <remarks>
        /// It is erroneous for the same identifier to occur more than once in the list of class variable names.
        /// It is erroneous if any of the class variable names is the same identifier as class variable name defined by any superclass.
        /// See also remarks for InstanceVariableNames.
        /// </remarks>
        public DiscreteBindingDictionary<ClassVariableBinding> ClassVariableBindings { get; private set; }

        /// <summary>
        /// Specifies variable pools whose elements may be referenced from within methods or initializers.
        /// </summary>
        /// <remarks>
        /// It is erroneous if each pool name is not the pool name of a pool definition
        /// whose binding exists in the global scope of this program. 
        /// It is erroneous for the same identifier to occur more than once in the list of pool names.
        /// It is the error binding if the pool variable is defined in more than one imported pool definition.
        /// </remarks>
        public DiscreteBindingDictionary<PoolBinding> ImportedPoolBindings { get; private set; }

        /// <summary>
        /// Instance methods
        /// </summary>
        public InstanceMethodDictionary InstanceBehavior { get; private set; }

        /// <summary>
        /// Class methods
        /// </summary>
        public ClassMethodDictionary ClassBehavior { get; private set; }

        /// <summary>
        /// Describes the type of data contained in instances of the class.
        /// </summary>
        public InstanceStateEnum InstanceState { get; private set; }

        /// <summary>
        /// Internal! This is used by the Installer to create new classes.
        /// </summary>
        /// <param name="runtime">Smalltalk runtime this class is part of.</param>
        /// <param name="name">Name of the class.</param>
        /// <param name="superclass">Optional binding to the class' superclass.</param>
        /// <param name="instanceState">State of the class' instances (named, object-indexable, byte-indexable).</param>
        /// <param name="instanceVariables">Instance variables bindings. Those are initially not initialized.</param>
        /// <param name="classVariables">Class variable binding.</param>
        /// <param name="classInstanceVariables">Class-instance variables bindings. Those are initially not initialized.</param>
        /// <param name="importedPools">Collection of pools that are imported by the class.</param>
        public SmalltalkClass(SmalltalkRuntime runtime, Symbol name, ClassBinding superclass, InstanceStateEnum instanceState,
            BindingDictionary<InstanceVariableBinding> instanceVariables, DiscreteBindingDictionary<ClassVariableBinding> classVariables,
            BindingDictionary<ClassInstanceVariableBinding> classInstanceVariables, DiscreteBindingDictionary<PoolBinding> importedPools)
            : this(runtime, name, superclass, instanceState, instanceVariables, classVariables, classInstanceVariables, importedPools, null, null)
        {
        }

        /// <summary>
        /// Internal! This is used by the Installer to create new classes.
        /// </summary>
        /// <param name="runtime">Smalltalk runtime this class is part of.</param>
        /// <param name="name">Name of the class.</param>
        /// <param name="superclass">Optional binding to the class' superclass.</param>
        /// <param name="instanceState">State of the class' instances (named, object-indexable, byte-indexable).</param>
        /// <param name="instanceVariables">Instance variables bindings. Those are initially not initialized.</param>
        /// <param name="classVariables">Class variable binding.</param>
        /// <param name="classInstanceVariables">Class-instance variables bindings. Those are initially not initialized.</param>
        /// <param name="importedPools">Collection of pools that are imported by the class.</param>
        /// <param name="instanceMethods">Collection with the methods defining the instance behaviors.</param>
        /// <param name="classMethods">Collection with the methods defining the class behaviors.</param>
        public SmalltalkClass(SmalltalkRuntime runtime, Symbol name, ClassBinding superclass, InstanceStateEnum instanceState,
            BindingDictionary<InstanceVariableBinding> instanceVariables, DiscreteBindingDictionary<ClassVariableBinding> classVariables,
            BindingDictionary<ClassInstanceVariableBinding> classInstanceVariables, DiscreteBindingDictionary<PoolBinding> importedPools,
            InstanceMethodDictionary instanceMethods, ClassMethodDictionary classMethods)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");
            if ((name == null) || (name.Value.Length == 0))
                throw new ArgumentNullException("name");
            if (!SmalltalkClass.ValidateIdentifiers <InstanceVariableBinding>(instanceVariables))
                throw new ArgumentException("Invalid or duplicate instance variable name found", "instanceVariables");
            if (!SmalltalkClass.ValidateIdentifiers<ClassVariableBinding>(classVariables))
                throw new ArgumentException("Invalid or duplicate class variable name found", "classVariables");
            if (!SmalltalkClass.ValidateIdentifiers<ClassInstanceVariableBinding>(classInstanceVariables))
                throw new ArgumentException("Invalid or duplicate class instance variable name found", "classInstanceVariables");
            if (!SmalltalkClass.ValidateIdentifiers<PoolBinding>(importedPools))
                throw new ArgumentException("Invalid or duplicate imported pool name found", "importedPools");
            if (!SmalltalkClass.CheckDuplicates <InstanceVariableBinding, ClassVariableBinding>(instanceVariables, classVariables))
                throw new ArgumentException("Duplicate instance or class variable name. Instance and class variable names must be unique.");
            if (!SmalltalkClass.CheckDuplicates<ClassInstanceVariableBinding, ClassVariableBinding>(classInstanceVariables, classVariables))
                throw new ArgumentException("Duplicate class-instance or class variable name. Class-instance and class variable names must be unique.");
            this.Runtime = runtime;
            this.ClassInstanceVariableBindings = (classInstanceVariables == null) ? 
                new BindingDictionary<ClassInstanceVariableBinding>(this.Runtime, 1) : classInstanceVariables;
            this.ClassBehavior = (classMethods == null) ?
                new ClassMethodDictionary(this.Runtime) : classMethods;
            this.ClassVariableBindings = (classVariables == null) ?
                new DiscreteBindingDictionary<ClassVariableBinding>(this.Runtime, 1) : classVariables;
            this.ImportedPoolBindings = (importedPools == null) ?
                new DiscreteBindingDictionary<PoolBinding>(this.Runtime, 1) : importedPools;
            this.InstanceBehavior = (instanceMethods == null) ?
                new InstanceMethodDictionary(this.Runtime) : instanceMethods;
            this.InstanceState = instanceState;
            this.InstanceVariableBindings = (instanceVariables == null) ?
                new BindingDictionary<InstanceVariableBinding>(this.Runtime, 1) : instanceVariables;
            this.Name = name;
            this.SuperclassBinding = superclass; // Null is OK .... Object has null
            this.InstanceSize = 0;
            this.ClassInstanceSize = 0;
            this.ClassInstanceVariables = new object[0];
        }

        /// <summary>
        /// Cached list of our direct subclasses.
        /// </summary>
        private List<SmalltalkClass> _subclasses = null;

        /// <summary>
        /// Classes that directly inherit from this class.
        /// </summary>
        public IEnumerable<SmalltalkClass> Subclasses
        {
            get
            {
                List<SmalltalkClass> result = this._subclasses;
                if (result == null)
                {
                    result = new List<SmalltalkClass>();
                    foreach (ClassBinding binding in this.Runtime.Classes)
                    {
                        if (binding.Value.Superclass == this)
                            result.Add(binding.Value);
                    }

                    lock (this)
                    {
                        if (this._subclasses == null)
                            this._subclasses = result;
                        else
                            result = this._subclasses;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Array with the class-instance variables of the class.
        /// </summary>
        public object[] ClassInstanceVariables { get; private set; }

        /// <summary>
        /// Number of all named instance variables defined in the class (including superclasses).
        /// </summary>
        public int InstanceSize { get; private set; }

        /// <summary>
        /// Number of all named class-instance variables defined in the class (including superclasses).
        /// </summary>
        public int ClassInstanceSize { get; private set; }

        /// <summary>
        /// Recompiles the class which involces recalculating caches state.
        /// </summary>
        /// <remarks>
        /// This flushes internal caches for the class and any affected 
        /// classes (subclases or possible superclasses).
        /// 
        /// The process includes:
        /// - Resetting cached subclasses collection.
        /// - Remapping instance-names to indexes.
        /// - Remapping class-instance-names to indexes.
        /// - Register the class for mapping between native .Net types and Smalltalk classes.
        /// - Write-protects dictionaries of imported pools, variables and methods (etc.)
        /// </remarks>
        public void Recompile()
        {
            SmalltalkClass superclass = this.Superclass;
            if (superclass != null)
            {
                lock (superclass)
                    superclass._subclasses = null;
            }

            this.RecompileIndexedVariables();
            this.RegisterNativeTypeMapping();

            lock(this)
                this._subclasses = null;
            foreach (SmalltalkClass cls in this.Subclasses)
                cls.Recompile();

            this.ClassInstanceVariableBindings.WriteProtect();
            this.ClassVariableBindings.WriteProtect();
            this.ImportedPoolBindings.WriteProtect();
            this.InstanceBehavior.WriteProtect();
            this.InstanceVariableBindings.WriteProtect();
        }

        private void RecompileIndexedVariables()
        {
            lock (this)
                this._subclasses = null;

#if DEBUG
            {
                // Validate that we don't have some bug or inconsistency in the calculation.
                SmalltalkClass scls = this.Superclass;
                int lastVar = Int32.MaxValue;
                while (scls != null)
                {
                    if (scls.InstanceSize > lastVar)
                        throw new InvalidOperationException("Broken instance variable binding. Bad implementation.");

                    IEnumerable<InstanceVariableBinding> vars = scls.InstanceVariableBindings;
                    if (vars.Any())
                    {
                        int min = vars.Min(binding => binding.Value);
                        int max = vars.Max(binding => binding.Value);
                        if (max >= lastVar)
                            throw new InvalidOperationException("Broken instance variable binding. Bad implementation.");
                        lastVar = min;
                    }
                    else
                    {
                        lastVar = scls.InstanceSize;
                    }
                    scls = scls.Superclass;
                }
                // ... and the class-instance variables ...
                scls = this.Superclass;
                lastVar = Int32.MaxValue;
                while (scls != null)
                {
                    if (scls.ClassInstanceSize > lastVar)
                        throw new InvalidOperationException("Broken class-instance variable binding. Bad implementation.");

                    IEnumerable<ClassInstanceVariableBinding> vars = scls.ClassInstanceVariableBindings;
                    if (vars.Any())
                    {
                        int min = vars.Min(binding => binding.Value);
                        int max = vars.Max(binding => binding.Value);
                        if (max >= lastVar)
                            throw new InvalidOperationException("Broken class-instance variable binding. Bad implementation.");
                        lastVar = min;
                    }
                    else
                    {
                        lastVar = scls.ClassInstanceSize;
                    }
                    scls = scls.Superclass;
                }
            }
#endif
            SmalltalkClass cls = this.Superclass;
            int idx = (cls == null) ? 0 : cls.InstanceSize;
            foreach (InstanceVariableBinding binding in this.InstanceVariableBindings)
            {
                binding.SetValue(idx);
                idx++;
            }
            this.InstanceSize = idx;

            idx = (cls == null) ? 0 : cls.ClassInstanceSize;
            foreach (ClassInstanceVariableBinding binding in this.ClassInstanceVariableBindings)
            {
                binding.SetValue(idx);
                idx++;
            }
            this.ClassInstanceSize = idx;

            object[] clsInstVars = new object[idx];
            Array.Copy(this.ClassInstanceVariables, 0, clsInstVars, 0, Math.Min(this.ClassInstanceVariables.Length, clsInstVars.Length));
            this.ClassInstanceVariables = clsInstVars;
        }

        private void RegisterNativeTypeMapping()
        {
            ClassBinding binding = this.Runtime.Classes[this.Name];
            foreach (KeyValuePair<string, string> annotation in binding.Annotations)
            {
                if ((annotation.Key == "ist.runtime.native-class") && !String.IsNullOrWhiteSpace(annotation.Value))
                    this.Runtime.NativeTypeClassMap.RegisterClass(this, annotation.Value);
            }
        }

        private static bool ValidateIdentifiers<TValue>(IEnumerable<TValue> items)
            where TValue : IBinding
        {
            if (items == null)
                return true;
            List<Symbol> names = new List<Symbol>();
            foreach (TValue item in items)
            {
                if (item == null)
                    return false;
                if (item.Name == null)
                    return false;
                if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(item.Name.Value))
                    return false;
                if (names.IndexOf(item.Name) != -1)
                    return false;
                if (IronSmalltalk.Common.GlobalConstants.ReservedIdentifiers.Contains(item.Name.Value))
                    return false;
                names.Add(item.Name);
            }
            return true;
        }

        private static bool CheckDuplicates<TValue1, TValue2>(IEnumerable<TValue1> items1, IEnumerable<TValue2> items2)
            where TValue1 : IBinding
            where TValue2 : IBinding
        {
            if (items1 == null)
                return true;
            if (items2 == null)
                return true;
            List<Symbol> names = new List<Symbol>();
            foreach (TValue1 item in items1)
            {
                if (names.IndexOf(item.Name) != -1)
                    return false;
                names.Add(item.Name);
            }
            foreach (TValue2 item in items2)
            {
                if (names.IndexOf(item.Name) != -1)
                    return false;
                names.Add(item.Name);
            }
            return true;
        }

        /// <summary>
        /// Returns a System.String that represents the current Smalltalk class object.
        /// </summary>
        /// <returns>A System.String that represents the current Smalltalk class object.</returns>
        public override string ToString()
        {
            return String.Format("{0}: {1}", base.ToString(), this.Name);
        }

        /// <summary>
        /// Instance state of the class as defined by X3J20 "3.3.2.1 Instance State Specification".
        /// </summary>
        public enum InstanceStateEnum
        {
            /// <summary>
            /// Variable number of unnamed instance variables containing binary data.
            /// </summary>
            /// <remarks>
            /// IronSmalltalk allows a binary object to have named instance variables.
            /// De-facto in .Net only byte[] makes sense for this type of object,
            /// because binary objects are primarily used when interfacing the OS.
            /// </remarks>
            ByteIndexable,
            /// <summary>
            /// Variable number of unnamed instance variables referencing other objects.
            /// </summary>
            /// <remarks>
            /// Indexable object may also have named instance variables.
            /// </remarks>
            ObjectIndexable,
            /// <summary>
            /// Object contains named instance variables only.
            /// </summary>
            NamedObjectVariables,
            /// <summary>
            /// This object is a native .Net object that has been mapped to a Smalltalk class.
            /// </summary>
            /// <remarks>
            /// The instance state of those object is implemented by the object itself.
            /// </remarks>
            Native
        }

        /// <summary>
        /// Create a new Smalltalk object.
        /// </summary>
        /// <remarks>
        /// If the class has unnamed instance variables (i.e. ByteIndexable or ObjectIndexable),
        /// an object with 0 (zero) unnamed instance variables is created.
        /// </remarks>
        /// <returns>A SmalltalkObject for the current class.</returns>
        public SmalltalkObject NewObject()
        {
            if (this.InstanceState == InstanceStateEnum.ByteIndexable)
                return new SmalltalkObject.ByteIndexableSmalltalkObject(this, 0);
            else if (this.InstanceState == InstanceStateEnum.ObjectIndexable)
                return new SmalltalkObject.ObjectIndexableSmalltalkObject(this, 0);
            else if (this.InstanceState == InstanceStateEnum.Native)
                throw new InvalidOperationException("Cannot instanciate object with native instance state");
            else
                return new SmalltalkObject(this);
        }

        /// <summary>
        /// Creat a new SmalltalkObject with the given number of unnamed instance variables.
        /// </summary>
        /// <param name="objectSize">Number of unnamed instance variables. Must be 0 (zero) or larger.</param>
        /// <returns>
        /// A SmalltalkObject.ByteIndexableSmalltalkObject or SmalltalkObject.ObjectIndexableSmalltalkObject
        /// with the given number of unnamed instance variables.
        /// Throws a SmalltalkRuntimeException if the class does not have unnamed instance variables.
        /// </returns>
        public SmalltalkObject NewObject(int objectSize)
        {
            if (this.InstanceState == InstanceStateEnum.ByteIndexable)
                return new SmalltalkObject.ByteIndexableSmalltalkObject(this, objectSize);
            else if (this.InstanceState == InstanceStateEnum.ObjectIndexable)
                return new SmalltalkObject.ObjectIndexableSmalltalkObject(this, objectSize);
            else
                throw new SmalltalkRuntimeException(String.Format("Class {0} does not have unnamed instance variables.", this.Name));
        }
    }
}
