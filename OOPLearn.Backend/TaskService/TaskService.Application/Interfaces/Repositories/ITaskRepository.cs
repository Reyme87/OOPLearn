using TaskService.Domain.Models;

namespace TaskService.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetByLessonAsync(Guid lessonId, CancellationToken cancellationToken);

        Task<TaskModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
