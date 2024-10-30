using Domain;

namespace Models;

public class GetRoomsResponse
{
    public List<Room> Rooms { get; set; }
    
    public GetRoomsResponse(List<Room> rooms)
    {
        Rooms = rooms;
    }
    
    public List<string> ToArgs()
    {
        var roomNames = Rooms.Select(x => x.Name).ToList();
        return roomNames;
    }
}
