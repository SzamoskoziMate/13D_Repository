using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    using Microsoft.EntityFrameworkCore;

    public class Drawing
    {
        public int Id { get; set; }
        public string Name { get; set; } // A rajz neve
        public ICollection<ConsoleCharacter> ConsoleCharacters { get; set; }
    }

    public class ConsoleCharacter
    {
        public int Id { get; set; }
        public char Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public string Szín { get; set; }

        public int DrawingId { get; set; }
        public Drawing Drawing { get; set; }
    }


    public class DrawingDbContext : DbContext
    {
        public DbSet<ConsoleCharacter> ConsoleCharacters { get; set; }
        public DbSet<Drawing> Drawings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ConsoleCharacter>()
                .HasOne(c => c.Drawing)
                .WithMany(d => d.ConsoleCharacters)
                .HasForeignKey(c => c.DrawingId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=YOUR_SERVER_NAME;Database=DrawingDB;Trusted_Connection=True;");
        }
    }
}

