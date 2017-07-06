namespace ApiRouteDescriptor
{
    public class NamedOperationDescription
    {
        public string Name { get; set; }
        public string Method { get; set; }
        public string Route { get; set; }
        public IResponseDescriptor ResponseDescriptor { get; set; }
    }
}