using BusinessLogic.Entities;

namespace BusinessLogic.DataAccess.Interfaces;

public interface INotificationRepository
{
    List<Notification> CreateNotificationSensor(Guid homeId, Guid hardwareId, string anEvent);

    List<Notification> CreateNotificationCamera(Guid homeId, Guid hardwareId, string anEvent);
}
