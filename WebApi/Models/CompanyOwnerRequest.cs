using Domain;

namespace WebApi.Models;

public class CompanyOwnerRequest
{
    public required string Name;
    public required string LastName;
    public required string Email;
    public required string Password;

    public User ToArgs()
    {
        return new User
        {
            Name = Name,
            LastName = LastName,
            Email = Email,
            Password = Password
        };
    }
}
