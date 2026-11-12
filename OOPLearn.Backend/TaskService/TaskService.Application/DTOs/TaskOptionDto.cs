namespace TaskService.Application.DTOs
{
    public class TaskOptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
