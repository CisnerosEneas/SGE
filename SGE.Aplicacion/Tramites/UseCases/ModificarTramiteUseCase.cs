using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;
namespace SGE.Aplicacion.Tramites.UseCases;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpedienteService _estadoService;
    private readonly SGE.Aplicacion.Comun.IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarTramiteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService, ActualizacionEstadoExpedienteService estadoService, SGE.Aplicacion.Comun.IUnidadDeTrabajo unidadDeTrabajo)
    {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion);
        if (!autorizado)
            throw new AutorizacionException();
        var tramite = _tramiteRepository.ObtenerPorId(request.TramiteId);
        if (tramite is null)
            throw new TramiteNoEncontradoException();
        var contenido =new ContenidoTramite(request.Contenido);
        tramite.Modificar(request.Etiqueta, contenido, request.IdUsuario);
        _tramiteRepository.Modificar(tramite);
        _estadoService.ActualizarEstadoExpediente(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
        return new ModificarTramiteResponse(tramite.Id, tramite.ExpedienteId, tramite.Etiqueta, tramite.Contenido.Texto, tramite.FechaUltimaModificacion);
    }
}