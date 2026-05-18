using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class ModificarCaratulaExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService)
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
        return new ModificarCaratulaExpedienteResponse(true);
    }
}