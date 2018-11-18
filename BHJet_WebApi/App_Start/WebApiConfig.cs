using BHJet_WebApi.Util;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace BHJet_WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //SwaggerConfig.Register();

            config.Services.Add(
                typeof(IExceptionLogger),
                new LogarExcecao()
            );
        }
    }
}
