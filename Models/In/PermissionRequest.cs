namespace Models;

public class PermissionRequest
{
    public bool CanAsociateDevices { get; set; }
    public bool CanListDevices { get; set; }
    public bool CanGetNotifications { get; set; }
    public bool CanAddMembers { get; set; }
}
