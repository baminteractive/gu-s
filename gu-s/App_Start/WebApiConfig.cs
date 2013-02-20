using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace gu_s
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CountriesRoute",
                routeTemplate: "api/countries",
                defaults: new { controller = "Countries"}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
