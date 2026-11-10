using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Application.Interfaces.Repositories;
using TaskService.Domain.Models;

namespace TaskService.Persistence.Repositories
{
    public class TaskSubmissionRepository : ITaskSubmissionRepository
    {
        private readonly IAppDbContext _context;

        public TaskSubmissionRepository(IAppDbContext context) => _context = context;

        public async Task<TaskSubmission?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.TaskSubmissions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<TaskSubmission>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.TaskSubmissions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.SubmittedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TaskSubmission submission, CancellationToken cancellationToken)
        {
            await _context.TaskSubmissions.AddAsync(
                submission,
                cancellationToken);
        }
    }
}
