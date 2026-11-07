using LessonService.Application.Interfaces.Services;
using LessonService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LessonService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILessonService, LessonsService>();

            return services;
        }
    }
}
