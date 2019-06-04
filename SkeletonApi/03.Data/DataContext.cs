﻿using Microsoft.EntityFrameworkCore;
using SkeletonApi.Entity;

namespace SkeletonApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<EntityUser> Users { get; set; }
    }
}
