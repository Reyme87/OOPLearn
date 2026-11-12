namespace TaskService.Domain.Models
{
    public class TaskOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TaskId { get; set; }

        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int OrderIndex { get; set; }

        public TaskModel Task { get; set; } = null!;
    }
}
