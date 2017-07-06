using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiRouteDescriptor.Responders
{
    public interface IResponder<TDescriptor> where TDescriptor : IResponseDescriptor
    {
        Task Respond(TDescriptor options, HttpContext context);
    }
}