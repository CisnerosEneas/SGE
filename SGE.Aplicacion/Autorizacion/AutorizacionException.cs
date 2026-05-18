using System;
using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Autorizacion;

public class AutorizacionException : AplicacionException
{
    public AutorizacionException()
        : base("No cuenta con la autorizacion necesaria")
    {
    }
}
