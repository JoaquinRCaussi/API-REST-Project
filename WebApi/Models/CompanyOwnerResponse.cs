using Domain;

namespace WebApi.Models;

public class CompanyOwnerResponse
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public CompanyOwnerResponse(User user)
    {
        Name = user.Name;
        LastName = user.LastName;
        Email = user.Email;
    }
}
