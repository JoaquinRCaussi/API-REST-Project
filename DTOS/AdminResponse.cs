using Domain;

namespace DTOS;

public class AdminResponse
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public AdminResponse(User user)
    {
        Name = user.Name;
        LastName = user.LastName;
        Email = user.Email;
    }
}
