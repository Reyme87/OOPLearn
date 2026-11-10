using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Application.Interfaces.Repositories;
using TaskService.Domain.Models;

namespace TaskService.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IAppDbContext _context;

        public TaskRepository(IAppDbContext context) => _context = context;

        public async Task<List<TaskModel>> GetByLessonAsync(Guid lessonId, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(x => x.LessonId == lessonId)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(x => x.Options)
                .Include(x => x.TestCases)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
