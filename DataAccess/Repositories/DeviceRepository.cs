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

    public bool ExistsDevice(string? name, Guid companyId)
    {
        return _dbContext.Devices?.Any(x => x.Name == name && x.CompanyId == companyId) ?? false;
    }

    public (List<Device> Devices, int TotalResults) GetDevices(string name, string model, string companyName, DeviceType? deviceType, int pageNumber, int pageSize)
    {
        var query = _dbContext.Devices?
            .Include(x => x.Company)
            .Where(x => x.Name.Contains(name) && x.Model.Contains(model) && x.Company.Name.Contains(companyName));

        if (deviceType.HasValue)
        {
            query = query?.Where(x => x.DeviceType == deviceType.Value);
        }

        var totalResults = query == null ? 0 : query.Count();

        var paginatedDevices = query?
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (paginatedDevices ?? [], totalResults);
    }
}
