using System;

namespace ApiRouteDescriptor
{
    public interface IResponseDescriptor
    {
        Type GetResponderType();
    }
}