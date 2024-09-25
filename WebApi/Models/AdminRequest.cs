using Domain;

namespace WebApi.Models;

public class AdminRequest
{
    public AdminRequest(string name, string lastName, string email, string password)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

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
