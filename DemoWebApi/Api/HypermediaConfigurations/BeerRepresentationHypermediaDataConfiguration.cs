using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hypermedia;

namespace DemoWebApi.Api.HypermediaConfigurations
{
    public class BeerRepresentationHypermediaObjectConfiguration : HypermediaObjectConfigurationBase<BeerRepresentation>
    {
        public override void Configure()
        {
            this.Metadata.Add(representation => representation, new SelfLink("Beer/{id}", null, true));
            this.Metadata.Add(rep => rep, new Link(LinkTypes.Query, "Brewery", "Breweries/{BreweryId}"));
        }
    }
}