using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Farm.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public T Get(Expression<Func<T, bool>> filter, string? includePropertes = null);
        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includePropertes = null);
        public void Add(T entity);
        public void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
