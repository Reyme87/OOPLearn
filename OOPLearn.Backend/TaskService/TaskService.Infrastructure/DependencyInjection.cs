using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Interfaces.Services;
using TaskService.Infrastructure.Services;
using TaskServiceAppService = TaskService.Infrastructure.Services.TaskService;

namespace TaskService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskServiceAppService>();
            services.AddScoped<ITaskSubmissionService, TaskSubmissionService>();

            return services;
        }
    }
}
