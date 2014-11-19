using System;
using System.Collections.Generic;

namespace WebApi.Hypermedia
{
    public class ActionLink : Link
    {
        public ActionLink() : base()
        {
            LinkType = LinkTypes.Action;
        }

        public ActionLink(string name, string uri, MethodTypes method, Type requestBodyType, Dictionary<string, string> metadata = null, bool templated = false)
            : base(LinkTypes.Action, name, uri, metadata, templated)
        {
            Method = method;
            RequestBodyType = requestBodyType;
        }

        public MethodTypes Method { get; set; }

        /// <summary>
        /// This is the Type used as the parameter for the ApiController method.  
        /// It represents the object that the client must submit with the request 
        /// in order to complete the action.
        /// </summary>
        public Type RequestBodyType { get; set; }
    }
}