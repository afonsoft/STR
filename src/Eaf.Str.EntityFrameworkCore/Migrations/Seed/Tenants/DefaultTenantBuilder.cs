using System.Linq;
using Eaf.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Eaf.Str.EntityFrameworkCore;
using Eaf.Middleware.MultiTenancy;

namespace Eaf.Str.Migrations.Seed.Tenants
{
    public class DefaultTenantBuilder
    {
        private readonly ProjectNameDbContext _context;

        public DefaultTenantBuilder(
            ProjectNameDbContext context
        )
        {
            _context = context;
        }

        public void Create()
        {
            var defaultTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == EafTenantBase.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant(EafTenantBase.DefaultTenantName, EafTenantBase.DefaultTenantName);
                _context.Tenants.Add(defaultTenant);
                _context.SaveChanges();
            }
        }

    }
}
