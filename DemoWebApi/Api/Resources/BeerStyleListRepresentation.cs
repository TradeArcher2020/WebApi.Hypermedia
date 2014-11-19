using System.Collections.Generic;

namespace WebApi.Hal.Web.Api.Resources
{
    public class BeerStyleListRepresentation : SimpleListRepresentation<BeerStyleRepresentation>
    {
        public BeerStyleListRepresentation(IList<BeerStyleRepresentation> beerStyles) : base(beerStyles)
        {
        }

    }
}