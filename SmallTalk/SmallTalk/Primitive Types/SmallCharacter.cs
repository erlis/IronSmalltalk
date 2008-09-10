using System;
using System.Runtime.Serialization;

namespace SmallTalk
{
    [Serializable()]
    public class SmallCharacter : SmallPrimitive<SmallCharacter, char>
    {
        #region Classes

        public class asLowercase : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                SmallCharacter ch = context.SymbolTable[new SmallSymbol("#value")] as SmallCharacter;
                return new SmallCharacter(string.Format("${0}", char.ToLower(ch.Value)));
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
            SymbolTable.Add(new SmallSymbol("#value"), this);
        }

        private void InitializeSelectors()
        {
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