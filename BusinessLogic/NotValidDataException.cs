namespace BusinessLogic;

public class NotValidDataException : Exception
{
    public NotValidDataException(string message) : base(message)
    {
    }
}
