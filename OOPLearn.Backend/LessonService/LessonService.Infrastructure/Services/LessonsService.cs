using LessonService.Application.DTOs;
using LessonService.Application.Interfaces;
using LessonService.Application.Interfaces.Repositories;
using LessonService.Application.Interfaces.Services;

namespace LessonService.Infrastructure.Services
{
    public class LessonsService : ILessonService
    {
        private readonly ILessonRepository _repository;
        private readonly IFileStorage _fileStorage;

        public LessonsService(ILessonRepository repository, IFileStorage fileStorage) => (_repository, _fileStorage) = (repository, fileStorage);

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

            var sections = new List<LessonSectionDto>();

            foreach (var section in lesson.Sections.OrderBy(s => s.OrderIndex))
            {
                var sectionDto = new LessonSectionDto
                {
                    Id = section.Id,
                    Title = section.Title,
                    ContentMarkdown = section.ContentMarkdown,
                    CodeExample = section.CodeExample,
                    CodeLanguage = section.CodeLanguage,
                    OrderIndex = section.OrderIndex
                };

                foreach (var media in section.Media.OrderBy(m => m.OrderIndex))
                {
                    var url = await _fileStorage.GetPresignedUrlAsync(media.S3Key);

                    sectionDto.Media.Add(new LessonMediaDto
                    {
                        Id = media.Id,
                        FileName = media.FileName,
                        ContentType = media.ContentType,
                        MediaType = media.MediaType,
                        Description = media.Description,
                        S3Key = url,
                        OrderIndex = media.OrderIndex
                    });
                }

                sections.Add(sectionDto);
            }

            return new LessonDetailDto
            {
                Id = lesson.Id,
                CategoryId = lesson.CategoryId,
                Title = lesson.Title,
                Description = lesson.Description,
                EstimatedMinutes = lesson.EstimatedMinutes,
                Sections = sections
            };
        }
    }
}
