using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApi.Hal.Web.Api.Resources
{
    public class BeerRepresentation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? BreweryId { get; set; }
        public string BreweryName { get; set; }

        public int? StyleId { get; set; }
        public string StyleName { get; set; }

        [JsonIgnore]
        public List<int> ReviewIds { get; set; }

    }
}
