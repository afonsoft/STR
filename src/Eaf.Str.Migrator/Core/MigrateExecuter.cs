using Eaf.Configuration.Startup;
using Eaf.Dependency;
using Eaf.Domain.Uow;
using Eaf.Hangfire;
using Eaf.MultiTenancy;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Str.Migrations.Seed;
using System;

namespace Eaf.Str.Migrator
{
    public class MigrateExecuter : ITransientDependency
    {
        public Log Log { get; private set; }

        private readonly StrDbMigrator _migrator;
        private readonly IEafStartupConfiguration _startupConfiguration;
        private readonly DefaultConnectionStringResolver _connectionStringResolver;

        public MigrateExecuter(
            Log log,
            StrDbMigrator migrator,
            IEafStartupConfiguration startupConfiguration,
            DefaultConnectionStringResolver connectionStringResolver
        )
        {
            Log = log;
            _migrator = migrator;
            _startupConfiguration = startupConfiguration;
            _connectionStringResolver = connectionStringResolver;
        }

        public void Run(bool skipConnVerification, bool isDockerEnabled = false)
        {
            var hostConnStr = _connectionStringResolver.GetNameOrConnectionString(new ConnectionStringResolveArgs(MultiTenancySides.Host));
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                Log.Write("Configuration file should contain a connection string named 'LOCAL'");
                return;
            }

            Log.Write("Database: " + Environment.GetEnvironmentVariable("EafMigrator"));
            if (!skipConnVerification && !isDockerEnabled)
            {
                Log.Write("Continue to migration for database? (Y/N): ", false);
                var command = Console.ReadLine();
                if (!command.IsIn("Y", "y"))
                {
                    Log.Write("Migration canceled.");
                    return;
                }
            }

            Log.Write("Database migration started...");
            try
            {
                _migrator.CreateOrMigrate(SeedHelper.SeedHostDb);
            }
            catch (Exception ex)
            {
                Log.Write("An error occured during database migration:");
                Log.Write(ex.ToString());
                Log.Write("Canceled migrations.");
                Console.ReadKey();
                throw;
            }

            if (_startupConfiguration.Database.IsOracleEnabled)
            {
                Log.Write("Hangfire migration started...");
                try
                {
                    EafHangfireOracleConfigurer.DataBaseConfigure(hostConnStr);
                }
                catch (Exception ex)
                {
                    Log.Write("An error occured during hangfire migration:");
                    Log.Write(ex.ToString());
                    Log.Write("Canceled migrations.");
                    throw;
                }
            }

            Log.Write("Database migration completed.");
            Log.Write("--------------------------------------------------------");
        }
    }
}