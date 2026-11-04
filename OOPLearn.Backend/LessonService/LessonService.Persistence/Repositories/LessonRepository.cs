using LessonService.Application.Interfaces;
using LessonService.Application.Interfaces.Repositories;
using LessonService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LessonService.Persistence.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IAppDbContext _context;

        public LessonRepository(IAppDbContext context) => _context = context;

        public async Task AddAsync(LessonModel model, CancellationToken cancellationToken)
        {
            await _context.Lessons.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(LessonModel model, CancellationToken cancellationToken)
        {
            _context.Lessons.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<LessonModel>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _context.Lessons.AsNoTracking()
                                         .Where(l => l.CategoryId == categoryId)
                                         .OrderBy(l => l.OrderIndex)
                                         .ToListAsync(cancellationToken);
        }

        public async Task<LessonModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Lessons.AsNoTracking()
                                         .Include(l => l.Sections.OrderBy(s => s.OrderIndex))
                                         .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(LessonModel model, CancellationToken cancellationToken)
        {
            _context.Lessons.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
