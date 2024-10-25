using Domain;

namespace Models;

public class HomeOwnerRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string ImagePath { get; set; }


    public User ToUser()
    {
        return new User { Name = Name, Email = Email, LastName = LastName, Password = Password, ImagePath = ImagePath };
    }
}
