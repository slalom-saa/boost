using System;

namespace Slalom.Boost.Serialization
{
    public interface ISpecifySerializationPayload
    {
        object GetSerializationPayload();
    }
}