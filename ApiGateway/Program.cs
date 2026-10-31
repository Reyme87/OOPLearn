using Lesson;
using Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<LessonService.LessonServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5290");
});

builder.Services.AddGrpcClient<TaskService.TaskServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5059");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway v1");

    config.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();
