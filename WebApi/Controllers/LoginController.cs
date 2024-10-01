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

        return Ok(new LoginResponse(authResult.UserId.ToString(), authResult.RoleId.ToString() ?? throw new InvalidOperationException()));
    }

}
