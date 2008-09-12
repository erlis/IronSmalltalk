namespace IronSmalltalk
{
    /// <summary>
    /// Represents a block of Smalltalk code.
    /// </summary>
    public class SmallCodeBlock : ICodeBlock
    {
        #region Variables

        /*
        private ArgumentBlock _arguments;
        private LocalBlock _localVariables;
        private ExpressionSequence _expressions;
        */

        #endregion

        #region Methods

        public SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}