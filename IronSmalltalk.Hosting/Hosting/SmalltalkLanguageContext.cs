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
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting;
using System.Collections.Generic;
using IronSmalltalk.Compiler;

namespace IronSmalltalk.Runtime.Hosting
{
    /// <summary>
    /// Iron Smalltalk Language Context. This is the entry point from the DLR into IST.
    /// </summary>
    /// <remarks>
    /// The IronSmalltalk LanguageContext is the representation of the language and the
    /// workhorse at the language implementation level for supporting the DLR
    /// Hosting APIs. It has many members on it, but we only have to override
    /// a couple to get basic DLR hosting support enabled.
    ///
    /// Other things a LanguageContext might do are provide an implementation for
    /// ObjectOperations, offer other services (exception formatting, colorization,
    /// tokenization, etc), provide ExecuteProgram semantics, and so on.
    /// </remarks>
    public class SmalltalkLanguageContext : LanguageContext, 
        SmalltalkScriptCode<SmalltalkEnvironment>.IRuntimeLanguageContext
    {
        public readonly SmalltalkEnvironment SmalltalkEnvironment;

        public SmalltalkLanguageContext(ScriptDomainManager domainManager)
            : base(domainManager)
        {
            this.SmalltalkEnvironment = new SmalltalkEnvironment();
        }

        public SmalltalkLanguageContext(ScriptDomainManager domainManager, IDictionary<string, object> options)
            : this(domainManager)
        {
        }


        public override ScriptCode CompileSourceCode(SourceUnit sourceUnit, CompilerOptions options, ErrorSink errorSink)
        {
            if (sourceUnit == null)
                throw new ArgumentNullException("sourceUnit");
            if (options == null)
                throw new ArgumentNullException("options");
            if (errorSink == null)
                throw new ArgumentNullException("errorSink");
            if (sourceUnit.LanguageContext != this)
                throw new ArgumentException("Language context mismatch");


            //SmalltalkEnvironment env = new SmalltalkEnvironment();
            //Experimental.XSystemDictionary smalltalk = new Experimental.XSystemDictionary();
            //Experimental.XFileInProcessor fileInProcessor = new Experimental.XFileInProcessor(smalltalk);

            //using (var reader = sourceUnit.GetReader())
            //{

            //    Compiler.Interchange.InterchangeFormatProcessor processor =
            //        new Compiler.Interchange.InterchangeFormatProcessor(reader, fileInProcessor, env.CompilerService.VersionServicesMap);
            //    processor.ErrorSink = new ErrorSinkWrapper(sourceUnit, errorSink);
            //    processor.ProcessInterchangeFile();
            //}

            var envParam = System.Linq.Expressions.Expression.Parameter(typeof(SmalltalkEnvironment), "Environment");
            var selfParam = System.Linq.Expressions.Expression.Parameter(typeof(object), "self");

            var expr = System.Linq.Expressions.Expression.Constant("Hello World");

            var code = System.Linq.Expressions.Expression.Lambda<Func<SmalltalkEnvironment, object, object>>(
                expr, envParam, selfParam);

            return new SmalltalkScriptCode<SmalltalkEnvironment>(code, this, sourceUnit);
        }

        #region IRuntimeLanguageContext

        SmalltalkEnvironment SmalltalkScriptCode<SmalltalkEnvironment>.IRuntimeLanguageContext.Environment
        {
            get { return this.SmalltalkEnvironment; }
        }

        SmalltalkLanguageContext SmalltalkScriptCode<SmalltalkEnvironment>.IRuntimeLanguageContext.LanguageContext
        {
            get { return this; }
        }

        Func<SmalltalkEnvironment, object, object> SmalltalkScriptCode<SmalltalkEnvironment>.IRuntimeLanguageContext.Compile(System.Linq.Expressions.Expression<Func<SmalltalkEnvironment, object, object>> code)
        {
            if (code == null)
                throw new ArgumentNullException();
            return code.Compile();
        }

        #endregion
    }
}
