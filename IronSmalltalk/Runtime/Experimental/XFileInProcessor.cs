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
using IronSmalltalk.Compiler.Interchange;

namespace IronSmalltalk.Runtime.Experimental
{
    public class XFileInProcessor : IInterchangeFileInProcessor
    {
        public XSystemDictionary Smalltalk { get; private set; }

        private List<KeyValuePair<XBinding, object>> Initializers = new List<KeyValuePair<XBinding,object>>();

        public XFileInProcessor(XSystemDictionary smalltalk)
        {
            if (smalltalk == null)
                throw new ArgumentNullException();
            this.Smalltalk = smalltalk;
        }

        public bool FileInClass(Installer.Definitions.ClassDefinition definition)
        {
            XBinding binding = this.Smalltalk.GetOrCreateBinding(definition.Name.Value);
            if (binding.IsBound)
                throw new InvalidOperationException(String.Format("Global {0} already defined!", definition.Name.Value));
            binding.Value = new XClass(definition, this.Smalltalk);
            binding.MakeConstant();

            return true;
        }

        public bool FileInGlobal(Installer.Definitions.GlobalDefinition definition)
        {
            XBinding binding = this.Smalltalk.GetOrCreateBinding(definition.Name.Value);
            if (binding.IsBound)
                throw new InvalidOperationException(String.Format("Global {0} already defined!", definition.Name.Value));
            binding.Value = null; // Bind it
            if (definition is Installer.Definitions.GlobalConstantDefinition)
                binding.MakeConstant();
            else
                binding.MakeVariable();

            return true;
        }

        public bool FileInGlobalInitializer(Installer.Definitions.GlobalInitializer initializer)
        {
            this.Initializers.Add(new KeyValuePair<XBinding, object>(
                this.Smalltalk.GetBinding(initializer.GlobalName.Value),
                this.CreateEvaluatableObject(initializer.ParseTree)));

            return true;
        }

        public bool FileInMethod(Installer.Definitions.MethodDefinition definition)
        {
            XClass cls = (XClass) this.Smalltalk[definition.ClassName.Value]; // At this time, it should be defined
            if (definition is Installer.Definitions.InstanceMethodDefinition)
                cls.InstanceMethods.Add(definition.ParseTree.Selector, (Installer.Definitions.InstanceMethodDefinition)definition);
            else 
                cls.ClassMethods.Add(definition.ParseTree.Selector, (Installer.Definitions.ClassMethodDefinition)definition);

            return true;
        }

        public bool FileInPool(Installer.Definitions.PoolDefinition definition)
        {
            XBinding binding = this.Smalltalk.GetOrCreateBinding(definition.Name.Value);
            if (binding.IsBound)
                throw new InvalidOperationException(String.Format("Global {0} already defined!", definition.Name.Value));
            binding.Value = new XPool(definition, this.Smalltalk);
            binding.MakeConstant();

            return true;            
        }

        public bool FileInPoolVariable(Installer.Definitions.PoolValueDefinition definition)
        {
            XPool pool = (XPool) this.Smalltalk[definition.PoolName.Value]; // At this time, it should be defined
            pool.CreateValue(definition);

            return true;
        }

        public bool FileInPoolVariableInitializer(Installer.Definitions.PoolVariableInitializer initializer)
        {
            this.Initializers.Add(new KeyValuePair<XBinding, object>(
                ((XPool)this.Smalltalk[initializer.PoolName.Value]).GetBinding(initializer.VariableName.Value),
                this.CreateEvaluatableObject(initializer.ParseTree)));

            return true;
        }

        public bool FileInProgramInitializer(Installer.Definitions.ProgramInitializer initializer)
        {
            XBinding binding = new XBinding("Program Initializer"); // The key is not really used
            binding.MakeVariable();
            this.Initializers.Add(new KeyValuePair<XBinding, object>(binding, this.CreateEvaluatableObject(initializer.ParseTree)));

            return true;
        }

        private object CreateEvaluatableObject(Compiler.SemanticNodes.InitializerNode parseTree)
        {
            return parseTree;
        }
    }
}
