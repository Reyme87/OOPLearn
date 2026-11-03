using LessonService.Domain;

namespace LessonService.Application.Interfaces.Repositories
{
    public interface ILessonRepository
    {
        Task<List<LessonModel>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken);

        Task<LessonModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task AddAsync(LessonModel model, CancellationToken cancellationToken);

        Task UpdateAsync(LessonModel model, CancellationToken cancellationToken);

        Task DeleteAsync(LessonModel model, CancellationToken cancellationToken);
    }
}
