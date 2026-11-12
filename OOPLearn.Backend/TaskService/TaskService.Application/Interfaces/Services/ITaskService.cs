using TaskService.Application.DTOs;

namespace TaskService.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetByLessonAsync(Guid lessonId, CancellationToken cancellationToken);

        Task<TaskDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
