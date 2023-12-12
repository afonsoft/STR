using Eaf.Dependency;
using Eaf.EntityFrameworkCore.Configuration;
using Eaf.Middleware.EntityFrameworkCore;
using Eaf.Modules;
using Eaf.Str.EntityHistory;
using Eaf.Str.Migrations.Seed;
using Microsoft.Extensions.Logging;
using System;

namespace Eaf.Str.EntityFrameworkCore
{
    [DependsOn(
        typeof(EafMiddlewareCoreEntityFrameworkCoreModule),
        typeof(StrCoreModule)
    )]
    public class StrEntityFrameworkCoreModule : EafModule
    {
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                if (Configuration.Database.IsOracleEnabled)
                {
                    //With Wallet - Oracle Autonomous Database
                    // Set TnsAdmin value to directory location of tnsnames.ora and sqlnet.ora files
                    //Oracle.ManagedDataAccess.Client.OracleConfiguration.TnsAdmin = @"<DIRECTORY LOCATION>";

                    // Set WalletLocation value to directory location of the ADB wallet (i.e. cwallet.sso)
                    //Oracle.ManagedDataAccess.Client.OracleConfiguration.WalletLocation = @"<DIRECTORY LOCATION>";
                }

                Configuration.Modules.EafEfCore().AddDbContext<StrDbContext>(options =>
                {
                    options.DbContextOptions.EnableDetailedErrors(Configuration.Database.EnableDetailedErrors);
                    options.DbContextOptions.EnableSensitiveDataLogging(Configuration.Database.EnableSensitiveDataLogging);

                    if (Configuration.Database.EnableDetailedErrors && Configuration.IocManager.IsRegistered<ILoggerFactory>())
                    {
                        options.DbContextOptions.UseLoggerFactory(Configuration.IocManager.Resolve<ILoggerFactory>());
                    }

                    if (options.ExistingConnection != null)
                        StrDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection, Configuration.Database.IsOracleEnabled);
                    else
                        StrDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString, Configuration.Database.IsOracleEnabled);
                });
            }

            Configuration.EntityHistory.Selectors.Add("StrEntities", EntityHistoryHelper.TrackedTypes);
            Configuration.CustomConfigProviders.Add(new EntityHistoryConfigProvider(Configuration));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(StrEntityFrameworkCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            using (var scope = IocManager.CreateScope())
            {
                if (!SkipDbSeed)
                {
                    SeedHelper.SeedHostDb(IocManager);
                }
            }
        }
    }
}