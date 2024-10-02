using Domain;
using LogicInterface;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/companies")]
public sealed class CompanyController(ICompanyLogic companyLogic) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyRequest company)
    {
        Company companyToCreate = company.ToArgs();
        Company createdCompany = companyLogic.CreateCompany(companyToCreate);
        var response = new CompanyResponse(createdCompany);
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetCompanies()
    {
        var companies = companyLogic.GetCompanies().Select(c => new CompanyResponse(c)).ToList();
        return Ok(companies);
    }
}
