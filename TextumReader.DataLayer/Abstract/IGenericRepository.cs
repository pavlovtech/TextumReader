using System;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace TextumReader.DataLayer.Abstract
{
    public interface IGenericRepository: IDisposable
    {
        IQueryable<T> Get<T>() where T: class;

        IQueryable<T> Get<T>(Func<T, bool> filter) where T : class;

        T GetSingle<T>(Func<T, bool> filter) where T : class;

        void Add<T>(T item) where T : class;

        void Remove<T>(T item) where T: class;

        void SaveChanges();
    }
}
