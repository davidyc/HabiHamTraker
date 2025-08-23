using HabiHamTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace HabiHamTracker.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<WeightEntry> WeightEntries => Set<WeightEntry>();
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeightEntry>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.WeightKg).IsRequired();
                e.Property(x => x.Date).IsRequired();
                e.HasIndex(x => x.Date);
            });
        }
    }
}
