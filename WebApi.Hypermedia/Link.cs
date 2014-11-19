using System;
using System.Collections.Generic;

namespace WebApi.Hypermedia
{
    public class Link
    {
        public Link()
        {
            LinkType = LinkTypes.Query;
        }

        public Link(LinkTypes linkType, string name, string uri, Dictionary<string, string> metadata = null, bool templated = false)
        {
            LinkType = linkType;
            Name = name;
            Uri = uri;
            Metadata = metadata;
            Templated = templated;
        }

        public LinkTypes LinkType { get; set; }

        public string Name { get; set; }
        public string Uri { get; set; }
        /// <summary>
        /// This is a collection of extra metadata about the link.  
        /// For example, you might want to include the type (e.g. text/html), hreflang, or charset.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
        public bool Templated { get; set; }

        public virtual bool IsValid()
        {
            if (String.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (this.Uri == null)
            {
                return false;
            }

            return true;
        }
    }
}