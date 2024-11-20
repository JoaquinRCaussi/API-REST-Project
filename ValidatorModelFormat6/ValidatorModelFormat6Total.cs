using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ValidatorModelFormat6;

[ExcludeFromCodeCoverage]
public class ValidatorModelFormat6Total : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return Regex.IsMatch(modelo.Value, @"^[A-Z]{6}$");
    }
}
