using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ValidatorModelFormat1;

[ExcludeFromCodeCoverage]
public class ValidatorModelFormat13 : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return Regex.IsMatch(modelo.Value, @"^[A-Z]{1}\d{3}$");
    }
}
