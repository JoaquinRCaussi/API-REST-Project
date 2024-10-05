using System.Diagnostics.CodeAnalysis;
using ServiceFactory;
using WebApi.Filters;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers(
    options =>
    {
        options.Filters.Add<ExceptionFilter>();
    });
builder.Services.AddServices(builder.Configuration.GetConnectionString("DefaultConnection")!);

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
