using LessonService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LessonService.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<LessonModel> Lessons => Set<LessonModel>();
        public DbSet<LessonSection> LessonSections => Set<LessonSection>();
        public DbSet<LessonMedia> LessonMedia => Set<LessonMedia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
