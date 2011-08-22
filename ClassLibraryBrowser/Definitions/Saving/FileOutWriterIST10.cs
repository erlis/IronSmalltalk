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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;
using System.IO;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Saving
{
    public class FileOutWriterIST10 : FileOutWriterSIF10
    {
        public new static void FileOut(SystemImplementation systemImplementation, TextWriter writer)
        {
            FileOutWriterIST10 fileout = new FileOutWriterIST10(systemImplementation, writer);
            fileout.EmitInterchangeFile();
        }

        protected FileOutWriterIST10(SystemImplementation systemImplementation, TextWriter writer)
            : base(systemImplementation, writer)
        {
        }

        protected override string InterchangeVersionId
        {
            get { return "IronSmalltalk 1.0"; }
        }

        protected override string GetInstanceState(Implementation.Class cls)
        {
            if (cls.InstanceState == Implementation.InstanceStateEnum.Native)
                return "native";
            return base.GetInstanceState(cls);
        }
    }
}
