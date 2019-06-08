using System;
using System.ComponentModel.DataAnnotations;

namespace SkeletonApi.Entity
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
