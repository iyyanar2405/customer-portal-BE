using CustomerPortal.MasterService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.MasterService.Data
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
        {
        }

        // DbSets for Master entities
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Clause> Clauses { get; set; }
        public DbSet<FocusArea> FocusAreas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(co => co.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Site>()
                .HasOne(s => s.Company)
                .WithMany(c => c.Sites)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Site>()
                .HasOne(s => s.City)
                .WithMany()
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Clause>()
                .HasOne(cl => cl.Chapter)
                .WithMany(ch => ch.Clauses)
                .HasForeignKey(cl => cl.ChapterId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure indexes for better performance
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.CountryName)
                .IsUnique();

            modelBuilder.Entity<Country>()
                .HasIndex(c => c.CountryCode)
                .IsUnique();

            modelBuilder.Entity<City>()
                .HasIndex(c => new { c.CityName, c.CountryId })
                .IsUnique();

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.CompanyName);

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.CompanyCode)
                .IsUnique();

            modelBuilder.Entity<Site>()
                .HasIndex(s => new { s.SiteName, s.CompanyId })
                .IsUnique();

            modelBuilder.Entity<Site>()
                .HasIndex(s => s.SiteCode)
                .IsUnique();

            modelBuilder.Entity<Service>()
                .HasIndex(s => s.ServiceName)
                .IsUnique();

            modelBuilder.Entity<Service>()
                .HasIndex(s => s.ServiceCode)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<Chapter>()
                .HasIndex(ch => ch.ChapterNumber)
                .IsUnique();

            modelBuilder.Entity<Clause>()
                .HasIndex(cl => cl.ClauseNumber)
                .IsUnique();

            modelBuilder.Entity<FocusArea>()
                .HasIndex(f => f.FocusAreaName)
                .IsUnique();
        }
    }
}