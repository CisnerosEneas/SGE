using System;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Expedientes.DTOs;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;

    public EliminarExpedienteUseCase(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
    }

    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        bool autorizado = _autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja);
        if (!autorizado)
            throw new AutorizacionException();
        var expediente = _expedienteRepository.ObtenerPorId(request.IdExpediente);
        if (expediente is null)
            throw new ExpedienteNoEncontradoException();
        var tramites = _tramiteRepository.ObtenerPorExpedienteId(request.IdExpediente);
        foreach (var tramite in tramites)
            _tramiteRepository.Eliminar(tramite.Id);
        _expedienteRepository.Eliminar(request.IdExpediente);
        return new EliminarExpedienteResponse(true);
    }
}
