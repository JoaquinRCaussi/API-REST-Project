using Domain;

namespace IBusinessLogic;

public interface ISessionLogic
{
    User? GetCurrentUser(Guid? token);
    User? Authenticate(string email, string password);
}
