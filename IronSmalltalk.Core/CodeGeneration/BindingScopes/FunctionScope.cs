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
using RTB = IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime;

namespace IronSmalltalk.Runtime.CodeGeneration.BindingScopes
{
    /************* Name Resolution Scopes *************
    See full description in BindingScope.cs     

    *** CLASS ***
    instance_variable_names         : All instance variables defined in the class 
    class_instance_variable_names   : All class instance variables defined in the class
    
    instance_variable_scope := superclass:instance_variable_scope                      // Not clearly defined in X3J20 (3.3.2.1), but
                                        + class:instance_variable_names                // IST allows shadowing of a superclass' variables
    
    class_instance_variable_scope := superclass:class_instance_variable_scope          // Not clearly defined in X3J20 (3.3.2.1), but
                                        + class:class_instance_variable_names          // IST allows shadowing of a superclass' variables  
  
    *** BEHAVIOR  ***   
    instance_function_scope := class_scope + instance_variable_scope                            // X3J20:3.3.2.3
    class_funtion_scope := class_scope + class_instance_variable_scope                          // X3J20:3.3.2.3
    
    */

    public abstract class FunctionScope : ClassRelatedBindingScope
    {
        public FunctionScope(SmalltalkClass cls, ClassScope outerScope)
            : base(cls, outerScope)
        {
        }
    }

    public class InstanceFunctionScope : FunctionScope
    {
        public InstanceFunctionScope(SmalltalkClass cls, ClassScope outerScope)
            : base(cls, outerScope)
        {
        }

        protected override NameBinding ResolveBinding(string name)
        {
            // inheritable_class_variable_scope
            SmalltalkClass cls = this.Class;
            RTB.InstanceVariableBinding binding;
            while (cls != null)
            {
                cls.InstanceVariableBindings.TryGetValue(name, out binding);
                if (binding != null)
                    return new InstanceVariableBinding(name, binding.Value);
                cls = cls.Superclass;
            }
            return null; // null means try outer scope.
        }
    }

    public class ClassFunctionScope : FunctionScope
    {
        public ClassFunctionScope(SmalltalkClass cls, ClassScope outerScope)
            : base(cls, outerScope)
        {
        }

        protected override NameBinding ResolveBinding(string name)
        {
            // inheritable_class_variable_scope
            SmalltalkClass cls = this.Class;
            RTB.ClassInstanceVariableBinding binding;
            while (cls != null)
            {
                cls.ClassInstanceVariableBindings.TryGetValue(name, out binding);
                if (binding != null)
                    return new ClassInstanceVariableBinding(name, binding.Value);
                cls = cls.Superclass;
            }
            return null; // null means try outer scope.
        }
    }
}
