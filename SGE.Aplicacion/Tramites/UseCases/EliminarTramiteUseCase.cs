using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Dominio.Usuarios;
namespace SGE.Aplicacion.Tramites.UseCases;

public class EliminarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpedienteService _estadoService;

    private readonly SGE.Aplicacion.Comun.IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarTramiteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService,  ActualizacionEstadoExpedienteService estadoService, SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
    {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja);
        if (!autorizado)
            throw new AutorizacionException();
        _tramiteRepository.Eliminar(request.TramiteId);
        _estadoService.ActualizarEstadoExpediente(request.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
        return new EliminarTramiteResponse(request.TramiteId);
    }
}