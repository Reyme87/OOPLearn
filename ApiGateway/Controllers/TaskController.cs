using ApiGateway.Dtos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("/api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly TasksService.TasksServiceClient _taskClient;

        public TaskController(TasksService.TasksServiceClient taskClient)
        {
            _taskClient = taskClient;
        }

        // GET api/tasks/lessons/{lessonId}
        [HttpGet("lessons/{lessonId}")]
        public async Task<IActionResult> GetTasksByLesson(string lessonId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(lessonId, out _))
                return BadRequest("Некорректный идентификатор урока.");

            try
            {
                var response = await _taskClient.GetTasksByLessonAsync(
                    new LessonTaskRequest { LessonId = lessonId },
                    cancellationToken: cancellationToken);

                return Ok(response.Tasks.Select(task => new
                {
                    task.Id,
                    task.LessonId,
                    task.Title,
                    task.Description,
                    Type = task.Type.ToString(),
                    task.Points,
                    task.OrderIndex
                }));
            }
            catch (RpcException exception)
            {
                return HandleGrpcException(exception);
            }
        }

        // GET api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out _))
                return BadRequest("Некорректный идентификатор задания.");

            try
            {
                var response = await _taskClient.GetTaskDetailsAsync(
                    new TaskDetailRequest { TaskId = id },
                    cancellationToken: cancellationToken);

                return Ok(new
                {
                    response.Id,
                    response.LessonId,
                    response.Title,
                    response.Description,
                    response.Content,
                    Type = response.Type.ToString(),
                    response.Points,
                    response.CodeLanguage,
                    response.StarterCode,
                    Options = response.Options.Select(option => new
                    {
                        option.Id,
                        option.Text,
                        option.OrderIndex
                    }),
                    TestCases = response.TestCases.Select(testCase => new
                    {
                        testCase.Id,
                        testCase.Input,
                        testCase.ExpectedOutput,
                        testCase.OrderIndex
                    })
                });
            }
            catch (RpcException exception)
            {
                return HandleGrpcException(exception);
            }
        }

        // POST api/tasks/{id}/submissions
        [Authorize]
        [HttpPost("{id}/submissions")]
        public async Task<IActionResult> SubmitSolution(string id, [FromBody] SubmitTaskRequest request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(id, out _))
                return BadRequest("Некорректный идентификатор задания.");

            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            try
            {
                var grpcRequest = new SubmitSolutionRequest
                {
                    TaskId = id,
                    UserId = userId.ToString(),
                    SubmittedCode = request.SubmittedCode
                };

                grpcRequest.SelectedOptionIds.AddRange(
                    request.SelectedOptionIds);

                var response = await _taskClient.SubmitTaskSolutionAsync(
                    grpcRequest,
                    cancellationToken: cancellationToken);

                return Ok(new
                {
                    response.SubmissionId,
                    Status = response.Status.ToString(),
                    response.EarnedPoints,
                    response.Feedback,
                    response.SubmittedAt
                });
            }
            catch (RpcException exception)
            {
                return HandleGrpcException(exception);
            }
        }

        // GET api/tasks/users/{userId}/submissions
        [Authorize]
        [HttpGet("users/submissions")]
        public async Task<IActionResult> GetUserSubmissions(CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            try
            {
                var response = await _taskClient.GetUserSubmissionsAsync(
                    new UserSubmissionsRequest { UserId = userIdValue },
                    cancellationToken: cancellationToken);

                return Ok(response.Submissions.Select(submission => new
                {
                    submission.Id,
                    submission.TaskId,
                    Status = submission.Status.ToString(),
                    submission.EarnedPoints,
                    submission.Feedback,
                    submission.SubmittedAt
                }));
            }
            catch (RpcException exception)
            {
                return HandleGrpcException(exception);
            }
        }

        private IActionResult HandleGrpcException(RpcException exception)
        {
            return exception.StatusCode switch
            {
                Grpc.Core.StatusCode.InvalidArgument =>
                    BadRequest(exception.Status.Detail),

                Grpc.Core.StatusCode.NotFound =>
                    NotFound(exception.Status.Detail),

                Grpc.Core.StatusCode.Unimplemented =>
                    StatusCode(501, exception.Status.Detail),

                _ => StatusCode(500, "Ошибка при обращении к TaskService.")
            };
        }
    }
}