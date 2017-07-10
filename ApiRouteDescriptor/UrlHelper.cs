using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ApiRouteDescriptor
{
    public class UrlHelper : IUrlHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UrlHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string ResolveLink(string routeName)
        {
            return ResolveLink(routeName, new RouteValueDictionary());
        }

        public string ResolveLink(string routeName, object routeValues)
        {
            return ResolveLink(routeName, new RouteValueDictionary(routeValues));
        }

        public string ResolveLink(string routeName, RouteValueDictionary rvd)
        {
            var context = _contextAccessor.HttpContext;
            var routeData = context.GetRouteData();
            var router = routeData.Routers[0];
            return router.GetVirtualPath(new VirtualPathContext(context, routeData.Values, rvd, routeName))?.VirtualPath;
        }
    }
}