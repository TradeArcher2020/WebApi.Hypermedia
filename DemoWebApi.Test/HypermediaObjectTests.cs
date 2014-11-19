using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DemoWebApi.Api.HypermediaConfigurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hypermedia;

namespace DemoWebApi.Test
{
    [TestClass]
    public class HypermediaObjectTests
    {
        [TestMethod]
        public void HypermediaObjectConfiguration_Should_Have_Matching_Link_Expression()
        {
            var config = new BeerRepresentationHypermediaObjectConfiguration();

            config.Configure();

            //Add a dummy test link for the BreweryId property.
            config.Metadata.Add(r => r.BreweryId, new Link(LinkTypes.Query, "BreweryId", "BreweryId"));

            var beerRepresentation = new BeerRepresentation();
            Expression<Func<BeerRepresentation, object>> expression = representation => representation.BreweryId;
            var hasMatch = config.Metadata.ContainsKey(expression);

            Assert.IsTrue(hasMatch);
        }

        [TestMethod]
        public void HypermediaObject_Should_Be_Able_To_Get_Property_Value_From_Metadata_Keys()
        {
            var config = new BeerDetailRepresentationHypermediaObjectConfiguration();

            config.Configure();

            var beerDetailRepresentation = new BeerDetailRepresentation();
            var reviewRepresentation = new ReviewRepresentation { Id = 1000 };
            beerDetailRepresentation.Reviews.Add(reviewRepresentation); ;

            var fullPropertyPathName = config.Metadata.Keys.First(k => k.Contains("Reviews"));

            var reviewsListValue = beerDetailRepresentation.NullSafeGetValue(fullPropertyPathName, new List<ReviewRepresentation>());

            Assert.IsTrue(reviewsListValue == beerDetailRepresentation.Reviews);
        }

        [TestMethod]
        public void HypermediaObject_Should_Be_Able_To_Get_Metadata_For_Root_Object()
        {
            var config = new BeerDetailRepresentationHypermediaObjectConfiguration();

            config.Configure();

            var beerDetailRepresentation = new BeerDetailRepresentation();
            var reviewRepresentation = new ReviewRepresentation { Id = 1000 };
            beerDetailRepresentation.Reviews.Add(reviewRepresentation); ;

            var fullPropertyPathName = config.Metadata.Keys.First(k => k == String.Empty);

            var beerDetailRepresentationValue = beerDetailRepresentation.NullSafeGetValue(fullPropertyPathName, new BeerDetailRepresentation());

            Assert.IsTrue(beerDetailRepresentation == beerDetailRepresentationValue);
        }

        [TestMethod]
        public void HypermediaObject_Get_Dto_Properties()
        {
            
        }

        private void ProcessDtoProperties(object hypermediaObject, StringBuilder propertyPathBuilder)
        {
            if (propertyPathBuilder == null)
            {
                propertyPathBuilder = new StringBuilder();
            }
            //var dto = hypermediaObject.OriginalDto;
            var dto = hypermediaObject;

            foreach (var property in dto.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                propertyPathBuilder.Append(property.Name);

                //hypermediaObject.Data.Add();
            }
        }

        public class Employee
        {
            public string Name { get; set; }
            public List<Address> Addresses { get; set; }
            public Manager Boss { get; set; }
        }
        public class Address
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
        }
        public class Manager : Employee
        {
            public List<Employee> DirectReports { get; set; }
        }
    }
}
