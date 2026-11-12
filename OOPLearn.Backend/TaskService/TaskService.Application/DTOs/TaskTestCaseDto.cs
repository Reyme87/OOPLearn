namespace TaskService.Application.DTOs
{
    public class TaskTestCaseDto
    {
        public Guid Id { get; set; }
        public string Input { get; set; } = string.Empty;
        public string ExpectedOutput { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
