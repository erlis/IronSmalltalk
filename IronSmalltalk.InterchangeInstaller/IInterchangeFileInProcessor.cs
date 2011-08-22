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
using IronSmalltalk.Runtime.Installer.Definitions;

namespace IronSmalltalk.Interchange
{
    /// <summary>
    /// Interface describing an object that will be notified for each element found in the source code.
    /// It is up to the IInterchangeFileInProcessor to do something usefull with the given meta objects 
    /// describing each filed-in element.
    /// </summary>
    public interface IInterchangeFileInProcessor
    {
        /// <summary>
        /// Process and file-in a class definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the class.</param>
        /// <returns></returns>
        bool FileInClass(ClassDefinition definition);

        /// <summary>
        /// Process and file-in a global (variable or constant) definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the global.</param>
        /// <returns></returns>
        bool FileInGlobal(GlobalDefinition definition);

        /// <summary>
        /// Process and file-in a global initializer definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the initializer.</param>
        /// <returns></returns>
        bool FileInGlobalInitializer(GlobalInitializer initializer);

        /// <summary>
        /// Process and file-in a (class or instance) method definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the method.</param>
        /// <returns></returns>
        bool FileInMethod(MethodDefinition definition);

        /// <summary>
        /// Process and file-in a shared pool definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the pool.</param>
        /// <returns></returns>
        bool FileInPool(PoolDefinition definition);

        /// <summary>
        /// Process and file-in a pool variable or pool constant definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the pool variable or pool constant.</param>
        /// <returns></returns>
        bool FileInPoolVariable(PoolValueDefinition definition);

        /// <summary>
        /// Process and file-in a pool variable or pool constant initializer definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the initializer.</param>
        /// <returns></returns>
        bool FileInPoolVariableInitializer(PoolVariableInitializer initializer);

        /// <summary>
        /// Process and file-in a program initializer definition.
        /// </summary>
        /// <param name="definition">Meta-object describing the definition of the initializer.</param>
        /// <returns></returns>
        bool FileInProgramInitializer(ProgramInitializer initializer);
    }
}
