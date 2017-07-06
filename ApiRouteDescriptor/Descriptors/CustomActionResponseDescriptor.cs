using System;
using ApiRouteDescriptor.Responders;

namespace ApiRouteDescriptor.Descriptors
{
    public class CustomActionResponseDescriptor<TActionResponder> : ResponseDescriptor where TActionResponder: CustomActionResponder<TActionResponder>
    {
        public CustomActionResponseDescriptor(Type responderImplementationType) : base(typeof(TActionResponder))
        {
            if (responderImplementationType == null)
                return;
            this.ResourceTypes.Add(responderImplementationType);
        }
    }
}
