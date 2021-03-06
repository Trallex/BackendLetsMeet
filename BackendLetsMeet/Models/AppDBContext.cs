﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FreeTime> FreeTime { get; set; }
        public DbSet<IsGoing> IsGoings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Group>().HasKey(f => f.GroupId);
            builder.Entity<Event>().HasKey(f => f.Id);
            builder.Entity<GroupUser>().HasKey(f => new { f.GroupId, f.UserId });
            builder.Entity<FreeTime>().HasKey(f => new {f.Id, f.GroupId, f.UserId });
            builder.Entity<IsGoing>().HasKey(f => new { f.EventId, f.UserId });

            builder.Entity<GroupUser>().HasOne(f => f.Group).WithMany(f => f.GroupUsers).HasForeignKey(f => f.GroupId);
            builder.Entity<GroupUser>().HasOne(f => f.User).WithMany(f => f.GroupUsers).HasForeignKey(f => f.UserId);

            builder.Entity<Event>().HasOne(foo => foo.Group).WithMany(foo => foo.Events);

            builder.Entity<FreeTime>().HasOne(f => f.Group).WithMany(f => f.FreeTimes).HasForeignKey(f => f.GroupId);
            builder.Entity<FreeTime>().HasOne(f => f.User).WithMany(f => f.FreeTime).HasForeignKey(f => f.UserId);

            builder.Entity<IsGoing>().HasOne(foo => foo.Event).WithMany(foo => foo.IsGoing);
            builder.Entity<IsGoing>().HasOne(foo => foo.User).WithMany(foo => foo.IsGoing);

            base.OnModelCreating(builder);
        }
    }

}
    

