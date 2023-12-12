using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Eaf.Str.EntityFrameworkCore
{
    public static class StrDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<StrDbContext> builder, string connectionString, bool isOracleEnabled)
        {
            if (isOracleEnabled)
                builder.UseOracle(connectionString);
            else
                builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<StrDbContext> builder, DbConnection connection, bool isOracleEnabled)
        {
            if (isOracleEnabled)
                builder.UseOracle(connection);
            else
                builder.UseSqlServer(connection);
        }
    }
}