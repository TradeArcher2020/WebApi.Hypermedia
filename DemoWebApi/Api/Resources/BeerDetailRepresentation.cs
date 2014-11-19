using System.Collections.Generic;

namespace WebApi.Hal.Web.Api.Resources
{
    public class BeerDetailRepresentation
    {
        public BeerDetailRepresentation()
        {
            Reviews = new List<ReviewRepresentation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public BeerStyleRepresentation Style { get; set; }
        public BreweryRepresentation Brewery { get; set; }
        public List<ReviewRepresentation> Reviews { get; set; }

    }
}