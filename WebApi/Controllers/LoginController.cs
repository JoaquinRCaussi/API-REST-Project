using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly ISessionLogic _sessionLogic;

    public LoginController(ISessionLogic sessionLogic)
    {
        _sessionLogic = sessionLogic;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var authResult = _sessionLogic.Authenticate(loginRequest.Email, loginRequest.Password);

        if (authResult == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = authResult.UserId.ToString();

        Response.Headers.Append("Authorization", token);

        return Ok(new LoginResponse(authResult.UserId.ToString(), authResult.RoleId.ToString() ?? throw new InvalidOperationException())
        {
            Token = token,
            UserRole = authResult.RoleId.ToString()

        });
    }
}
