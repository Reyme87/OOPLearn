namespace LessonService.Application.DTOs
{
    public class LessonDetailDto
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int EstimatedMinutes { get; set; }

        public List<LessonSectionDto> Sections { get; set; }
            = new();
    }
}
