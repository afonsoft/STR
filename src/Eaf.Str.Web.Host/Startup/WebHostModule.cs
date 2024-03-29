using Eaf.AspNetCore.Configuration;
using Eaf.Configuration.Startup;
using Eaf.Middleware.Configuration;
using Eaf.Middleware.Web;
using Eaf.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace Eaf.Str.Web.Startup
{
    [DependsOn(
        typeof(StrApplicationModule),
        typeof(MiddlewareWebCoreModule)
    )]
    public class WebHostModule : EafModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public WebHostModule(
            IWebHostEnvironment env
        )
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WebHostModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            //Set default connection string
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(StrConsts.ConnectionStringName);

            //Database Configurations
            Configuration.Database.IsOracleEnabled = Convert.ToBoolean(_appConfiguration["Database:IsOracleEnabled"]);

            //Create Controllers APIs
            Configuration.Modules.EafAspNetCore()
                .CreateControllersForAppServices(
                    typeof(StrApplicationModule).GetAssembly()
                );

            //Send All Exceptions To Clients Angular
            Configuration.Modules.EafWebCommon().SendAllExceptionsToClients = true;

            //Enable Delete Expired Logs
            Configuration.EntityHistory.LogExpireTime = TimeSpan.FromDays(360);
            Configuration.EntityHistory.LogExpireEnabled = true;
            Configuration.Auditing.LogExpireTime = TimeSpan.FromDays(360);
            Configuration.Auditing.LogExpireEnabled = true;

            Configuration.Caching.MemoryCacheOptions = new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions
            {
                SizeLimit = 256 //Mb
            };
        }

        public override void PostInitialize()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        }
    }
}