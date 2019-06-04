using Microsoft.EntityFrameworkCore;
using SkeletonApi.Data;
using SkeletonApi.Entity;
using SkeletonApi.Service;
using System;
using Xunit;

namespace SkeletonApiTest.Service
{
    public class ServiceUserTest : IDisposable
    {

        private ServiceUser _serviceUser;
        private DataContext _context;

        public ServiceUserTest()
        {
            DbContextOptions<DataContext> options;
            var b = new DbContextOptionsBuilder<DataContext>();
            b.UseInMemoryDatabase("DataUserMemoryTest", null);
            options = b.Options;
            _context = new DataContext(options);
            _serviceUser = new ServiceUser(_context);
        }

        public void Dispose()
        {
            _serviceUser = null;
            _context = null;
        }

        [Fact]
        public void Test_Insert_Ok()
        {
            var user = new EntityUser { Id = Guid.NewGuid(), Email = "teste@teste.com", Name = "Name", Password = "123" };
            Assert.Equal(1, _serviceUser.Insert(user));
        }

        [Fact]
        public void Test_Update_Ok()
        {
            var user = new EntityUser { Id = Guid.NewGuid(), Email = "teste@teste.com", Name = "Name", Password = "123" };
            _serviceUser.Insert(user);
            user.Name = "Name2";
            Assert.Equal(1, _serviceUser.Update(user));
            Assert.Equal("Name2", _serviceUser.Find(user).Name);
        }

        [Fact]
        public void Test_Delete_Ok()
        {
            var user = new EntityUser { Id = Guid.NewGuid(), Email = "teste@teste.com", Name = "Name", Password = "123" };
            _serviceUser.Insert(user);
            Assert.Equal(1, _serviceUser.Remove(user));
            Assert.Null(_serviceUser.Find(user));
        }
    }
}
