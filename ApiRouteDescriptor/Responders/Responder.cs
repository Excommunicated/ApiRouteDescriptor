using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Extensions;
using ApiRouteDescriptor.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ApiRouteDescriptor.Responders
{
    public abstract class Responder<TDescriptor>: IResponder<TDescriptor> where TDescriptor: IResponseDescriptor
    {
        protected HttpContext Context { get; private set; }
        protected TDescriptor Options { get; private set; }

        protected abstract Task<Resource> Execute();

        public async Task Respond(TDescriptor options, HttpContext context)
        {
            this.Context = context;
            this.Options = options;
            var response = await this.Execute();
            await this.Context.Response.WriteAsJsonAsync(response);
        }

        protected TValue QueryString<TValue>(string queryValue, TValue defaultValue)
        {
            var value = this.Context.Request.Query[queryValue];
            if (value.Count == 0) return defaultValue;
            return (TValue) Convert.ChangeType(value.Aggregate((a, s) =>
            {
                if (string.IsNullOrWhiteSpace(a)) return s;
                return $"{a},{s}";
            }), typeof(TValue));
        }

        protected string ResolveLink(string routeName)
        {
            return this.ResolveLink(routeName, new RouteValueDictionary());
        }

        protected string ResolveLink(string routeName, RouteValueDictionary rvd)
        {
            var routeData = this.Context.GetRouteData();
            var router = routeData.Routers[0];
            return router.GetVirtualPath(new VirtualPathContext(this.Context, routeData.Values, rvd, routeName))?.VirtualPath;
        }
    }
}
