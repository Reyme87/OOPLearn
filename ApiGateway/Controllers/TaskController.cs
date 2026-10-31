using ApiGateway.Dtos;
using Microsoft.AspNetCore.Mvc;
using Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("/api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService.TaskServiceClient _taskClient;

        public TaskController(TaskService.TaskServiceClient taskClient) => _taskClient = taskClient;

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetTaskByLesson(string lessonId)
        {
            var response = await _taskClient.GetTaskByLessonAsync(new LessonTaskRequest { LessonId = lessonId });

            return Ok(response.Tasks);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitSolutionDto dto)
        {
            var request = new SubmitSolutionRequest
            {
                TaskId = dto.TaskId,
                UserId = dto.UserId,
                UserAnswer = dto.UserAnswer,
            };
            var response = await _taskClient.SubmitTaskSolutionAsync(request);

            return Ok(response);
        }
    }
}
