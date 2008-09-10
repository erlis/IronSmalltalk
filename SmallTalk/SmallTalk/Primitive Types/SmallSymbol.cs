using System;
using System.Runtime.Serialization;

namespace SmallTalk
{
    [Serializable()]
    public class SmallSymbol : SmallPrimitive<SmallSymbol, string>
    {
        #region Constructors

        public SmallSymbol(string tokenText)
            : base(tokenText)
        {
        }

        public SmallSymbol(SmallSymbol clone)
            : base(clone)
        {
        }

        public SmallSymbol(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Methods

        protected override void Parse()
        {
            if (string.IsNullOrEmpty(TokenText) || (TokenText[0] != '#'))
            {
                throw new Exception(string.Format("Invalid string format: '{0}'", TokenText));
            }

            Value = TokenText.Replace("#", "");
        }

        #endregion
    }
}