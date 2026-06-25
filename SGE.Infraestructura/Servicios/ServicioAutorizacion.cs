using System;
using SGE.Aplicacion.Autorizacion;
namespace SGE.Infraestructura.Servicios;

public class ServicioAutorizacion : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        return true;
    }
}