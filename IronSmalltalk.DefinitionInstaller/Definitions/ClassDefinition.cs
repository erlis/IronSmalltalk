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
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.Bindings;

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    /// <summary>
    /// Definition description of a Smalltalk class.
    /// </summary>
    public class ClassDefinition : GlobalBase 
    {
        /// <summary>
        /// Name of the class.
        /// </summary>
        public SourceReference<string> ClassName
        {
            get { return this.Name; }
        }
        /// <summary>
        /// Name of the superclass.
        /// </summary>
        public SourceReference<string> SuperclassName { get; private set; }
        /// <summary>
        /// Instance state of class, i.e. Byte-Indexable, Object-Indexable or Named-Instance-Variables.
        /// </summary>
        public SourceReference<SmalltalkClass.InstanceStateEnum> InstanceState { get; private set; }
        /// <summary>
        /// Names of the class-instance-variables directly defined in this class.
        /// </summary>
        public IEnumerable<SourceReference<string>> ClassInstanceVariableNames { get; private set; }
        /// <summary>
        /// Names of the class-variables directly defined in this class.
        /// </summary>
        public IEnumerable<SourceReference<string>> ClassVariableNames { get; private set; }
        /// <summary>
        /// Names of the instance-variables directly defined in this class.
        /// </summary>
        public IEnumerable<SourceReference<string>> InstanceVariableNames { get; private set; }
        /// <summary>
        /// Names of the pools imported by this class.
        /// </summary>
        public IEnumerable<SourceReference<string>> ImportedPoolNames { get; private set; }

        /// <summary>
        /// Creates a new definition description of a Smalltalk class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="superclassName">Name of the superclass.</param>
        /// <param name="instanceState">Instance state of class, i.e. Byte-Indexable, Object-Indexable or Named-Instance-Variables.</param>
        /// <param name="classInstanceVariableNames">Names of the class-instance-variables directly defined in this class.</param>
        /// <param name="classVariableNames">Names of the class-variables directly defined in this class.</param>
        /// <param name="instanceVariableNames">Names of the instance-variables directly defined in this class.</param>
        /// <param name="importedPoolNames">Names of the pools imported by this class.</param>
        public ClassDefinition(SourceReference<string> className, SourceReference<string> superclassName,
            SourceReference<SmalltalkClass.InstanceStateEnum> instanceState, IEnumerable<SourceReference<string>> classInstanceVariableNames,
            IEnumerable<SourceReference<string>> classVariableNames, IEnumerable<SourceReference<string>> instanceVariableNames,
            IEnumerable<SourceReference<string>> importedPoolNames)
            : base(className)
        {
            if (superclassName == null)
                throw new ArgumentNullException("superclassName");
            if (instanceState == null)
                throw new ArgumentNullException("instanceState");
            if (classInstanceVariableNames == null)
                throw new ArgumentNullException("classInstanceVariableNames");
            if (classVariableNames == null)
                throw new ArgumentNullException("classVariableNames");
            if (instanceVariableNames == null)
                throw new ArgumentNullException("instanceVariableNames");
            if (importedPoolNames == null)
                throw new ArgumentNullException("importedPoolNames");
            this.SuperclassName = superclassName;
            this.InstanceState = instanceState;
            this.ClassInstanceVariableNames = classInstanceVariableNames;
            this.ClassVariableNames = classVariableNames;
            this.InstanceVariableNames = instanceVariableNames;
            this.ImportedPoolNames = importedPoolNames;
        }

        /// <summary>
        /// Returns a System.String that represents the class definition object.
        /// </summary>
        /// <returns>A System.String that represents the class definition object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Class named: '{0}' \n", this.ClassName.Value);
            sb.AppendFormat("\tsuperclass: '{0}' \n", this.SuperclassName.Value);
            sb.AppendFormat("\tindexedInstanceVariables: {0} \n", this.InstanceState.Value);
            sb.AppendFormat("\tinstanceVariableNames: '{0}' \n", String.Join(" ", this.InstanceVariableNames.Select(sr => sr.Value)));
            sb.AppendFormat("\tclassVariableNames: '{0}' \n", String.Join(" ", this.ClassVariableNames.Select(sr => sr.Value)));
            sb.AppendFormat("\tsharedPools: '{0}' \n", String.Join(" ", this.ImportedPoolNames.Select(sr => sr.Value)));
            sb.AppendFormat("\tclassInstanceVariableNames: '{0}'", String.Join(" ", this.ClassInstanceVariableNames.Select(sr => sr.Value)));
            base.ToString();
            return sb.ToString();
        }

        /// <summary>
        /// Create the binding object (association) for the class in the global name scope (system dictionary).
        /// </summary>
        /// <param name="installer">Context within which the binding is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool CreateGlobalBinding(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 1. Check if the name is not complete garbage ... or reserved identifier
            if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(this.Name.Value))
                return installer.ReportError(this.Name, InstallerErrors.ClassInvalidName);
            if (IronSmalltalk.Common.GlobalConstants.ReservedIdentifiers.Contains(this.Name.Value))
                return installer.ReportError(this.Name, InstallerErrors.ClassReservedName);
            // 2. Check if there is already a global with that name in the inner name scope
            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            IDiscreteGlobalBinding existingBinding = installer.GetLocalGlobalBinding(name);
            if (existingBinding != null)
                return installer.ReportError(this.Name, InstallerErrors.ClassNameNotUnique);
            // 3. No global defined in the inner scope, but check the scopes (inner or outer) for protected names like Object etc.
            if (installer.IsProtectedName(name))
                return installer.ReportError(this.Name, InstallerErrors.ClassNameProtected);

            // 4. OK ... create a binding - so far pointing to null ... this will be set in CreateGlobalObject().
            installer.AddClassBinding(new ClassBinding(name));
            return true;
        }

        /// <summary>
        /// Create the class object (and sets the value of the binding).
        /// </summary>
        /// <param name="installer">Context within which the class is to be created.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool CreateGlobalObject(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 1. Get the binding to the global 
            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            ClassBinding binding = installer.GetLocalClassBinding(name);
            // 2. Check consistency that we didn't mess up in the implementation.
            if (binding == null)
                throw new InvalidOperationException("Should have found a binding, because CreateGlobalBinding() created it!");
            if (binding.Value != null)
                throw new InvalidOperationException("Should be an empty binding, because CreateGlobalBinding() complained if one already existed!");

            // 3. Prepare stuff ....
            SmalltalkClass.InstanceStateEnum instanceState = this.InstanceState.Value;
            ClassBinding superclass;
            if (this.SuperclassName.Value.Length == 0)
            {
                superclass = null; // Object has no superclass
            }
            else
            {
                superclass = installer.GetClassBinding(this.SuperclassName.Value);
                if (superclass == null)
                    return installer.ReportError(this.SuperclassName, InstallerErrors.ClassInvalidSuperclass);
            }

            // Create the collection of class, class-instance, instance variables and imported pools
            BindingDictionary<InstanceVariableBinding> instVars = new BindingDictionary<InstanceVariableBinding>(installer.Runtime);
            DiscreteBindingDictionary<ClassVariableBinding> classVars = new DiscreteBindingDictionary<ClassVariableBinding>(installer.Runtime, this.ClassVariableNames.Count());
            BindingDictionary<ClassInstanceVariableBinding> classInstVars = new BindingDictionary<ClassInstanceVariableBinding>(installer.Runtime);
            DiscreteBindingDictionary<PoolBinding> pools = new DiscreteBindingDictionary<PoolBinding>(installer.Runtime);
            // Validate class variable names ...
            foreach (SourceReference<string> identifier in this.ClassVariableNames)
            {
                Symbol varName = installer.Runtime.GetSymbol(identifier.Value);
                if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(identifier.Value))
                    return installer.ReportError(identifier, InstallerErrors.ClassClassVariableNotIdentifier);
                if (classVars.Any<ClassVariableBinding>(varBinding => varBinding.Name == varName))
                    return installer.ReportError(identifier, InstallerErrors.ClassClassVariableNotUnique);
                classVars.Add(new ClassVariableBinding(varName));
            }
            // Validate instance variable names ...
            foreach (SourceReference<string> identifier in this.InstanceVariableNames)
            {
                Symbol varName = installer.Runtime.GetSymbol(identifier.Value);
                if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(identifier.Value))
                    return installer.ReportError(identifier, InstallerErrors.ClassInstanceVariableNotIdentifier);
                if (((IEnumerable<InstanceVariableBinding>) instVars).Any(varBinding => varBinding.Name == varName))
                    return installer.ReportError(identifier, InstallerErrors.ClassInstanceVariableNotUnique);
                if (classVars.Any<ClassVariableBinding>(varBinding => varBinding.Name == varName))
                    return installer.ReportError(identifier, InstallerErrors.ClassInstanceOrClassVariableNotUnique);
                instVars.Add(new InstanceVariableBinding(varName));
            }
            // Validate class instance variable names ...
            foreach (SourceReference<string> identifier in this.ClassInstanceVariableNames)
            {
                Symbol varName = installer.Runtime.GetSymbol(identifier.Value);
                if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(identifier.Value))
                    return installer.ReportError(identifier, InstallerErrors.ClassClassInstanceVariableNotIdentifier);
                if (classInstVars.Any<ClassInstanceVariableBinding>(varBinding => varBinding.Name == varName))
                    return installer.ReportError(identifier, InstallerErrors.ClassClassInstanceVariableNotUnique);
                if (classVars.Any<ClassVariableBinding>(varBinding => varBinding.Name == varName))
                    return installer.ReportError(identifier, InstallerErrors.ClassClassInstanceOrClassVariableNotUnique);
                classInstVars.Add(new ClassInstanceVariableBinding(varName));
            }
            // Validate imported pool names ...
            foreach (SourceReference<string> identifier in this.ImportedPoolNames)
            {
                Symbol varName = installer.Runtime.GetSymbol(identifier.Value);
                if (!IronSmalltalk.Common.Utilities.ValidateIdentifier(identifier.Value))
                    return installer.ReportError(identifier, InstallerErrors.ClassImportedPoolNotIdentifier);
                if (pools.Any<PoolBinding>(varBinding => varBinding.Name == varName))
                    return installer.ReportError(identifier, InstallerErrors.ClassImportedPoolNotUnique);
                PoolBinding pool = installer.GetPoolBinding(varName);
                if (pool == null)
                    return installer.ReportError(identifier, InstallerErrors.ClassImportedPoolNotDefined);
                pools.Add(pool);
            }

            // 4. Finally, create the behavior object
            binding.SetValue(new SmalltalkClass(
                installer.Runtime, binding.Name, superclass, instanceState, instVars, classVars, classInstVars, pools));
            return true;
        }

        /// <summary>
        /// Validate that the definition of the class object does not break any rules set by the Smalltalk standard.
        /// </summary>
        /// <param name="installer">Context within which the validation is to be performed.</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        protected internal override bool ValidateObject(IInstallerContext installer)
        {
            if (installer == null)
                throw new ArgumentNullException();
            // 1. Get the binding to the global 
            Symbol name = installer.Runtime.GetSymbol(this.Name.Value);
            ClassBinding binding = installer.GetLocalClassBinding(name);
            // 2. Check consistency that we didn't mess up in the implementation.
            if (binding == null)
                throw new InvalidOperationException("Should have found a binding, because CreateGlobalBinding() created it!");
            if (binding.Value == null)
                throw new InvalidOperationException("Should not be an empty binding, because CreateGlobalObject() just created it!");
            // 3. Get the class object.
            SmalltalkClass cls = binding.Value;
            // 4. Validate that superclass is correct
            List<Symbol> classes = new List<Symbol>();
            classes.Add(cls.Name);
            SmalltalkClass tmp = cls.Superclass;
            while (tmp != null)
            {
                if (classes.IndexOf(tmp.Name) != -1)
                    return installer.ReportError(this.SuperclassName, String.Format(
                        InstallerErrors.ClassCircularReference, this.SuperclassName.Value, this.Name.Value));
                classes.Add(tmp.Name);
                tmp = tmp.Superclass;
            }

            if (cls.InstanceState != SmalltalkClass.InstanceStateEnum.ByteIndexable)
            {
                tmp = cls;
                while (tmp != null)
                {
                    if (tmp.InstanceState == SmalltalkClass.InstanceStateEnum.ByteIndexable)
                        return installer.ReportError(this.InstanceState, InstallerErrors.ClassCannotChangeInstanceState);
                    tmp = tmp.Superclass;
                }
            }

            foreach (PoolBinding pool in cls.ImportedPoolBindings)
            {
                if (pool.Value == null)
                {
                    SourceReference<string> src = this.ImportedPoolNames.SingleOrDefault(sr => sr.Value == pool.Name.Value);
                    if (src == null)
                        throw new InvalidOperationException("Should not be null, cause we just added this pool in CreateGlobalObject().");
                    return installer.ReportError(src, InstallerErrors.ClassMissingPoolDefinition);
                }
            }

            installer.RegisterNewClass(cls);

            return true; // OK
        }

    }
}
