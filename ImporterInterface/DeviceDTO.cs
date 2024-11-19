namespace ImporterInterface;

public class DeviceDTO
{
    public string Id { get; set; }
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public List<string> Fotos { get; set; }
    public bool? PersonDetection { get; set; }
    public bool? MovementDetection { get; set; }
}

