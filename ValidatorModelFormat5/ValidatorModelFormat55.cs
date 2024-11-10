using ModeloValidador.Abstracciones;

namespace ValidatorModelFormat5;

public class ValidatorModelFormat55 : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Length == 5;
    }
}
