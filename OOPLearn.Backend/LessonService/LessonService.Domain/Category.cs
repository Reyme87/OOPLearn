namespace LessonService.Domain
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        public ICollection<LessonModel> Lessons { get; set; } = new List<LessonModel>();
    }
}
