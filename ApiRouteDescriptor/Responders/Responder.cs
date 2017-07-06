using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiRouteDescriptor.Responders
{
    public abstract class Responder<TDescriptor>: IResponder<TDescriptor> where TDescriptor: IResponseDescriptor
    {
        protected HttpContext Context { get; private set; }
        protected TDescriptor Options { get; private set; }

        protected abstract Task Execute();

        public Task Respond(TDescriptor options, HttpContext context)
        {
            this.Context = context;
            this.Options = options;
            return this.Execute();
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
    }
}
