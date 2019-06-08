using SkeletonApi.Entity;
using System;
using System.Collections.Generic;

namespace SkeletonApi.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        int Insert(T obj);

        int Update(T obj);

        int Remove(Guid id);

        T Select(Guid id);

        IList<T> SelectAll();
    }
}
