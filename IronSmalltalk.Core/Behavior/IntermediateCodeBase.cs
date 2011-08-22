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
using System.Dynamic;
using System.Linq.Expressions;
using IronSmalltalk.Runtime.Bindings;
using IronSmalltalk.Common;

namespace IronSmalltalk.Runtime.Behavior
{
    /// <summary>
    /// Abstract class that represents the intermediate (byte) code in a Smalltalk method.
    /// </summary>
    public abstract class IntermediateCodeBase
    {  
    }

    /// <summary>
    /// Compilation result returned when compiling an IntermediateCode. 
    /// </summary>
    /// <typeparam name="TExpression">Type of the executable code expression.</typeparam>
    public abstract class CompilationResult<TExpression>
        where TExpression : Expression 
    {
        /// <summary>
        /// The Expression for the executable code.
        /// </summary>
        public TExpression ExecutableCode { get; private set; }

        /// <summary>
        /// Optional restrictions attached to the executable code expression.
        /// </summary>
        public BindingRestrictions Restrictions { get; private set; }


        /// <summary>
        /// Create a new CompilationResult.
        /// </summary>
        /// <param name="executableCode">The Expression for the executable code.</param>
        public CompilationResult(TExpression executableCode)
            : this(executableCode, null)
        {
        }

        /// <summary>
        /// Create a new CompilationResult.
        /// </summary>
        /// <param name="executableCode">The Expression for the executable code.</param>
        /// <param name="restrictions">Optional restrictions attached to the executable code expression.</param>
        public CompilationResult(TExpression executableCode, BindingRestrictions restrictions)
        {
            if (executableCode == null)
                throw new ArgumentNullException("executableCode");
            this.ExecutableCode = executableCode;
            this.Restrictions = restrictions;
        }

        /// <summary>
        /// Merge the restrictions of this compilation result with the given restrictions.
        /// </summary>
        /// <param name="restrictions">Restrictions to merge with.</param>
        /// <returns>The merged binding restrictions.</returns>
        public BindingRestrictions MergeRestrictions(BindingRestrictions restrictions)
        {
            if (restrictions == null)
                return this.Restrictions;
            if (this.Restrictions == null)
                return restrictions;
            return restrictions.Merge(this.Restrictions);
        }

        /// <summary>
        /// Merge the restrictions of the given compilation results.
        /// </summary>
        /// <param name="compilationResults">Compilation results whos restrictions are to be merge.</param>
        /// <returns>The merged binding restrictions.</returns>
        public static BindingRestrictions MergeRestrictions(IEnumerable<CompilationResult<TExpression>> compilationResults)
        {
            BindingRestrictions result = null;
            foreach (var compilationResult in compilationResults)
                result = compilationResult.MergeRestrictions(result);
            return result;
        }

        /// <summary>
        /// Create a DynamicMetaObject for the current compilation result containd 
        /// the merged restrictions of this compilation result with the given restrictions.
        /// </summary>
        /// <param name="restrictions">Optional. Restrictions to merge with.</param>
        /// <returns>DynamicMetaObject for the executable code and the merged restrictions.</returns>
        public DynamicMetaObject GetDynamicMetaObject(BindingRestrictions restrictions)
        {
            return new DynamicMetaObject(this.ExecutableCode, this.MergeRestrictions(restrictions));
        }
    }

    /// <summary>
    /// Error sink for reporting errors while validating intermediate code.
    /// </summary>
    public interface IIntermediateCodeValidationErrorSink
    {
        /// <summary>
        /// Report an intermediate code validation error.
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="start">Start location in the source code where the error occured.</param>
        /// <param name="stop">Stop location in the source code where the error occured.</param>
        // <returns></returns>
        void ReportError(string errorMessage, SourceLocation start, SourceLocation stop);
    }
}
