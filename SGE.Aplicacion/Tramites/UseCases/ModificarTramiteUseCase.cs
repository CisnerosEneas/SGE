using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Aplicacion.Tramites.Services;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly ActualizacionEstadoExpedienteService _estadoService;

    public ModificarTramiteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService, ActualizacionEstadoExpedienteService estadoService)
    {
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _estadoService = estadoService;
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
        return new ModificarTramiteResponse(tramite.Id, tramite.ExpedienteId, tramite.Etiqueta, tramite.Contenido.Texto, tramite.FechaUltimaModificacion);
    }
}