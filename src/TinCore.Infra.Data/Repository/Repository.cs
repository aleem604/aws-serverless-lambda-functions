using System;
using System.Linq;
using TinCore.Domain.Interfaces;
using TinCore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace TinCore.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepositoryT<TEntity> where TEntity : class
    {
        protected readonly TinCoreContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(TinCoreContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public virtual TEntity GetById(long id)
        {
            return DbSet.Find(id);
        }
        
        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual void Remove(long id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
