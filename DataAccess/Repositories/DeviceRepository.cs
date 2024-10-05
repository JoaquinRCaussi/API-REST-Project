using DataAccess.Data;
using Domain;
using IDataAccess;

namespace DataAccess.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly HMDbContext _dbContext;

    public DeviceRepository(HMDbContext context)
    {
        _dbContext = context;
    }

    public Device CreateDevice(Device device)
    {
        throw new NotImplementedException();
    }

    public Camera CreateCamera(Camera camera)
    {
        throw new NotImplementedException();
    }

    public List<Device> GetDevices(string name, string companyName, string deviceType)
    {
        throw new NotImplementedException();
    }

    public bool ExistsDevice(string? name, Guid companyId)
    {
        throw new NotImplementedException();
    }
}
