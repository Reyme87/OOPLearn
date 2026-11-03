namespace LessonService.Application.DTOs
{
    public class LessonDto
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        public int EstimatedMinutes { get; set; }
    }
}
