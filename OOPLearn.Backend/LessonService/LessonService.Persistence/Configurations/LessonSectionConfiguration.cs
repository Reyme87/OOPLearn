using LessonService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessonService.Persistence.Configurations
{
    public class LessonSectionConfiguration : IEntityTypeConfiguration<LessonSection>
    {
        public void Configure(EntityTypeBuilder<LessonSection> builder)
        {
            builder.ToTable("lesson_sections");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ContentMarkdown)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(x => x.CodeExample)
                .HasColumnType("text");

            builder.Property(x => x.CodeLanguage)
                .HasMaxLength(50);

            builder.Property(x => x.OrderIndex)
                .IsRequired();

            builder.HasMany(x => x.Media)
                .WithOne(x => x.LessonSection)
                .HasForeignKey(x => x.LessonSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.LessonId);
            builder.HasIndex(x => new { x.LessonId, x.OrderIndex });
        }
    }
}
