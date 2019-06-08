using Microsoft.IdentityModel.Tokens;
using SkeletonApi.Data;
using SkeletonApi.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SkeletonApi.Service
{
    public class ServiceUser : IServiceUser
    {
        private readonly IRepository<EntityUser> _repository; 

        public ServiceUser(IRepository<EntityUser> repository)
        {
            _repository = repository;

            InitializeDefaultUser();
        }

        private void InitializeDefaultUser()
        {
            var newUser = new EntityUser { Email = "admin@admin.com", Name = "Admin", Password = "pass123" };
            var userRegistered = _repository.SelectAll().FirstOrDefault(x => x.Email == newUser.Email && x.Password == newUser.Password);

            if (userRegistered == null)
                Insert(newUser);
        }

        public int Remove(EntityUser user)
        {
            return _repository.Remove(user.Id);
        }

        public EntityUser Find(EntityUser user)
        {
            return _repository.Select(user.Id);
        }

        public int Insert(EntityUser user)
        {
            user.Id = Guid.NewGuid();
            return _repository.Insert(user);
        }

        public int Update(EntityUser user)
        {
            return _repository.Update(user);
        }

        public EntityUser Authenticate(string email, string password)
        {
            var user = _repository.SelectAll().FirstOrDefault(x => x.Email == email && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Environment.GetEnvironmentVariable("Secret");

            if (secretKey == null)
                secretKey = Guid.NewGuid().ToString();

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user;
        }

    }
}
