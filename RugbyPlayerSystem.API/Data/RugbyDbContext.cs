using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RugbyPlayerSystem.Models;
using System.ComponentModel;

namespace RugbyPlayerSystem.API.Data
{
    public class RugbyDbContext : DbContext
    {
        public RugbyDbContext(DbContextOptions<RugbyDbContext> options) : base(options)
        {

        }

        public class DateOnlyComparer : ValueComparer<DateOnly>
        {
            public DateOnlyComparer() : base(
                (d1, d2) => d1.DayNumber == d2.DayNumber,
                d => d.GetHashCode())
            {
            }
        }

        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
                    dateTime => DateOnly.FromDateTime(dateTime))
            {
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RugbySystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter, DateOnlyComparer>().HaveColumnType("date"); //DateOnly to date in database
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasData(new Player
            {
                PlayerId = 1,
                Name = "Player 1",
                BirthDate = new DateOnly(2000, 2, 12),
                Height = 190,
                Weight = 90,
                PlaceOfBirth = "Auckland",
                TeamName = "Team 1",
            });

            modelBuilder.Entity<Player>().HasData(new Player
            {
                PlayerId = 2,
                Name = "Player 2",
                BirthDate = new DateOnly(1994, 8, 15),
                Height = 180,
                Weight = 100,
                PlaceOfBirth = "Christchurch",
                TeamName = "Team 2",
            });

            modelBuilder.Entity<Player>().HasData(new Player
            {
                PlayerId = 3,
                Name = "Player 3",
                BirthDate = new DateOnly(1998, 4, 20),
                Height = 185,
                Weight = 79,
                PlaceOfBirth = "Otago",
                TeamName = "Team 3",
            });

            modelBuilder.Entity<Team>().HasData(new Team
            {
                Name = "Team 1",
                Ground = "Default",
                Coach = "Default",
                FoundedYear = 1988,
                Region = "Default",
                UnionName = "Nationl man's Union",
            });

            modelBuilder.Entity<Team>().HasData(new Team
            {
                Name = "Team 2",
                Ground = "Default",
                Coach = "Default",
                FoundedYear = 1975,
                Region = "Default",
                UnionName = "Nationl man's Union",
            });

            modelBuilder.Entity<Team>().HasData(new Team
            {
                Name = "Team 3",
                Ground = "Default",
                Coach = "Default",
                FoundedYear = 1963,
                Region = "Default",
                UnionName = "Nationl man's Union"
            });

            modelBuilder.Entity<Union>().HasData(new Union
            {
                Name = "Nationl man's Union",
            });

        }

        public DbSet<Union> Unions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
