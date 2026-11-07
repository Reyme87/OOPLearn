using LessonService.Application.DTOs;

namespace LessonService.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
