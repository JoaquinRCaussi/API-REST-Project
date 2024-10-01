using Domain;
using Models;

namespace IBusinessLogic;

public interface ISessionLogic
{
    AuthenticationResult Authenticate(string email, string password);
}
