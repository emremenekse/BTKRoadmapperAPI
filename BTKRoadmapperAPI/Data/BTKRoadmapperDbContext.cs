using BTKRoadmapperAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BTKRoadmapperAPI.Data
{
    public class BTKRoadmapperDbContext : DbContext
    {
        public BTKRoadmapperDbContext(DbContextOptions<BTKRoadmapperDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Modules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.AvailableHoursPerDaily)
                    .IsRequired();

                entity.Property(u => u.InterestedFields)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(u => u.EducationLevel)
                    .IsRequired();

                entity.Property(u => u.InterestedFieldSkillLevel)
                    .IsRequired();

                entity.Property(u => u.TargetField)
                    .IsRequired();
            });

        }
    }
}
