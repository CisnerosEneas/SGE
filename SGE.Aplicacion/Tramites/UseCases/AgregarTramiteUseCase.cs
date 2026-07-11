using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Tramites.UseCases;

public class AgregarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpedienteService _estadoService;
    private readonly SGE.Aplicacion.Comun.IUnidadDeTrabajo _unidadDeTrabajo;

    public AgregarTramiteUseCase(
        ITramiteRepository tramiteRepository,
        IAutorizacionService autorizacionService,
        ActualizacionEstadoExpedienteService estadoService,
        SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
    {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
        _unidadDeTrabajo = unidadDeTrabajo;
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
        _unidadDeTrabajo.Guardar();
        return new AgregarTramiteResponse(tramite.Id, tramite.ExpedienteId, tramite.Etiqueta, tramite.Contenido.Texto, tramite.FechaCreacion);
    }
}