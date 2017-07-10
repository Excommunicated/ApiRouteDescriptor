using System;
using System.Collections.Generic;

namespace ApiRouteDescriptor.Resources
{
    public class LinkCollection : Dictionary<string, string>
    {
        public LinkCollection() : base((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
        {
            
        }

        public new LinkCollection Add(string rel, string href)
        {
            base.Add(rel, href);
            return this;
        }

        public static LinkCollection Self(string href)
        {
            return new LinkCollection {{"Self", href}};
        }
    }
}