using Eaf.Domain.Uow;
using Eaf.EntityFrameworkCore;
using Eaf.Middleware.EntityFrameworkCore;

namespace Eaf.Str.EntityFrameworkCore
{
    public class ProjectNameDbMigrator : EafMiddlewareDbMigrator<ProjectNameDbContext>
    {
        public ProjectNameDbMigrator(
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
