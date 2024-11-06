

namespace BusinessLogic.LogicInterfaces;

public interface ISessionLogic
{
    AuthenticationResult Authenticate(string email, string password);
}
