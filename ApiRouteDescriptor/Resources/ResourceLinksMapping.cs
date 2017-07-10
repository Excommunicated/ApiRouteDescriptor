using System;
using System.Collections.Generic;
using AutoMapper;

namespace ApiRouteDescriptor.Resources
{
    public abstract class ResourceLinksMapping : Profile
    {
        public Type ModelType { get; }
        public Type ResourceType { get; }

        protected ResourceLinksMapping(Type modelType, Type resourceType)
        {
            ModelType = modelType;
            ResourceType = resourceType;
        }

        public virtual void ModelToResource(object model, object resource, IUrlHelper urlHelper)
        {
            
        }



        public virtual void PagedModelToPagedResource(object model, object resource,
            IUrlHelper urlHelper)
        {
            
        }

        public virtual ResourceLinksMapping<TResource, TModel> InheritFrom<TResource, TModel>()
        {
            var mapping = new ResourceLinksMapping<TResource,TModel>();
            return mapping;
        }

        public static ResourceLinksMapping<TResource, TModel> Create<TResource, TModel>()
        {
            return new ResourceLinksMapping<TResource, TModel>();
        }
    }

    public class ResourceLinksMapping<TResource, TModel> : ResourceLinksMapping
    {

        private readonly List<Action<TModel,TResource, IUrlHelper>> _afterMapActions = new List<Action<TModel, TResource, IUrlHelper>>();
        private readonly List<Action<IMappingExpression<TModel, TResource>>> _afterConfigureActions = new List<Action<IMappingExpression<TModel, TResource>>>();

        public ResourceLinksMapping():base (typeof(TModel), typeof(TResource))
        {
            var mapping = CreateMap<TModel, TResource>();
            mapping.ReverseMap();
            foreach (var afterConfigureAction in _afterConfigureActions)
            {
                afterConfigureAction(mapping);
            }
        }

        public override void ModelToResource(object model, object resource, IUrlHelper urlHelper)
        {
            base.ModelToResource(model, resource, urlHelper);
            foreach (var afterMapAction in _afterMapActions)
            {
                afterMapAction((TModel) model, (TResource) resource, urlHelper);
            }
        }



        public ResourceLinksMapping<TResource, TModel> EnrichResource(
            Action<TModel, TResource, IUrlHelper> callback)
        {
            _afterMapActions.Add(callback);
            return this;
        }

        public ResourceLinksMapping<TResource, TModel> AddCustomMapping(
            Action<IMappingExpression<TModel, TResource>> callback)
        {
            _afterConfigureActions.Add(callback);
            return this;
        }

    }
}
