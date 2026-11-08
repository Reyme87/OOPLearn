using TaskService.Application.DTOs;

namespace TaskService.Application.Interfaces.Services
{
    public interface ITaskSubmissionService
    {
        Task<TaskSubmissionDto> SubmitAsync(
            SubmitTaskDto submission,
            CancellationToken cancellationToken);

        Task<TaskSubmissionDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<List<TaskSubmissionDto>> GetByUserAsync(
            Guid userId,
            CancellationToken cancellationToken);
    }
}
