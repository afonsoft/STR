using Eaf.Authorization.Users;
using Eaf.Events.Bus;
using Eaf.Events.Bus.Entities;
using Eaf.Middleware.Authorization.Roles;
using Eaf.Middleware.Authorization.Users;
using Eaf.Middleware.MultiTenancy;
using Eaf.Modules;
using Eaf.MultiTenancy;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Str.Test.Base.TestData;
using Eaf.Runtime.Session;
using Eaf.TestBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eaf.Str.Test.Base
{
    /// <summary>
    /// This is base class for all our test classes.
    /// It prepares ABP system, modules and a fake, in-memory database.
    /// Seeds database with initial data.
    /// Provides methods to easily work with <see cref="ProjectNameDbContext"/>.
    /// </summary>
    public abstract class AppTestBase<T> : EafIntegratedTestBase<T> where T : EafModule
    {
        protected AppTestBase()
        {
            SeedTestData();
            LoginAsDefaultTenantAdmin();
        }

        private void SeedTestData()
        {
            void NormalizeDbContext(ProjectNameDbContext context)
            {
                context.EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
                context.EventBus = NullEventBus.Instance;
                context.SuppressAutoSetTenantId = true;
            }

            //Seed initial data for default tenant
            EafSession.TenantId = 1;

            UsingDbContext(context =>
            {
                NormalizeDbContext(context);
                new TestDataBuilder(context, 1).Create();
            });
        }

        protected IDisposable UsingTenantId(int? tenantId)
        {
            var previousTenantId = EafSession.TenantId;
            EafSession.TenantId = tenantId;
            return new DisposeAction(() => EafSession.TenantId = previousTenantId);
        }

        protected void UsingDbContext(Action<ProjectNameDbContext> action)
        {
            UsingDbContext(EafSession.TenantId, action);
        }

        protected Task UsingDbContextAsync(Func<ProjectNameDbContext, Task> action)
        {
            return UsingDbContextAsync(EafSession.TenantId, action);
        }

        protected TResult UsingDbContext<TResult>(Func<ProjectNameDbContext, TResult> func)
        {
            return UsingDbContext(EafSession.TenantId, func);
        }

        protected Task<TResult> UsingDbContextAsync<TResult>(Func<ProjectNameDbContext, Task<TResult>> func)
        {
            return UsingDbContextAsync(EafSession.TenantId, func);
        }

        protected void UsingDbContext(int? tenantId, Action<ProjectNameDbContext> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<ProjectNameDbContext>())
                {
                    action(context);
                    context.SaveChanges();
                }
            }
        }

        protected async Task UsingDbContextAsync(int? tenantId, Func<ProjectNameDbContext, Task> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<ProjectNameDbContext>())
                {
                    await action(context);
                    await context.SaveChangesAsync();
                }
            }
        }

        protected TResult UsingDbContext<TResult>(int? tenantId, Func<ProjectNameDbContext, TResult> func)
        {
            TResult result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<ProjectNameDbContext>())
                {
                    result = func(context);
                    context.SaveChanges();
                }
            }

            return result;
        }

        protected async Task<TResult> UsingDbContextAsync<TResult>(int? tenantId, Func<ProjectNameDbContext, Task<TResult>> func)
        {
            TResult result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<ProjectNameDbContext>())
                {
                    result = await func(context);
                    await context.SaveChangesAsync();
                }
            }

            return result;
        }

        #region Login

        protected void LoginAsHostAdmin()
        {
            LoginAsHost(EafUserBase.AdminUserName);
        }

        protected void LoginAsDefaultTenantAdmin()
        {
            LoginAsTenant(EafTenantBase.DefaultTenantName, EafUserBase.AdminUserName);
        }

        protected void LoginAsHost(string userName)
        {
            EafSession.TenantId = null;

            var user = UsingDbContext(context => context.Users.FirstOrDefault(u => u.TenantId == EafSession.TenantId && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for host.");
            }

            EafSession.UserId = user.Id;
        }

        protected void LoginAsTenant(string tenancyName, string userName)
        {
            EafSession.TenantId = null;

            var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
            if (tenant == null)
            {
                throw new Exception("There is no tenant: " + tenancyName);
            }

            EafSession.TenantId = tenant.Id;

            var user = UsingDbContext(context => context.Users.FirstOrDefault(u => u.TenantId == EafSession.TenantId && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for tenant: " + tenancyName);
            }

            EafSession.UserId = user.Id;
        }

        #endregion Login

        #region GetCurrentUser

        /// <summary>
        /// Gets current user if <see cref="IEafSession.UserId"/> is not null.
        /// Throws exception if it's null.
        /// </summary>
        protected User GetCurrentUser()
        {
            var userId = EafSession.GetUserId();
            return UsingDbContext(context => context.Users.Single(u => u.Id == userId));
        }

        /// <summary>
        /// Gets current user if <see cref="IEafSession.UserId"/> is not null.
        /// Throws exception if it's null.
        /// </summary>
        protected async Task<User> GetCurrentUserAsync()
        {
            var userId = EafSession.GetUserId();
            return await UsingDbContext(context => context.Users.SingleAsync(u => u.Id == userId));
        }

        #endregion GetCurrentUser

        #region GetCurrentTenant

        /// <summary>
        /// Gets current tenant if <see cref="IEafSession.TenantId"/> is not null.
        /// Throws exception if there is no current tenant.
        /// </summary>
        protected Tenant GetCurrentTenant()
        {
            var tenantId = EafSession.GetTenantId();
            return UsingDbContext(null, context => context.Tenants.Single(t => t.Id == tenantId));
        }

        /// <summary>
        /// Gets current tenant if <see cref="IEafSession.TenantId"/> is not null.
        /// Throws exception if there is no current tenant.
        /// </summary>
        protected async Task<Tenant> GetCurrentTenantAsync()
        {
            var tenantId = EafSession.GetTenantId();
            return await UsingDbContext(null, context => context.Tenants.SingleAsync(t => t.Id == tenantId));
        }

        #endregion GetCurrentTenant

        #region GetTenant / GetTenantOrNull

        protected Tenant GetTenant(string tenancyName)
        {
            return UsingDbContext(null, context => context.Tenants.Single(t => t.TenancyName == tenancyName));
        }

        protected async Task<Tenant> GetTenantAsync(string tenancyName)
        {
            return await UsingDbContext(null, async context => await context.Tenants.SingleAsync(t => t.TenancyName == tenancyName));
        }

        protected Tenant GetTenantOrNull(string tenancyName)
        {
            return UsingDbContext(null, context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
        }

        protected async Task<Tenant> GetTenantOrNullAsync(string tenancyName)
        {
            return await UsingDbContext(null, async context => await context.Tenants.FirstOrDefaultAsync(t => t.TenancyName == tenancyName));
        }

        #endregion GetTenant / GetTenantOrNull

        #region GetRole

        protected Role GetRole(string roleName)
        {
            return UsingDbContext(context => context.Roles.Single(r => r.Name == roleName && r.TenantId == EafSession.TenantId));
        }

        protected async Task<Role> GetRoleAsync(string roleName)
        {
            return await UsingDbContext(async context => await context.Roles.SingleAsync(r => r.Name == roleName && r.TenantId == EafSession.TenantId));
        }

        #endregion GetRole

        #region GetUserByUserName

        protected User GetUserByUserName(string userName)
        {
            var user = GetUserByUserNameOrNull(userName);
            if (user == null)
            {
                throw new Exception("Can not find a user with username: " + userName);
            }

            return user;
        }

        protected async Task<User> GetUserByUserNameAsync(string userName)
        {
            var user = await GetUserByUserNameOrNullAsync(userName);
            if (user == null)
            {
                throw new Exception("Can not find a user with username: " + userName);
            }

            return user;
        }

        protected User GetUserByUserNameOrNull(string userName)
        {
            return UsingDbContext(context =>
                context.Users.FirstOrDefault(u =>
                    u.UserName == userName &&
                    u.TenantId == EafSession.TenantId
                    ));
        }

        protected async Task<User> GetUserByUserNameOrNullAsync(string userName, bool includeRoles = false)
        {
            return await UsingDbContextAsync(async context =>
                await context.Users
                    .IncludeIf(includeRoles, u => u.Roles)
                    .FirstOrDefaultAsync(u =>
                            u.UserName == userName &&
                            u.TenantId == EafSession.TenantId
                    ));
        }

        #endregion GetUserByUserName
    }
}