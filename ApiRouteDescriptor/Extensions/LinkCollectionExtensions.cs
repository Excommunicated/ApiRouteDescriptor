using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Resources;

namespace ApiRouteDescriptor.Extensions
{
    public static class LinkCollectionExtensions
    {
        public static LinkCollection AddPaging(this LinkCollection linkCollection, int skip, int take, int total,
            Func<int, int, string> formatFunc)
        {
            if (skip > 0)
                linkCollection.Add("Page.Previous", formatFunc(skip, take));
            if (total > skip + take)
                linkCollection.Add("Page.Next", formatFunc(skip + take, take));
            linkCollection.Add("Page.Current", formatFunc(skip, take));

            return linkCollection;
        }
    }
}
