using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRouteDescriptor.Resources
{
    public class ResourceCollection<TResource> : Resource where TResource: Resource
    {
        public int TotalResults { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentIndex { get; set; }
        public IList<TResource> Items { get; set; }
    }
}
