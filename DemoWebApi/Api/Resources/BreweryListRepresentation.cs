using System.Collections.Generic;

namespace WebApi.Hal.Web.Api.Resources
{
    public class BreweryListRepresentation : SimpleListRepresentation<BreweryRepresentation>
    {
        public BreweryListRepresentation(IList<BreweryRepresentation> breweries)
            : base(breweries)
        {
        }

    }
}