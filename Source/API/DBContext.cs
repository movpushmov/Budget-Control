using Microsoft.EntityFrameworkCore;
using Budget_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API
{
    class DBContext: DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventsGroup> EventsGroups { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        public DBContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Metadata.db");
        }
    }
}
