using System.Diagnostics.CodeAnalysis;
using ServiceFactory;
using WebApi.Filters;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddScoped<AuthenticationFilter>();

WebApplication? app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}
