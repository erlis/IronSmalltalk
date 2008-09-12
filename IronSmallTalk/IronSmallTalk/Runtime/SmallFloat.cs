using System;
using System.Runtime.Serialization;

namespace IronSmalltalk
{
    [Serializable()]
    public class SmallFloat : SmallPrimitive<SmallFloat, double>
    {
        #region Constructors

        public SmallFloat(string tokenText)
            : base(tokenText)
        {
        }

        public SmallFloat(SmallFloat clone)
            : base(clone)
        {
        }

        public SmallFloat(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Methods

        protected override void Parse()
        {
            string tokenText = TokenText;
            int radix = 10;
            int exponent = 0;

            if (TokenText.Contains("r")) // get the radix
            {
                string[] arr = tokenText.Split('r');
                radix = int.Parse(arr[0]);
                tokenText = arr[1];
            }
            if (TokenText.Contains("e")) // get the exponent
            {
                string[] arr = tokenText.Split('e');
                exponent = int.Parse(arr[1]);
                tokenText = arr[0];
            }

            // Parse the floating-point number:
            Value = 0;
            bool parsingDecimal = false;
            int decimalPosition = tokenText.IndexOf('.');
            for (int index = 0; index < tokenText.Length; index++)
            {
                if (tokenText[index] == '.')
                {
                    parsingDecimal = true;
                    continue;
                }

                int subValue = 0;
                char ch = tokenText[index];
                if (char.IsDigit(ch))
                {
                    subValue = (int)(ch - '0');
                }
                else
                {
                    subValue = (int)(ch - 'A') + 10;
                }
                if (subValue >= radix)
                {
                    throw new Exception(string.Format("Incorrect floating-point base: '{0}'", TokenText));
                }

                if (parsingDecimal)
                {
                    Value += subValue / Math.Pow(10, index - decimalPosition);
                }
                else
                {
                    Value += subValue * Math.Pow(10, decimalPosition - index - 1);
                }
            }

            Value = TokenText[1];
        }

        #endregion
    }
}