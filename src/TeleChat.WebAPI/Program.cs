using Swashbuckle.AspNetCore.SwaggerUI;
using TeleChat.WebAPI.Extensions;
using TeleChat.WebAPI.Hub;

var builder = WebApplication.CreateBuilder(args);

builder.AddOptions();
builder.AddServices();
builder.AddHubChat();
builder.AddRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(DocExpansion.None);
        c.EnableTryItOutByDefault();
    });
    await app.MigrateDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/Chat");

app.Run();