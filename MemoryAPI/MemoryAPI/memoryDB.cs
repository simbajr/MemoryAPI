using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using MemoryAPI.Models;




//skapad av Simba
namespace MemoryAPI
{
    public class memoryDB : IdentityDbContext<IdentityUser>
    {
        public DbSet<User> User { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<VideoMedia> Video { get; set; }
        public DbSet<PictureMedia> Picture { get; set; }
        public DbSet<SoundMedia> Sound { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasMany(u => u.friendList).WithMany();
        }

        public memoryDB()
            : base("memoryDB")
        {

        }
    }


}