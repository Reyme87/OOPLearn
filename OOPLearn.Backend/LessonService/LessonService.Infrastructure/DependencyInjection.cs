using Amazon.Runtime;
using Amazon.S3;
using LessonService.Application.Interfaces.Services;
using LessonService.Infrastructure.Services;
using LessonService.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LessonService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILessonService, LessonsService>();

            var s3Options = new S3Options
            {
                Endpoint = configuration["S3:Endpoint"] ?? "",
                AccessKey = configuration["S3:AccessKey"] ?? "",
                SecretKey = configuration["S3:SecretKey"] ?? "",
                BucketName = configuration["S3:BucketName"] ?? "",
                UseSSL = bool.TryParse(configuration["S3:UseSSL"], out var useSSL) && useSSL
            };

            var credentials = new BasicAWSCredentials(s3Options.AccessKey, s3Options.SecretKey);

            var s3Config = new AmazonS3Config
            {
                ServiceURL = s3Options.Endpoint,
                ForcePathStyle = true,
                UseHttp = !s3Options.UseSSL
            };

            services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));

            return services;
        }
    }
}
