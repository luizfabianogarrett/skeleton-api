using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkeletonApi.Api.Model;
using SkeletonApi.Entity;
using SkeletonApi.Service;
using System;

namespace SkeletonApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IServiceUser _serviceUser;

        public UserController(IMapper mapper, IServiceUser serviceUser)
        {
            _mapper = mapper;
            _serviceUser = serviceUser;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _mapper.Map<EntityUser>(new UserDto { Id = id });

            var userDb = _serviceUser.Find(user);

            if (userDb == null)
                return NotFound();

            return Ok(_mapper.Map<UserDto>(userDb));
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserDto user)
        {
            var u = _mapper.Map<EntityUser>(user);

            if (_serviceUser.Insert(u) == 1)
                return new CreatedResult($"api/user/{u.Id}", new { u.Id });

            return BadRequest();

        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]UserDto user)
        {
            var userDb = _serviceUser.Find(_mapper.Map<EntityUser>(new UserDto { Id = id }));

            if (userDb == null)
                return NotFound();

            userDb.Name = user.Name;
            userDb.Password = user.Password;
            userDb.Email = user.Email;

            if (_serviceUser.Update(userDb) == 1)
                return NoContent();

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _mapper.Map<EntityUser>(new UserDto { Id = id });

            var userDb = _serviceUser.Find(user);

            if (userDb == null)
                return NotFound();

            _serviceUser.Remove(userDb);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userParam)
        {
            var user = _serviceUser.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }


    }
}
