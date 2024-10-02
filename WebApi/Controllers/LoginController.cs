using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public LoginController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var authResult = _sessionService.Authenticate(loginRequest.Email, loginRequest.Password);

        if (authResult == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = authResult.Token;

        Response.Headers.Append("Authorization", token);

        return Ok(new LoginResponse(authResult.Token ?? throw new InvalidOperationException(),
                authResult.RoleID.ToString() ?? throw new InvalidOperationException())
        {
            Token = token,
            UserRole = authResult.RoleID.ToString()
        });
    }
}
