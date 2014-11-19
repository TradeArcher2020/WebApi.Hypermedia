using System;
using System.Collections.Generic;

namespace WebApi.Hypermedia
{
    public class DocumentationLink : Link
    {
        public DocumentationLink()
            : base()
        {
            LinkType = LinkTypes.Documentation;
        }

        public DocumentationLink(string name, string uri, Dictionary<string, string> metadata = null, bool templated = false)
            : base(LinkTypes.Documentation, name, uri)
        {
        }
    }
}