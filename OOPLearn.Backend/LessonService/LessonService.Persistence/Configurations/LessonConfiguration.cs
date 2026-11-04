using LessonService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessonService.Persistence.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<LessonModel>
    {
        public void Configure(EntityTypeBuilder<LessonModel> builder)
        {
            builder.ToTable("lessons");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            builder.Property(x => x.EstimatedMinutes)
                .IsRequired();

            builder.HasMany(x => x.Sections)
                .WithOne(x => x.LessonModel)
                .HasForeignKey(x => x.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => new { x.CategoryId, x.OrderIndex });
        }
    }
}
