using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ValidatorModelFormat3;

public class ValidatorModelFormat3: IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return Regex.IsMatch(modelo.Value, @"^[A-Z]{3}\d{3}$");
    }
}
