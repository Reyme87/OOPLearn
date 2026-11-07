using LessonService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LessonService.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Category> Categories { get; }
        DbSet<LessonModel> Lessons { get; }

        DbSet<LessonSection> LessonSections { get; }

        DbSet<LessonMedia> LessonMedia { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
