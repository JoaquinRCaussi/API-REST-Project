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
        _dbContext.Devices?.Add(device);
        _dbContext.SaveChanges();
        return device;
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
        return _dbContext.Devices?.Any(x => x.Name == name && x.CompanyId == companyId) ?? false;
    }
}
