namespace Domain;

public class Session
{
    public Guid UserID { get; set; }
    public Guid? RoleID { get; set; }
    public Guid Token { get; set; }
    
    public Session()
    {
        Token = Guid.NewGuid();
    }
}
