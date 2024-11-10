using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace ValidatorModelFormat6;

public class ValidatorModelFormat6: IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return Regex.IsMatch(modelo.Value, @"^[A-Z]{6}$");
    }
}
