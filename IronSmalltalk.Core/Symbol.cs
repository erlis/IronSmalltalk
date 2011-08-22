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
using IronSmalltalk.Runtime.Internal;

namespace IronSmalltalk.Runtime
{
    /// <summary>
    /// A Smalltalk Symbol object. Symbols are unique within a SmalltalkRuntime.
    /// </summary>
    public class Symbol 
    {
        /// <summary>
        /// The string value of the Symbol.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Internal. SymbolTable that contains the symbol and ensures uniqueness within the runtime.
        /// </summary>
        public readonly SymbolTable Manager;
        
        /// <summary>
        /// Create a new symbol. NB: Do not use this! It is intended for the SymbolTable!
        /// </summary>
        /// <param name="value">The string value of the Symbol.</param>
        /// <param name="manager">SymbolTable that contains the symbol and is notified when the symbol is GC'ed.</param>
        internal Symbol (string value, SymbolTable manager)
	    {
            if (value == null)
                throw new ArgumentNullException("value");
            if (manager == null)
                throw new ArgumentNullException("manager");

            this.Value = value;
            this.Manager = manager;
	    }

        /// <summary>
        /// Destructor - Notify the Manager (SymbolTable) that we are GC'ed, so it can remove the symbol and clean-up.
        /// </summary>
        ~Symbol()
        {
            this.Manager.InternalRemoveSymbol(this.Value);
        }

        /// <summary>
        /// Return the "printString" of the symbol.
        /// </summary>
        /// <returns>String representation of the symbol.</returns>
        public override string ToString()
        {
            if (this.Value.Length == 0)
                goto HashedString;

            for (int i = 0; i < this.Value.Length; i++)
            {
                char ch = this.Value[i];
                if (!(((ch >= 'A') && (ch <= 'Z')) || ((ch >= 'a') && (ch <= 'z')) || (ch == '_')))
                {
                    if (i == 0)
                        goto HashedString;
                    if (!((ch == ':') || ((ch >= '0') && (ch <= '9'))))
                        goto HashedString;
                }

            }
            // 3.5.3 or quoted selector 3.5.10
            return "#" + this.Value;
        HashedString: // 3.5.9
            return "#'" + this.Value.Replace("'", "''") + "'";
        }

        /// <summary>
        /// Defines an implicit conversion of Symbol value to a String value.
        /// </summary>
        /// <param name="value">The value to convert to a String.</param>
        /// <returns>A String that contains the value of the value parameter.</returns>
        public static implicit operator string(Symbol value)
        {
            if (value == null)
                return null;
            return value.Value;
        }
    }
}
