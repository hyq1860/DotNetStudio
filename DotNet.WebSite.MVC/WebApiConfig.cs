using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace DotNet.WebSite.MVC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter)); 
        }
    }
}
