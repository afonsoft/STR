using Eaf.Domain.Uow;
using Eaf.EntityFrameworkCore;
using Eaf.Middleware.EntityFrameworkCore;

namespace Eaf.Str.EntityFrameworkCore
{
    public class StrDbMigrator : EafMiddlewareDbMigrator<StrDbContext>
    {
        public StrDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbContextResolver dbContextResolver,
            DefaultConnectionStringResolver connectionStringResolver
        ) : base(
            unitOfWorkManager,
            dbContextResolver,
            connectionStringResolver
        )
        {}
    }
}
