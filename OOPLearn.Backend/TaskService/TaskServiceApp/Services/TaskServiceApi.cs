using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tasks;
using TaskService.Application.DTOs;
using TaskService.Application.Interfaces.Services;

namespace TaskServiceApp.Services
{
    public class TaskServiceApi : TasksService.TasksServiceBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskSubmissionService _submissionService;

        public TaskServiceApi(
            ITaskService taskService,
            ITaskSubmissionService submissionService)
        {
            _taskService = taskService;
            _submissionService = submissionService;
        }

        public override async Task<TaskListResponse> GetTasksByLesson(LessonTaskRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.LessonId, out var lessonId))
            {
                throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    "Некорректный идентификатор урока."));
            }

            var tasks = await _taskService.GetByLessonAsync(
                lessonId,
                context.CancellationToken);

            var response = new TaskListResponse();

            response.Tasks.AddRange(tasks.Select(task => new Tasks.TaskDto
            {
                Id = task.Id.ToString(),
                LessonId = task.LessonId.ToString(),
                Title = task.Title,
                Description = task.Description,
                Points = task.Points,
                OrderIndex = task.OrderIndex,
                Type = MapTaskType(task.Type)
            }));

            return response;
        }

        public override async Task<TaskDetailResponse> GetTaskDetails(TaskDetailRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.TaskId, out var taskId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный идентификатор задания."));
            }

            var task = await _taskService.GetByIdAsync(taskId, context.CancellationToken);

            if (task is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Задание не найдено."));
            }

            var response = new TaskDetailResponse
            {
                Id = task.Id.ToString(),
                LessonId = task.LessonId.ToString(),
                Title = task.Title,
                Description = task.Description,
                Content = task.Content,
                Type = MapTaskType(task.Type),
                Points = task.Points,
                CodeLanguage = task.CodeLanguage ?? string.Empty,
                StarterCode = task.StarterCode ?? string.Empty
            };

            response.Options.AddRange(task.Options.Select(option =>
                new Tasks.TaskOptionDto
                {
                    Id = option.Id.ToString(),
                    Text = option.Text,
                    OrderIndex = option.OrderIndex
                }));

            response.TestCases.AddRange(task.TestCases.Select(testCase =>
                new Tasks.TaskTestCaseDto
                {
                    Id = testCase.Id.ToString(),
                    Input = testCase.Input,
                    ExpectedOutput = testCase.ExpectedOutput,
                    OrderIndex = testCase.OrderIndex
                }));

            return response;
        }

        public override async Task<SubmitSolutionResponse> SubmitTaskSolution(SubmitSolutionRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.TaskId, out var taskId) ||
                !Guid.TryParse(request.UserId, out var userId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный идентификатор задания или пользователя."));
            }

            try
            {
                var submission = await _submissionService.SubmitAsync(
                    new SubmitTaskDto
                    {
                        TaskId = taskId,
                        UserId = userId,
                        SelectedOptionIds = request.SelectedOptionIds
                            .Select(id => Guid.TryParse(id, out var optionId)
                                ? optionId
                                : throw new RpcException(new Status(
                                    StatusCode.InvalidArgument,
                                    "Некорректный идентификатор варианта ответа.")))
                            .ToList(),
                        SubmittedCode = string.IsNullOrWhiteSpace(request.SubmittedCode)
                            ? null
                            : request.SubmittedCode
                    },
                    context.CancellationToken);

                return new SubmitSolutionResponse
                {
                    SubmissionId = submission.Id.ToString(),
                    Status = (SubmissionStatus)(int)submission.Status,
                    EarnedPoints = submission.Score,
                    Feedback = submission.Feedback ?? string.Empty,
                    SubmittedAt = Timestamp.FromDateTime(submission.SubmittedAt)
                };
            }
            catch (KeyNotFoundException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    exception.Message));
            }
            catch (ArgumentException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    exception.Message));
            }
            catch (NotSupportedException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.Unimplemented,
                    exception.Message));
            }
        }

        public override async Task<UserSubmissionsResponse> GetUserSubmissions(UserSubmissionsRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Некорректный идентификатор пользователя."));
            }

            var submissions = await _submissionService.GetByUserAsync(userId, context.CancellationToken);

            var response = new UserSubmissionsResponse();

            response.Submissions.AddRange(submissions.Select(submission =>
                new SubmissionDto
                {
                    Id = submission.Id.ToString(),
                    TaskId = submission.TaskId.ToString(),
                    Status = (Tasks.SubmissionStatus)(int)submission.Status,
                    EarnedPoints = submission.Score,
                    Feedback = submission.Feedback ?? string.Empty,
                    SubmittedAt = submission.SubmittedAt.ToString("O")
                }));

            return response;
        }

        private static Tasks.TaskType MapTaskType(TaskService.Domain.TaskType type)
        {
            return type switch
            {
                TaskService.Domain.TaskType.SingleChoice => Tasks.TaskType.SingleChoice,

                TaskService.Domain.TaskType.MultipleChoice => Tasks.TaskType.MultipleChoice,

                TaskService.Domain.TaskType.Code => Tasks.TaskType.CodeSnippet,

                _ => Tasks.TaskType.Unknown
            };
        }
    }
}
