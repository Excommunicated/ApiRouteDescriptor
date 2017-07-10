namespace ApiRouteDescriptor.Resources
{
    public interface IResourceMapper
    {

        TResource MapTo<TModel, TResource>(TModel model);
    }
}