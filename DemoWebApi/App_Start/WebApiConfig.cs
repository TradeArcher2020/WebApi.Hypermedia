using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DemoWebApi.Api;
using DemoWebApi.Api.HypermediaConfigurations;
using WebApi.Hypermedia;
using WebApi.Hypermedia.Formatters;

namespace DemoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Add(new JsonHalMediaTypeFormatter());
            config.Filters.Add(new HypermediaFilter());
            //config.Formatters.Add(new XmlHalMediaTypeFormatter());
            //config.MessageHandlers.Add(new HypermediaHandler());


            // Web API routes
            config.Routes.MapHttpRoute("BeersRoute", "beers/{id}", new { controller = "Beer" }); // this one is needed only because beers vs. 1 beer are separated into 2 controllers
            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("BreweryBeersRoute", "breweries/{id}/beers", new { controller = "BeersFromBrewery" });
            config.Routes.MapHttpRoute("StyleBeersRoute", "styles/{id}/beers", new { controller = "BeersFromStyle" });

            config.AddOrUpdateHypermediaObjectConfiguration(new BeerRepresentationHypermediaObjectConfiguration());
            config.AddOrUpdateHypermediaObjectConfiguration(new BeerDetailRepresentationHypermediaObjectConfiguration());
        }
    }
}
