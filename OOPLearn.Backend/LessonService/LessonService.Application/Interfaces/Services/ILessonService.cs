using LessonService.Application.DTOs;

namespace LessonService.Application.Interfaces.Services
{
    public interface ILessonService
    {
        Task<List<LessonDto>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken);

        Task<LessonDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
