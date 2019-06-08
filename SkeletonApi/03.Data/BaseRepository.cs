using SkeletonApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonApi.Data
{
    public class BaseRepository<T> : IRepository<T> where T : EntityBase
    {
        private DataContext context;

        public BaseRepository(DataContext dtc)
        {
            context = dtc;
        }

        public int Insert(T obj)
        {
            context.Set<T>().Add(obj);
            return context.SaveChanges();
        }

        public int Update(T obj)
        {
            context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return context.SaveChanges();
        }

        public int Delete(Guid id)
        {
            context.Set<T>().Remove(Select(id));
            return context.SaveChanges();
        }

        public IList<T> Select()
        {
            return context.Set<T>().ToList();
        }

        public T Select(Guid id)
        {
            return context.Set<T>().Find(id);
        }

        public int Remove(Guid id)
        {
            context.Remove(Select(id));
            return context.SaveChanges();
        }

        public IList<T> SelectAll()
        {
            return context.Set<T>().ToList();
        }
    }
}
