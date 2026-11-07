using LessonService.Infrastructure;
using LessonService.Persistence;
using LessonServiceApp.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5290, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await DbInitializer.InitializeAsync(dbContext);
}

app.MapGrpcService<LessonServiceApi>();
app.Run();
