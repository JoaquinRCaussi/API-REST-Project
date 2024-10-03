using Domain;

namespace IBusinessLogic;

public interface IDeviceLogic
{
    Device CreateDevice(Device device);
    Camera CreateCamera(Camera camera);

}
