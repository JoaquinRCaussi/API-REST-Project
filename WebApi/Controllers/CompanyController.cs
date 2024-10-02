using Domain;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
[Route("api/companies")]
public sealed class CompanyController(ICompanyLogic companyLogic) : ControllerBase
{
    [HttpPost]
    [AuthorizationFilter("CanCreateCompany")]
    public IActionResult CreateCompany([FromBody] CompanyRequest company)
    {
        var user = HttpContext.Items[0] as User;
        Company companyToCreate = company.ToArgs(user);
        Company createdCompany = companyLogic.CreateCompany(companyToCreate);
        var response = new CompanyResponse(createdCompany);
        return Ok(response);
    }

    [HttpGet]
    
    public IActionResult GetCompanies([FromQuery]string name, [FromQuery] string ownerName)
    {
        var companies = companyLogic.GetCompanies(name, ownerName).Select(c => new CompanyResponse(c)).ToList();
        return Ok(companies);
    }
}
