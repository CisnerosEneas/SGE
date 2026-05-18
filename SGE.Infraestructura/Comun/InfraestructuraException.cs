using System;
namespace SGE.Infraestructura.Comun;

public class InfraestructuraException : Exception
{
    public InfraestructuraException(string mensaje) : base(mensaje)
    {
    }
}