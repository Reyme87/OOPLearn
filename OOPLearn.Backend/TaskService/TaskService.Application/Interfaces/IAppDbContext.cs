using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Models;

namespace TaskService.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<TaskModel> Tasks { get; }
        DbSet<TaskOption> TaskOptions { get; }
        DbSet<TaskTestCase> TaskTestCases { get; }
        DbSet<TaskSubmission> TaskSubmissions { get; }
        DbSet<TaskSubmissionOption> TaskSubmissionOptions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
