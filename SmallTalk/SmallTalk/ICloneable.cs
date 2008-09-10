using System;

namespace SmallTalk
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}