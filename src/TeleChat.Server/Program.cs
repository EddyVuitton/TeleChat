using TeleChat.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.AddServices();

var app = builder.Build();

app.AddMiddleware();

app.Run();