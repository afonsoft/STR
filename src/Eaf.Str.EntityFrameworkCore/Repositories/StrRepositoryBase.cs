using Eaf.Domain.Entities;
using Eaf.EntityFrameworkCore;
using Eaf.EntityFrameworkCore.Repositories;
using Eaf.Str.EntityFrameworkCore;

namespace Eaf.Str.Repositories
{
    public abstract class StrRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<StrDbContext, TEntity, TPrimaryKey>
          where TEntity : class, IEntity<TPrimaryKey>
    {
        protected StrRepositoryBase(IDbContextProvider<StrDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public abstract class StrRepositoryBase<TEntity> : StrRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected StrRepositoryBase(IDbContextProvider<StrDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}