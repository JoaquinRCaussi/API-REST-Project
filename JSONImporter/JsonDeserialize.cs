using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace JSONImporter;

[ExcludeFromCodeCoverage]
public class Root
{
    [JsonPropertyName("dispositivos")]
    public List<Device> Dispositivos { get; set; }
}

public class Device
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }

    [JsonPropertyName("modelo")]
    public string Modelo { get; set; }

    [JsonPropertyName("fotos")]
    public List<Photo> Fotos { get; set; }

    [JsonPropertyName("person_detection")]
    public bool? PersonDetection { get; set; }

    [JsonPropertyName("movement_detection")]
    public bool? MovementDetection { get; set; }
}

public class Photo
{
    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("es_principal")]
    public bool EsPrincipal { get; set; }
}

