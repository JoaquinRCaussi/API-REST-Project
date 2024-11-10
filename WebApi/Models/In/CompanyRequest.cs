using BusinessLogic.Entities;

namespace WebApi.Models.In;

public class CompanyRequest
{
    public CompanyRequest(string name, string RUT, string logo, string validatorModelName)
    {
        Name = name;
        this.RUT = RUT;
        this.logo = logo;
        ValidatorModelName = validatorModelName;
    }

    public string Name { get; set; }
    public string RUT { get; set; }
    public string logo { get; set; }
    public string ValidatorModelName { get; set; }

    public Company ToArgs(User? user)
    {
        return new Company
        {
            Name = Name,
            RUT = RUT,
            Logo = logo,
            Owner = user,
            OwnerId = user?.Id ?? Guid.Empty,
            ValidatorModelName = ValidatorModelName

        };
    }
}
