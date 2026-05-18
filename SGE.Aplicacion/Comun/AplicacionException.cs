using System;
namespace SGE.Aplicacion.Comun;

public class AplicacionException : Exception
{
    public AplicacionException(string mensaje) : base(mensaje)
    {
    }
}