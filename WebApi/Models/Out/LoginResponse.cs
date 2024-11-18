namespace WebApi.Models.Out;

public class LoginResponse
{
    public Guid? UserId { get; set; }
    public string Token { get; set; }
    public string UserRole { get; set; }

    public LoginResponse(string token, string userRole)
    {
        Token = token;
        UserRole = userRole;
    }
}
