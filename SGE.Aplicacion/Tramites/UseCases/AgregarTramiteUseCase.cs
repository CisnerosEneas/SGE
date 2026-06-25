using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class AgregarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpedienteService _estadoService;

    public AgregarTramiteUseCase(
        ITramiteRepository tramiteRepository,
        IAutorizacionService autorizacionService,
        ActualizacionEstadoExpedienteService estadoService)
    {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
    }

    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta);
        if (!autorizado)
            throw new AutorizacionException();
        var contenido = new ContenidoTramite(request.Contenido);
        var tramite = new Tramite(request.ExpedienteId, request.Etiqueta, contenido, request.IdUsuario);
        _tramiteRepository.Agregar(tramite);
        _estadoService.ActualizarEstadoExpediente(request.ExpedienteId, request.IdUsuario);
        return new AgregarTramiteResponse(tramite.Id, tramite.ExpedienteId, tramite.Etiqueta, tramite.Contenido.Texto, tramite.FechaCreacion);
    }
}