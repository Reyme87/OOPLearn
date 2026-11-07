using LessonService.Application.DTOs;
using LessonService.Application.Interfaces.Repositories;
using LessonService.Application.Interfaces.Services;

namespace LessonService.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository) => _repository = repository;

        public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllAsync(cancellationToken);

            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description,
                OrderIndex = category.OrderIndex,
            }).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(id, cancellationToken);

            if (category is null)
            {
                return null;
            }

            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description,
                OrderIndex = category.OrderIndex
            };
        }
    }
}
