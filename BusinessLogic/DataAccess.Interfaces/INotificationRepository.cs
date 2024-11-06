using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface INotificationRepository
{
    List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, SensorRequest sensor);

    List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, SensorRequest sensor);
}
