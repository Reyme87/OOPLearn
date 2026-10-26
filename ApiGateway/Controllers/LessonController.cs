using Lesson;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController : ControllerBase
    {
        private readonly LessonService.LessonServiceClient _lessonClient;

        public LessonController(LessonService.LessonServiceClient lessonClient) => _lessonClient = lessonClient;

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _lessonClient.GetCategoriesAsync(new EmptyRequest());

            return Ok(response.Categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonDetails(string id)
        {
            var request = new LessonDetailRequest { LessonId = id };
            var response = await _lessonClient.GetLessonDetailsAsync(request);

            return Ok(response);
        }
    }
}
