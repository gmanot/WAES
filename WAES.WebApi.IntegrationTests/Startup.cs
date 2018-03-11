using System.Net.Http.Headers;
using System.Web.Http;
using Owin;
using Unity;
using Unity.Lifetime;
using WAES.BusinessLogic;
using WAES.DataStorage;
using WAES.WebApi.SelfHost;

namespace WAES.WebApi.IntegrationTests
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            var container = new UnityContainer();
            container.RegisterType<IByteDiffClient, ByteDiffClient>();
            container.RegisterType<IDataStorageClient, DataStorageClient>(new ExternallyControlledLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}