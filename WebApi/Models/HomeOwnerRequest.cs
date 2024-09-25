using Domain;

namespace WebApi.Models;

public class HomeOwnerRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    
    public HomeOwnerRequest(User user)
    {
        Name = user.Name;
        Email = user.Email;
        LastName = user.LastName;
        Password = user.Password;
    }
    
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
