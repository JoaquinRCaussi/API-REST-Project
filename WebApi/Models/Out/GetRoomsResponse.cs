using BusinessLogic.Entities;

namespace WebApi.Models.Out;

public class GetRoomsResponse
{
    public List<RoomResponse> Rooms { get; set; }

    public GetRoomsResponse(List<Room> rooms)
    {
        Rooms = rooms.Select(r => new RoomResponse(r)).ToList();
    }
}

public class RoomResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<HomeDevice> Devices { get; set; }

    public RoomResponse(Room room)
    {
        Id = room.Id;
        Name = room.Name;
        Devices = room.Devices;
    }
}
