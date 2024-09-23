using Domain;

namespace DTOS;

public class UserResponse
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    
    public UserResponse(User user)
    {
        Name = user.Name;
        LastName = user.LastName;
        Email = user.Email;
        
    }
}
