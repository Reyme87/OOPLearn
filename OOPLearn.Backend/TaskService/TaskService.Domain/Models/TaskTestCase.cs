namespace TaskService.Domain.Models
{
    public class TaskTestCase
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TaskId { get; set; }

        public string Input { get; set; } = string.Empty;

        public string ExpectedOutput { get; set; } = string.Empty;

        public bool IsHidden { get; set; }

        public int OrderIndex { get; set; }

        public TaskModel Task { get; set; } = null!;
    }
}
