using System;
using System.Runtime.Serialization;

namespace SmallTalk
{
    /// <summary>
    /// A primitive is a simple object, i.e. integer, symbol, character.  The simple objects
    /// tie directly to .NET objects.
    /// </summary>
    /// <typeparam name="T">The inheriting type.</typeparam>
    /// <typeparam name="V">The value-type.</typeparam>
    [Serializable()]
    public class SmallPrimitive<T, V> : SmallClass, ISmallPrimitive<T, V> where T : ISmallPrimitive<T, V>
    {
        #region Variables

        private string _tokenText;
        private V _value;

        #endregion

        #region Constructors

        public SmallPrimitive(string tokenText)
            : base(null)
        {
            Class = this;
            Set(tokenText);
        }

        public SmallPrimitive(T clone)
            : base(null)
        {
            Class = this;
            Set(clone.TokenText);
        }

        public SmallPrimitive(SerializationInfo info, StreamingContext context)
            : base(null)
        {
            Class = this;
            Set(info.GetString("TokenText"));
        }

        #endregion

        #region Properties

        public V Value
        {
            get
            {
                return _value;
            }
            protected set
            {
                _value = value;
            }
        }

        public string TokenText
        {
            get
            {
                return _tokenText;
            }
            protected set
            {
                _tokenText = value;
            }
        }

        #endregion

        #region Methods

        public static T Parse(V value)
        {
            return (T)Activator.CreateInstance(typeof(T), value);
        }

        protected virtual void Parse()
        {
        }

        public void Set(string tokenText)
        {
            _tokenText = tokenText;
            Parse();
        }

        public string ToCodeString()
        {
            return TokenText;
        }

        public string ToPrintableString()
        {
            return ToString();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public T Clone()
        {
            return (T)Activator.CreateInstance(typeof(T), this);
        }

        object ICloneable.Clone()
        {
            return Activator.CreateInstance(typeof(T), this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TokenText", _tokenText);
        }

        public override bool Equals(object obj)
        {
            return (obj is T) && (((T)obj).Value.Equals(Value));
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion
    }
}