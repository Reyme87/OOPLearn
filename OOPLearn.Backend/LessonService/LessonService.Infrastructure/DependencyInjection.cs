using Amazon.Runtime;
using Amazon.S3;
using LessonService.Application.Interfaces;
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
                InternalEndpoint = configuration["S3:InternalEndpoint"] ?? "",
                PublicEndpoint = configuration["S3:PublicEndpoint"] ?? "",
                AccessKey = configuration["S3:AccessKey"] ?? "",
                SecretKey = configuration["S3:SecretKey"] ?? "",
                BucketName = configuration["S3:BucketName"] ?? "",
                UseSSL = bool.TryParse(configuration["S3:UseSSL"], out var useSSL) && useSSL
            };

            var credentials = new BasicAWSCredentials(s3Options.AccessKey, s3Options.SecretKey);

            var internalConfig = new AmazonS3Config
            {
                ServiceURL = s3Options.InternalEndpoint,
                ForcePathStyle = true,
                UseHttp = true
            };

            var publicConfig = new AmazonS3Config
            {
                ServiceURL = s3Options.PublicEndpoint,
                ForcePathStyle = true,
                UseHttp = true
            };

            var internalClient = new AmazonS3Client(
                credentials,
                internalConfig);

            var publicClient = new AmazonS3Client(
                credentials,
                publicConfig);

            services.AddSingleton(s3Options);
            services.AddSingleton(new S3Clients(internalClient, publicClient));
            services.AddScoped<IFileStorage, S3FileStorage>();

            return services;
        }
    }
}
