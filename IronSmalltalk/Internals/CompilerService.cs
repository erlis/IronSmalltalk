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
using System.Linq;
using IronSmalltalk.Interchange;
using IronSmalltalk.Runtime;
using System.Collections.Generic;
using System.IO;
using IronSmalltalk.Runtime.Installer;


namespace IronSmalltalk.Internals
{
    /// <summary>
    /// Service for compiling and installing smalltalk code into the smalltalk environment / context.
    /// </summary>
    public class CompilerService
    {
        public Dictionary<string, InterchangeVersionService> VersionServicesMap { get; private set; }
        public SmalltalkRuntime Runtime { get; private set; }

        /// <summary>
        /// Determines if meta-annotations (comments, documentation, etc.) 
        /// are installed (saved) in the corresponding runtime objects.
        /// </summary>
        public bool InstallMetaAnnotations { get; set; }

        public CompilerService(SmalltalkRuntime runtime)
        {
            if (runtime == null)
                throw new ArgumentNullException("runtime");

            this.Runtime = runtime;
            this.VersionServicesMap = new Dictionary<string, InterchangeVersionService>();
            this.VersionServicesMap.Add("1.0", new InterchangeVersionService10());
            this.VersionServicesMap.Add("IronSmalltalk 1.0", new InterchangeVersionServiceIST10());
#if DEBUG
            this.InstallMetaAnnotations = true;
#else
            this.InstallMetaAnnotations = false;
#endif
        }

        public void InstallFile(string path, IInterchangeErrorSink parseErrorSink, IInstallErrorSink installErrorSink)
        {
            this.InstallFiles(new string[] { path }, parseErrorSink, installErrorSink);
        }

        public void InstallFiles(string[] paths, IInterchangeErrorSink parseErrorSink, IInstallErrorSink installErrorSink)
        {
            this.CompleteInstall(this.ReadFiles(paths, parseErrorSink), installErrorSink);
        }

        public void InstallSource(string interchangeCode, IInterchangeErrorSink parseErrorSink, IInstallErrorSink installErrorSink)
        {
            this.InstallSources(new string[] { interchangeCode }, parseErrorSink, installErrorSink);
        }

        public void InstallSources(string[] interchangeCode, IInterchangeErrorSink parseErrorSink, IInstallErrorSink installErrorSink)
        {
            this.CompleteInstall(this.ReadSources(interchangeCode, parseErrorSink), installErrorSink);
        }

        public InterchangeInstallerContext ReadFile(string path, IInterchangeErrorSink errorSink)
        {
            return this.ReadFiles(new string[] { path }, errorSink);
        }

        public InterchangeInstallerContext ReadFiles(string[] paths, IInterchangeErrorSink errorSink)
        {
            if (paths == null)
                throw new ArgumentNullException("paths");

            InterchangeInstallerContext installer = this.CreateInstallerContext();
            
            foreach (string path in paths)
            {
                // Issue - expects UTF-8 encoding
                using (StreamReader sourceFileReader = File.OpenText(path))
                {
                    InterchangeFormatProcessor processor = new InterchangeFormatProcessor(sourceFileReader, installer, this.VersionServicesMap);
                    processor.ErrorSink = errorSink;
                    processor.ProcessInterchangeFile();
                }
            }

            return installer;
        }

        public InterchangeInstallerContext ReadSource(string interchangeCode, IInterchangeErrorSink errorSink)
        {
            return this.ReadSources(new string[] { interchangeCode }, errorSink);
        }

        public InterchangeInstallerContext ReadSources(string[] interchangeCode, IInterchangeErrorSink errorSink)
        {
            if (interchangeCode == null)
                throw new ArgumentNullException("interchangeCode");

            InterchangeInstallerContext installer = this.CreateInstallerContext();

            foreach (string code in interchangeCode)
            {
                InterchangeFormatProcessor processor = new InterchangeFormatProcessor(new StringReader(code), installer, this.VersionServicesMap);
                processor.ErrorSink = errorSink;
                processor.ProcessInterchangeFile();
            }

            return installer;
        }

        protected virtual InterchangeInstallerContext CreateInstallerContext()
        {
            return new InterchangeInstallerContext(this.Runtime);
        }

        private void CompleteInstall(InterchangeInstallerContext installer, IInstallErrorSink errorSink)
        {
            installer.ErrorSink = errorSink;
            installer.InstallMetaAnnotations = this.InstallMetaAnnotations;
            if (installer.Install())
                installer.Initialize();
        }
    }
}
