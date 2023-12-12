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
    public class StrCoreModule : EafModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<StrAuthorizationProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<StrSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<StrNotificationProvider>();

            //Adding feature providers
            Configuration.Features.Providers.Add<StrFeatureProvider>();

            //Starting localization settings
            StrLocalizationConfigurer.Configure(Configuration.Localization);

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = StrConsts.MultiTenancyEnabled;

            if (StrDebugHelper.IsDebug)
            {
                //Disabling email/sms sending in debug mode
                Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
                Configuration.ReplaceService<ISmsSender, NullSmsSender>(DependencyLifeStyle.Transient);
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(StrCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<EafVirtualFileSystemOptions>().FileSets.AddEmbedded<StrCoreModule>();
        }
    }
}