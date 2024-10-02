namespace Models;

public class LoginResponse
{
    public string Token { get; set; }
    public string UserRole { get; set; }

    public LoginResponse(string token, string userRole)
    {
        Token = token;
        UserRole = userRole;
    }
}
