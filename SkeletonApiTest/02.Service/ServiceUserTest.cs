using Microsoft.EntityFrameworkCore;
using SkeletonApi.Api.Mapper;
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
        private UserProfile _userProfile;

        public ServiceUserTest()
        {
            DbContextOptions<DataContext> options;
            var b = new DbContextOptionsBuilder<DataContext>();
            b.UseInMemoryDatabase("DataUserMemoryTest", null);
            options = b.Options;
            _context = new DataContext(options);
            _serviceUser = new ServiceUser(_context);
            _userProfile = new UserProfile();
        }

        public void Dispose()
        {
            _serviceUser = null;
            _context = null;
            _userProfile = null;
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

        [Fact]
        public void Test_Authenticate_Null()
        {
            var user = new EntityUser { Id = Guid.NewGuid(), Email = "teste2@teste.com", Name = "Name", Password = "123" };
            var authenticated = _serviceUser.Authenticate(user.Email, user.Password);
            Assert.Null(authenticated);
        }

        [Fact]
        public void Test_Authenticate_Ok()
        {
            var user = new EntityUser { Id = Guid.NewGuid(), Email = "admin@admin.com", Name = "Name", Password = "pass123" };
            var authenticated = _serviceUser.Authenticate(user.Email, user.Password);
            Assert.NotNull(authenticated);
            Assert.NotNull(authenticated.Token);
        }

    }
}
