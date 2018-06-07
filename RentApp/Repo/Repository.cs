using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RentApp.Repo
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
        
    {
        protected readonly DbContext Context;

       
        public Repository(DbContext applicationDbContext)
        {
            Context = applicationDbContext;
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().Where(expression);
        }

        public TEntity Get(int key)
        {
            return Context.Set<TEntity>().Find(key);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            //Context.Set<TEntity>().Attach(entity);
            //Context.Entry(entity).State = EntityState.Detached;
            //(Context as RADBContext).Entry<TEntity>(entity).State = EntityState.Modified;
            Context.Set<TEntity>().AddOrUpdate(entity);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().FirstOrDefault(expression);
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Context.Set<TEntity>().Any(expression);
        }
    }
}