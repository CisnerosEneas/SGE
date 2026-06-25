using System;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record EliminarExpedienteRequest(Guid IdExpediente, Guid IdUsuario);