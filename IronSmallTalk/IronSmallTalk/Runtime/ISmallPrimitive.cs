using System;
using System.Runtime.Serialization;

namespace IronSmalltalk
{
    public interface ISmallPrimitive : ISmallObject, ISerializable, ICloneable
    {
        #region Properties

        string TokenText { get; }

        #endregion

        #region Methods

        void Set(string tokenText);

        string ToCodeString();

        string ToPrintableString();

        string ToString();

        #endregion
    }

    public interface ISmallPrimitive<T, V> : ISmallPrimitive, ICloneable<T> where T : ISmallPrimitive
    {
        #region Properties

        V Value { get; }

        #endregion
    }
}