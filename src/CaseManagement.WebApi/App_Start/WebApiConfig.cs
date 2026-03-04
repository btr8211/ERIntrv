using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CaseManagement.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // JSON 序列化設定
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;

            // 移除 XML 格式
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
