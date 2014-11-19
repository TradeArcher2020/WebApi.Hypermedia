using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;
using WebApi.Hal.Web.Data.Queries;
using WebApi.Hal.Web.Models;

namespace WebApi.Hal.Web.Api
{
    public class BeersController : ApiController
    {
        public const int PageSize = 5;

        readonly IRepository repository;

        public BeersController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET beers
        public BeerListRepresentation Get(int page = 1)
        {
            var beers = repository.Find(new GetBeersQuery(), page, PageSize);

            var resourceList = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page);

            return resourceList;
        }

        [HttpGet]
        public BeerListRepresentation Search(string searchTerm, int page = 1)
        {
            var beers = repository.Find(new GetBeersQuery(b => b.Name.Contains(searchTerm)), page, PageSize);

            // snap page back to actual page found
            if (page > beers.TotalPages) page = beers.TotalPages;

            //var link = LinkTemplates.Beers.SearchBeers.CreateLink(new { searchTerm, page });
            var beersResource = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages, page)
            {
                Page = page,
                TotalResults = beers.TotalResults
            };

            return beersResource;
        }

        // POST beers
        [ResponseType(typeof(BeerRepresentation))]
        public IHttpActionResult Post(BeerRepresentation value)
        {
            var beer = new Beer(value.Name);
            repository.Add(beer);

            var beerRepresentation = new BeerRepresentation
            {
                Id = beer.Id,
                Name = beer.Name,
                BreweryId = beer.Brewery == null ? (int?)null : beer.Brewery.Id,
                BreweryName = beer.Brewery == null ? null : beer.Brewery.Name,
                StyleId = beer.Style == null ? (int?)null : beer.Style.Id,
                StyleName = beer.Style == null ? null : beer.Style.Name,
            };

            return Created("beers/" + beerRepresentation.Id, beerRepresentation);
        }

        
    }
}