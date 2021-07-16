using Microsoft.EntityFrameworkCore;
using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salary_Control.Source.API
{
    class DBContext: DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventsGroup> EventsGroups { get; set; }

        public DBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Metadata.db");
        }
    }
}
