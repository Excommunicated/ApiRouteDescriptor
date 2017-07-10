namespace ApiRouteDescriptor.Resources
{
    public abstract class Resource
    {
        public LinkCollection Links { get; set; } = new LinkCollection();
    }
}