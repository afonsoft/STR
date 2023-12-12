using System;
using Eaf.Configuration.Startup;
using Eaf.Dependency;
using Eaf.Modules;
using Eaf.Net.Mail;
using Eaf.TestBase;
using Castle.MicroKernel.Registration;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Str.Test.Base.DependencyInjection;
using Eaf.Str.Test.Base.UiCustomization;
using Eaf.Str.Test.Base.Url;
using NSubstitute;
using Eaf.Middleware.Authorization.Users;
using Eaf.Middleware.MultiTenancy;
using Eaf.Middleware.Configuration;
using Eaf.Middleware.UiCustomization;
using Eaf.Middleware.Url;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Eaf.Str.Test.Base
{
    [DependsOn(
        typeof(StrApplicationModule),
        typeof(StrEntityFrameworkCoreModule),
        typeof(EafTestBaseModule))]
    public class StrTestBaseModule : EafModule
    {
        public StrTestBaseModule(StrEntityFrameworkCoreModule eafTemplateEntityFrameworkCoreModule)
        {
            eafTemplateEntityFrameworkCoreModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            //Use database for language management
            Configuration.Modules.Middleware().LanguageManagement.IsEnabled = true;

            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            RegisterFakeService<StrDbMigrator>();

            IocManager.RegisterIfNot<ILoggerFactory, LoggerFactory>();
            IocManager.RegisterIfNot<ILogger, NullLogger>();
            IocManager.RegisterIfNot(typeof(ILogger<>), typeof(Logger<>));

            IocManager.Register<IAppUrlService, FakeAppUrlService>();
            IocManager.Register<IWebUrlService, FakeWebUrlService>();

            Configuration.ReplaceService<IAppConfigurationAccessor, TestAppConfigurationAccessor>();
            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IUiThemeCustomizerFactory, NullUiThemeCustomizerFactory>();

            //Uncomment below line to write change logs for the entities below:
            Configuration.EntityHistory.IsEnabled = true;
            Configuration.EntityHistory.Selectors.Add("Eaf.Str", typeof(User), typeof(Tenant));
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>()
            where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }
    }
}