namespace LessonService.Domain
{
    public class LessonMedia
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid LessonSectionId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string S3Key { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public string MediaType { get; set; } = string.Empty;

        public string? Description { get; set; }

        public LessonSection LessonSection { get; set; } = null!;
    }
}
