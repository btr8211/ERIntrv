using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using CaseManagement.Repository.Implementations;
using CaseManagement.Repository.Interfaces;
using CaseManagement.Service.Implementations;
using CaseManagement.Service.Interfaces;

namespace CaseManagement.WebApi
{
    public static class AutofacConfig
    {
        public static IContainer Container { get; private set; }

        public static void Configure()
        {
            var builder = new ContainerBuilder();

            // 註冊 WebApi Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // 註冊 Services
            builder.RegisterType<CaseService>()
                   .As<ICaseService>()
                   .InstancePerRequest();

            builder.RegisterType<CaseNumberGenerator>()
                   .As<ICaseNumberGenerator>()
                   .InstancePerRequest();

            // 註冊 Repositories
            builder.RegisterType<CaseRepository>()
                   .As<ICaseRepository>()
                   .InstancePerRequest();

            // 註冊資料庫連線
            builder.Register(c => new SqlConnection(
                ConfigurationManager.ConnectionStrings["CaseDB"].ConnectionString))
                   .As<IDbConnection>()
                   .InstancePerRequest();

            Container = builder.Build();

            // 設定 WebApi DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(Container);
        }
    }
}
