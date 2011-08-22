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
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Interchange;

namespace IronSmalltalk.Interchange
{
    /// <summary>
    /// Installer context that handles the concurrent installation of a batch of sources.
    /// </summary>
    public class InterchangeInstallerContext : InstallerContext, IInterchangeFileInProcessor
    {
        public InterchangeInstallerContext(IronSmalltalk.SmalltalkRuntime runtime)
            : base(runtime)
        {
        }

        bool IInterchangeFileInProcessor.FileInClass(Runtime.Installer.Definitions.ClassDefinition definition)
        {
            this.AddClass(definition);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInGlobal(Runtime.Installer.Definitions.GlobalDefinition definition)
        {
            this.AddGlobal(definition);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInGlobalInitializer(Runtime.Installer.Definitions.GlobalInitializer initializer)
        {
            this.AddGlobalInitializer(initializer);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInMethod(Runtime.Installer.Definitions.MethodDefinition definition)
        {
            this.AddMethod(definition);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInPool(Runtime.Installer.Definitions.PoolDefinition definition)
        {
            this.AddPool(definition);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInPoolVariable(Runtime.Installer.Definitions.PoolValueDefinition definition)
        {
            this.AddPoolVariable(definition);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInPoolVariableInitializer(Runtime.Installer.Definitions.PoolVariableInitializer initializer)
        {
            this.AddPoolVariableInitializer(initializer);
            return true;
        }

        bool IInterchangeFileInProcessor.FileInProgramInitializer(Runtime.Installer.Definitions.ProgramInitializer initializer)
        {
            this.AddProgramInitializer(initializer);
            return true;
        }
    }
}
