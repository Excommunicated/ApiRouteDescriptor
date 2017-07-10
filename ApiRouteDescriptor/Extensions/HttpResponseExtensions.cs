using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiRouteDescriptor.Extensions
{
    public static class HttpResponseExtensions
    {
        private static JsonSerializerSettings _settings;

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return _settings ?? (_settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        public static Task WriteAsJsonAsync(this HttpResponse response, object value)
        {
            response.ContentType = "application/json";
            var serializer = JsonSerializer.Create(GetJsonSerializerSettings());
            using (var sw = new StreamWriter(response.Body))
            {
                using (var tw = new JsonTextWriter(sw))
                {
                    serializer.Serialize(tw,value);
                }
            }
            return Task.CompletedTask;
        }
    }
}
