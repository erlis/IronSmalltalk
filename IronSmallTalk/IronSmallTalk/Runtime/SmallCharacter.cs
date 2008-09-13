using System;
using System.Runtime.Serialization;

namespace IronSmalltalk
{
    [Serializable()]
    public class SmallCharacter : SmallPrimitive<SmallCharacter, char>
    {
        #region Classes

        public class value : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                return context as SmallObject;
            }
        }

        /// <summary>
        /// Return the lower-case version of the input character:
        /// </summary>
        public class asLowercase : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                string value = (context as SmallObject).SendMessage(new SmallSymbol("#value")).ToString();
                return new SmallCharacter(string.Format("${0}", char.ToLower(value[0])));
            }
        }

        /// <summary>
        /// Return the upper-case version of the input character:
        /// </summary>
        public class asUppercase : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                string value = (context as SmallObject).SendMessage(new SmallSymbol("#value")).ToString();
                return new SmallCharacter(string.Format("${0}", char.ToUpper(value[0])));
            }
        }

        /// <summary>
        /// Return the next character in the ASCII sequence.
        /// </summary>
        public class next : CoreCodeBlock
        {
            public override SmallObject Execute(ICodeContext context, params SmallObject[] parameters)
            {
                string value = (context as SmallObject).SendMessage(new SmallSymbol("#value")).ToString();
                return new SmallCharacter(string.Format("${0}", (char)((int)value[0] + 1)));
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
            AttachSelector(new SmallSymbol("#value"), new value());
            AttachSelector(new SmallSymbol("#asLowercase"), new asLowercase());
            AttachSelector(new SmallSymbol("#asUppercase"), new asUppercase());
            AttachSelector(new SmallSymbol("#next"), new next());
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