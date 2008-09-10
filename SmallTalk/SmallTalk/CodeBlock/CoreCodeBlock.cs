using System;

namespace SmallTalk
{
    /// <summary>
    /// Represents a block of .NET code that can interface with the Smalltalk environment.
    /// </summary>
    public abstract class CoreCodeBlock : ICodeBlock
    {
        #region Methods

        public abstract SmallObject Execute(ICodeContext context, params SmallObject[] parameters);

        #endregion
    }
}