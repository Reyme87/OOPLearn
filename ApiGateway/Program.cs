using Lesson;
using Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<LessonService.LessonServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:5001");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler     // временно
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
}); ;

builder.Services.AddGrpcClient<TaskService.TaskServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:5002");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler     // временно
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
