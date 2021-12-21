using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TinCore.Domain.Interfaces
{
    public interface IRepository : IDisposable
    {
        IDataContext DataContext { get; }

        T Add<T>(T entity) where T : class;
        IQueryable<T> All<T>() where T : class;
        void Attach<T>(T entity) where T : class;
        long Count<T>() where T : class;
        //long Count<T>(ISpecification<T> criteria) where T : class;
        long Count<T>(Expression<Func<T, bool>> criteria) where T : class;
        void Detach<T>(T entity) where T : class;
        //IList<T> Execute<T>(string command, ParamList parameters) where T : class;
        //T Find<T>(ISpecification<T> criteria) where T : class;
        T Find<T>(Expression<Func<T, bool>> criteria) where T : class;
        //IEnumerable<T> FindAll<T>(ISpecification<T> criteria, int offset = 0, int limit = 0, Expression<Func<T, object>> orderBy = null) where T : class;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> criteria, int offset = 0, int limit = 0, Expression<Func<T, object>> orderBy = null) where T : class;
        T GetById<T, ID>(ID id) where T : class;
        T Remove<T>(T entity) where T : class;
        T Remove<T, ID>(ID id) where T : class;
        T Update<T>(T entity) where T : class;
    }
}
