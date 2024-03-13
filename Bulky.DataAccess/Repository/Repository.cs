
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> set;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.set= _db.Set<T>();
            _db.Products.Include(u=>u.Category).Include(u=>u.CategoryId);
        }
        public void Add(T entity)
        {
            set.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = set;

            }
            else
            {
                query = set.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Category, CategoryId
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = set;

            if(filter!=null)
            {
                query=query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            { 
                foreach(var includeProp in includeProperties.Split(new Char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            set.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            set.RemoveRange(entities);
        }
    }
}
