using LessonService.Domain;

namespace LessonService.Application.DTOs
{
    public class LessonMediaDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public MediaType MediaType { get; set; }

        public string? Description { get; set; }

        public string S3Key { get; set; }
            = string.Empty;

        public int OrderIndex { get; set; }
    }
}
