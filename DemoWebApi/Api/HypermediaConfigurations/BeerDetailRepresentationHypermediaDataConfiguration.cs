using System.Collections.Generic;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hypermedia;

namespace DemoWebApi.Api.HypermediaConfigurations
{
    public class BeerDetailRepresentationHypermediaObjectConfiguration : HypermediaObjectConfigurationBase<BeerDetailRepresentation>
    {
        public override void Configure()
        {
            this.Metadata.Add(r => r, new SelfLink("BeerDetail/{id}"));
            this.Metadata.Add(representation => representation.Reviews, new Link(LinkTypes.Query, "Reviews", "Beers/{id}/Reviews", null, true));
        }
    }
}