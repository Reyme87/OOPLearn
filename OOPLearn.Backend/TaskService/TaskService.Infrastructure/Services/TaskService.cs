using TaskService.Application.DTOs;
using TaskService.Application.Interfaces.Repositories;
using TaskService.Application.Interfaces.Services;

namespace TaskService.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository) => _taskRepository = taskRepository;

        public async Task<List<TaskDto>> GetByLessonAsync(Guid lessonId, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByLessonAsync(lessonId, cancellationToken);

            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                LessonId = task.LessonId,
                Title = task.Title,
                Description = task.Description,
                OrderIndex = task.OrderIndex,
                Points = task.Points
            }).ToList();
        }

        public async Task<TaskDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(id, cancellationToken);

            if (task is null)
                return null;

            return new TaskDetailDto
            {
                Id = task.Id,
                LessonId = task.LessonId,
                Title = task.Title,
                Description = task.Description,
                Content = task.Content,
                Type = task.Type,
                Points = task.Points,
                CodeLanguage = task.CodeLanguage,
                StarterCode = task.StarterCode,

                Options = task.Options
                    .OrderBy(x => x.OrderIndex)
                    .Select(option => new TaskOptionDto
                    {
                        Id = option.Id,
                        Text = option.Text,
                        OrderIndex = option.OrderIndex
                    })
                    .ToList(),

                TestCases = task.TestCases
                    .Where(testCase => !testCase.IsHidden)
                    .OrderBy(x => x.OrderIndex)
                    .Select(testCase => new TaskTestCaseDto
                    {
                        Id = testCase.Id,
                        Input = testCase.Input,
                        ExpectedOutput = testCase.ExpectedOutput,
                        OrderIndex = testCase.OrderIndex
                    })
                    .ToList()
            };
        }
    }
}
