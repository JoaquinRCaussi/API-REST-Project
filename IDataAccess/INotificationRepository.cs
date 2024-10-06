using Domain;
using Models;

namespace IDataAccess;

public interface INotificationRepository
{
    List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, SensorRequest sensor);
}
