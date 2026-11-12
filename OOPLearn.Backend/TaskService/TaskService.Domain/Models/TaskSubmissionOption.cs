namespace TaskService.Domain.Models
{
    public class TaskSubmissionOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SubmissionId { get; set; }

        public Guid TaskOptionId { get; set; }

        public TaskSubmission Submission { get; set; } = null!;

        public TaskOption TaskOption { get; set; } = null!;
    }
}
