using System;
using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record ExpedienteDto(Guid Id, string Caratula, EstadoExpediente Estado, DateTime FechaCreacion);