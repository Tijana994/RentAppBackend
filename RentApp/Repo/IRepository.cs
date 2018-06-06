using System;
using System.Collections.Generic;

using System.Linq.Expressions;

namespace RentApp.Repo
{
    //prebaci na TPKey
    public interface IRepository<TEntity>
        where TEntity : class

    {
        TEntity Get(int key);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity,bool>> expression);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        bool Any(Expression<Func<TEntity, bool>> expression);
    }

    
}
