using System;
using System.Collections.Generic;
using System.IO;

namespace SmallTalk
{
    public class Lexer
    {
        #region Variables

        private string _text;
        private int _index;
        private List<ISmallPrimitive> _objects;

        #endregion

        #region Constructors

        private Lexer(string text)
        {
            _text = text;
            _index = 0;
            _objects = new List<ISmallPrimitive>();
        }

        #endregion

        #region Methods

        private void Tokenize()
        {
            while (!AtEnd())
            {
                char ch = Peek();
                switch (ch)
                {
                    case '$':
                        ReadCharacter();
                        break;
                    case '\'':
                        ReadString();
                        break;
                    case '#':
                        ReadSymbol();
                        break;
                    case '"':
                        ReadComment();
                        break;
                    default:
                        if (char.IsLetterOrDigit(ch) || (ch == '-'))
                        {
                            if (!ReadNumber())
                            {
                                // read the minus token
                            }
                        }
                        else if (char.IsWhiteSpace(ch))
                        {
                            ReadWhiteSpace();
                        }
                        else
                        {
                            throw new Exception(string.Format("Unexpected character found: '{0}'", ch));
                        }
                        break;
                }
            }
        }

        #region Stream Methods

        private bool AtEnd()
        {
            return _index >= _text.Length;
        }

        private char Read()
        {
            if (AtEnd())
            {
                throw new Exception("End of stream reached unexpectantly.");
            }
            char c = _text[_index];
            _index++;
            return c;
        }

        private char Peek()
        {
            return _text[_index];
        }

        private int Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _index = offset;
                    break;
                case SeekOrigin.Current:
                    _index += offset;
                    break;
                case SeekOrigin.End:
                    _index = _text.Length - offset;
                    break;
            }
            return _index;
        }

        private void Rewind()
        {
            Seek(0, SeekOrigin.Begin);
        }

        private void Rewind(int offset)
        {
            Seek(-offset, SeekOrigin.Current);
        }

        #endregion

        #region Token Read Methods

        private void ReadCharacter()
        {
            _objects.Add(new SmallCharacter(string.Empty + Read() + Read()));
        }

        private void ReadString()
        {
            string tokenText = Read().ToString(); // read the beginning quote

            while (true)
            {
                char c = Read();
                if (c == '\'')
                {
                    if (Peek() == '\'')
                    {
                        Read(); // read the next quote in the quote literal
                        tokenText += "''";
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    tokenText += c;
                }
            }

            _objects.Add(new SmallString(tokenText));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>False if the next token is actually a minus sign.</returns>
        private bool ReadNumber()
        {
            string tokenText = string.Empty;
            bool isFloat = false;

            #region Check for the negative sign.

            if (Peek() == '-')
            {
                tokenText = Read().ToString();
            }
            if (!char.IsLetterOrDigit(Peek()))
            {
                return false;
            }

            #endregion

            // Begin by reading the integer:
            tokenText = ReadInteger();

            // Was the last integer a radix?
            if ("r".Contains(Peek().ToString()))
            {
                ValidateBase10(tokenText);
                tokenText += Read() // read the radix symbol
                    + ReadInteger(); // read the actual integer
            }

            // Is this a floating-point number?
            if (Peek() == '.')
            {
                isFloat = true;
                tokenText += Read() // read the decimal symbol
                    + ReadInteger(); // read the decimal portion of the number
            }

            // Is this an exponent?
            if ("e".Contains(Peek().ToString()))
            {
                tokenText += Read(); // read the exponent symbol
                string expText = ReadInteger();
                ValidateBase10(expText);
                tokenText += expText;
            }

            if (isFloat)
            {
                _objects.Add(new SmallFloat(tokenText));
            }
            else
            {
                _objects.Add(new SmallInteger(tokenText));
            }

            return true;
        }

        private string ReadInteger()
        {
            string tokenText = Read().ToString();
            while (true)
            {
                char ch = Peek();
                if (char.IsDigit(ch) || char.IsUpper(ch))
                {
                    tokenText += Read();
                }
                else
                {
                    break;
                }
            }
            return tokenText;
        }

        private void ReadSymbol()
        {
            string tokenText = Read().ToString(); // read the #
            if (!char.IsLetter(Peek()))
            {
                throw new Exception("Invalid symbol.");
            }
            tokenText += Read();
            while (char.IsLetterOrDigit(Peek()))
            {
                tokenText += Read();
            }
            _objects.Add(new SmallSymbol(tokenText));
        }

        /// <summary>
        /// White space doesn't get added to the token stream.
        /// </summary>
        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Peek()))
            {
                Read();
            }
        }

        /// <summary>
        /// Comments don't get added to the token stream.
        /// </summary>
        private void ReadComment()
        {
            Read(); // read the initial "
            while (Read() != '"')
            {
            }
        }

        /// <summary>
        /// Is the input text a positive base-10 integer?
        /// </summary>
        /// <param name="tokenText"></param>
        private void ValidateBase10(string tokenText)
        {
            foreach (char c in tokenText)
            {
                if (!char.IsNumber(c))
                {
                    throw new Exception(string.Format("Value must be base-10: {0}", tokenText));
                }
            }
        }

        #endregion

        #endregion
    }
}