﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Group>().HasKey(f => f.GroupId);
            builder.Entity<Event>().HasKey(f => f.Id);
            builder.Entity<GroupUser>().HasKey(f => new { f.GroupId, f.UserId });
            builder.Entity<Days>().HasKey(f => new { f.GroupId, f.UserId });

            builder.Entity<GroupUser>().HasOne(f => f.Group).WithMany(f => f.GroupUsers).HasForeignKey(f => f.GroupId);
            builder.Entity<GroupUser>().HasOne(f => f.User).WithMany(f => f.GroupUsers).HasForeignKey(f => f.UserId);

            builder.Entity<Days>().HasOne(f => f.Group).WithMany(f => f.Days).HasForeignKey(f => f.GroupId);
            builder.Entity<Days>().HasOne(f => f.User).WithMany(f => f.Days).HasForeignKey(f => f.UserId);

            builder.Entity<Event>().HasOne(foo => foo.Group).WithMany(foo => foo.Events);
            base.OnModelCreating(builder);
        }
    }

}
    

