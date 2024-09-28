using Domain;

namespace IBusinessLogic;

public interface ISessionLogic
{
    User? GetCurrentUser(Guid? token);
    Guid Authenticate(string email, string password);
}
