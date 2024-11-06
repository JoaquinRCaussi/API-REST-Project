using BusinessLogic.Entities;
using BusinessLogic.LogicInterfaces;
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
        return CreatedAtAction(nameof(CreateCompany), new { id = createdCompany.Id }, response);
    }

    [HttpGet]
    [AuthorizationFilter("CanGetCompanies")]
    public IActionResult GetCompanies(
    [FromQuery] string? name,
    [FromQuery] string? ownerName,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        List<Company> companies = companyLogic.GetCompanies(name, ownerName);

        var totalResults = companies.Count;

        var paginatedCompanies = companies
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = paginatedCompanies.Select(x => new CompanyResponse(x)).ToList();

        return Ok(new
        {
            TotalResults = totalResults,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Companies = response
        });
    }
}
