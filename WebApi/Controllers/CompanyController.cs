using Domain;
using LogicInterface;
using WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/companies")]
public sealed class CompanyController(ICompanyLogic companyLogic) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateCompany(CompanyRequest company)
    {
        Company companyToCreate = company.ToArgs();
        Company createdCompany = companyLogic.CreateCompany(companyToCreate);
        var response = new CompanyResponse(createdCompany);
        return Ok(response);
    }
}
