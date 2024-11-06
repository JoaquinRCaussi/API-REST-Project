namespace WebApi.Models.Out;

public class AuthenticationResult
{
    public Guid UserId { get; set; }
    public Guid? RoleId { get; set; }
}
