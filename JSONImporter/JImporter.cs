using System.Text.Json;
using ImporterInterface;

namespace JSONImporter;

public class JImporter: IDeviceImporter
{
    public List<DeviceDTO> ImportDevices()
    {
        var path = @"D:\Ort\";
        var fileName = "devices-to-import.json";
        List<DeviceDTO> devices = new();

        try
        {
            var jsonString = File.ReadAllText(path + fileName);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            
            var rootObject = JsonSerializer.Deserialize<Root>(jsonString, options);
            
            if (rootObject != null && rootObject.Dispositivos != null)
            {
                Console.WriteLine("Deserialización exitosa!");
                
                foreach (var device in rootObject.Dispositivos)
                {
                    Console.WriteLine($"Nombre del dispositivo: {device.Nombre}");
                    
                    DeviceDTO deviceDto = new()
                    {
                        Id = device.Id,
                        Tipo = device.Tipo,
                        Nombre = device.Nombre,
                        Modelo = device.Modelo,
                        Fotos = device.Fotos.Select(f => f.Path).ToList(),
                        PersonDetection = device.PersonDetection,
                        MovementDetection = device.MovementDetection
                    };
                    
                    devices.Add(deviceDto);
                }
            }
            else
            {
                Console.WriteLine("Error en la deserialización");
            }
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error al hacer serializacion: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al leer archivo: {e.Message}");
        }
        
        return devices;

    }
}
