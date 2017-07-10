using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRouteDescriptor
{
    public interface IHaveId<TId>
    {
        TId Id { get; set; }
    }
}
