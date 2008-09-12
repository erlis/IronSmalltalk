using System;
using System.Runtime.Serialization;

namespace IronSmalltalk
{
    [Serializable()]
    public class SmallCharacter : SmallPrimitive<SmallCharacter, char>
    {
        #region Classes

        public class asLowercase : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                string value = (context as SmallObject).SendMessage("value").ToString();
                return new SmallCharacter(string.Format("${0}", char.ToLower(value[0])));
            }
        }

        public class value : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                return context as SmallObject;
            }
        }

        #endregion

        #region Constructors

        public SmallCharacter(string tokenText)
            : base(tokenText)
        {
            Initialize();
        }

        public SmallCharacter(SmallCharacter clone)
            : base(clone)
        {
            Initialize();
        }

        public SmallCharacter(SerializationInfo info, StreamingContext context)
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
            AttachSelector("value", new value());
            AttachSelector("asLowercase", new asLowercase());
        }

        protected override void Parse()
        {
            if (string.IsNullOrEmpty(TokenText) || (TokenText[0] != '$') || (TokenText.Length != 2))
            {
                throw new Exception(string.Format("Invalid character format: '{0}'", TokenText));
            }

            Value = TokenText[1];
        }

        #endregion
    }
}