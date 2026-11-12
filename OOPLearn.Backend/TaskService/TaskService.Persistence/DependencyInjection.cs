using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Application.Interfaces;
using TaskService.Application.Interfaces.Repositories;
using TaskService.Persistence.Repositories;

namespace TaskService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {

                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskSubmissionRepository, TaskSubmissionRepository>();

            return services;
        }
    }
}
