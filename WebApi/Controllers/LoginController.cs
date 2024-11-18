using BusinessLogic.LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.In;
using WebApi.Models.Out;

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

        var strToken = token.ToString();

        Response.Headers.Append("Authorization", strToken);

        var response = new LoginResponse(strToken ?? throw new InvalidOperationException(), authResult.RoleID.ToString() ?? throw new InvalidOperationException())
        {
            Token = strToken,
            UserRole = authResult.RoleID.ToString(),
            UserId = authResult.UserID
        };

        return CreatedAtAction(nameof(Login), response);
    }
}
