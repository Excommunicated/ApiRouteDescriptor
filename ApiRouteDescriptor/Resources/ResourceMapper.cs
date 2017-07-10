using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TypeExtensions = System.Reflection.TypeExtensions;

namespace ApiRouteDescriptor.Resources
{
    public abstract class ResourceMapper: IResourceMapper
    {
        private readonly IUrlHelper _helper;

        protected ResourceMapper(IUrlHelper helper)
        {
            _helper = helper;
            Init();
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                var profiles = Enumerable.SelectMany<KeyValuePair<Type, List<ResourceLinksMapping>>, ResourceLinksMapping>(_mappings, x => x.Value);
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile);
                }
            });
            _mapper = configuration.CreateMapper();
        }

        private void Init() { InitializeMappings();}
        private readonly IMapper _mapper;
        private readonly Dictionary<Type,List<ResourceLinksMapping>> _mappings = new Dictionary<Type, List<ResourceLinksMapping>>();

        public abstract void InitializeMappings();

        public ResourceLinksMapping<TResource, TModel> Map<TModel, TResource>()
        {
            List<ResourceLinksMapping> list = Enumerable.SelectMany<KeyValuePair<Type, List<ResourceLinksMapping>>, ResourceLinksMapping>(_mappings, x => x.Value)
                .Where(x => x.ModelType == typeof(TModel) && x.ResourceType == typeof(TResource)).ToList();
            if (list.Any())
            {
                throw new NotSupportedException("Mapping already Defined");

            }

            ResourceLinksMapping<TResource, TModel> mapping = ResourceLinksMapping.Create<TResource, TModel>();
            if (!_mappings.ContainsKey(typeof(TResource)))
            {
                _mappings.Add(typeof(TResource), new List<ResourceLinksMapping>());
            }
            _mappings[typeof(TResource)].Add(mapping);
            return mapping;
        }

        public TResource MapTo<TModel, TResource>(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            TResource resource = _mapper.Map<TResource>(model, options =>
            {
                List<ResourceLinksMapping> list;
                if (_mappings.TryGetValue(typeof(TResource), out list))
                {
                    options.AfterMap((src, dest) =>
                    {
                        var mapping = list.FirstOrDefault(x =>
                        {
                            if (!TypeExtensions.IsAssignableFrom(src.GetType(), x.ModelType))
                            {
                                //TODO Error handling

                            }
                            return true;
                        });
                        mapping?.ModelToResource(src, dest, _helper);
                    });
                }
            });
            return resource;
        }
    }
}