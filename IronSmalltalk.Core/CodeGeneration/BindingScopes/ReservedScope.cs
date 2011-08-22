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
using System.Linq.Expressions;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;

namespace IronSmalltalk.Runtime.CodeGeneration.BindingScopes
{
    public static class ReservedScope
    {
        private static BindingScope CreateBindings()
        {
            return new BindingScope(new NameBinding[] {
                new ConstantBinding(SemanticConstants.Nil, null, typeof(object)),
                new ConstantBinding(SemanticConstants.True, true, typeof(object)),
                new ConstantBinding(SemanticConstants.False, false, typeof(object))
            });
        }


        public static BindingScope ForPoolInitializer()
        {
            return ReservedScope.ForGlobalInitializer(); // Same as for globals
        }

        public static BindingScope ForProgramInitializer()
        {
            return ReservedScope.ForGlobalInitializer(); // Same as for globals
        }

        public static BindingScope ForGlobalInitializer()
        {
            BindingScope result = ReservedScope.CreateBindings();
            result.DefineBinding(new ErrorBinding(SemanticConstants.Self));    // error binding unless <<class initializer>>. 
            result.DefineBinding(new ErrorBinding(SemanticConstants.Super));   // Within any type of initializer super has the error binding.
            return result;
        }

        public static BindingScope ForClassInitializer()
        {
            BindingScope result = ReservedScope.CreateBindings();
            result.DefineBinding(new ArgumentBinding(SemanticConstants.Self));
            result.DefineBinding(new ErrorBinding(SemanticConstants.Super));   // Within any type of initializer super has the error binding.
            return result;
        }

        public static BindingScope ForInstanceMethod(Expression selfBinding)
        {
            BindingScope result = ReservedScope.CreateBindings();
            result.DefineBinding(new ArgumentBinding(SemanticConstants.Self, selfBinding));
            result.DefineBinding(new ArgumentBinding(SemanticConstants.Super, selfBinding));
            return result;
        }

        public static BindingScope ForClassMethod(Expression selfBinding)
        {
            BindingScope result = ReservedScope.CreateBindings();
            result.DefineBinding(new ArgumentBinding(SemanticConstants.Self, selfBinding));
            result.DefineBinding(new ArgumentBinding(SemanticConstants.Super, selfBinding));
            return result;
        }

        public static BindingScope ForRootClassInstanceMethod(Expression selfBinding)
        {
            BindingScope result = ReservedScope.CreateBindings();
            result.DefineBinding(new ArgumentBinding(SemanticConstants.Self, selfBinding));
            result.DefineBinding(new ErrorBinding(SemanticConstants.Super));   // Erroneous if instance method and no superclass.
            return result;
        }
    }
}
