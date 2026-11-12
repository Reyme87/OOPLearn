using TaskService.Domain;

namespace TaskService.Application.DTOs
{
    public class TaskSubmissionDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public SubmissionStatus Status { get; set; }
        public int Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
