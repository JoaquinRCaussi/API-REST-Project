using System.Diagnostics.CodeAnalysis;

namespace ImporterInterface;
public interface IDeviceImporter
{
    [ExcludeFromCodeCoverage]
    List<DeviceDTO> ImportDevices();
}
