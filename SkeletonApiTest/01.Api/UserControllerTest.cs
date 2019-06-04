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
            var newUser = Builder<User>.CreateNew().Build();
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(newUser);
            _serviceUser.Setup(s => s.Insert(It.IsAny<User>())).Returns(1);
            var result = _controller.Post(It.IsAny<UserDto>()) as CreatedResult;
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public void Test_Post_BadRequest()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _mapper.Setup(m => m.Map<User>(It.IsAny<UserDto>())).Returns(newUser);
            _serviceUser.Setup(s => s.Insert(It.IsAny<User>())).Returns(0);
            var result = _controller.Post(It.IsAny<UserDto>()) as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Test_Get_Ok()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<User>())).Returns(newUser);
            var result = _controller.Get(It.IsAny<Guid>()) as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void Test_Get_NotFound()
        {
            var newUser = Builder<User>.CreateNew().Build();
            var result = _controller.Get(It.IsAny<Guid>()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Test_Put_Ok()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<User>())).Returns(newUser);
            _serviceUser.Setup(s => s.Update(It.IsAny<User>())).Returns(1);
            var result = _controller.Put(It.IsAny<Guid>(), Builder<UserDto>.CreateNew().Build()) as NoContentResult;
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void Test_Put_NotFound()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.Update(It.IsAny<User>())).Returns(1);
            var result = _controller.Put(It.IsAny<Guid>(), Builder<UserDto>.CreateNew().Build()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void Test_Put_BadRequest()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<User>())).Returns(newUser);
            _serviceUser.Setup(s => s.Update(It.IsAny<User>())).Returns(0);
            var result = _controller.Put(It.IsAny<Guid>(), Builder<UserDto>.CreateNew().Build()) as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void Test_Delete_Ok()
        {
            var newUser = Builder<User>.CreateNew().Build();
            _serviceUser.Setup(s => s.Find(It.IsAny<User>())).Returns(newUser);
            var result = _controller.Delete(It.IsAny<Guid>()) as OkResult;
            Assert.Equal(200, result.StatusCode);
            _serviceUser.Verify(s => s.Remove(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void Test_Delete_NotFound()
        {
            var newUser = Builder<User>.CreateNew().Build();
            var result = _controller.Delete(It.IsAny<Guid>()) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
            _serviceUser.Verify(s => s.Remove(It.IsAny<User>()), Times.Never);
        }

    }
}
