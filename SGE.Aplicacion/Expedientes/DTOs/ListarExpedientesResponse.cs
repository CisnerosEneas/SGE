using System;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record ListarExpedientesResponse(IEnumerable<ExpedienteDto> Expedientes);