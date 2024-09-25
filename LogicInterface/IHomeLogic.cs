using Domain;

namespace LogicInterface;

public interface IHomeLogic
{
    Home CreateHome(Home home);
    
    List<Home> GetHomes();
}
