using ApiRouteDescriptor.Descriptors;

namespace ApiRouteDescriptor.Responders
{
    public abstract class
        CustomActionResponder<TImplementation> : Responder<CustomActionResponseDescriptor<TImplementation>>
        where TImplementation : CustomActionResponder<TImplementation>
    {
        
    }
}