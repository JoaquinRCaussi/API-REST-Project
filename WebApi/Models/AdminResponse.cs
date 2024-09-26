using Domain;

namespace WebApi.Models;

public class AdminResponse
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}
