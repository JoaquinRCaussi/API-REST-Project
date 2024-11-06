using Domain;

namespace Models;

public class GetUserResponse
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string LastName { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Role Role { get; set; }
}
