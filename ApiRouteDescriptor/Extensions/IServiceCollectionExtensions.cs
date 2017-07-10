using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiRouteDescriptor.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiRouteDescriptor(this IServiceCollection services, Action<ApiRouteDescriptorBuilder> builderAction = null)
        {
            services.AddRouting();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUrlHelper, UrlHelper>();
            var types = typeof(ApiDefinition).GetTypeInfo().Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i=>i.Name.StartsWith("IResponder")) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in types)
            {
                services.TryAddScoped(type);
            }

            if (builderAction != null)
            {
                ApiRouteDescriptorBuilder builder = new ApiRouteDescriptorBuilder(services);
                builderAction(builder);
            }

            return services;
        }
       
    }
}
