using System.Collections.Generic;
using ApiRouteDescriptor.Descriptors;
using ApiRouteDescriptor.Responders;

namespace ApiRouteDescriptor
{
    public abstract class ApiDefinition
    {
        private readonly List<NamedOperationDescription> _descriptions = new List<NamedOperationDescription>();

        protected NamedConventionRouteBuilder Get { get; }
        protected NamedConventionRouteBuilder Put { get; }
        protected NamedConventionRouteBuilder Post { get; }
        protected NamedConventionRouteBuilder Delete { get; }
        protected NamedConventionRouteBuilder Options { get; }

        protected ApiDefinition()
        {
            this.Get = new NamedConventionRouteBuilder("GET",this.CaptureOperation);
            this.Put = new NamedConventionRouteBuilder("PUT", this.CaptureOperation);
            this.Post = new NamedConventionRouteBuilder("POST", this.CaptureOperation);
            this.Delete = new NamedConventionRouteBuilder("DELETE", this.CaptureOperation);
            this.Options = new NamedConventionRouteBuilder("OPTIONS", this.CaptureOperation);
        }

        private void CaptureOperation(string method, string name, string route, IResponseDescriptor responseDescriptor)
        {
            this._descriptions.Add(new NamedOperationDescription
            {
                Name = name,
                Route = route,
                Method = method,
                ResponseDescriptor = responseDescriptor
            });
        }

        public IList<NamedOperationDescription> GetOperations()
        {
            return this._descriptions.AsReadOnly();
        }

        protected CustomActionResponseDescriptor<TResponder> CustomAction<TResponder>()
            where TResponder : CustomActionResponder<TResponder>
        {
            return new CustomActionResponseDescriptor<TResponder>(null);
        }

        protected PagedResponseDescriptor<TModel, TResource> Paged<TModel,TResource>(string orderBy, bool orderDesc = false) where TModel: class 
        {
            return new PagedResponseDescriptor<TModel, TResource>(orderBy, orderDesc);
        }
    }
}