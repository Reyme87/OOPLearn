using LessonService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessonService.Persistence.Configurations
{
    public class LessonMediaConfiguration
    : IEntityTypeConfiguration<LessonMedia>
    {
        public void Configure(EntityTypeBuilder<LessonMedia> builder)
        {
            builder.ToTable("lesson_media");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.S3Key)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.MediaType)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasIndex(x => x.S3Key)
                .IsUnique();

            builder.HasIndex(x => x.LessonSectionId);
        }
    }
}
