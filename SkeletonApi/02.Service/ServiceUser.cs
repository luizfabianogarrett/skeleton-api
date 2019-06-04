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
        private readonly DataContext _context;

        public ServiceUser(DataContext context)
        {
            _context = context;

            InitializeDefaultUser();
        }

        private void InitializeDefaultUser()
        {
            var newUser = new EntityUser { Email = "admin@admin.com", Name = "Admin", Password = "pass123" };
            var userRegistered = _context.Users.FirstOrDefault(x => x.Email == newUser.Email && x.Password == newUser.Password);

            if (userRegistered == null)
                Insert(newUser);
        }

        public int Remove(EntityUser user)
        {
            _context.Users.Remove(user);
            return _context.SaveChanges();
        }

        public EntityUser Find(EntityUser user)
        {
            return _context.Users.FirstOrDefault(s => s.Id == user.Id);
        }

        public int Insert(EntityUser user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            return _context.SaveChanges();
        }

        public int Update(EntityUser user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges();
        }

        public EntityUser Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("Secret"));
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
