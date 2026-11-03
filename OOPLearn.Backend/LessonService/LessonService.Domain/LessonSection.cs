namespace LessonService.Domain
{
    public class LessonSection
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid LessonId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ContentMarkdown { get; set; } = string.Empty;

        public string? CodeExample { get; set; }

        public string? CodeLanguage { get; set; }

        public int OrderIndex { get; set; }

        public LessonModel LessonModel { get; set; } = null!;

        public ICollection<LessonMedia> Media { get; set; } = new List<LessonMedia>();
    }
}
