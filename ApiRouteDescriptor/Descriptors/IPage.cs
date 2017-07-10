namespace ApiRouteDescriptor.Descriptors
{
    public interface IPage
    {
        int ItemsPerPage { get; set; }
        string LinkRouteName { get; set; }
    }
}