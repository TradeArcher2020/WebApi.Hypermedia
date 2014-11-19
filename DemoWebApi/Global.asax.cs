using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using WebApi.Hal.Web;
using WebApi.Hal.Web.Data;

namespace DemoWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        IContainer container;
        string connectionString;

        protected void Application_Start()
        {

            connectionString = ConfigurationManager.AppSettings["BeerDatabase"];

            //GlobalConfiguration.Configuration.Formatters.Add(new JsonHalMediaTypeFormatter());
            //GlobalConfiguration.Configuration.Formatters.Add(new XmlHalMediaTypeFormatter());

            var containerBuilder = new ContainerBuilder();

            ConfigureContainer(containerBuilder);

            Database.SetInitializer(new DbUpDatabaseInitializer(connectionString));

            container = containerBuilder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            // Register API controllers using assembly scanning.
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            containerBuilder
                .Register(c => new BeerDbContext(connectionString))
                .As<IBeerDbContext>()
                .InstancePerRequest();

            containerBuilder
                .RegisterType<BeerRepository>()
                .As<IRepository>()
                .InstancePerRequest();
        }

    }
}
