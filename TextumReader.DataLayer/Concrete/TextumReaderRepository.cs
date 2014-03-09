using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using TextumReader.DataLayer.Abstract;

namespace TextumReader.DataLayer.Concrete
{
    public class TextumReaderRepository : IGenericRepository
    {
        private readonly DbContext _dbContext;
        public TextumReaderRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public IQueryable<T> Get<T>(Func<T, bool> filter) where T : class
        {
            return _dbContext.Set<T>().ToList().Where(filter).AsQueryable();
        }

        public T GetSingle<T>(Func<T, bool> filter) where T : class
        {
            return _dbContext.Set<T>().FirstOrDefault(filter);
        }

        public void Add<T>(T item) where T : class
        {
            _dbContext.Set<T>().Add(item);
        }

        public void Remove<T>(T item) where T : class
        {
            _dbContext.Set<T>().Remove(item);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
