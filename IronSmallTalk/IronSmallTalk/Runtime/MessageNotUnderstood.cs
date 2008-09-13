using System;

namespace IronSmalltalk
{
    public class MessageNotUnderstood : Exception
    {
        #region Constructors

        public MessageNotUnderstood(Type type, SmallSymbol selectorName)
            : base(string.Format("{0}>>{1}", type.Name, selectorName))
        {
        }

        #endregion
    }
}