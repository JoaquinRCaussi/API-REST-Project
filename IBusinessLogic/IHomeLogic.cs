using Domain;

namespace LogicInterface;

public interface IHomeLogic
{
    Home CreateHome(Home home);

    List<Home> GetHomes();

    List<Home> GetHomesByUser(Guid userId);

    Home GetHome(Guid homeId);

    List<User> GetHomeMembers(Guid homeId);

    Home AddMember(Guid homeId, Guid userId);
}
