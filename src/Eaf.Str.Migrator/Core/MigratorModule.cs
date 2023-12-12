using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Eaf.Events.Bus;
using Eaf.Middleware.Configuration;
using Eaf.Middleware.Identity;
using Eaf.Modules;
using Eaf.Str.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Eaf.Str.Migrator
{
    [DependsOn(typeof(ProjectNameEntityFrameworkCoreModule))]
    public class MigratorModule : EafModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public MigratorModule(ProjectNameEntityFrameworkCoreModule eafMiddlewareTemplateEntityFrameworkCoreModule)
        {
            eafMiddlewareTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(MigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            //ConnectionString Configurations
            var _environment = Environment.GetEnvironmentVariable("EafMigrator");
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(_environment);

            //Database Configurations
            Configuration.Database.IsOracleEnabled = Convert.ToBoolean(_appConfiguration["Database:IsOracleEnabled"]);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MigratorModule).GetAssembly());

            var services = new ServiceCollection();
            IdentityRegistrar.Register(services);
            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }
    }
}