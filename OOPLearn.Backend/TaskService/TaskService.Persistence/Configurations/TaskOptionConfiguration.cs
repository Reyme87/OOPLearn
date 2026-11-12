using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Models;

namespace TaskService.Persistence.Configurations
{
    public class TaskOptionConfiguration : IEntityTypeConfiguration<TaskOption>
    {
        public void Configure(EntityTypeBuilder<TaskOption> builder)
        {
            builder.ToTable("task_options");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasIndex(x => new { x.TaskId, x.OrderIndex });
        }
    }
}
