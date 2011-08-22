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
using System.Collections.Concurrent;

namespace IronSmalltalk.Runtime.Internal
{
    /// <summary>
    /// The SymbolTable class is where the 'magic' required for symbol identity happens.
    /// This have the functionality of SymbolSet / SymbolTable in classic implementations,
    /// but the implementation is much different.
    /// </summary>
    /// <remarks>
    /// Some implementation remarks about the SymbolTable:
    /// a. The symbol table is to be thread safe / concurrent. 
    /// b. The symbol table has weak references to the symbols.
    /// c. The symbol table is a dictionary keyed by the string value of the symbols.
    /// d. The string keys are used for fast and easy conversion from strings to symbols.
    /// e. Symbols hold internally reference to the same string that created them.
    /// f. Symbols hold reference to the symbol table, and notify the table when they get GC'ed.
    /// g. When the table gets notification that a symbol has been GC'ed it removes the entry for it.
    /// h. The internal dictionary holds WeakReferences, that hold SHORT reference to the symbol.
    /// </remarks>
    public class SymbolTable
    {
        private ConcurrentDictionary<string, WeakReference> _contents;

        /// <summary>
        /// The SmalltalkRuntime that owns this symbol table.
        /// </summary>
        public SmalltalkRuntime Runtime { get; private set; }

        /// <summary>
        /// Create and initialize an empty symbol table.
        /// </summary>
        public SymbolTable(SmalltalkRuntime runtime)
        {
            if (runtime == null)
                throw new ArgumentNullException();
            this.Runtime = runtime;
            // We expect very low concurrency on writing ... we use Environment.ProcessorCount
            // Pre-allocate 4000 objects ... we expect some symbols.
            // Use StringComparer.InvariantCulture ... because symbols are case-sensitive etc.
            this._contents = new ConcurrentDictionary<string, WeakReference>(
                Environment.ProcessorCount, 4000, StringComparer.InvariantCulture);
        }

        /// <summary>
        /// Get or create a symbol that has the requested string value.
        /// </summary>
        /// <param name="value">String value of the symbol.</param>
        /// <returns>An unique symbol with the requested string value.</returns>
        public Symbol GetSymbol(string value)
        {
            if (value == null)
                throw new ArgumentNullException();

            // 1. Try to get the symbol from the dictionary. There are good changes that:
            //      a. It's not there, then create it ... new WeakReference(new Symbol(value, this), false)
            //      b. It's already there, so return the weak reference holding it ... but ...
            WeakReference reference = this._contents.GetOrAdd(value, key => new WeakReference(new Symbol(value, this), false));
            // 2. Get the symbol from the weak reference holding it
            //      NB: We will ALWAYS get weak reference, because GetOrAdd() creates one if one doesn't exist.
            Symbol result = reference.Target as Symbol;
            // 3. Check that there was a symbol in the weak reference. The reference may be empty if:
            //      a. The symbol was GC'ed, but we still haven't got notification and removed the reference.
            //          NB: We use short lived weak references, meaning that the WeakReference.Target will be set to null
            //              as soon as the symbol can be GC'ed ... but this is before the finalizer is called
            //              and before we have a chance to clean up and remove the weak reference from the dictionary.
            //      b. Very unlucky, GC occured between 1. and 2. before we managed to get hard reference to the symbol.
            //         Otherwise, same logic as a. applies.
            // NB: As soon as we get here and <result> holds a reference to a symbol, there should be no way
            //     for the symbol to get GC'ed. Therefore the reference's Target property will stay valid and we are happy.
            if (result == null)
            {
                // 4. Lock the whole thing and go into single-thread mode. Symbols are expected to be long-lived
                //    objects, and this will happen very seldom, so performance is not considered a problem.
                lock (this._contents)
                {
                    // 5. In case that GC occured between 2. and 4. and the reference was 
                    //    removed by InternalRemoveSymbol(), get it one more time.
                    reference = this._contents.GetOrAdd(value, key => new WeakReference(new Symbol(value, this), false));
                    // 6. As previously, check that the weak reference holds a symbol. If:
                    //      a. There was a symbol, then InternalRemoveSymbol() managed to remove the original weak 
                    //         reference, and we create a new one in 5. ... new WeakReference(new Symbol(value, this), false)
                    //      b. Still null (this is the most probable case) ... because of 3.a., 3.b. or GC between 5. and 6.
                    result = reference.Target as Symbol;
                    if (result == null)
                    {
                        // 7. Create a new symbol and set the Target of the weak reference.
                        //    NB: We have locked contents, and we are holding a strong reference, so no concurrency issues.
                        result = new Symbol(value, this);
                        reference.Target = result;
                    }
                }
            }
            // Return the result ... it was acquired in either 2. or 7.
            return result;
        }

        /// <summary>
        /// A symbol was GC'ed. Remove the symbol info from the internal string-symbol dictionary.
        /// </summary>
        /// <param name="key">String value of the symbol that was GC'ed.</param>
        internal void InternalRemoveSymbol(string key)
        {
            // 1. Assumptions when this method starts:
            //      a. WeakReference.Target is null, because we use short lived weak references and the symbol was GC'ed,
            //         i.e. the GC set the Target property to null. Only the GC may/will set the Target to null.
            //      b. WeakReference.Target is not null, because a symbol with the same string value got re-created.
            WeakReference reference;
            // 2. Lock the contents, because we are going to bring it in inconsistent state.
            //    NB: GetSymbol() 5., 6. and 7. expect consistent state, therefore they also lock.
            lock (this._contents)
            {
                // 3. Get the weak reference from the contents dictionary.
                this._contents.TryGetValue(key, out reference);
                // 4. Check if the weak reference's Target reference a symbol. It may:
                //      a. Symbol is null ... 1.a. ... symbol was GC'ed and GC set Target to null.
                //      b. Symbol is NOT null ... 1.b. ... symbol was GC'ed and GC set Target to null,
                //         however, before this code managed to run, somebody requested a symbol with the
                //         same string value, and a new Symbol object was created, see: GetSymbol() 5., 6. and 7.
                //         Therefore, we cannot throw the weak reference away!
                Symbol symbol = (reference != null) ? reference.Target as Symbol : null;
                if (symbol == null)
                    // 5. Remove the weak reference from the contents dictionary ... this is case 4.a.
                    this._contents.TryRemove(key, out reference);
            }
        }
    }
}
