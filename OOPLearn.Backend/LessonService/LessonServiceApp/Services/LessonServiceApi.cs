using Grpc.Core;
using Lesson;
using LessonService.Application.Interfaces.Services;

namespace LessonServiceApp.Services;

public class LessonServiceApi : LessonsService.LessonsServiceBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILessonService _lessonService;
    private readonly ILogger<LessonServiceApi> _logger;

    public LessonServiceApi(ILogger<LessonServiceApi> logger, ICategoryService categoryService, ILessonService lessonService) 
        => (_logger, _categoryService, _lessonService) = (logger, categoryService, lessonService);

    public override async Task<CategoryListResponse> GetCategories(EmptyRequest request, ServerCallContext context)
    {
        var categories = await _categoryService.GetAllAsync(context.CancellationToken);

        var response = new CategoryListResponse();

        response.Categories.AddRange(categories.Select(category =>
            new CategoryDto
            {
                Id = category.Id.ToString(),
                Title = category.Title,
                Description = category.Description,
                OrderIndex = category.OrderIndex
            }));

        return response;
    }

    public override async Task<LessonListResponse> GetLessonsByCategories(CategoryRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CategoryId, out var categoryId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный идентификатор категории."));
        }

        var lessons = await _lessonService.GetByCategoryAsync(categoryId, context.CancellationToken);

        var response = new LessonListResponse();

        response.Lessons.AddRange(lessons.Select(lesson =>
            new LessonSummaryDto
            {
                Id = lesson.Id.ToString(),
                CategoryId = lesson.CategoryId.ToString(),
                Title = lesson.Title,
                Description = lesson.Description,
                OrderIndex = lesson.OrderIndex,
                EstimatedMinutes = lesson.EstimatedMinutes
            }));

        return response;
    }

    public override async Task<LessonDetailResponse> GetLessonDetails(LessonDetailRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.LessonId, out var lessonId))
        {
            throw new RpcException(new Status(
                StatusCode.InvalidArgument,
                "Некорректный идентификатор урока."));
        }

        var lesson = await _lessonService.GetByIdAsync(lessonId, context.CancellationToken);

        if (lesson is null)
        {
            throw new RpcException(new Status(
                StatusCode.NotFound,
                "Урок не найден."));
        }

        var response = new LessonDetailResponse
        {
            Id = lesson.Id.ToString(),
            CategoryId = lesson.CategoryId.ToString(),
            Title = lesson.Title,
            Description = lesson.Description,
            EstimatedMinutes = lesson.EstimatedMinutes
        };

        response.Sections.AddRange(lesson.Sections.Select(section =>
        {
            var sectionDto = new LessonSectionDto
            {
                Id = section.Id.ToString(),
                Title = section.Title,
                ContentMarkdown = section.ContentMarkdown,
                CodeExample = section.CodeExample ?? string.Empty,
                CodeLanguage = section.CodeLanguage ?? string.Empty,
                OrderIndex = section.OrderIndex
            };

            sectionDto.Media.AddRange(section.Media.Select(media =>
                new LessonMediaDto
                {
                    Id = media.Id.ToString(),
                    FileName = media.FileName,
                    ContentType = media.ContentType,
                    Description = media.Description ?? string.Empty,
                    MediaType = media.MediaType switch
                    {
                        LessonService.Domain.MediaType.Image => Lesson.MediaType.Image,
                        LessonService.Domain.MediaType.Video => Lesson.MediaType.Video,
                        LessonService.Domain.MediaType.Document => Lesson.MediaType.Document,
                        _ => Lesson.MediaType.Document
                    },
                    Url = media.S3Key,
                    OrderIndex = media.OrderIndex
                }));

            return sectionDto;
        }));

        return response;
    }
}