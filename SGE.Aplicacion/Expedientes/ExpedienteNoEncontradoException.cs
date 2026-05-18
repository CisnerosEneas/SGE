using System;
using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Expedientes;

public class ExpedienteNoEncontradoException
    : AplicacionException
{
    public ExpedienteNoEncontradoException()
        : base("El expediente no existe")
    {
    }
}