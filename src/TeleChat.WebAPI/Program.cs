using TeleChat.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.AddServices();
builder.AddHubChat();
builder.AddRepositories();

var app = builder.Build();

await app.AddMiddlewareAsync();

app.Run();