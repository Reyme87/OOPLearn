using TaskService.Domain.Models;

namespace TaskService.Application.Interfaces.Repositories
{
    public interface ITaskSubmissionRepository
    {
        Task<TaskSubmission?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<TaskSubmission>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);

        Task AddAsync(TaskSubmission taskSubmission, CancellationToken cancellationToken);
    }
}
