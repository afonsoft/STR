using Eaf.Middleware.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace Eaf.Str.EntityFrameworkCore
{
    /* THIS CLASS IS NEEDED TO RUN "DOTNET EF ..." COMMANDS FROM COMMAND LINE ON DEVELOPMENT. NOT USED ANYWHERE ELSE */

    public class StrDbContextFactory : IDesignTimeDbContextFactory<StrDbContext>
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> _configurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();

        public StrDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<StrDbContext>();
            var configuration = GetConfigurationRoot();
            var isOracleEnabled = Convert.ToBoolean(configuration["Database:IsOracleEnabled"]);

            StrDbContextConfigurer.Configure(builder, configuration.GetConnectionString(StrConsts.ConnectionStringName), isOracleEnabled);

            return new StrDbContext(builder.Options);
        }

        public static IConfigurationRoot GetConfigurationRoot()
        {
            var path = CalculateContentRootFolder();
            return _configurationCache.GetOrAdd(path, _ => buildConfiguration(path));
        }

        private static IConfigurationRoot buildConfiguration(string path)
        {
            return AppConfigurations.Get(path);
        }

        private static string CalculateContentRootFolder()
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(StrDbContextFactory).Assembly.Location);
            if (coreAssemblyDirectoryPath == null)
            {
                throw new ArgumentNullException($"Could not find location of Suite.Docs.Core assembly!");
            }

            var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);

            while (!DirectoryContains(directoryInfo.FullName, "appsettings.json"))
            {
                if (directoryInfo.Parent == null)
                    throw new ArgumentNullException($"Could not find content root folder!");

                directoryInfo = directoryInfo.Parent;
            }

            return directoryInfo.FullName;
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }
}