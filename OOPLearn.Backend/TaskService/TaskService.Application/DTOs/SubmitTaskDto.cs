namespace TaskService.Application.DTOs
{
    public class SubmitTaskDto
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }

        public string? AnswerText { get; set; }
        public string? SubmittedCode { get; set; }

        public List<Guid> SelectedOptionIds { get; set; } = new();
    }
}
