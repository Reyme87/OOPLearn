using Grpc.Core;
using Lesson;

namespace LessonServiceApp.Services;

public class LessonServiceApi : LessonService.LessonServiceBase
{
    private readonly ILogger<LessonServiceApi> _logger;

    public LessonServiceApi(ILogger<LessonServiceApi> logger) => _logger = logger;

    public override Task<CategoryListResponse> GetCategories(EmptyRequest request, ServerCallContext context)
    {
        var response = new CategoryListResponse();

        response.Categories.Add(new CategoryDto
        {
            Id = "1",
            Title = "Основы ООП",
            Description = "Классы, объекты, паттерны"
        });

        response.Categories.Add(new CategoryDto
        {
            Id = "2",
            Title = "Язык программирования C#",
            Description = "Синтаксис, структуры данных, особенности"
        });

        return Task.FromResult(response);
    }

    public override Task<LessonDetailResponse> GetLessonDetails(LessonDetailRequest request, ServerCallContext context)
    {
        var response = new LessonDetailResponse
        {
            Id = request.LessonId,
            Title = "Инкапсуляция в C#",
            ContentMarkdown = "Инкапсуляция — это сокрытие состояния и защита данных...",
            CodeExample = "public class Person { private string name; }"
        };

        return Task.FromResult(response);
    }
}