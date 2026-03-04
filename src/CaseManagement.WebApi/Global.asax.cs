using System.Web;
using System.Web.Http;

namespace CaseManagement.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.Configure();
        }
    }
}
