using Domain;
using Models;

namespace IDataAccess;

public interface INotificationRepository
{
    Notification CreateNotificationSensor(Guid homeId, Guid hardwareId, SensorRequest sensor);
}
