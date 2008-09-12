using System;
using System.Runtime.Serialization;

namespace IronSmalltalk
{
    [Serializable()]
    public class SmallString : SmallPrimitive<SmallString, string>
    {
        #region Constructors

        public SmallString(string tokenText)
            : base(tokenText)
        {
        }

        public SmallString(SmallString clone)
            : base(clone)
        {
        }

        public SmallString(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Methods

        protected override void Parse()
        {
            if (string.IsNullOrEmpty(TokenText) || (TokenText[0] != '\'') || (TokenText[TokenText.Length - 1] != '\''))
            {
                throw new Exception(string.Format("Invalid string format: '{0}'", TokenText));
            }

            Value = TokenText.Replace("''", "'").Substring(1, TokenText.Length - 2);
        }

        #endregion
    }
}
