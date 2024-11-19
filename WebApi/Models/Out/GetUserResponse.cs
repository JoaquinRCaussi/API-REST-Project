using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class GetUserResponse
{
    public Guid? Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string LastName { get; set; }
    public required DateTime CreatedAt { get; set; }
    public Role? Role { get; set; }
}
