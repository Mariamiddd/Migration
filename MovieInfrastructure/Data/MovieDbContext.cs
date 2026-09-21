using Microsoft.EntityFrameworkCore;
using MovieDomain.Entities;

namespace MovieInfrastructure.Data
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<StudioDetails> StudioDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=MovieMigration;Trusted_Connection=true;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies);

            modelBuilder.Entity<Movie>().Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Studio>()
                .HasOne(s => s.StudioDetails)
                .WithOne(sd => sd.Studio)
                .HasForeignKey<StudioDetails>(sd => sd.StudioId);

            modelBuilder.Entity<Studio>().Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100);

            modelBuilder.Entity<Studio>()
                .HasMany(s => s.Movies)
                .WithOne(m => m.Studio)
                .HasForeignKey(m => m.StudioId);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Studios)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryId);


            //data seeding 
            modelBuilder.Entity<Country>()
                .HasData(
                    new Country { Id = 1, Name = "USA" },
                    new Country { Id = 2, Name = "UK" },
                    new Country { Id = 3, Name = "France" }
                );
        }
    }
}