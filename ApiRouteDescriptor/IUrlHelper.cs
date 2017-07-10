using Microsoft.AspNetCore.Routing;

namespace ApiRouteDescriptor
{
    public interface IUrlHelper
    {
        string ResolveLink(string routeName);
        string ResolveLink(string routeName, object routeValues);
        string ResolveLink(string routeName, RouteValueDictionary rvd);
    }
}