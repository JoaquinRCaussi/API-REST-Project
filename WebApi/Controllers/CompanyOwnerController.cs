using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/company-owner")]
public class CompanyOwnerController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public CompanyOwnerController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpPost]
    public IActionResult CreateCompanyOwner(User user)
    {
        User companyOwner = _userLogic.CreateCompanyOwner(user);
        return Ok(companyOwner);
    }

    [HttpPut]
    public IActionResult AddCompanyToCompanyOwner(AddCompanyToOwnerRequest request)
    {
        User user = _userLogic.AddCompanyToCompanyOwner(request.CompanyOwner, request.Company);

        var response = new AddCompanyToOwnerResponse(user, request.Company);

        return Ok(response);
    }
}
