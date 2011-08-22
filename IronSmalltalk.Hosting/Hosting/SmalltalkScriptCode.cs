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
using System.Linq.Expressions;
using System.Text;
using Microsoft.Scripting;
using System.Threading;

namespace IronSmalltalk.Runtime.Hosting
{   
    /// <summary>
    /// SmalltalkScriptCode is an instance of Smalltalk compiled code that is bound to a specific 
    /// SmalltalkLanguageContext but not a specific ScriptScope. The code can be re-executed multiple 
    /// times in different scopes. Hosting API counterpart for this class is <c>CompiledCode</c>.
    /// </summary>
    /// <typeparam name="TEnvironment">Type of the SmalltalkEnvironment passed to the target delegate.</typeparam>
    public class SmalltalkScriptCode<TEnvironment> : ScriptCode
        where TEnvironment : class
    {
        /// <summary>
        /// Helper interface that wraps the LanguageContext that this source is bound to.
        /// </summary>
        /// <typeparam name="TEnvironment">Type of the SmalltalkEnvironment passed to the target delegate.</typeparam>
        /// <remarks>
        /// The reason we have this is because we would like to avoid hardcoding the SmalltalkEnvironment and 
        /// SmalltalkLanguageContext into this class. We would like to use generics in case we refactor this
        /// in the future and want somebody else to use/reuse this class.
        /// </remarks>
        public interface IRuntimeLanguageContext
        {
            TEnvironment Environment { get; }
            SmalltalkLanguageContext LanguageContext { get; }
            Func<TEnvironment, object, object> Compile(Expression<Func<TEnvironment, object, object>> code);
        }

        /// <summary>
        /// Create a new SmalltalkScriptCode.
        /// </summary>
        /// <param name="code">Expression/Expression Tree code representing the code to be execution.</param>
        /// <param name="context">Wrapper to the LanguageContext that this source is bound to.</param>
        /// <param name="sourceUnit">SourceUnit that resulted in the generated code.</param>
        public SmalltalkScriptCode(Expression<Func<TEnvironment, object, object>> code, IRuntimeLanguageContext context, SourceUnit sourceUnit)
            : base(sourceUnit)
        {
            if (code == null)
                throw new ArgumentNullException("code");
            if (context == null)
                throw new ArgumentNullException("context");
            if (sourceUnit == null)
                throw new ArgumentNullException("sourceUnit");
            this.Code = code;
            this.RuntimeLanguageContext = context;
        }

        /// <summary>
        /// Wrapper to the LanguageContext that this source is bound to.
        /// </summary>
        public IRuntimeLanguageContext RuntimeLanguageContext { get; private set; }

        /// <summary>
        /// This is the Expression/Expression Tree code representing the code to be execution.
        /// </summary>
        /// <remarks>
        /// This is equivelent to metadata for the actual IL code to be generated.
        /// </remarks>
        public Expression<Func<TEnvironment, object, object>> Code { get; private set; }

        /// <summary>
        /// Private variable being lazy-initialized and that holds the compiled executable code.
        /// </summary>
        private Func<TEnvironment, object, object> _target;

        /// <summary>
        /// This is the compiled code (delegate) that can be executed.
        /// </summary>
        public Func<TEnvironment, object, object> Target
        {
            get
            {
                // If not compiled, compile the metadata to IL code (delegate).
                if (this._target == null)
                {
                    Func<TEnvironment, object, object> compiled = this.RuntimeLanguageContext.Compile(this.Code);
                    Interlocked.CompareExchange(ref this._target, compiled, null);
                }
                return this._target;
            }
        }

        /// <summary>
        /// Run the code in the given scope.
        /// </summary>
        /// <param name="scope">ScriptScope to be used when running the code.</param>
        /// <returns>The value of the last evaluated expression.</returns>
        public override object Run(Microsoft.Scripting.Runtime.Scope scope)
        {
            // This is where the magic happens.
            // 1. The Target property compiled the code to an executable delegate.
            // 2. We execute it with:
            //      - The SmalltalkEnvironment (that is bound to the LanguageContext)
            //      - The receiver ... which is currently set to nil (null).
            return this.Target(this.RuntimeLanguageContext.Environment, null);
        }
    }
}
