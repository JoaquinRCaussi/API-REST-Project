using Domain;

namespace IDataAccess;

public interface IHomeRepository
{
    public Home CreateHome(Home home);
    
    public List<Home> GetHomes();
}
