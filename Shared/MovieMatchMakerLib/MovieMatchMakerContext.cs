using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib
{
    public class MovieMatchMakerContext : DbContext
    {
        private static string DbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "moviematchmaker.sqlite");
        //private static string DbPath => Path.Combine(Directory.GetCurrentDirectory(), "moviematchmaker.sqlite");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlite($"Data Source={DbPath}")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        public DbSet<Movie> Movies { get; set; }
        //public DbSet<Director> Directors { get; set; }
        //public DbSet<Actor> Actors { get; set; }
        //public DbSet<Writer> Writers { get; set; }
        //public DbSet
        public DbSet<PersonsMovieCredits> PersonsMovieCredits { get; set; }
        public DbSet<MoviesCredits> MoviesCredits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasKey(p => new { p.Title, p.ReleaseYear });
            //modelBuilder.Entity<Name>().HasKey(n => new { n.FirstName, n.Surname });
            //modelBuilder.Entity<ProductionMember>().HasKey(pm => pm.Name);

            base.OnModelCreating(modelBuilder);
        }
    }
}
