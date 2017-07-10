using ApiRouteDescriptor.Descriptors;
using ApiRouteDescriptor.Resources;

namespace ApiRouteDescriptor.Responders
{
    public abstract class
        CustomActionResponder<TImplementation> : Responder<CustomActionResponseDescriptor<TImplementation>>
        where TImplementation : CustomActionResponder<TImplementation>
    {
        
    }
}