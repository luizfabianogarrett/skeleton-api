using System;
using System.ComponentModel.DataAnnotations;

namespace SkeletonApi.Entity
{
    public class EntityUser
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
