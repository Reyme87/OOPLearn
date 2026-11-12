using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Domain.Models;

namespace TaskService.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<TaskModel> Tasks => Set<TaskModel>();
        public DbSet<TaskOption> TaskOptions => Set<TaskOption>();
        public DbSet<TaskTestCase> TaskTestCases => Set<TaskTestCase>();
        public DbSet<TaskSubmission> TaskSubmissions => Set<TaskSubmission>();
        public DbSet<TaskSubmissionOption> TaskSubmissionOptions => Set<TaskSubmissionOption>();

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }
    }
}
