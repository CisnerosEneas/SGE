using System;
using System.Linq;
using System.Collections.Generic;
using SGE.Aplicacion.Expedientes.DTOs;
using SGE.Aplicacion.Tramites.DTOs;
namespace SGE.Aplicacion.Expedientes.UseCases;

public class ListarExpedientesUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;

    public ListarExpedientesUseCase(IExpedienteRepository expedienteRepository)
    {
        _expedienteRepository = expedienteRepository;
    }

    public ListarExpedientesResponse Ejecutar()
    {
        var expedientes = _expedienteRepository.ObtenerTodos();
        var expedientesDto = expedientes
            .Select(expediente => new ExpedienteDto(
                expediente.Id,
                expediente.Caratula.Texto,
                expediente.Estado,
                expediente.FechaCreacion,
                new List<TramiteDto>()
            ))
            .ToList();
        return new ListarExpedientesResponse(expedientesDto);
    }
}