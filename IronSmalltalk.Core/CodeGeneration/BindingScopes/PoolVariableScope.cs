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
using RTB = IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Execution.Internals;

namespace IronSmalltalk.Runtime.CodeGeneration.BindingScopes
{
    /*
    See full description in BindingScope.cs     
      
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
  
    *** BEHAVIOR  ***   
    class_scope := (global_scope + pool_variable_scope) + inheritable_class_variable_scope      // X3J20:3.3.2.3
     */
    public class PoolVariableScope : ClassRelatedBindingScope
    {
        public PoolVariableScope(SmalltalkClass cls, GlobalScope outerScope)
            : base(cls, outerScope)
        {
        }

        protected override NameBinding ResolveBinding(string name)
        {
            NameBinding result = null;
            foreach (var poolBinding in this.Class.ImportedPoolBindings)
            {
                if (poolBinding.Value != null)
                {
                    RTB.PoolVariableOrConstantBinding binding;
                    poolBinding.Value.TryGetValue(name, out binding);
                    if (binding != null)
                    {
                        if (result != null)
                            return new ErrorBinding(name, RuntimeCodeGenerationErrors.PoolVariableNotUnique);
                        if (binding is RTB.PoolConstantBinding)
                            result = new PoolConstantBinding(name, (RTB.PoolConstantBinding) binding);
                        else 
                            result = new PoolVariableBinding(name, (RTB.PoolVariableBinding) binding);
                    }
                }
            }
            return result; // null means try outer scope.
        }
    }
}
