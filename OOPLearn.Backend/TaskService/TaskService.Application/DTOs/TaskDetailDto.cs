using TaskService.Domain;

namespace TaskService.Application.DTOs
{
    public class TaskDetailDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public TaskType Type { get; set; }
        public int Points { get; set; }

        public string? CodeLanguage { get; set; }
        public string? StarterCode { get; set; }

        public List<TaskOptionDto> Options { get; set; } = new();
        public List<TaskTestCaseDto> TestCases { get; set; } = new();
    }
}
