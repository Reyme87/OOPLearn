namespace TaskService.Domain.Models
{
    public class TaskSubmission
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TaskId { get; set; }

        public Guid UserId { get; set; }

        public string? AnswerText { get; set; }

        public string? SubmittedCode { get; set; }

        public SubmissionStatus Status { get; set; }
            = SubmissionStatus.Pending;

        public int Score { get; set; }

        public string? Feedback { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public TaskModel Task { get; set; } = null!;

        public ICollection<TaskSubmissionOption> SelectedOptions { get; set; }
            = new List<TaskSubmissionOption>();
    }
}
