using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Models;

namespace TaskService.Persistence.Configurations
{
    public class TaskSubmissionOptionConfiguration : IEntityTypeConfiguration<TaskSubmissionOption>
    {
        public void Configure(EntityTypeBuilder<TaskSubmissionOption> builder)
        {
            builder.ToTable("task_submission_options");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.TaskOption)
                .WithMany()
                .HasForeignKey(x => x.TaskOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.SubmissionId, x.TaskOptionId })
                .IsUnique();
        }
    }
}
