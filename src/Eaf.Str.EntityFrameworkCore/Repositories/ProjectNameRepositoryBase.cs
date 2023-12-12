using Eaf.Domain.Entities;
using Eaf.EntityFrameworkCore;
using Eaf.EntityFrameworkCore.Repositories;
using Eaf.Str.EntityFrameworkCore;

namespace Eaf.Str.Repositories
{
    public abstract class ProjectNameRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<ProjectNameDbContext, TEntity, TPrimaryKey>
          where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public abstract class ProjectNameRepositoryBase<TEntity> : ProjectNameRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}