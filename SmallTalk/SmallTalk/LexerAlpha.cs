using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SmallTalk
{
    public class LexerAlpha
    {
        #region Classes

        internal class TokenTemplate
        {
            public string Name { get; set; }
            public string Expression { get; set; }
            public bool Skip { get; set; }
            public bool IgnoreCase { get; set; }
        }

        public class Token
        {
            #region Variables

            private string _name;
            private string _value;

            #endregion

            #region Constructors

            public Token(string name, string value)
            {
                Name = name;
                Value = value;
            }

            #endregion

            #region Properties

            public string Name
            {
                get
                {
                    return _name;
                }
                private set
                {
                    _name = value;
                }
            }

            public string Value
            {
                get
                {
                    return _value;
                }
                private set
                {
                    _value = value;
                }
            }

            #endregion

            #region Methods

            public override string ToString()
            {
                return string.Format("{0}: {1}", Name, Value);
            }

            public string ToErrorString()
            {
                return string.Format("Unexpected token found: '{0}'", Value);
            }

            #endregion
        }

        public class TokenStream
        {
            #region Variables

            private List<Token> _tokens;
            private int _position;

            #endregion

            #region Constructors

            public TokenStream(IEnumerable<Token> tokens)
            {
                Tokens = new List<Token>(tokens);
                Position = 0;
            }

            #endregion

            #region Properties

            private List<Token> Tokens
            {
                get
                {
                    return _tokens;
                }
                set
                {
                    _tokens = value;
                }
            }

            public int Length
            {
                get
                {
                    return Tokens.Count;
                }
            }

            public int Position
            {
                get
                {
                    return _position;
                }
                set
                {
                    if ((value < 0) || (value > Length))
                    {
                        // If value == Length, Read and Peek will return null, and AtEnd will return True.
                        throw new IndexOutOfRangeException();
                    }
                    _position = value;
                }
            }

            public bool AtStart
            {
                get
                {
                    return Position == 0;
                }
            }

            public bool AtEnd
            {
                get
                {
                    return Position == Tokens.Count;
                }
            }

            #endregion

            #region Methods

            public Token Read()
            {
                if (AtEnd)
                {
                    return null;
                }
                LexerAlpha.Token token = Tokens[(int)Position];
                Position++;
                return token;
            }

            public Token Peek()
            {
                if (AtEnd)
                {
                    return null;
                }
                return Tokens[Position];
            }

            public int Seek(int offset, SeekOrigin origin)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        Position = offset;
                        break;
                    case SeekOrigin.Current:
                        Position += offset;
                        break;
                    case SeekOrigin.End:
                        Position = Length - offset;
                        break;
                }
                return Position;
            }

            public void Rewind()
            {
                Seek(0, SeekOrigin.Begin);
            }

            public void Rewind(int offset)
            {
                Seek(-offset, SeekOrigin.Current);
            }

            #endregion
        }

        public class TokenSet
        {
            #region Classes

            internal class Subset
            {
                public string Name { get; set; }
                public string[] Expression { get; set; }
            }

            public class MatchRecord
            {
                #region Variables

                private string _name;
                private List<Token> _tokens;

                #endregion

                #region Constructors

                public MatchRecord(string name, List<Token> tokens)
                {
                    Name = name;
                    Tokens = tokens;
                }

                #endregion

                #region Properties

                public string Name
                {
                    get
                    {
                        return _name;
                    }
                    private set
                    {
                        _name = value;
                    }
                }

                public List<Token> Tokens
                {
                    get
                    {
                        return _tokens;
                    }
                    set
                    {
                        _tokens = value;
                    }
                }

                #endregion

                #region Methods

                public override string ToString()
                {
                    return Name;
                }

                #endregion
            }

            #endregion

            #region Constructors

            public TokenSet(string filename)
            {
                Subsets = new List<Subset>
                    (
                        from e in XElement.Load(filename).Elements("TokenSet")
                        select new Subset
                        {
                            Name = (string)e.Attribute("name"),
                            Expression = ((string)e.Attribute("expression")).Split(' ')
                        }
                    );
            }

            #endregion

            #region Properties

            private List<Subset> Subsets { get; set; }

            #endregion

            #region Methods

            public MatchRecord FindMatch(TokenStream tokens)
            {
                foreach (Subset subset in Subsets)
                {
                    int start = tokens.Position;
                    MatchRecord match = AttemptMatch(subset, tokens);
                    if (match != null)
                    {
                        return match;
                    }
                    tokens.Position = start;
                }
                return null;
            }

            private MatchRecord AttemptMatch(Subset subset, TokenStream tokens)
            {
                List<Token> tokenSet = new List<Token>();

                foreach (string name in subset.Expression)
                {
                    Token current = tokens.Read();
                    if (current.Name != name)
                    {
                        return null;
                    }
                    tokenSet.Add(current);
                }

                return new MatchRecord(subset.Name, tokenSet);
            }

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="filename">Name of the file that contains the lexer grammar.</param>
        public LexerAlpha(string filename)
        {
            LoadGrammar(filename);
            BuildRegularExpression();
        }

        #endregion

        #region Properties

        private List<TokenTemplate> TokenTemplates { get; set; }
        private string Pattern { get; set; }

        #endregion

        #region Methods

        private void LoadGrammar(string filename)
        {
            TokenTemplates = new List<TokenTemplate>
                (
                    from e in XElement.Load(filename).Elements("Token")
                    select new TokenTemplate
                    {
                        Name = (string)e.Attribute("name"),
                        Expression = (string)e.Attribute("expression"),
                        Skip = (bool)e.Attribute("skip"),
                        IgnoreCase = (bool)e.Attribute("ignorecase")
                    }
                );
        }

        private void BuildRegularExpression()
        {
            string[] patterns = new string[TokenTemplates.Count];
            for (int index = 0; index < TokenTemplates.Count; index++)
            {
                TokenTemplate token = TokenTemplates[index];
                patterns[index] = string.Format("(?<{0}>{1})", token.Name, token.Expression);
            }
            Pattern = string.Join("|", patterns);
        }

        private TokenTemplate FindTemplate(string name)
        {
            foreach (TokenTemplate token in TokenTemplates)
            {
                if (token.Name == name)
                {
                    return token;
                }
            }
            return null;
        }

        public TokenStream Tokenize(string input)
        {
            List<Token> tokenList = new List<Token>();
            Regex regexPattern = new Regex(Pattern);
            MatchCollection matches = regexPattern.Matches(input);

            foreach (Match match in matches)
            {
                for (int i = 0; i < match.Groups.Count; i++)
                {
                    Group group = match.Groups[i];

                    bool success = group.Success;
                    if (success)
                    {
                        string groupName = regexPattern.GroupNameFromNumber(i);
                        TokenTemplate template = FindTemplate(groupName);
                        if (template == null) // only save named groups
                        {
                            continue;
                        }
                        if (template.Skip)
                        {
                            continue;
                        }
                        string matchValue = group.Captures[0].Value;
                        if (template.IgnoreCase)
                        {
                            matchValue = matchValue.ToUpper();
                        }
                        tokenList.Add(new Token(groupName, matchValue));
                    }
                }
            }
            return new TokenStream(tokenList);
        }

        #endregion
    }
}