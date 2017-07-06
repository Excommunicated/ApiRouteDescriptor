using System;

namespace ApiRouteDescriptor.Descriptors
{
    public abstract class ModelResponseDescriptor : ResponseDescriptor
    {
        public Type ModelType { get; }

        protected ModelResponseDescriptor(Type modelType,Type responderImplementationType) : base(responderImplementationType)
        {
            ModelType = modelType;
        }
    }
}