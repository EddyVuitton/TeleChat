using TeleChat.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.AddServices();
builder.AddHubChat();

var app = builder.Build();

app.AddMiddleware();

app.Run();