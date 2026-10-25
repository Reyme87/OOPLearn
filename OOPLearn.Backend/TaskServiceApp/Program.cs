using Tasks;
using TaskServiceApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<TaskServiceApi>();

app.Run();
