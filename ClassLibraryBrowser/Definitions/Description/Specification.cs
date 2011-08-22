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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Description
{
    public abstract class Specification
    {
        public Message Message { get; private set; }


        public IList<string> Protocols { get; private set; }

        protected Specification(Message parent)
        {
            if (parent == null)
                throw new ArgumentNullException();
            this.Message = parent;
            this.Protocols = new List<string>();
        }
    }
}
