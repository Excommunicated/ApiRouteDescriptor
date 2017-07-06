namespace ApiRouteDescriptor
{
    public delegate void CaptureResponseDelegate(string method, string name, string route,
        IResponseDescriptor operation);
}