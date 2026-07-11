using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Usuarios;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class ModificarCaratulaExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService, SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
{
    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        bool autorizado = autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion);
        if (!autorizado)
            throw new AutorizacionException();
        var expediente = expedienteRepository.ObtenerPorId(request.IdExpediente);
        if (expediente is null)
            throw new ExpedienteNoEncontradoException();
        var nuevaCaratula = new Caratula(request.NuevaCaratula);
        expediente.ModificarCaratula(nuevaCaratula, request.IdUsuario);
        expedienteRepository.Modificar(expediente);
        unidadDeTrabajo.Guardar();
        return new ModificarCaratulaExpedienteResponse(true);
    }
}