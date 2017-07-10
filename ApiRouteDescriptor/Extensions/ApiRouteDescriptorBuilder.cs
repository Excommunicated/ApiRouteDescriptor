using Microsoft.Extensions.DependencyInjection;

namespace ApiRouteDescriptor.Extensions
{
    public class ApiRouteDescriptorBuilder
    {
        public IServiceCollection Services { get; }

        public ApiRouteDescriptorBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}