using System.Collections.Generic;
using System.Linq;

namespace WebApi.Hal.Web.Api.Resources
{
    public class BeerListRepresentation : PagedRepresentationList<BeerRepresentation>
    {
        public BeerListRepresentation(IList<BeerRepresentation> beers, int totalResults, int totalPages, int page) :
            base(beers, totalResults, totalPages, page)
        { }
    }
}