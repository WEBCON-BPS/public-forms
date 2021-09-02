using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Interface
{
    public interface IRepository<T> where T:class,new()
    {
        void Add(T model);
        bool Any();
        bool Any(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetFiltered(Expression<Func<T, bool>> predicate);
        void Delete(T model);
        void Update(T model);
    }
}
