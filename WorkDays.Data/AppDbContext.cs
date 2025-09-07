using Microsoft.EntityFrameworkCore;
using WorkDays.Data.Models;

namespace WorkDays.Data;

public class AppDbContext : DbContext
{
       // private const string ConnectionString = "Data Source=app.db";
        public DbSet<WorkDay> WorkDays { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
      
        modelBuilder.Entity<WorkDay>(entity =>
            {
                entity.HasKey(e => e.WorkDayId);
                
                entity.Property(e => e.Date).IsRequired()
                .HasConversion(
                    v => v, // Store as UTC
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Retrieve as UTC
                );
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
                entity.Property(e => e.Break).IsRequired();
                entity.Property(e => e.IsHoliday)
                    .IsRequired()
                    .HasDefaultValue(false);
                entity.Property(e => e.Type)
                    .IsRequired();
                entity.Property(e => e.TotalHours)
                    .HasComputedColumnSql("(\"EndTime\" - \"StartTime\" - \"Break\")", stored:true);
            });
    }
}
