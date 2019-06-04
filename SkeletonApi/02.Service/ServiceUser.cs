using SkeletonApi.Data;
using SkeletonApi.Entity;
using System;
using System.Linq;

namespace SkeletonApi.Service
{
    public class ServiceUser : IServiceUser
    {
        private readonly DataContext _context;

        public ServiceUser(DataContext context)
        {
            _context = context;
        }

        public int Remove(User user)
        {
            _context.Users.Remove(user);
            return _context.SaveChanges();
        }

        public User Find(User user)
        {
            return _context.Users.FirstOrDefault(s => s.Id == user.Id);
        }

        public int Insert(User user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            return _context.SaveChanges();
        }

        public int Update(User user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges();
        }
    }
}
