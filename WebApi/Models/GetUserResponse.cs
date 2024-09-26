namespace WebApi.Models;

public class GetUserResponse
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string LastName { get; set; }
}
