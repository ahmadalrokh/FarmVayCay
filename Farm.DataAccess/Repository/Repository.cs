using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Farm.DataAccess.Repository.IRepository;
using FarmVayCayTestOne.Data;
using Microsoft.EntityFrameworkCore;

namespace Farm.DataAccess.Repository
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> Dbset { get; set; }
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            Dbset = db.Set<T>();
        }
        public void Add(T entity)
        {
            Dbset.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includePropertes = null)
        {
            IQueryable<T> query = Dbset.Where(filter);

            if (!string.IsNullOrEmpty(includePropertes))
            {
                foreach (var property in includePropertes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return query.FirstOrDefault();
        }


        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includePropertes = null)
        {
            IQueryable<T> query = Dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includePropertes))
            {
                foreach (var propertes in includePropertes.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propertes);
                }
            }

            return query;
        }

        public void Remove(T entity)
        {
            Dbset.Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            Dbset.RemoveRange(entities);
        }
    }
}
