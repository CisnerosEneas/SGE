using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Expedientes.DTOs;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly SGE.Aplicacion.Comun.IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarExpedienteUseCase(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService, SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja);
        if (!autorizado)
            throw new Comun.AutorizacionException();
        var expediente = _expedienteRepository.ObtenerPorId(request.IdExpediente);
        if (expediente is null)
            throw new ExpedienteNoEncontradoException();
        var tramites = _tramiteRepository.ObtenerPorExpedienteId(request.IdExpediente);
        foreach (var tramite in tramites)
            _tramiteRepository.Eliminar(tramite.Id);
        _expedienteRepository.Eliminar(request.IdExpediente);
        _unidadDeTrabajo.Guardar();
        return new EliminarExpedienteResponse(true);
    }
}
