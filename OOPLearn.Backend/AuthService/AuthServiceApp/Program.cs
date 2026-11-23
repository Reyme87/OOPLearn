using AuthService.Infrastructure;
using AuthService.Persistence;
using AuthServiceApp.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5060, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapGrpcService<AuthServiceApi>();

app.Run();
