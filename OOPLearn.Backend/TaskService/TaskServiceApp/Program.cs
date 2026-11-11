using Microsoft.AspNetCore.Server.Kestrel.Core;
using Tasks;
using TaskService.Infrastructure;
using TaskService.Persistence;
using TaskServiceApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5059, o => o.Protocols = HttpProtocols.Http2);
});


builder.Services.AddGrpc();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapGrpcService<TaskServiceApi>();

app.Run();
