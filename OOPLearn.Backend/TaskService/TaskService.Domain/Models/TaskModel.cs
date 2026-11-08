namespace TaskService.Domain.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid LessonId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public TaskType Type { get; set; }

        public int Points { get; set; }

        public int OrderIndex { get; set; }

        public string? CodeLanguage { get; set; }

        public string? StarterCode { get; set; }

        public ICollection<TaskOption> Options { get; set; }
            = new List<TaskOption>();

        public ICollection<TaskTestCase> TestCases { get; set; }
            = new List<TaskTestCase>();

        public ICollection<TaskSubmission> Submissions { get; set; }
            = new List<TaskSubmission>();
    }
}
