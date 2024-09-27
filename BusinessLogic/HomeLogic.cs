using Domain;
using IDataAccess;
using LogicInterface;

namespace BusinessLogic;

public class HomeLogic : IHomeLogic
{
    private readonly IHomeRepository _homeRepository;

    public HomeLogic(IHomeRepository homeRepository)
    {
        _homeRepository = homeRepository;
    }

    public Home CreateHome(Home home)
    {
        return _homeRepository.CreateHome(home);
    }

    public List<Home> GetHomes()
    {
        return _homeRepository.GetHomes();
    }

    public List<Home> GetHomesByUser(Guid userId)
    {
        return _homeRepository.GetHomesByUser(userId);
    }
    
    public Home GetHome(Guid homeId)
    {
        return _homeRepository.GetHome(homeId);
    }
    
    public List<User> GetHomeMembers(Guid homeId)
    {
        return _homeRepository.GetHomeMembers(homeId);
    }
    
    public Home AddMember(Guid homeId, Guid userId)
    {
        return _homeRepository.AddMember(homeId, userId);
    }
    
}
