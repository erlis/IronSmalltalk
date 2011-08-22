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

namespace IronSmalltalk.Runtime.Installer
{
    public static class InstallerErrors
    {
        public const string ClassInvalidName = "Invalid class name.";
        public const string ClassReservedName = "The name is a reserved identifier and cannot be used as class name.";
        public const string ClassNameNotUnique = "A global already exists with the given name.";
        public const string ClassNameProtected = "The class name is protected global name and cannot be shadowed.";
        public const string ClassInvalidSuperclass = "Cannot find difinition of superclass.";
        public const string ClassClassVariableNotIdentifier = "Invalid class variable name. Must be an identifier.";
        public const string ClassClassVariableNotUnique = "Duplicate class variable name. Names must be unique within class variables.";
        public const string ClassInstanceVariableNotIdentifier = "Invalid instance variable name. Must be an identifier.";
        public const string ClassInstanceVariableNotUnique = "Duplicate instance variable name. Names must be unique within instance variables.";
        public const string ClassInstanceOrClassVariableNotUnique = "Duplicate instance variable name. Names must be unique within instance and class variables.";
        public const string ClassClassInstanceVariableNotIdentifier = "Invalid class instance variable name. Must be an identifier.";
        public const string ClassClassInstanceVariableNotUnique = "Duplicate class instance variable name. Names must be unique within class instance variables.";
        public const string ClassClassInstanceOrClassVariableNotUnique = "Duplicate class instance variable name. Names must be unique within class instance and class variables.";
        public const string ClassImportedPoolNotIdentifier = "Invalid pool name. Must be an identifier.";
        public const string ClassImportedPoolNotUnique = "Duplicate pool name. Imported pool names must be unique.";
        public const string ClassImportedPoolNotDefined = "Cannot find global pool.";
        public const string ClassCircularReference = "Superclass named '{0}' cannot be direct or indirect subclass of '{1}'.";
        public const string ClassCannotChangeInstanceState = "Cannot inherit from a class with ByteIndexable instance state and change the behavior to non-ByteIndexable.";
        public const string ClassMissingPoolDefinition = "Cannot resolve global pool.";

        public const string GlobalInvalidName = "Invalid global name.";
        public const string GlobalNameNotUnique = "A global already exists with the given name.";
        public const string GlobalNameProtected = "The global name is protected global name and cannot be shadowed.";
        public const string GlobalReservedName = "The global name is a reserved identifier.";
        public const string GlobalIsConstant = "The global constant has already been set and value cannot be changed.";

        public const string PoolInvalidName = "Invalid pool name.";
        public const string PoolNameNotUnique = "A global already exists with the given name.";
        public const string PoolNameProtected = "The pool name is protected global name and cannot be shadowed.";
        public const string PoolReservedName = "The pool name is a reserved identifier.";

        public const string PoolConstInvalidName = "Invalid pool constant name.";
        public const string PoolVarInvalidName = "Invalid pool variable name.";
        public const string PoolInvalidPoolName = "Invalid pool name.";
        public const string PoolConstReservedName = "The pool constant name is a reserved identifier.";
        public const string PoolItemNameNotUnique = "A pool variable or constant with the given name already exists in the pool.";
        public const string PoolVarReservedName = "The pool variable name is a reserved identifier.";
        public const string PoolItemIsConstant = "The pool constant has already been set and value cannot be changed.";

        public const string MethodInvalidSelector = "Invalid method selector.";
        public const string MethodInvalidClassName = "Invalid class name.";

    }
}
