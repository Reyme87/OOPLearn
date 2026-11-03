using LessonService.Application.DTOs;
using LessonService.Application.Interfaces.Repositories;
using LessonService.Application.Interfaces.Services;

namespace LessonService.Infrastructure.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _repository;

        public LessonService(ILessonRepository repository) => _repository = repository;

        public async Task<List<LessonDto>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var lessons = await _repository.GetByCategoryAsync(categoryId, cancellationToken);

            return lessons.Select(lesson => new LessonDto
            {
                Id = lesson.Id,
                CategoryId = lesson.CategoryId,
                Title = lesson.Title,
                Description = lesson.Description,
                OrderIndex = lesson.OrderIndex,
                EstimatedMinutes = lesson.EstimatedMinutes,
            }).ToList();
        }

        public async Task<LessonDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var lesson = await _repository.GetByIdAsync(id, cancellationToken);

            if (lesson is null)
            {
                return null;
            }

            return new LessonDetailDto
            {
                Id = lesson.Id,
                CategoryId = lesson.CategoryId,
                Title = lesson.Title,
                Description = lesson.Description,
                EstimatedMinutes = lesson.EstimatedMinutes,
                Sections = lesson.Sections.OrderBy(s => s.OrderIndex)
                                          .Select(section => new LessonSectionDto
                                          {
                                              Id = section.Id,
                                              Title = section.Title,
                                              ContentMarkdown = section.ContentMarkdown,
                                              CodeExample = section.CodeExample,
                                              CodeLanguage = section.CodeLanguage,
                                              OrderIndex = section.OrderIndex,
                                              Media = section.Media.OrderBy(m => m.OrderIndex)
                                                                   .Select(media => new LessonMediaDto
                                                                   {
                                                                       Id = media.Id,
                                                                       FileName = media.FileName,
                                                                       ContentType = media.ContentType,
                                                                       MediaType = media.MediaType,
                                                                       Description = media.Description,
                                                                       S3Key = media.S3Key,
                                                                       OrderIndex = media.OrderIndex
                                                                   }).ToList()
                                          }).ToList()
            };
        }
    }
}
