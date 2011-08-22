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
using System.Linq.Expressions;
using AST = System.Linq.Expressions;
using IronSmalltalk.Runtime.Execution.Internals;

namespace IronSmalltalk.Runtime.CodeGeneration.Bindings
{
    public abstract class NameBinding
    {
        public string Name { get; private set; }

        protected NameBinding(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException();
            this.Name = name;
        }

        public bool IsErrorBinding 
        {
            get { return (this is IErrorBinding); }
        }

        /// <summary>
        /// This returns true if the value of the binding will always be the same. 
        /// Some read-only bindings (e.g. self, super) are NOT constant-value-bindings.
        /// </summary>
        public virtual bool IsConstantValueBinding
        {
            get { return false; }
        }

        public abstract Expression GenerateReadExpression(IBindingClient client);
    }

    public class ErrorBinding : NameBinding, IErrorBinding
    {
        public string ErrorDescription { get; private set; }

        public ErrorBinding(string name)
            : this(name, RuntimeCodeGenerationErrors.UndefinedBinding)
        {
        }

        public ErrorBinding(string name, string errorDescription)
            : base(name)
        {
            if (String.IsNullOrWhiteSpace(errorDescription))
                throw new ArgumentNullException("errorDescription");

            this.ErrorDescription = errorDescription;
        }

        public override Expression GenerateReadExpression(IBindingClient client)
        {
            throw new InvalidOperationException();
        }
    }

    public interface IErrorBinding
    {
        string ErrorDescription { get; }
    }

    public interface IBindingClient
    {
        Expression SelfExpression { get; }
    }

    public interface IAssignableBinding
    {
        Expression GenerateAssignExpression(Expression value, IBindingClient client);
    }


}
