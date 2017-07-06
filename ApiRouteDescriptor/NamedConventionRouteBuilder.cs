namespace ApiRouteDescriptor
{
    public class NamedConventionRouteBuilder
    {
        private readonly string _method;
        private readonly CaptureResponseDelegate _handler;

        public IResponseDescriptor this[string name, string route]
        {
            set => this._handler(this._method, name, route, value);
        }

        public NamedConventionRouteBuilder(string method, CaptureResponseDelegate handler)
        {
            this._handler = handler;
            this._method = method;
        }
    }
}
