using Domain;

namespace Models;

public class AdminRequest
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public User ToArgs()
    {
        return new User { Name = Name, LastName = LastName, Email = Email, Password = Password };
    }
}
