using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRouteDescriptor.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApiRouteDescriptor<TDefinition>(this IApplicationBuilder builder,
            Action<ApiDefinition> definitionBuilder) where TDefinition: ApiDefinition, new()
        {
            var definition = new TDefinition();
            definitionBuilder(definition);
            return builder.UseApiRouteDescriptor(definition);
        }

        public static IApplicationBuilder UseApiRouteDescriptor(this IApplicationBuilder builder, ApiDefinition apiDefinition)
        {
            var routeBuilder = new RouteBuilder(builder);
            foreach (var operation in apiDefinition.GetOperations())
            {
                routeBuilder.Routes.Add(new Route(new RouteHandler(context =>
                    {
                        var respType = operation.ResponseDescriptor.GetResponderType();
                        var resp = context.RequestServices.GetService(respType);
                        return (Task) respType.GetMethod("Respond")
                            .Invoke(resp, new object[] {operation.ResponseDescriptor, context});

                    }),
                    operation.Name,
                    operation.Route,
                    null,
                    new RouteValueDictionary(new {httpMethod = new HttpMethodRouteConstraint(new [] {operation.Method})}),
                    null,
                    routeBuilder.ServiceProvider.GetRequiredService<IInlineConstraintResolver>()));
            }

            builder.UseRouter(routeBuilder.Build());
            return builder;
        }
    }
}
