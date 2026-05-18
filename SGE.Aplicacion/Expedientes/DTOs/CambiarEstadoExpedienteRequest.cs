using System;
using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Expedientes.DTOs;

public record CambiarEstadoExpedienteRequest(Guid IdExpediente, EstadoExpediente NuevoEstado, Guid IdUsuario);
