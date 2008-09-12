using System;

namespace IronSmalltalk
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}