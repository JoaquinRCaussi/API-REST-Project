using Domain;
using DTOS;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("[controller]")]
public sealed class CompanyController : ControllerBase
{
    private readonly ICompanyLogic _companyLogic;
    public CompanyController(ICompanyLogic companyLogic)
    {
        _companyLogic = companyLogic;
    }
    [HttpPost]
    public IActionResult CreateCompany(CompanyRequest company)
    {
        Company companyToCreate = company.ToArgs();
        Company createdCompany = _companyLogic.CreateCompany(companyToCreate);
        var response = new CompanyResponse(createdCompany);
        return Ok(response);
    }
    
}
