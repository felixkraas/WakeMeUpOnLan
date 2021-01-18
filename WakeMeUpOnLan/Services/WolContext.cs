using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WakeMeUpOnLan.Core.Models;

namespace WakeMeUpOnLan.Services {
    public class WolContext : DbContext {

        public DbSet<WolTarget> WolTargets {
            get;
            set;
        }

        public DbSet<ApiUser> ApiUsers {
            get;
            set;
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            optionsBuilder.UseSqlite( "Data Source=WakeMeUp.db" );
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<WolTarget>();
            modelBuilder.Entity<ApiUser>();
        }

    }
}
