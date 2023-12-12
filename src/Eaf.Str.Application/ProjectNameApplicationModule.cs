using Eaf.AutoMapper;
using Eaf.Configuration.Startup;
using Eaf.Dependency;
using Eaf.Domain.Uow;
using Eaf.Hangfire;
using Eaf.Middleware;
using Eaf.Modules;
using Eaf.MultiTenancy;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Str.Migrations.Seed;
using Eaf.VirtualFileSystem;
using System;

namespace Eaf.Str
{
    [DependsOn(
        typeof(ProjectNameEntityFrameworkCoreModule),
        typeof(MiddlewareApplicationModule)
    )]
    public class ProjectNameApplicationModule : EafModule
    {
        public override void PreInitialize()
        {
            //Adding custom AutoMapper configuration
            Configuration.Modules.EafAutoMapper().Configurators.Add(ProjectNameCustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectNameApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<EafVirtualFileSystemOptions>().FileSets.AddEmbedded<ProjectNameApplicationModule>();
            using (var _connectionStringResolver = IocManager.ResolveAsDisposable<DefaultConnectionStringResolver>())
            {
                var hostConnStr = _connectionStringResolver.Object.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host));
                if (hostConnStr.IsNullOrWhiteSpace())
                {
                    Logger.Error("Configuration file should contain a connection string");
                    return;
                }

                using (var _startupConfiguration = IocManager.ResolveAsDisposable<IEafStartupConfiguration>())
                {
                    using (var _migrator = IocManager.ResolveAsDisposable<ProjectNameDbMigrator>())
                    {
                        Logger.Info("Database migration started...");
                        try
                        {
                            _migrator.Object.CreateOrMigrate(SeedHelper.SeedHostDb);
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorFormat(ex, "An error occured during hangfire migration: {0}", ex.Message);
                            Logger.Info("Canceled migrations.");
                            return;
                        }

                        if (_startupConfiguration.Object.Database.IsOracleEnabled)
                        {
                            Logger.Info("Hangfire migration started...");
                            try
                            {
                                EafHangfireOracleConfigurer.DataBaseConfigure(hostConnStr);
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorFormat(ex, "An error occured during hangfire migration: {0}", ex.Message);
                                Logger.Info("Canceled migrations Hangfire.");
                                return;
                            }
                        }

                        Logger.Info("Database migration completed.");
                    }
                }
            }
        }
    }
}