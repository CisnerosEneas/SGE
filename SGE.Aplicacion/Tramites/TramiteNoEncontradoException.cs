using System;
using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Tramites;

public class TramiteNoEncontradoException
    : AplicacionException
{
    public TramiteNoEncontradoException()
        : base("El tramite no existe")
    {
    }
}