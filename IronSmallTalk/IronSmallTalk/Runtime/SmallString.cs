using System;
using System.Runtime.Serialization;

namespace IronSmalltalk
{
    [Serializable()]
    public class SmallString : SmallPrimitive<SmallString, string>
    {
        #region Classes

        public class value : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                return context as SmallObject;
            }
        }

        public class size : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                string value = (context as SmallObject).SendMessage(new SmallSymbol("#value")).ToString();
                return new SmallInteger(value.Length);
            }
        }

        #endregion

        #region Constructors

        public SmallString(string tokenText)
            : base(tokenText)
        {
            Initialize();
        }

        public SmallString(SmallString clone)
            : base(clone)
        {
            Initialize();
        }

        public SmallString(SerializationInfo info, StreamingContext context)
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
            //SymbolTable.Add(new SmallSymbol("#value"), this);
        }

        private void InitializeSelectors()
        {
            AttachSelector(new SmallSymbol("#value"), new value());
            AttachSelector(new SmallSymbol("#size"), new size());
        }

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
