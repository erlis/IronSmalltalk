using System.Collections.Generic;

namespace SmallTalk
{
    /// <summary>
    /// The base context would be the global environment, containing global variables etc.
    /// After that you would have the object context with its instance variables.
    /// After that would be the "method" context with its local variables, followed by a
    /// succession of block contexts.
    /// </summary>
    public interface ICodeContext
    {
        #region Properties

        /// <summary>
        /// The parent context was used to create this context.
        /// </summary>
        ICodeContext ParentContext { get; set; }

        Dictionary<SmallSymbol, SmallObject> SymbolTable { get; }

        #endregion
    }
}