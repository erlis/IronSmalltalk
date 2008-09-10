using System;
using System.Runtime.Serialization;

namespace SmallTalk
{
    [Serializable()]
    public class SmallInteger : SmallPrimitive<SmallInteger, int>
    {
        #region Classes

        public class negated : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                SmallInteger n = context.SymbolTable[new SmallSymbol("#value")] as SmallInteger;
                return new SmallInteger(-n.Value);
            }
        }

        /*
        public class negative : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                SmallInteger n = context.SymbolTable[new SmallSymbol("#value")] as SmallInteger;
                return new SmallInteger(-n.Value);
            }
        }
        */

        #endregion

        #region Constructors

        public SmallInteger(int value)
            : this(value.ToString())
        {
        }

        public SmallInteger(string tokenText)
            : base(tokenText)
        {
            Initialize();
        }

        public SmallInteger(SmallInteger clone)
            : base(clone)
        {
            Initialize();
        }

        public SmallInteger(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Initialize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            InitializeInstanceVariables();
            InitializeSelectors();
        }

        private void InitializeInstanceVariables()
        {
            SymbolTable.Add(new SmallSymbol("#value"), this);
        }

        private void InitializeSelectors()
        {
            AttachSelector("negated", new negated());
        }

        protected override void Parse()
        {
            string tokenText = TokenText;
            int radix = 10;
            int exponent = 0;
            bool isNegative = false;

            if (tokenText.Contains("r")) // get the radix
            {
                string[] arr = tokenText.Split('r');
                radix = int.Parse(arr[0]);
                tokenText = arr[1];
            }
            if (tokenText.Contains("e")) // get the exponent
            {
                string[] arr = tokenText.Split('e');
                exponent = int.Parse(arr[1]);
                tokenText = arr[0];
            }

            if (tokenText.Contains("-")) // get the negative
            {
                tokenText = tokenText.Replace("-", "");
                isNegative = true;
            }

            // Parse the integer:
            Value = 0;
            for (int index = 0; index < tokenText.Length; index++)
            {
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
                    throw new Exception(string.Format("Incorrect integer base: '{0}'", TokenText));
                }

                Value += (int)(subValue * Math.Pow(10, tokenText.Length - index - 1));
            }

            if (isNegative)
            {
                Value = -Value;
            }
        }

        #endregion
    }
}