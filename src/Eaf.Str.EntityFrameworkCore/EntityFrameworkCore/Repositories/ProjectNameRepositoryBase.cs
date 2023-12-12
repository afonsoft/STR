using Eaf.Domain.Entities;
using Eaf.EntityFrameworkCore;
using Eaf.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eaf.Str.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class ProjectNameRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<ProjectNameDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="NewTemplateRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class ProjectNameRepositoryBase<TEntity> : ProjectNameRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ProjectNameRepositoryBase(IDbContextProvider<ProjectNameDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}