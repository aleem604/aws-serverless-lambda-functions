using System;
using System.Linq;

namespace TinCore.Domain.Interfaces
{
    public interface IRepositoryT<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        TEntity GetById(long id);        
        IQueryable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(long id);
        int SaveChanges();
    }
}
