using LessonService.Application.Interfaces;
using LessonService.Application.Interfaces.Repositories;
using LessonService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LessonService.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IAppDbContext _context;

        public CategoryRepository(IAppDbContext context) => _context = context;

        public async Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking()
                                            .OrderBy(c => c.OrderIndex)
                                            .ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking()
                                            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
