using System;
using System.Collections.Generic;

namespace ApiRouteDescriptor.Descriptors
{
    public abstract class ResponseDescriptor : IResponseDescriptor
    {
        private readonly Type _responderImplementationType;
        public List<Type> ResourceTypes { get; set; }

        protected ResponseDescriptor(Type responderImplementationType)
        {
            _responderImplementationType = responderImplementationType;
            this.ResourceTypes = new List<Type>();
        }
        public Type GetResponderType()
        {
            return _responderImplementationType;
        }
    }
}