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

namespace IronSmalltalk.Runtime.Experimental
{
    public class XPool : XBindingScope
    {
        public readonly string Name;

        public XPool(PoolDefinition definition, XSystemDictionary smalltalk)
        {
            if ((definition == null) || (smalltalk == null))
                throw new ArgumentNullException();

            this.Name = definition.Name.Value;
        }

        internal void CreateValue(PoolValueDefinition definition)
        {
            XBinding binding = this.GetOrCreateBinding(definition.VariableName.Value);
            if (binding.IsBound)
                throw new InvalidOperationException(String.Format("Pool value {0} already defined!", definition.VariableName.Value));
            binding.Value = null; // Bind it
            if (definition is PoolConstantDefinition)
                binding.MakeConstant();
            else
                binding.MakeVariable();
        }
    }
}
