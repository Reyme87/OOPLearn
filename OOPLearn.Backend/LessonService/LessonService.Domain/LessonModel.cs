namespace LessonService.Domain
{
    public class LessonModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        public int EstimatedMinutes { get; set; }

        public Category Category { get; set; } = null!;

        public ICollection<LessonSection> Sections { get; set; } = new List<LessonSection>();
    }
}
