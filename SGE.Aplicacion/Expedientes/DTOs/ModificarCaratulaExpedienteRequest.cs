using System;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record ModificarCaratulaExpedienteRequest(Guid IdExpediente, string NuevaCaratula, Guid IdUsuario);