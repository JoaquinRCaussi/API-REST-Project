using Domain;

namespace IDataAccess;

public interface IHomeRepository
{
    public Home CreateHome(Home home);

    public List<Home> GetHomes();

    public List<Home> GetHomesByUser(Guid userId);
    
    public Home GetHome(Guid homeId);
    
    public List<User> GetHomeMembers(Guid homeId);
}
