using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Eaf.Str.EntityFrameworkCore
{
    public static class StrDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<StrDbContext> builder, string connectionString, bool isOracleEnabled, bool isMySqlEnabled)
        {
            if (isOracleEnabled)
                builder.UseOracle(connectionString);
            else if (isMySqlEnabled)
                builder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
            else
                builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<StrDbContext> builder, DbConnection connection, bool isOracleEnabled, bool isMySqlEnabled)
        {
            if (isOracleEnabled)
                builder.UseOracle(connection);
            else if (isMySqlEnabled)
                builder.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 29)));
            else
                builder.UseSqlServer(connection);
        }
    }
}