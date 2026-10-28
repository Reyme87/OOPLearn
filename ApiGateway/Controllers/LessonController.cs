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

        [HttpGet("categories/{categoryId}/lessons")]
        public async Task<IActionResult> GetLessonByCategory(string categoryId)
        {
            var request = new CategoryRequest { CategoryId = categoryId };
            var response = await _lessonClient.GetLessonsByCategoriesAsync(request);

            return Ok(response.Lessons);
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
