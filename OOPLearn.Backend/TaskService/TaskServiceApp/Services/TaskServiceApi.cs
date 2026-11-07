using Grpc.Core;
using Tasks;

namespace TaskServiceApp.Services
{
    public class TaskServiceApi : TaskService.TaskServiceBase
    {
        private readonly ILogger<TaskServiceApi> _logger;

        public TaskServiceApi(ILogger<TaskServiceApi> logger) => _logger = logger;

        public override Task<TaskListResponse> GetTaskByLesson(LessonTaskRequest request, ServerCallContext context)
        {
            var response = new TaskListResponse();

            response.Tasks.Add(new TaskDto
            {
                Id = "1",
                LessonId = request.LessonId,
                Title = "Что такое модификатор private?",
                Description = "Выберите верное утверждение об инкапсуляции и модификаторе private.",
                Type = TaskType.SingleChoice,
                Options = {
                "Доступно везде в проекте",
                "Доступно только внутри текущего класса",
                "Доступно классам-наследникам"
                }
            });

            return Task.FromResult(response);
        }

        public override Task<SubmitSolutionResponse> SubmitTaskSolution(SubmitSolutionRequest request, ServerCallContext context)
        {
            _logger.LogInformation("User {UserId} submitted solution for task {TaskId}", request.UserId, request.TaskId);

            bool isCorrect = request.UserAnswer.Contains("текущего класса"); ;

            var response = new SubmitSolutionResponse
            {
                IsCorrect = isCorrect,
                Feedback = isCorrect ? "Верно! Инкапсуляция скрывает состояние." : "Неверно. Повторите теорию про модификаторы доступа.",
                EarnedPoints = isCorrect ? 10 : 0
            };

            return Task.FromResult(response);
        }
    }
}
