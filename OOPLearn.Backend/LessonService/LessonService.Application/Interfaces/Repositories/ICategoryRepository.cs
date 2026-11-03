using LessonService.Domain;

namespace LessonService.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);

        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task AddAsync(Category category, CancellationToken cancellationToken);

        Task UpdateAsync(Category category, CancellationToken cancellationToken);

        Task DeleteAsync(Category category, CancellationToken cancellationToken);
    }
}
