using AutoMapper;
using Moq;
using SkeletonApi.Controllers;
using SkeletonApi.Service;
using System;
using Xunit;
using FizzWare.NBuilder;
using SkeletonApi.Api.Model;
using Microsoft.AspNetCore.Mvc;
using SkeletonApi.Entity;

namespace SkeletonApiTest.Api
{
    public class UserControllerTest : IDisposable
    {

        private Mock<IServiceUser> _serviceUser;
        private Mock<IMapper> _mapper;
        private UserController _controller;

        public UserControllerTest()
        {
            _serviceUser = new Mock<IServiceUser>();
            _mapper = new Mock<IMapper>();
            _controller = new UserController(_mapper.Object, _serviceUser.Object);
        }

        public void Dispose()
        {
            _serviceUser = null;
            _mapper = null;
            _controller = null;

        }

        [Fact]
        public void Test_Post_Ok()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _mapper.Setup(m => m.Map<EntityUser>(It.IsAny<UserDto>())).Returns(newUser);
            _serviceUser.Setup(s => s.Insert(It.IsAny<EntityUser>())).Returns(1);
            var result = _controller.Post(It.IsAny<UserDto>()) as CreatedResult;
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void Test_Post_BadRequest()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _mapper.Setup(m => m.Map<EntityUser>(It.IsAny<UserDto>())).Returns(newUser);
            _serviceUser.Setup(s => s.Insert(It.IsAny<EntityUser>())).Returns(0);
            var result = _controller.Post(It.IsAny<UserDto>()) as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Test_Get_Ok()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<EntityUser>())).Returns(newUser);
            var result = _controller.Get(It.IsAny<Guid>()) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Test_Get_NotFound()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            var result = _controller.Get(It.IsAny<Guid>()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Test_Put_Ok()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<EntityUser>())).Returns(newUser);
            _serviceUser.Setup(s => s.Update(It.IsAny<EntityUser>())).Returns(1);
            var result = _controller.Put(It.IsAny<Guid>(), Builder<UserDto>.CreateNew().Build()) as NoContentResult;
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void Test_Put_NotFound()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _serviceUser.Setup(s => s.Update(It.IsAny<EntityUser>())).Returns(1);
            var result = _controller.Put(It.IsAny<Guid>(), Builder<UserDto>.CreateNew().Build()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Test_Put_BadRequest()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<EntityUser>())).Returns(newUser);
            _serviceUser.Setup(s => s.Update(It.IsAny<EntityUser>())).Returns(0);
            var result = _controller.Put(It.IsAny<Guid>(), Builder<UserDto>.CreateNew().Build()) as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Test_Delete_Ok()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<EntityUser>())).Returns(newUser);
            var result = _controller.Delete(It.IsAny<Guid>()) as OkResult;
            Assert.Equal(200, result.StatusCode);
            _serviceUser.Verify(s => s.Remove(It.IsAny<EntityUser>()), Times.Once);
        }

        [Fact]
        public void Test_Delete_NotFound()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            var result = _controller.Delete(It.IsAny<Guid>()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
            _serviceUser.Verify(s => s.Remove(It.IsAny<EntityUser>()), Times.Never);
        }

        [Fact]
        public void Test_Authenticate_Ok()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            var userDto = Builder<UserDto>.CreateNew().Build();
            _mapper.Setup(m => m.Map<EntityUser>(It.IsAny<UserDto>())).Returns(newUser);
            _serviceUser.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(newUser);
            var result = _controller.Authenticate(userDto) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Test_Authenticate_NotFound()
        {
            var newUser = Builder<EntityUser>.CreateNew().Build();
            var userDto = Builder<UserDto>.CreateNew().Build();
            _mapper.Setup(m => m.Map<EntityUser>(It.IsAny<UserDto>())).Returns(newUser);
            var result = _controller.Authenticate(userDto) as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
        }

    }
}
