using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using WAES.BusinessLogic;
using WAES.DataStorage;

namespace WAES.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
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
        }
    }
}
