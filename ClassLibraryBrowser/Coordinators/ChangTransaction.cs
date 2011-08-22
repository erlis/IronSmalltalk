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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Coordinators
{
    public class ChangTransaction
    {
        private static ChangTransaction Current;

        public static TResult Perform<TResult>(Func<ChangTransaction, TResult> func)
        {
            TResult result;
            ChangTransaction transaction = ChangTransaction.Current;
            if (transaction == null)
                ChangTransaction.Current = new ChangTransaction();
            try
            {
                result = func(ChangTransaction.Current);
                if (transaction == null)
                    ChangTransaction.Current.Perform();
            }
            finally
            {
                ChangTransaction.Current = transaction;
            }
            return result;
        }

        public static void Perform(Action<ChangTransaction> func)
        {
            ChangTransaction transaction = ChangTransaction.Current;
            if (transaction == null)
                ChangTransaction.Current = new ChangTransaction();
            try
            {
                func(ChangTransaction.Current);
                if (transaction == null)
                    ChangTransaction.Current.Perform();
            }
            finally
            {
                ChangTransaction.Current = transaction;
            }
        }

        public List<TransactionAction> Actions = new List<TransactionAction>();

        public void AddAction(object identifier, Action action)
        {
            if (this.Actions.Any(a => a.Identifier.Equals(identifier)))
                return;
            this.Actions.Add(new TransactionAction(identifier, action));
        }

        public void AddAction(Action action)
        {
            this.AddAction(action, action);
        }


        public void Perform()
        {
            foreach (var action in this.Actions)
                action.Action();
        }
    }

    public class TransactionAction
    {
        public readonly object Identifier;
        public readonly Action Action;
        public TransactionAction(object identifier, Action  action)
        {
            if (identifier == null)
                throw new ArgumentNullException();
            if (action == null)
                throw new ArgumentNullException();
            this.Identifier = identifier;
            this.Action = action;
        }
    }
}
