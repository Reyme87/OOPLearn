using Lesson;
using Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<LessonService.LessonServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5290");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler     // временно
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
}); ;

builder.Services.AddGrpcClient<TaskService.TaskServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5059");
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
