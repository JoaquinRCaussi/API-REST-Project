using Domain;
using Models;

namespace IBusinessLogic;

public interface ISessionLogic
{
    User? GetCurrentUser(Guid? token);
    AuthenticationResult Authenticate(string email, string password);
}
