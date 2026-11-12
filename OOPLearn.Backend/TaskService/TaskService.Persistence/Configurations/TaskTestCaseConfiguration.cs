using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskService.Domain.Models;

namespace TaskService.Persistence.Configurations
{
    public class TaskTestCaseConfiguration : IEntityTypeConfiguration<TaskTestCase>
    {
        public void Configure(EntityTypeBuilder<TaskTestCase> builder)
        {
            builder.ToTable("task_testcases");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Input)
                .IsRequired();

            builder.Property(x => x.ExpectedOutput)
                .IsRequired();

            builder.HasIndex(x => new { x.TaskId, x.OrderIndex });
        }
    }
}
