namespace LessonService.Application.DTOs
{
    public class LessonSectionDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ContentMarkdown { get; set; }
            = string.Empty;

        public string? CodeExample { get; set; }

        public string? CodeLanguage { get; set; }

        public int OrderIndex { get; set; }

        public List<LessonMediaDto> Media { get; set; }
            = new();
    }
}
