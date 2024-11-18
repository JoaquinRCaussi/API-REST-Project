using System.Reflection;
using ModeloValidador.Abstracciones;

namespace BusinessLogic.Validators;

public class ValidatorService
{
    private readonly string _pluginsPath;

    protected readonly List<Type> implementations = [];

    public ValidatorService()
    {
        _pluginsPath = Path.Combine(Directory.GetCurrentDirectory(), "Validators");

        if (!Directory.Exists(_pluginsPath))
        {
            Directory.CreateDirectory(_pluginsPath);
        }
    }

    public virtual List<string> ChargeValidators()
    {
        // Cargar cada DLL que esté en la carpeta Validators
        foreach (var dllPath in Directory.GetFiles(_pluginsPath, "*.dll"))
        {
            var assembly = Assembly.LoadFile(dllPath);

            // Buscar y crear instancias de IModeloValidador en el DLL cargado
            var types = assembly.GetTypes().Where(t => typeof(IModeloValidador).IsAssignableFrom(t) && t.IsClass);

            if (!types.Any())
            {
                Console.WriteLine($"No classes found that implement IModeloValidador in {dllPath}");
                continue;
            }

            foreach (var tipo in types)
            {
                if (!implementations.Contains(tipo))
                {
                    implementations.Add(tipo);
                }
            }
        }

        return implementations.ConvertAll(t => t.Name);
    }

    public IModeloValidador GetValidator(int index, params object[] args)
    {
        var type = implementations.ElementAt(index);
        return Activator.CreateInstance(type, args) as IModeloValidador;
    }

    //Todavia no se cual de los dos vamos a usar, dejo este por aca tambien
    public virtual IModeloValidador GetValidatorByName(string name, params object[] args)
    {
        var type = implementations.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (type == null)
        {
            throw new ArgumentException($"No validator found by the name: {name}");
        }
        return Activator.CreateInstance(type, args) as IModeloValidador;
    }

}

