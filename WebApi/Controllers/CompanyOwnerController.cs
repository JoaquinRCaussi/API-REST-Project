using Domain;
using LogicInterface;
using WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
        var companyOwner = _userLogic.CreateCompanyOwner(user);
        return Ok(companyOwner);
    }
    
    [HttpPut]
    public IActionResult AddCompanyToCompanyOwner(AddCompanyToOwnerRequest request)
    {
        var user = _userLogic.AddCompanyToCompanyOwner(request.CompanyOwner, request.Company);
        
        var response = new AddCompanyToOwnerResponse(user, request.Company);

        return Ok(response);
    }
}
