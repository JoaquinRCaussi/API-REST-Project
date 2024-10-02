using Domain;

namespace LogicInterface;

public interface IDeviceLogic
{
    Device CreateDevice(Device device);
    Camera CreateCamera(Camera camera);

}
