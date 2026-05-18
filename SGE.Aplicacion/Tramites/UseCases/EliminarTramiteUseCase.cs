using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.Services;

namespace SGE.Aplicacion.Tramites;

public class EliminarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpedienteService _estadoService;

    public EliminarTramiteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService,  ActualizacionEstadoExpedienteService estadoService)
    {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
    }

    public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja);
        if (!autorizado)
            throw new AutorizacionException();
        _tramiteRepository.Eliminar(request.TramiteId);
        _estadoService.ActualizarEstadoExpediente(request.ExpedienteId, request.IdUsuario);
        return new EliminarTramiteResponse(request.TramiteId);
    }
}