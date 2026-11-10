using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Models;

namespace TaskService.Persistence.Configurations
{
    public class TaskSubmissionConfiguration : IEntityTypeConfiguration<TaskSubmission>
    {
        public void Configure(EntityTypeBuilder<TaskSubmission> builder)
        {
            builder.ToTable("task_submissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            builder.Property(x => x.AnswerText);

            builder.Property(x => x.SubmittedCode);

            builder.Property(x => x.Feedback);

            builder.HasIndex(x => new { x.UserId, x.SubmittedAt });

            builder.HasMany(x => x.SelectedOptions)
                .WithOne(x => x.Submission)
                .HasForeignKey(x => x.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
