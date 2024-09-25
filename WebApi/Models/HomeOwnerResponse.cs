using Domain;

namespace WebApi.Models;

public class HomeOwnerResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }

    public HomeOwnerResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        LastName = user.LastName;
    }
}
