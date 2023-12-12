using System;
using System.Transactions;
using Eaf.Dependency;
using Eaf.Domain.Uow;
using Eaf.EntityFrameworkCore.Uow;
using Eaf.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Str.Migrations.Seed.Host;
using Eaf.Str.Migrations.Seed.Tenants;

namespace Eaf.Str.Migrations.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<StrDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(StrDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            //Host seed
            new InitialHostDbBuilder(context).Create();

            //Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction) where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
            {
                var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);
                contextAction(context);
                uow.Complete();
            }
        }
    }
}
