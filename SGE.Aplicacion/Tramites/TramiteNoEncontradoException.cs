using System;
using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Tramites;

public class TramiteNoEncontradoException
    : EntidadNoEncontradaException
{
    public TramiteNoEncontradoException()
        : base("El tramite no existe")
    {
    }
}