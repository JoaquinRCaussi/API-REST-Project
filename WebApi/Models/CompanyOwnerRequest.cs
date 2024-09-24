using Domain;

namespace WebApi.Models;

public class CompanyOwnerRequest
{
    public string Name;
    public string LastName;
    public string Email;
    public string Password;
    
    public CompanyOwnerRequest(User user)
    {
        Name = user.Name;
        LastName = user.LastName;
        Email = user.Email;
        Password = user.Password;
    }
    
    public User ToArgs()
    {
        return new User(Name, LastName, Email, Password);
    }
}
