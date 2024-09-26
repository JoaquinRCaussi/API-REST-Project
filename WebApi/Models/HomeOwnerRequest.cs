using Domain;

namespace WebApi.Models;

public class HomeOwnerRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    

    public User ToUser()
    {
        return new User
        {
            Name = Name,
            Email = Email,
            LastName = LastName,
            Password = Password
        };
    }
}
