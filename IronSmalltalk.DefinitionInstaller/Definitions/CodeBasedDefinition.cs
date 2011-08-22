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
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Common;

namespace IronSmalltalk.Runtime.Installer.Definitions
{
    public abstract class CodeBasedDefinition<TCode> : DefinitionBase
    {
        /// <summary>
        /// Intermediate code that can be compiled into executable code.
        /// </summary>
        public TCode IntermediateCode { get; private set; }

        /// <summary>
        /// Source code service for translating source code positions.
        /// </summary>
        public ISourceCodeReferenceService SourceCodeService { get; private set; }

        /// <summary>
        /// Source code service for translating source code positions in the method code.
        /// </summary>
        public ISourceCodeReferenceService MethodSourceCodeService { get; private set; }

        public CodeBasedDefinition(ISourceCodeReferenceService sourceCodeService, ISourceCodeReferenceService methodSourceCodeService, TCode code)
        {
            if (sourceCodeService == null)
                throw new ArgumentNullException("sourceCodeService");
            if (methodSourceCodeService == null)
                throw new ArgumentNullException("methodSourceCodeService");
            if (code == null)
                throw new ArgumentNullException("code");
            this.SourceCodeService = sourceCodeService;
            this.MethodSourceCodeService = methodSourceCodeService;
            this.IntermediateCode = code;
        }

        protected class IntermediateCodeValidationErrorSink : IIntermediateCodeValidationErrorSink
        {
            private ISourceCodeReferenceService SourceCodeService;
            private IInstallerContext Installer;

            public IntermediateCodeValidationErrorSink(ISourceCodeReferenceService sourceCodeService, IInstallerContext installer)
            {
                if (installer == null)
                    throw new ArgumentNullException();
                if (sourceCodeService == null)
                    throw new ArgumentNullException();
                this.Installer = installer;
                this.SourceCodeService = sourceCodeService;
            }

            public void ReportError(string errorMessage, SourceLocation start, SourceLocation stop)
            {
                this.Installer.ReportError(
                    new SourceReference(start, stop, this.SourceCodeService),
                    errorMessage);
            }
        }
    }
}
