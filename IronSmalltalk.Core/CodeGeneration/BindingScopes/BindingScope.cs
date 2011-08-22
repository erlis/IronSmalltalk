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
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime;
using IronSmalltalk.Runtime.Bindings;

namespace IronSmalltalk.Runtime.CodeGeneration.BindingScopes
{
    /************* Name Resolution Scopes *************
    This is defined in X3J20: 3.3.1.1, 3.3.2 (especially in 3.3.2.3), 3.4.1
    The syntax here is listed just for convinience. X3J20 is authoritative!
      
    *** GLOBAL ***
    global_scope                : Everything global in Smalltalk, e.g. Smalltalk at: ...
    extansion_scope             : Everything global defined by IronSmalltalk
    global_definition_scope     : Everything global defined by the user program
      
    global_scope := extansion_scope + global_definition_scope       // X3J20:3.3.1.1
      
      
    *** CLASS ***
    instance_variable_names         : All instance variables defined in the class 
    class_variable_names            : All class variables defined in the class
    class_instance_variable_names   : All class instance variables defined in the class
    imported_pool_names             : All pools imported by the class

    (class_variable_names & instance_variable_names) = Ø           // No duplicates X3J20:3.3.2.1
    (class_variable_names & class_instance_variable_names) = Ø     // No duplicates X3J20:3.3.2.2

    inheritable_class_variable_scope := superclass:inheritable_class_variable_scope 
                                        + class:class_variable_names               // X3J20:3.3.2.2
                                           
    error_pool_variable_scope := imported_pool_1:pool_variables                        // pool variables defined in more pools
                                        & imported_pool_2:pool_variables & ... 
                                        & imported_pool_n:pool_variables               // X3J20:3.3.2.2
      
    pool_variable_scope := (imported_pool_1:pool_variables                             // Usable pool variables (excl. duplicates)
                            + imported_pool_2:pool_variables + ... 
                            + imported_pool_n:pool_variables) 
                            - error_pool_variable_scope                                // X3J20:3.3.2.2
    
    instance_variable_scope := superclass:instance_variable_scope                      // Not clearly defined in X3J20 (3.3.2.1), but
                                        + class:instance_variable_names                // IST allows shadowing of a superclass' variables
    
    class_instance_variable_scope := superclass:class_instance_variable_scope          // Not clearly defined in X3J20 (3.3.2.1), but
                                        + class:class_instance_variable_names          // IST allows shadowing of a superclass' variables  
  
    *** BEHAVIOR  ***   
    class_scope := (global_scope + pool_variable_scope) + inheritable_class_variable_scope      // X3J20:3.3.2.3
    instance_function_scope := class_scope + instance_variable_scope                            // X3J20:3.3.2.3
    class_funtion_scope := class_scope + class_instance_variable_scope                          // X3J20:3.3.2.3
    
    
    *** FUNCTIONS ***
    temporary_variable_names        : Temporaries defined in the function (mehtod, initializer or block)
    argument_names                  : Arguments passed to the function (method or block)
    statement_scope                 : Name binding for the function (depends on each type of function ... see below)
    home_context_scope              : The scope of the home function, depending on the type of function 
                                           (this is our own nomenclature based on X3J20 definitions and descriptions)
    reserved_scope                  : The reserved keywords (nil, true, false, self, super) ... self and/or super may be error bindings
                                           (this is our own nomenclature based on X3J20 definitions and descriptions)     
    
    (temporary_variable_names & argument_names) = Ø             // No duplicates 
    local_scope := temporary_variable_names + argument_names    // X3J20:3.4.1
    statement_scope := home_context_scope + local_scope

    *** INSTANCE METHOD ***
    statement_scope := instance_function_scope + local_scope + reserved_scope    // X3J20:3.4.2
    home_context_scope := instance_function_scope

    *** CLASS METHOD ***
    statement_scope := class_funtion_scope + local_scope + reserved_scope       // X3J20:3.4.2
    home_context_scope := class_funtion_scope
    
    *** INITIALIZER ***
    statement_scope := global_scope + local_scope + reserved_scope              // Globals and Program Initializers     // X3J20:3.4.3
    home_context_scope := global_scope
    statement_scope := class_funtion_scope + local_scope + reserved_scope       // Class Initializer                    // X3J20:3.4.3
    home_context_scope := class_funtion_scope
    statement_scope := pool_scope + local_scope + reserved_scope                // Pool Dictionaries                    // X3J20:3.4.3
    home_context_scope := pool_scope
    
    pool_scope := global_scope + (pool_var_1 + pool_var_2 + ... + pool_var_n)
    
    *** BLOCK ***
    statement_scope := enclosing_function:statement_scope + statement_scope + reserved_scope     // X3J20:3.4.4
    home_context_scope := N/A .... this is used only for (root) functions
    */

    public class BindingScope
    {
        private readonly Dictionary<string, NameBinding> Bindings = new Dictionary<string, NameBinding>();

        public BindingScope()
        {
        }

        public BindingScope(IEnumerable<NameBinding> bindings)
            : this()
        {
            if (bindings != null)
            {
                foreach (NameBinding binding in bindings)
                    this.DefineBinding(binding);
            }
        }

        public virtual void DefineBinding(NameBinding binding)
        {
            if (binding == null)
                throw new ArgumentNullException();

            this.Bindings[binding.Name] = binding;
        }

        public virtual NameBinding GetBinding(string name)
        {
            NameBinding result;
            if (this.Bindings.TryGetValue(name, out result))
                return result;
            return null;
        }

        #region Factory Helpers

        public static BindingScope ForInstanceMethod(SmalltalkClass cls)
        {
            return BindingScope.ForInstanceMethod(cls, cls.Runtime.GlobalScope);
        }

        public static BindingScope ForInstanceMethod(SmalltalkClass cls, SmalltalkNameScope globalNameScope)
        {
            if (cls == null)
                throw new ArgumentNullException("cls");
            if (globalNameScope == null)
                throw new ArgumentNullException("globalNameScope");
            //class_scope := (global_scope + pool_variable_scope) + inheritable_class_variable_scope      // X3J20:3.3.2.3
            //instance_function_scope := class_scope + instance_variable_scope                            // X3J20:3.3.2.3
            return new InstanceFunctionScope(cls,
                new ClassScope(cls,
                    new PoolVariableScope(cls,
                        new GlobalScope(globalNameScope))));
        }

        public static BindingScope ForClassMethod(SmalltalkClass cls)
        {
            return BindingScope.ForClassMethod(cls, cls.Runtime.GlobalScope);
        }

        public static BindingScope ForClassMethod(SmalltalkClass cls, SmalltalkNameScope globalNameScope)
        {
            if (cls == null)
                throw new ArgumentNullException("cls");
            if (globalNameScope == null)
                throw new ArgumentNullException("globalNameScope");
            //class_scope := (global_scope + pool_variable_scope) + inheritable_class_variable_scope      // X3J20:3.3.2.3
            //class_funtion_scope := class_scope + class_instance_variable_scope                          // X3J20:3.3.2.3
            return new ClassFunctionScope(cls,
                new ClassScope(cls,
                    new PoolVariableScope(cls,
                        new GlobalScope(globalNameScope))));
        }

        public static BindingScope ForClassInitializer(SmalltalkClass cls)
        {
            return BindingScope.ForClassInitializer(cls, cls.Runtime.GlobalScope);
        }

        public static BindingScope ForClassInitializer(SmalltalkClass cls, SmalltalkNameScope globalNameScope)
        {
            if (cls == null)
                throw new ArgumentNullException("cls");
            if (globalNameScope == null)
                throw new ArgumentNullException("globalNameScope");
            //statement_scope := class_funtion_scope + local_scope + reserved_scope       // Class Initializer - X3J20:3.4.3
            //home_context_scope := class_funtion_scope
            return new ClassFunctionScope(cls,
                new ClassScope(cls,
                    new PoolVariableScope(cls,
                        new GlobalScope(globalNameScope))));
        }

        public static BindingScope ForGlobalInitializer(SmalltalkRuntime runtime)
        {
            return BindingScope.ForGlobalInitializer(runtime.GlobalScope);
        }

        public static BindingScope ForGlobalInitializer(SmalltalkNameScope globalNameScope)
        {
            if (globalNameScope == null)
                throw new ArgumentNullException("globalNameScope");
            return new GlobalScope(globalNameScope);
        }

        public static BindingScope ForProgramInitializer(SmalltalkRuntime runtime)
        {
            return BindingScope.ForProgramInitializer(runtime.GlobalScope);
        }

        public static BindingScope ForProgramInitializer(SmalltalkNameScope globalNameScope)
        {
            return BindingScope.ForGlobalInitializer(globalNameScope); // Same as global initializer
        }

        public static BindingScope ForPoolInitializer(Pool pool)
        {
            return BindingScope.ForPoolInitializer(pool, pool.Runtime.GlobalScope);
        }

        public static BindingScope ForPoolInitializer(Pool pool, SmalltalkNameScope nameScope)
        {
            return new PoolScope(pool, new GlobalScope(nameScope));
        }

        #endregion
    }
}
