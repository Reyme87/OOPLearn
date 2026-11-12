using Lesson;
using Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<LessonsService.LessonsServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5290");
});

builder.Services.AddGrpcClient<TasksService.TasksServiceClient>(options =>
{
    options.Address = new Uri("http://localhost:5059");
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueClient", policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:5173",
                "https://127.0.0.1:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiGateway v1");

    config.RoutePrefix = string.Empty;
});

app.UseCors("VueClient");

app.MapControllers();

app.Run();
