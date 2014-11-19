using System;
using System.Collections.Generic;

namespace WebApi.Hypermedia
{
    public class SelfLink : Link
    {
        const string SelfName = "Self";

        public SelfLink()
            : base()
        {
            LinkType = LinkTypes.Self;
        }

        public SelfLink(string uri, Dictionary<string, string> metadata = null, bool templated = false)
            : base(LinkTypes.Self, SelfName, uri, metadata, templated)
        {
        }
    }
}