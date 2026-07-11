using System;
using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Autorizacion;

public class AutorizacionException : SGE.Aplicacion.Comun.AutorizacionException
{
    public AutorizacionException()
        : base("No cuenta con la autorización necesaria")
    {
    }

    public AutorizacionException(string mensaje) : base(mensaje) { }

    public AutorizacionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
