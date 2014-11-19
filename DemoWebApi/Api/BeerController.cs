using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Linq;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;
using WebApi.Hypermedia;

namespace WebApi.Hal.Web.Api
{
    public class BeerController : ApiController
    {
        readonly IBeerDbContext beerDbContext;

        public BeerController(IBeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        // GET beers/5
        public BeerRepresentation Get(int id)
        {
            var beer = beerDbContext.Beers.Include("Brewery").Include("Style").Single(br => br.Id == id); // lazy loading isn't on for this query; force loading

            //Get a list of Links and add to it context-specific links.  
            this.GetHypermediaContextualLinks()
                .AddOrUpdateHypermediaContextualLinks(new Link(LinkTypes.Query, "beers", "~/beers{?page}"));

            
            return new BeerRepresentation
            {
                Id = beer.Id,
                Name = beer.Name,
                BreweryId = beer.Brewery == null ? (int?)null : beer.Brewery.Id,
                BreweryName = beer.Brewery == null ? null : beer.Brewery.Name,
                StyleId = beer.Style == null ? (int?)null : beer.Style.Id,
                StyleName = beer.Style == null ? null : beer.Style.Name,
                ReviewIds = beerDbContext.Reviews.Where(r => r.Beer_Id == id).Select(r => r.Id).ToList()
            };
        }

        // PUT beers/5
        public void Put(int id, string value)
        {
        }

        // DELETE beers/5
        public void Delete(int id)
        {
        }
    }

    //public class MyNonDelegatingHandler : HttpMessageHandler
    //{
    //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        return base.SendAsync(request, cancellationToken)
    //            .ContinueWith(task =>
    //            {
    //                var response = task.Result;
    //                return response;
    //            });
    //    }
    //}

}