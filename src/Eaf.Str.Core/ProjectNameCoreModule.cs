using Eaf.Configuration.Startup;
using Eaf.Dependency;
using Eaf.Middleware;
using Eaf.Modules;
using Eaf.Net.Mail;
using Eaf.Net.Sms;
using Eaf.Str.Authorization;
using Eaf.Str.Configuration;
using Eaf.Str.Debugging;
using Eaf.Str.Features;
using Eaf.Str.Localization;
using Eaf.Str.Notifications;
using Eaf.VirtualFileSystem;
using System;

namespace Eaf.Str
{
    [DependsOn(
        typeof(MiddlewareCoreModule))
    ]
    public class ProjectNameCoreModule : EafModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<ProjectNameAuthorizationProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<ProjectNameSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<ProjectNameNotificationProvider>();

            //Adding feature providers
            Configuration.Features.Providers.Add<ProjectNameFeatureProvider>();

            //Starting localization settings
            ProjectNameLocalizationConfigurer.Configure(Configuration.Localization);

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = ProjectNameConsts.MultiTenancyEnabled;

            if (ProjectNameDebugHelper.IsDebug)
            {
                //Disabling email/sms sending in debug mode
                Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
                Configuration.ReplaceService<ISmsSender, NullSmsSender>(DependencyLifeStyle.Transient);
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectNameCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<EafVirtualFileSystemOptions>().FileSets.AddEmbedded<ProjectNameCoreModule>();
        }
    }
}