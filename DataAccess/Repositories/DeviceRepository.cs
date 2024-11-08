using BusinessLogic.DataAccess.Interfaces;
using BusinessLogic.Entities;
using DataAccess.Data;

using Microsoft.EntityFrameworkCore;

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
        var company = _dbContext.Companies?.FirstOrDefault(x => x.Id == device.CompanyId);

        if (company == null)
        {
            throw new Exception("The Company does not exist");
        }

        device.Company = company;

        _dbContext.Devices?.Add(device);
        _dbContext.SaveChanges();
        return device;
    }

    public Camera CreateCamera(Camera camera)
    {
        var company = _dbContext.Companies?.FirstOrDefault(x => x.Id == camera.CompanyId);

        if (company == null)
        {
            throw new EntityNotFoundException("The Company does not exist");
        }

        camera.Company = company;
        _dbContext.Devices?.Add(camera);
        _dbContext.SaveChanges();
        return camera;
    }


    public List<Device> GetDevices(string name, string model, string companyName, DeviceType deviceType)
    {

        return _dbContext.Devices?
            .Include(x => x.Company)
            .Where(x => x.Name.Contains(name) && x.Model.Contains(model) && x.Company.Name.Contains(companyName) && x.DeviceType == deviceType).ToList()!;
    }

    public bool ExistsDevice(string? name, Guid companyId)
    {
        return _dbContext.Devices?.Any(x => x.Name == name && x.CompanyId == companyId) ?? false;
    }

    public List<Device> GetDevicesNoType(string name, string model, string companyName)
    {
        return _dbContext.Devices?
            .Include(x => x.Company)
            .Where(x => x.Name.Contains(name) && x.Model.Contains(model) && x.Company.Name.Contains(companyName))
            .ToList()!;
    }
}
