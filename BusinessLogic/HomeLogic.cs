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
}
