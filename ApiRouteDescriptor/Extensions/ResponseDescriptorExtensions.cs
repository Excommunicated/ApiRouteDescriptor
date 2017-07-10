using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Descriptors;

namespace ApiRouteDescriptor.Extensions
{
    public static  class ResponseDescriptorExtensions
    {
        public static TDescriptor WithLinkRouteName<TDescriptor>(this TDescriptor descriptor, string routeName)
            where TDescriptor : IPage
        {
            descriptor.LinkRouteName = routeName;
            return descriptor;
        }
    }
}
