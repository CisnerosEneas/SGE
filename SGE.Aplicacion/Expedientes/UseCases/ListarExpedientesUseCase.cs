using System;
using System.Linq;
using System.Collections.Generic;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Tramites.DTOs;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class ListarExpedientesUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;

    public ListarExpedientesUseCase(IExpedienteRepository expedienteRepository, ITramiteRepository tramiteRepository)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
    }

    public ListarExpedientesResponse Ejecutar(ListarExpedientesRequest request)
    {
        var expedientes = _expedienteRepository.ObtenerTodos();
        var expedientesDto = expedientes
            .Select(expediente => new ExpedienteDto(
                expediente.Id,
                expediente.Caratula.Texto,
                expediente.Estado,
                expediente.FechaCreacion,
                expediente.FechaUltimaModificacion,
                expediente.UsuarioUltimoCambio,
                _tramiteRepository.ObtenerPorExpedienteId(expediente.Id)
                    .Select(t => new TramiteDto(
                        t.Id,
                        t.ExpedienteId,
                        t.FechaCreacion,
                        t.Contenido.Texto,
                        t.Etiqueta.ToString()))
                    .ToList()
            ))
            .ToList();
        return new ListarExpedientesResponse(expedientesDto);
    }
}